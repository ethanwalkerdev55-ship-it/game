import info.ata4.io.buffer.ByteBufferUtils;
import info.ata4.io.util.PathUtils;
import info.ata4.unity.asset.bundle.AssetBundle;
import info.ata4.unity.asset.bundle.struct.AssetBundleHeader;
import java.io.IOException;
import java.lang.reflect.Field;
import java.nio.ByteBuffer;
import org.apache.commons.lang3.tuple.Pair;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Inject extracted files into a UnityWeb bundle, padding each entry to the
 * original client slot size so the uncompressed payload layout stays stable.
 */
public class BundleInjectPreserve {
    public static void main(String[] args) throws Exception, InterruptedException {
        if (args.length < 2) {
            System.err.println("Usage: BundleInjectPreserve <bundle.unity3d> <client-bundle.unity3d>");
            System.exit(1);
        }

        Path bundleFile = Paths.get(args[0]).toAbsolutePath();
        Path clientBundle = Paths.get(args[1]).toAbsolutePath();
        Path bundleDir = PathUtils.removeExtension(bundleFile);
        Path clientDir = PathUtils.removeExtension(clientBundle);

        if (!Files.exists(bundleFile) || !Files.isDirectory(bundleDir)) {
            System.err.println("Missing bundle or extract dir: " + bundleFile);
            System.exit(1);
        }
        if (!Files.exists(clientBundle)) {
            System.err.println("Missing client reference bundle: " + clientBundle);
            System.exit(1);
        }

        Map<String, Integer> originalSizes = loadOriginalSizes(clientBundle, clientDir);
        padEntries(bundleDir, originalSizes);

        AssetBundle bundle = new AssetBundle();
        bundle.load(ByteBufferUtils.openReadOnly(bundleFile));
        injectPaddedEntries(bundle, bundleDir);

        Path backup = PathUtils.append(bundleFile, ".bak");
        Files.move(bundleFile, backup, StandardCopyOption.REPLACE_EXISTING);
        try {
            saveUnityWeb(bundle, bundleFile, getHeader(bundle));
            verifyLayout(clientBundle, bundleFile);
            verifyUnityWeb(bundleFile, Files.size(clientBundle));
            Files.deleteIfExists(backup);
            System.out.println("[info] bundle-inject-preserve OK: " + bundleFile);
            System.out.println("[info] size: " + Files.size(bundleFile));
        } catch (Exception ex) {
            Files.move(backup, bundleFile, StandardCopyOption.REPLACE_EXISTING);
            throw ex;
        }
    }

    private static void injectPaddedEntries(AssetBundle bundle, Path bundleDir) throws IOException {
        String[] list = bundleDir.toFile().list();
        if (list == null) {
            throw new IOException("Empty extract dir: " + bundleDir);
        }
        Map<String, ByteBuffer> entries = bundle.getEntries();
        for (String entryName : list) {
            Path entryFile = bundleDir.resolve(entryName);
            if (!Files.isRegularFile(entryFile)) {
                continue;
            }
            ByteBuffer data = ByteBufferUtils.openReadOnly(entryFile);
            if (entries.containsKey(entryName)) {
                System.out.println("[info] Replacing " + entryName);
                entries.replace(entryName, data);
            } else {
                System.out.println("[info] Inserting " + entryName);
                entries.put(entryName, data);
            }
        }
    }

    private static void saveUnityWeb(AssetBundle bundle, Path file, AssetBundleHeader header) throws Exception {
        header.getClass().getMethod("setSignature", String.class)
                .invoke(header, AssetBundleHeader.SIGNATURE_WEB);
        bundle.setCompressed(true);
        bundle.save(file);
        System.out.println("[info] Saved UnityWeb bundle: " + file);
    }

