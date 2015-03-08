using System;

namespace SwoDec
{
    public enum SwoPacketType
    {
        Sync,
        Overflow,
        LocalTimeStamp,
        GlobalTimeStamp1,
        GlobalTimeStamp2,
        Extension,
        Instrumentation,
        Hardware,
        Unkown
    }

    public enum SwoLtsRelation
    {
        Synchronous,
        RelativeTimeStamp,
        RelativeSource,
        RelativeBoth
    }

    public enum SwoExtSource
    {
        Itm,
        Hardware
    }
}
