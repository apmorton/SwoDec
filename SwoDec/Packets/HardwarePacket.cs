using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class HardwarePacket : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.Hardware; } }

        public int Address { get; private set; }
        public int PayloadSize { get; private set; }
        public int Value { get; private set; }

        public HardwarePacket(int address, int payloadSize, int value)
        {
            Address = address;
            PayloadSize = payloadSize;
            Value = value;
        }
    }
}
