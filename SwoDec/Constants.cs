using System;

namespace SwoDec
{
    public class Constants
    {
        // Synchronization packet header.
        public const byte SyncHeader = 0x00;

        // Minimal number of 0 bits required for a synchronization packet.
        public const int SyncMinBits = 47;

        // Overflow packet header.
        public const byte OverflowHeader = 0x70;

        // Local timestamp packet header.
        public const byte LtsHeader = 0xC0;

        // Bitmask for the timestamp of a local timestamp (LTS1) packet.
        public const int Lts1TsMask = 0xFFFFFFF;

        // Bitmask for the relation information of a local timestamp (LTS1) packet.
        public const byte Lts1TcMask = 0x30;

        // Offset of the relation information of a local timestamp (LTS1) packet.
        public const int Lts1TcOffset = 4;

        // Bitmask for the timestamp of a local timestamp (LTS2) packet.
        public const byte Lts2TsMask = 0x70;

        // Offset of the timestamp of local timestamp (LTS2) packet.
        public const int Lts2TsOffset = 4;

        // Global timestamp packet header.
        public const byte GtsHeader = 0x94;

        // Bitmask for the global timestamp packet header.
        public const byte GtsHeaderMask = 0xDF;

        // Bitmask for the type of a global timestamp packet.
        public const byte GtsTypeMask = 0x20;

        // Bitmask for the timestamp of a global timestamp (GTS1) packet.
        public const int Gts1TsMask = 0x03FFFFFF;

        // Bitmask for the clkch bit of a global timestamp (GTS1) packet.
        public const int Gts1ClkChMask = 0x4000000;

        // Bitmask for the wrap bit of a global timestamp (GTS1) packet.
        public const int Gts1WrapMask = 0x8000000;

        // Payload size of a global timestamp (GTS2) packet in bytes.
        public const int Gts2PayloadSize = 4;

        // Bitmask for the timestamp of a global timestamp (GTS2) packet.
        public const int Gts2TsMask = 0x3FFFFF;

        // Extension packet header.
        public const byte ExtHeader = 0x08;

        // Bitmask for the extension packet header.
        public const byte ExtHeaderMask = 0x0B;

        // Bitmask for the source bit of an extension packet.
        public const byte ExtSrcMask = 0x04;

        // Bitmask for the extension information of an extension packet header.
        public const byte ExtTsMask = 0x70;

        // Offset of the extension information of an extension packet header.
        public const int ExtTsOffset = 4;

        // Bitmask for the payload size of a source packet.
        public const byte SrcSizeMask = 0x03;

        // Bitmask for the type of a source packet.
        public const byte SrcTypeMask = 0x04;

        // Bitmask for the address of a source packet.
        public const byte SrcAddrMask = 0xF8;

        // Offset of the address of a source packet.
        public const int SrcAddrOffset = 3;

        // Bitmask for the continuation bit.
        public const byte CMask = 0x80;
    }
}
