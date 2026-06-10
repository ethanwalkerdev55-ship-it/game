import info.ata4.io.buffer.ByteBufferUtils;
import info.ata4.io.util.PathUtils;
import info.ata4.unity.asset.bundle.AssetBundle;
import info.ata4.unity.asset.bundle.struct.AssetBundleHeader;
import java.nio.ByteBuffer;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.Map;

/**
 * Injects extracted bundle files and saves as UnityRaw (uncompressed).
 * FusionFall bootstrap rejects LZMA recompressed UnityWeb bundles after any DLL edit;
 * round-trip without edits is bit-identical. Raw inject avoids recompression.
 */
public class BundleInjectRaw {
    public static void main(String[] args) throws Exception {
        if (args.length < 1) {
            System.err.println("Usage: BundleInjectRaw <bundle.unity3d>");
            System.exit(1);
        }

        Path bundleFile = Paths.get(args[0]).toAbsolutePath();
        Path bundleDir = PathUtils.removeExtension(bundleFile);

        if (!Files.exists(bundleFile)) {
            System.err.println("Missing bundle: " + bundleFile);
            System.exit(1);
        }
        if (!Files.isDirectory(bundleDir)) {
            System.err.println("Missing extract dir: " + bundleDir);
            System.exit(1);
        }

        AssetBundle bundle = new AssetBundle();
        bundle.load(ByteBufferUtils.openReadOnly(bundleFile));

        String[] list = bundleDir.toFile().list();
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

        bundle.setCompressed(false);
        bundle.getClass(); // keep compiler happy if header accessor missing

        Path backup = PathUtils.append(bundleFile, ".bak");
        Files.move(bundleFile, backup, StandardCopyOption.REPLACE_EXISTING);

        try {
            saveRaw(bundle, bundleFile);
            System.out.println("[info] Saved UnityRaw bundle: " + bundleFile);
            System.out.println("[info] Size: " + Files.size(bundleFile));
        } catch (Exception ex) {
            Files.move(backup, bundleFile, StandardCopyOption.REPLACE_EXISTING);
            throw ex;
        }
    }

    private static void saveRaw(AssetBundle bundle, Path file) throws Exception {
        // AssetBundle.save() always LZMA-compresses UnityWeb bundles. Use reflection to
        // force UnityRaw signature on the header, then call save().
        Object header = getField(bundle, "header");
        header.getClass().getMethod("setSignature", String.class)
                .invoke(header, AssetBundleHeader.SIGNATURE_RAW);
        bundle.setCompressed(false);
        bundle.save(file);
    }

    private static Object getField(Object target, String name) throws Exception {
        java.lang.reflect.Field f = target.getClass().getDeclaredField(name);
        f.setAccessible(true);
        return f.get(target);
    }
}
