using System;
using System.Linq;

namespace SwoDec.Packets
{
    public class SyncPacket : ISwoPacket
    {
        public SwoPacketType Type { get { return SwoPacketType.Sync; } }

        public SyncPacket()
        {
        }
    }
}
