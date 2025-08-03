using System;

namespace HuntasICLib.Serial;

class SerialTransmitter {
    readonly int bits;
    
    ulong valueToSend;
    
    // True when no data is being sent
    public bool ready {get; private set;}
    
    public int state = 0;
    // 0: Idle
    // 1+: Bit being transmitted
    
    // Initialize transmitter with specified bit width from 0 to 64 bits
    public SerialTransmitter(int bitWidth) {
        if (bitWidth > 64 || bitWidth <= 0) {
            throw new ArgumentOutOfRangeException();
        }
        bits = bitWidth;
        ready = true;
    }
    
    // Update logic
    // Return value specifies the output state of the transmitter
    public bool LogicUpdate() {
        if (ready) {
            return false;
        }
        
        bool output = (valueToSend & 1) != 0;
        valueToSend >>= 1;
        
        if (++state == bits) ready = true;
        
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
}