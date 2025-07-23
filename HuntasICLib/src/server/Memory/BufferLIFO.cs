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
}