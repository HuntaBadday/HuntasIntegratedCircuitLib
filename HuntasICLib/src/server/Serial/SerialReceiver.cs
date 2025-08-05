using System;
using System.IO;

namespace HuntasICLib.Serial;

public class SerialReceiver {
    int bits;
    
    // Contains the received value
    public ulong value {get; private set;}
    public bool available = false;
    
    public int state {get; private set;}
    // 0: Idle
    // 1+: Bit being received
    
    // Initialize receiver with specified bit width from 1 to 64 bits
    public SerialReceiver(int bitWidth) {
        if (bitWidth > 64 || bitWidth <= 0) {
            throw new ArgumentOutOfRangeException();
        }
        bits = bitWidth;
        state = 0;
    }
    
    // Update the logic with specified input state
    // Returns true when value is received
    public bool LogicUpdate(bool pinState) {
        available = false;
        if (state == 0) {
            if (pinState) {
                state = 1;
                value = 0;
            }
        } else {
            value |= pinState ? (ulong)(1<<(state-1)) : 0;
            if (state++ == bits) {
                state = 0;
                available = true;
                return true;
            }
        }
        return false;
    }
    
    public byte[] Serialize() {
        MemoryStream m = new MemoryStream();
        BinaryWriter w = new BinaryWriter(m);
        w.Write(bits);
        w.Write(value);
        w.Write(state);
        return m.ToArray();
    }
    
    public void Deserialize(byte[] data) {
        if (data == null) {
            return;
        }
        
        try {
            MemoryStream m = new MemoryStream(data);
            BinaryReader r = new BinaryReader(m);
            bits = r.ReadInt32();
            value = r.ReadUInt64();
            state = r.ReadInt32();
        } catch {}
    }
}