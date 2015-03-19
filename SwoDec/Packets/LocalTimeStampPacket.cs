using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class LocalTimeStampPacket : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.LocalTimeStamp; } }

        public SwoLtsRelation Relation { get; private set; }
        public int Value { get; private set; }

        public LocalTimeStampPacket(int value, SwoLtsRelation relation)
        {
            Value = value;
            Relation = relation;
        }
    }
}
