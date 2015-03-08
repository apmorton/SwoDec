using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class GlobalTimeStamp2Packet : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.GlobalTimeStamp2; } }

        public int Value { get; private set; }
    }
}
