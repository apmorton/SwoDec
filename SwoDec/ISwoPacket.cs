using System;

namespace SwoDec
{
    public interface ISwoPacket
    {
        SwoPacketType Type { get; }
    }
}
