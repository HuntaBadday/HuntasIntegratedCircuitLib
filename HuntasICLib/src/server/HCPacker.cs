using System;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

namespace HuntasICLib;

// A class used to pack multiple byte[] into one
public class HCPacker : IDisposable {
    MemoryStream memstream;
    
    public HCPacker() {
        memstream = new MemoryStream();
    }
    
    public void Write(byte[] data) {
        BinaryWriter w = new BinaryWriter(memstream, Encoding.UTF8, true);
        w.Write(data.Length);
        w.Write(data);
    }
    
    public byte[] ToArray() {
        return memstream.ToArray();
    }
    
    public void Dispose(bool disposing) {
        if (disposing)
            memstream.Dispose();
    }

    public void Dispose() {
        Dispose(true);
    }
}

public class HCUnpacker : IDisposable {
    MemoryStream memstream;
    
    public HCUnpacker(byte[] packedData) {
        memstream = new MemoryStream(packedData);
    }
    
    public byte[] ReadNext() {
        BinaryReader r = new BinaryReader(memstream, Encoding.UTF8, true);
        
        try {
            int size = r.ReadInt32();
            return r.ReadBytes(size);
        } catch (EndOfStreamException) {
            return null;
        }
    }
    
    public void Dispose(bool disposing) {
        if (disposing)
            memstream.Dispose();
    }

    public void Dispose() {
        Dispose(true);
    }
}