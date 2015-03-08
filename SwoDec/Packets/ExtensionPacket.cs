using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class ExtensionPacket : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.Extension; } }

        public SwoExtSource Source { get; private set; }
        public int Value { get; private set; }

        public ExtensionPacket(SwoExtSource source, int value)
        {
            Source = source;
            Value = value;
        }
    }
}