    private static void verifyUnityWeb(Path bundleFile, long clientSize) throws IOException {
        byte[] head = Files.readAllBytes(bundleFile);
        int end = 0;
        while (end < head.length && head[end] != 0) {
            end++;
        }
        String sig = new String(head, 0, end, "US-ASCII");
        if (!AssetBundleHeader.SIGNATURE_WEB.equals(sig)) {
            throw new IOException("expected UnityWeb signature, got " + sig);
        }
        long patchedSize = Files.size(bundleFile);
        long minSize = (long) (clientSize * 0.98);
        if (patchedSize < minSize) {
            throw new IOException("UnityWeb size " + patchedSize + " below 98% of client " + clientSize);
        }
        System.out.println("[info] UnityWeb OK: size=" + patchedSize + " (client=" + clientSize + ")");
    }

    private static Map<String, Integer> loadOriginalSizes(Path clientBundle, Path clientDir)
            throws IOException, InterruptedException {
        if (!Files.isDirectory(clientDir)) {
            extractBundle(clientBundle);
        }

        Map<String, Integer> sizes = new LinkedHashMap<>();
        for (Path entry : Files.list(clientDir).toArray(Path[]::new)) {
            if (!Files.isRegularFile(entry)) {
                continue;
            }
            sizes.put(entry.getFileName().toString(), (int) Files.size(entry));
        }
        return sizes;
    }

    private static void padEntries(Path bundleDir, Map<String, Integer> originalSizes) throws IOException {
        for (Map.Entry<String, Integer> slot : originalSizes.entrySet()) {
            String name = slot.getKey();
            int targetSize = slot.getValue();
            Path entryFile = bundleDir.resolve(name);
            if (!Files.isRegularFile(entryFile)) {
                throw new IOException("Missing inject entry: " + name);
            }

            byte[] data = Files.readAllBytes(entryFile);
            if (data.length > targetSize) {
                throw new IOException(name + " patched size " + data.length + " exceeds client slot " + targetSize);
            }
            if (data.length < targetSize) {
                byte[] padded = new byte[targetSize];
                System.arraycopy(data, 0, padded, 0, data.length);
                Files.write(entryFile, padded);
                System.out.println("[info] Padded " + name + " " + data.length + " -> " + targetSize);
            } else {
                System.out.println("[info] " + name + " fits slot (" + targetSize + ")");
            }
        }
    }

    private static void extractBundle(Path bundle) throws IOException, InterruptedException {
        ProcessBuilder pb = new ProcessBuilder(
                disunityBat(),
                "bundle-extract",
                bundle.toString());
        pb.redirectErrorStream(true);
        Process p = pb.start();
        int code = p.waitFor();
        if (code != 0) {
            throw new IOException("bundle-extract failed for " + bundle + " exit=" + code);
        }
    }

    private static void verifyLayout(Path clientBundle, Path patchedBundle) throws Exception {
        AssetBundle client = new AssetBundle();
        client.load(ByteBufferUtils.openReadOnly(clientBundle));
        AssetBundle patched = new AssetBundle();
        patched.load(ByteBufferUtils.openReadOnly(patchedBundle));

        int clientU = lastUncompressed(getHeader(client));
        int patchedU = lastUncompressed(getHeader(patched));
        if (clientU != patchedU) {
            throw new IOException("uncompressed layout mismatch: client u=" + clientU + " patched u=" + patchedU);
        }

        System.out.println("[info] layout OK: u=" + patchedU + " (matches client)");
    }

    private static AssetBundleHeader getHeader(AssetBundle bundle) throws Exception {
        Field f = AssetBundle.class.getDeclaredField("header");
        f.setAccessible(true);
        return (AssetBundleHeader) f.get(bundle);
    }

    private static int lastUncompressed(AssetBundleHeader header) {
        int u = 0;
        for (Pair<Integer, Integer> p : header.getOffsetMap()) {
            u = p.getRight();
        }
        return u;
    }

    private static String disunityBat() {
        Path root = Paths.get("D:/work/roberto/tools/DisunityFF/disunity.bat");
        return root.toString();
    }
}
