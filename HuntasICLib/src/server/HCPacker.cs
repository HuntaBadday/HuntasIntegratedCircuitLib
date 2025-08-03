using System.Text;
using System.IO;

namespace HuntasICLib;

// A class used to pack multiple byte[] into one
public static class HCPacker {
    public static void Write(MemoryStream stream, byte[] data) {
        BinaryWriter w = new BinaryWriter(stream, Encoding.UTF8, true);
        w.Write(data.Length);
        w.Write(data);
    }
    
    public static byte[] ReadNext(MemoryStream stream) {
        BinaryReader r = new BinaryReader(stream, Encoding.UTF8, true);
        
        try {
            int size = r.ReadInt32();
            return r.ReadBytes(size);
        } catch (EndOfStreamException) {
            return null;
        }
    }
}