import info.ata4.io.buffer.ByteBufferUtils;
import info.ata4.unity.asset.bundle.AssetBundle;
import info.ata4.unity.asset.bundle.struct.AssetBundleHeader;
import java.lang.reflect.Field;
import java.nio.ByteBuffer;
import java.nio.file.Paths;
import org.apache.commons.lang3.tuple.Pair;

public class BundleHeaderDump {
    public static void main(String[] args) throws Exception {
        if (args.length < 1) {
            System.err.println("Usage: BundleHeaderDump <bundle.unity3d>");
            System.exit(1);
        }
        AssetBundle bundle = new AssetBundle();
        bundle.load(ByteBufferUtils.openReadOnly(Paths.get(args[0])));
        AssetBundleHeader header = getHeader(bundle);
        System.out.println("file=" + args[0]);
        System.out.println("compressed=" + bundle.isCompressed());
        System.out.println("format=" + header.getFormat());
        System.out.println("fileSize1=" + header.getFileSize1());
        System.out.println("dataOffset=" + header.getDataOffset());
        System.out.println("unknown1=" + header.getUnknown1());
        System.out.println("fileSize2=" + header.getFileSize2());
        System.out.println("unknown2=" + header.getUnknown2());
        System.out.println("offsetMap:");
        int lastU = 0;
        for (Pair<Integer, Integer> p : header.getOffsetMap()) {
            System.out.println("  c=" + p.getLeft() + " u=" + p.getRight());
            lastU = p.getRight();
        }
        System.out.println("lastU=" + lastU);
    }

    private static AssetBundleHeader getHeader(AssetBundle bundle) throws Exception {
        Field f = AssetBundle.class.getDeclaredField("header");
        f.setAccessible(true);
        return (AssetBundleHeader) f.get(bundle);
    }
}
