namespace HuntasICLib.Memory;

public class Ram8b {
    // IO
    public byte dataIn; // Data input
    public byte dataOut; // Data output
    public uint address; // Address input
    public bool readPin; // Read pin state
    public bool writePin; // Write pin state
    public bool enablePin; // Chip select enable
    
    byte[] memory;
    
    public Ram8b(int size) {
        memory = new byte[size];
    }
    
    public void LogicUpdate() {
        if (enablePin && readPin) {
            dataOut = memory[address];
        } else {
            dataOut = 0;
        }
        
        if (enablePin && writePin) {
            memory[address] = dataIn;
        }
    }
    
    public void LoadData(byte[] data) {
        int loadSize = data.Length > memory.Length ? memory.Length : data.Length;
        Buffer.BlockCopy(data, 0, memory, 0, loadSize);
    }
    
    public byte[] SerializeState() {
        MemoryStream ms = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(ms);
        
        writer.Write(dataIn);
        writer.Write(dataOut);
        writer.Write(address);
        writer.Write(readPin);
        writer.Write(writePin);
        writer.Write(enablePin);
        writer.Write(memory.Length);
        writer.Write(memory);
        
        return ms.ToArray();
    }
    
    public void DeserializeState(byte[] data) {
        MemoryStream ms = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(ms);
        
        dataIn = reader.ReadByte();
        dataOut = reader.ReadByte();
        address = reader.ReadUInt32();
        readPin = reader.ReadBoolean();
        writePin = reader.ReadBoolean();
        enablePin = reader.ReadBoolean();
        
        int s = reader.Read();
        memory = reader.ReadBytes(s);
    }
}

public class Ram16b {
    // IO
    public ushort dataIn; // Data input
    public ushort dataOut; // Data output
    public uint address; // Address input
    public bool readPin; // Read pin state
    public bool writePin; // Write pin state
    public bool enablePin; // Chip select enable
    
    ushort[] memory;
    
    public Ram16b(int size) {
        memory = new ushort[size];
    }
    
    public void LogicUpdate() {
        if (enablePin && readPin) {
            dataOut = memory[address];
        } else {
            dataOut = 0;
        }
        
        if (enablePin && writePin) {
            memory[address] = dataIn;
        }
    }
    
    public void LoadData(byte[] data) {
        int loadSize = data.Length > memory.Length ? memory.Length : data.Length;
        Buffer.BlockCopy(data, 0, memory, 0, loadSize);
    }
    
    public byte[] SerializeState() {
        MemoryStream ms = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(ms);
        
        writer.Write(dataIn);
        writer.Write(dataOut);
        writer.Write(address);
        writer.Write(readPin);
        writer.Write(writePin);
        writer.Write(enablePin);
        writer.Write(memory.Length);
        
        byte[] mc = new byte[memory.Length*2];
        Buffer.BlockCopy(memory, 0, mc, 0, mc.Length);
        
        writer.Write(mc);
        
        return ms.ToArray();
    }
    
    public void DeserializeState(byte[] data) {
        MemoryStream ms = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(ms);
        
        dataIn = reader.ReadByte();
        dataOut = reader.ReadByte();
        address = reader.ReadUInt32();
        readPin = reader.ReadBoolean();
        writePin = reader.ReadBoolean();
        enablePin = reader.ReadBoolean();
        
        int s = reader.Read();
        memory = new ushort[s];
        
        byte[] mc = reader.ReadBytes(s*2);
        Buffer.BlockCopy(mc, 0, memory, 0, mc.Length);
    }
}