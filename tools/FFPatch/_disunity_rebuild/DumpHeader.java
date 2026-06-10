import info.ata4.unity.asset.bundle.AssetBundle;
import info.ata4.io.buffer.ByteBufferUtils;
import info.ata4.unity.asset.bundle.struct.AssetBundleHeader;
import org.apache.commons.lang3.tuple.Pair;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;

public class DumpHeader {
    public static void main(String[] args) throws Exception {
        for (String p : args) {
            AssetBundle b = new AssetBundle();
            b.load(ByteBufferUtils.openReadOnly(Paths.get(p)));
            AssetBundleHeader h = b.getHeader();
            System.out.println("=== " + p);
            System.out.println("fileSize1=" + h.getFileSize1() + " fileSize2=" + h.getFileSize2());
            System.out.println("dataOffset=" + h.getDataOffset() + " unknown1=" + h.getUnknown1() + " unknown2=" + h.getUnknown2());
            List<Pair<Integer,Integer>> m = h.getOffsetMap();
            System.out.println("offsetMap.size=" + m.size());
            for (int i=0;i<m.size();i++) System.out.println("  ["+i+"] "+m.get(i));
            System.out.println("compressed=" + b.isCompressed());
            System.out.println("outerSize=" + java.nio.file.Files.size(Paths.get(p)));
        }
    }
}
