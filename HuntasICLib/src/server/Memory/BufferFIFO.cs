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
}