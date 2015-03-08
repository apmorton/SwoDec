using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class UnknownPacket : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.Unkown; } }

        public byte Value { get; private set; }

        public UnknownPacket(byte value)
        {
            Value = value;
        }
    }
}
