using System;
using System.Collections.Generic;
namespace HuntasICLib.Serial;

// A class to receive TNET packets
public class TNETReceiver {
    private const int MODE_IDLE = 0;
    private const int MODE_BYTE_RECV = 1;
    private const int MODE_IPG_WAIT = 2;
    private const int MODE_FAIL_WAIT = 3;
    private int currentMode;
    
    private int bitCounter;
    private byte currentByte; // Current byte to receive
    
    private byte[] receiveBuffer = new byte[1024]; // Current buffer for receiving
    private int receiveIndex; // Buffer index
    
    private List<byte[]> packetStack = new List<byte[]>(); // All packets received
    
    public void Reset(){
        currentMode = MODE_IPG_WAIT;
        bitCounter = 0;
        packetStack.Clear();
    }
    
    // Update logic
    public void LogicUpdate(bool pinState) {
        switch (currentMode) {
            case MODE_IDLE: {
                if (pinState) {
                    currentMode = MODE_BYTE_RECV;
                    receiveIndex = 0;
                    bitCounter = 0;
                }
                break;
            }
            case MODE_BYTE_RECV: {
                currentByte >>= 1;
                if (pinState) currentByte |= 0x80;
                if (++bitCounter == 8) {
                    receiveBuffer[receiveIndex++] = currentByte;
                    currentMode = MODE_IPG_WAIT;
                    bitCounter = 0;
                }
                break;
            }
            case MODE_IPG_WAIT: {
                if (!pinState) {
                    if (++bitCounter == 12) {
                        ProcessReceivedPacket();
                        currentMode = MODE_IDLE;
                    }
                } else if (receiveIndex < 1024) {
                    currentMode = MODE_BYTE_RECV;
                    bitCounter = 0;
                } else {
                    currentMode = MODE_FAIL_WAIT;
                    bitCounter = 0;
                }
                break;
            }
            case MODE_FAIL_WAIT: {
                if (!pinState) {
                    if (++bitCounter == 12) {
                        currentMode = MODE_IDLE;
                    }
                } else {
                    bitCounter = 0;
                }
                break;
            }
        }
    }
    
    // Get the next packet in the packet queue
    public byte[] GetNextPacket() {
        if (packetStack.Count > 0) {
            byte[] packet = packetStack[0];
            packetStack.RemoveAt(0);
            return packet;
        }
        return null;
    }
    
    // Check if there is an available packet
    public bool Available() {
        return packetStack.Count > 0;
    }
    
    private void ProcessReceivedPacket() {
        if (receiveIndex < 5) return;
        
        uint sum = 0;
        for (int i = 0; i < receiveIndex-4; i++) {
            sum += receiveBuffer[i];
        }
        
        uint checksum = 0;
        checksum |= receiveBuffer[receiveIndex-4];
        checksum |= (uint)receiveBuffer[receiveIndex-3]<<8;
        checksum |= (uint)receiveBuffer[receiveIndex-2]<<16;
        checksum |= (uint)receiveBuffer[receiveIndex-1]<<24;
        
        if (sum == checksum) {
            byte[] output = new byte[receiveIndex-4];
            Buffer.BlockCopy(receiveBuffer, 0, output, 0, receiveIndex-4);
        }
    }
}