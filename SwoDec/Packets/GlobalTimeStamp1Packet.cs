using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class GlobalTimeStamp1Packet : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.GlobalTimeStamp1; } }

        public bool ClockChange { get; private set; }
        public bool Wrap { get; private set; }
        public int Value { get; private set; }
    }
}
