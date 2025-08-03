using System;

namespace HuntasICLib.Serial;

class SerialTransmitter {
    readonly int bits;
    
    ulong valueToSend;
    
    public bool ready {get; private set;}
    
    public int state = 0;
    // 0: Idle
    // 1+: Bit being transmitted
    
    public SerialTransmitter(int bitWidth) {
        if (bitWidth > 64 || bitWidth <= 0) {
            throw new ArgumentOutOfRangeException();
        }
        bits = bitWidth;
        ready = true;
    }
    
    public bool LogicUpdate() {
        if (ready) {
            return false;
        }
        
        bool output = (valueToSend & 1) != 0;
        valueToSend >>= 1;
        
        if (++state == bits) ready = true;
        
        return output;
    }
    
    public bool Transmit(ulong value) {
        if (ready) {
            state = 0;
            ready = false;
            valueToSend = value;
            return true;
        }
        return false;
    }
}