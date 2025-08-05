using System;
using System.Collections.Generic;
namespace HuntasICLib.Serial;

// A class to send TNET packets
public class TNETTransmitter {
    private const int MODE_IDLE = 0;
    private const int MODE_SEND_START = 1;
    private const int MODE_BYTE_SEND= 2;
    private const int MODE_SEND_IPG = 3;
    private int currentMode;
    
    private int bitCounter;
    private byte currentByte; // Current byte to send
    
    private byte[] sendBuffer; // Current buffer for sending
    private int sendIndex; // Buffer index
    
    private List<byte[]> packetStack = new List<byte[]>(); // All packets to send
    
    public void Reset(){
        currentMode = MODE_SEND_IPG;
        bitCounter = 0;
        packetStack.Clear();
    }
    
    // Update logic
    public bool LogicUpdate() {
        bool output = false;
        switch (currentMode) {
            case MODE_IDLE: {
                if (packetStack.Count > 0) {
                    sendBuffer = packetStack[0];
                    packetStack.RemoveAt(0);
                    currentMode = MODE_BYTE_SEND;
                    bitCounter = 0;
                    sendIndex = 0;
                    output = true;
                    
                    currentByte = sendBuffer[sendIndex++];
                }
                break;
            }
            case MODE_SEND_START: {
                output = true;
                currentMode = MODE_BYTE_SEND;
                bitCounter = 0;
                currentByte = sendBuffer[sendIndex++];
                break;
            }
            case MODE_BYTE_SEND: {
                output = (currentByte & 1) != 0;
                currentByte >>= 1;
                if (++bitCounter == 8) {
                    if (sendIndex == sendBuffer.Length) {
                        currentMode = MODE_SEND_IPG;
                        bitCounter = 0;
                    } else {
                        currentMode = MODE_SEND_START;
                    }
                }
                break;
            }
            case MODE_SEND_IPG: {
                if (++bitCounter == 12) {
                    currentMode = MODE_IDLE;
                }
                break;
            }
        }
        return output;
    }
    
    // Send a byte array over TNET
    public void Send(byte[] data) {
        if (data.Length > 1020 || data.Length == 0) return;
        
        uint checksum = 0;
        for (int i = 0; i < data.Length; i++) {
            checksum += data[i];
        }
        
        byte[] newData = new byte[data.Length+4];
        Buffer.BlockCopy(data, 0, newData, 0, data.Length);
        
        newData[^4] = (byte)(checksum & 0xff);
        newData[^3] = (byte)(checksum >> 8 & 0xff);
        newData[^2] = (byte)(checksum >> 16 & 0xff);
        newData[^1] = (byte)(checksum >> 24 & 0xff);
        
        packetStack.Add(newData);
    }
}