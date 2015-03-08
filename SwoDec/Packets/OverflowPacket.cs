using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class OverflowPacket : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.Overflow; } }
    }
}
