using System;

namespace HuntasICLib.Serial;

class SerialReceiver {
    readonly int bits;
    
    public ulong receivedValue {get; private set;}
    
    int state = 0;
    // 0: Idle
    // 1+: Bit being received
    
    public SerialReceiver(int bitWidth) {
        if (bitWidth > 64 || bitWidth <= 0) {
            throw new ArgumentOutOfRangeException();
        }
        bits = bitWidth;
    }
    
    public bool LogicUpdate(bool pinState) {
        if (state == 0) {
            if (pinState) {
                state = 1;
                receivedValue = 0;
            }
        } else {
            receivedValue |= pinState ? (ulong)(1<<(state-1)) : 0;
            if (state++ == bits) {
                state = 0;
                return true;
            }
        }
        return false;
    }
}