using System;
using System.IO;

namespace HuntasICLib.Serial;

public class SerialTransmitter {
    int bits;
    
    ulong valueToSend;
    
    // True when no data is being sent
    public bool ready {get; private set;}
    
    public int state {get; private set;}
    // 0: Idle
    // 1+: Bit being transmitted
    
    // Initialize transmitter with specified bit width from 0 to 64 bits
    public SerialTransmitter(int bitWidth) {
        if (bitWidth > 64 || bitWidth <= 0) {
            throw new ArgumentOutOfRangeException();
        }
        bits = bitWidth;
        ready = true;
        state = 0;
    }
    
    // Update logic
    // Return value specifies the output state of the transmitter
    public bool LogicUpdate() {
        if (ready) {
            return false;
        }
        
        if (state == 0) {
            state++;
            return true;
        }
        
        bool output = (valueToSend & 1) != 0;
        valueToSend >>= 1;
        
        if (state++ == bits) ready = true;
        
        return output;
    }
    
    // Transmit a value. Returns true if value was accepted
    public bool Transmit(ulong value) {
        if (ready) {
            state = 0;
            ready = false;
            valueToSend = value;
            return true;
        }
        return false;
    }
    
    // Reset internal state
    public void Reset() {
        ready = true;
        state = 0;
    }
    
    public byte[] Serialize() {
        MemoryStream m = new MemoryStream();
        BinaryWriter w = new BinaryWriter(m);
        w.Write(bits);
        w.Write(valueToSend);
        w.Write(ready);
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
            valueToSend = r.ReadUInt64();
            ready = r.ReadBoolean();
            state = r.ReadInt32();
        } catch {}
    }
}