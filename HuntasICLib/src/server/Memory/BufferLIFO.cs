using System;
using System.IO;
namespace HuntasICLib.Memory;

public class BufferLIFO8b {
    public bool isFull = false;
    public bool dataAvailable = false;
    
    byte[] memory;
    
    int ptr;
    
    public BufferLIFO8b(int size) {
        memory = new byte[size];
    }
    
    public void Reset() {
        ptr = 0;
        isFull = false;
        dataAvailable = false;
    }
    
    public void Write(byte data) {
        if (isFull) return;
        memory[ptr++] = data;
        if (ptr == memory.Length) isFull = true;
        dataAvailable = true;
    }
    
    public byte Read() {
        if (ptr == 0) return 0;
        isFull = false;
        if (--ptr == 0) dataAvailable = false;
        return memory[ptr];
    }
    
    public byte[] Serialize() {
        MemoryStream m = new MemoryStream();
        BinaryWriter w = new BinaryWriter(m);
        
        w.Write(ptr);
        w.Write(isFull);
        w.Write(dataAvailable);
        w.Write(memory);
        
        return m.ToArray();
    }
    
    public void Deserialize(byte[] data) {
        if (data == null) return;
        try {
            MemoryStream m = new MemoryStream(data);
            BinaryReader r = new BinaryReader(m);
            
            ptr = r.ReadInt32();
            isFull = r.ReadBoolean();
            dataAvailable = r.ReadBoolean();
            
            byte[] mem = r.ReadBytes(memory.Length);
            if (mem.Length == memory.Length) {
                Buffer.BlockCopy(mem, 0, memory, 0, mem.Length);
            }
        } catch {}
    }
}

public class BufferLIFO16b {
    public bool isFull = false;
    public bool dataAvailable = false;
    
    ushort[] memory;
    
    int ptr;
    
    public BufferLIFO16b(int size) {
        memory = new ushort[size];
    }
    
    public void Reset() {
        ptr = 0;
        isFull = false;
        dataAvailable = false;
    }
    
    public void Write(ushort data) {
        if (isFull) return;
        memory[ptr++] = data;
        if (ptr == memory.Length) isFull = true;
        dataAvailable = true;
    }
    
    public ushort Read() {
        if (ptr == 0) return 0;
        isFull = false;
        if (--ptr == 0) dataAvailable = false;
        return memory[ptr];
    }
    
    public byte[] Serialize() {
        MemoryStream m = new MemoryStream();
        BinaryWriter w = new BinaryWriter(m);
        
        w.Write(ptr);
        w.Write(isFull);
        w.Write(dataAvailable);
        
        byte[] mem = new byte[memory.Length*2];
        Buffer.BlockCopy(memory, 0, mem, 0, mem.Length);
        w.Write(mem);
        
        return m.ToArray();
    }
    
    public void Deserialize(byte[] data) {
        if (data == null) return;
        try {
            MemoryStream m = new MemoryStream(data);
            BinaryReader r = new BinaryReader(m);
            
            ptr = r.ReadInt32();
            isFull = r.ReadBoolean();
            dataAvailable = r.ReadBoolean();
            
            byte[] mem = r.ReadBytes(memory.Length*2);
            if (mem.Length == memory.Length*2) {
                Buffer.BlockCopy(mem, 0, memory, 0, mem.Length);
            }
        } catch {}
    }
}