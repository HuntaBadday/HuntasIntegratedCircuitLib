using System;
using System.IO;
namespace HuntasICLib.Memory;

public class BufferFIFO8b {
    public bool isFull = false;
    public bool dataAvailable = false;
    
    byte[] memory;
    
    int writeVec;
    int readVec;
    
    // Initialize buffer with specified size
    public BufferFIFO8b(int size) {
        memory = new byte[size];
    }
    
    // Reset buffer (Clears)
    public void Reset() {
        writeVec = 0;
        readVec = 0;
        isFull = false;
        dataAvailable = false;
    }
    
    // Write data to the buffer
    public void Write(byte data) {
        if (isFull) return;
        memory[writeVec++] = data;
        writeVec %= memory.Length;
        dataAvailable = true;
        if (writeVec == readVec) isFull = true;
    }
    
    // Read data from the buffer
    public byte Read() {
        if (writeVec == readVec && !isFull) return 0;
        byte output = memory[readVec++];
        readVec %= memory.Length;
        isFull = false;
        if (writeVec == readVec) dataAvailable = false;
        return output;
    }
    
    public byte[] Serialize() {
        MemoryStream m = new MemoryStream();
        BinaryWriter w = new BinaryWriter(m);
        
        w.Write(writeVec);
        w.Write(readVec);
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
            
            writeVec = r.ReadInt32();
            readVec = r.ReadInt32();
            isFull = r.ReadBoolean();
            dataAvailable = r.ReadBoolean();
            
            byte[] mem = r.ReadBytes(memory.Length);
            if (mem.Length == memory.Length) {
                Buffer.BlockCopy(mem, 0, memory, 0, memory.Length);
            }
        } catch {}
    }
}

// Same as BufferFIFO8b except uses ushort for data
public class BufferFIFO16b {
    public bool isFull = false;
    public bool dataAvailable = false;
    
    ushort[] memory;
    
    int writeVec;
    int readVec;
    
    public BufferFIFO16b(int size) {
        memory = new ushort[size];
    }
    
    public void Reset() {
        writeVec = 0;
        readVec = 0;
        isFull = false;
        dataAvailable = false;
    }
    
    public void Write(ushort data) {
        if (isFull) return;
        memory[writeVec++] = data;
        writeVec %= memory.Length;
        dataAvailable = true;
        if (writeVec == readVec) isFull = true;
    }
    
    public ushort Read() {
        if (writeVec == readVec && !isFull) return 0;
        ushort output = memory[readVec++];
        readVec %= memory.Length;
        isFull = false;
        if (writeVec == readVec) dataAvailable = false;
        return output;
    }
    
    public byte[] Serialize() {
        MemoryStream m = new MemoryStream();
        BinaryWriter w = new BinaryWriter(m);
        
        w.Write(writeVec);
        w.Write(readVec);
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
            
            writeVec = r.ReadInt32();
            readVec = r.ReadInt32();
            isFull = r.ReadBoolean();
            dataAvailable = r.ReadBoolean();
            
            byte[] mem = r.ReadBytes(memory.Length*2);
            if (mem.Length == memory.Length*2) {
                Buffer.BlockCopy(mem, 0, memory, 0, mem.Length);
            }
        } catch {}
    }
}