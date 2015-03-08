using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class InstrumentationPacket : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.Instrumentation; } }

        public int Address { get; private set; }
        public int PayloadSize { get; private set; }
        public int Value { get; private set; }

        public InstrumentationPacket(int address, int payloadSize, int value)
        {
            Address = address;
            PayloadSize = payloadSize;
            Value = value;
        }
    }
}
