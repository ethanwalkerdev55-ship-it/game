"""Compare UnityWeb bundle headers (vanilla vs patched)."""
import struct
import sys
from pathlib import Path


def align4(n):
    return (n + 3) & ~3


def read_cstring(data, off):
    end = data.index(0, off)
    return data[off:end].decode("ascii", "replace"), end + 1


def parse_header(path):
    data = Path(path).read_bytes()
    off = 0
    sig, off = read_cstring(data, off)
    if sig != "UnityWeb":
        raise ValueError(f"expected UnityWeb, got {sig!r}")
    fmt = struct.unpack_from("<i", data, off)[0]
    off += 4
    ver_player, off = read_cstring(data, off)
    ver_engine, off = read_cstring(data, off)
    file_size1, data_offset, unknown1 = struct.unpack_from("<iii", data, off)
    off += 12
    map_count = struct.unpack_from("<i", data, off)[0]
    off += 4
    offset_map = []
    for _ in range(map_count):
        c, u = struct.unpack_from("<ii", data, off)
        off += 8
        offset_map.append((c, u))
    file_size2 = None
    unknown2 = None
    if fmt >= 2:
        file_size2 = struct.unpack_from("<i", data, off)[0]
        off += 4
    if fmt >= 3:
        unknown2 = struct.unpack_from("<i", data, off)[0]
        off += 4
    if off < len(data):
        off += 1  # trailing byte per DisUnity header
    return {
        "path": str(path),
        "file_len": len(data),
        "header_end": off,
        "data_offset": data_offset,
        "sig": sig,
        "format": fmt,
        "player": ver_player,
        "engine": ver_engine,
        "file_size1": file_size1,
        "unknown1": unknown1,
        "map_count": map_count,
        "offset_map": offset_map,
        "file_size2": file_size2,
        "unknown2": unknown2,
        "lzma_props": data[data_offset : data_offset + 5].hex(),
        "header_matches_data_offset": off == data_offset,
    }


def main():
    a = parse_header(sys.argv[1])
    b = parse_header(sys.argv[2])
    keys = [
        "file_len",
        "header_end",
        "data_offset",
        "file_size1",
        "unknown1",
        "map_count",
        "offset_map",
        "file_size2",
        "lzma_props",
    ]
    for k in keys:
        va, vb = a[k], b[k]
        mark = "==" if va == vb else "!="
        print(f"{k}: {mark}")
        if va != vb:
            print(f"  A: {va}")
            print(f"  B: {vb}")


if __name__ == "__main__":
    main()
