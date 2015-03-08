using System;
using System.Collections.Generic;
using System.Linq;

namespace SwoDec
{
    public class SwoDecoder
    {
        private readonly List<byte> buffer = new List<byte>();

        #region Events

        public class PacketAvailableArgs : EventArgs
        {
            public ISwoPacket Packet;

            public PacketAvailableArgs(ISwoPacket e)
            {
                Packet = e;
            }
        }

        public delegate void PacketAvailableHandler(object sender, PacketAvailableArgs args);
        public PacketAvailableHandler PacketAvailable;
        public void OnPacketAvailable(ISwoPacket e)
        {
            if (PacketAvailable != null)
            {
                PacketAvailable(this, new PacketAvailableArgs(e));
            }
        }

        #endregion

        public void Feed(byte[] data)
        {
            buffer.AddRange(data);
        }

        public void Decode()
        {
            // run until the buffer is empty
            while (buffer.Count > 0)
            {
                var decoded = false;
                var packetType = DecodePacketType(buffer[0]);
                switch (packetType)
                {
                    case SwoPacketType.Sync:
                        decoded = DecodeSyncPacket();
                        break;
                    case SwoPacketType.Overflow:
                        decoded = DecodeOverflowPacket();
                        break;
                    case SwoPacketType.LocalTimeStamp:
                        decoded = DecodeLocalTimeStampPacket();
                        break;
                    case SwoPacketType.GlobalTimeStamp1:
                        decoded = DecodeGlobalTimeStamp1Packet();
                        break;
                    case SwoPacketType.GlobalTimeStamp2:
                        decoded = DecodeGlobalTimeStamp2Packet();
                        break;
                    case SwoPacketType.Extension:
                        decoded = DecodeExtensionPacket();
                        break;
                    case SwoPacketType.Instrumentation:
                        decoded = DecodeInstrumentationPacket();
                        break;
                    case SwoPacketType.Hardware:
                        decoded = DecodeHardwarePacket();
                        break;
                    case SwoPacketType.Unkown:
                        decoded = DecodeUnkownPacket();
                        break;
                }

                // return if no packets were properly decoded
                if (!decoded) break;
            }
        }

        private int DecodeConditionalPayload(out int value)
        {
            // take bytes while the continuation bit is set
            var bytes = buffer.Skip(1).TakeWhile(b => (b & Constants.CMask) != 0).Select(b => b & ~Constants.CMask).ToList();
            if (bytes.Count == (buffer.Count - 1))
            {
                value = 0;
                return 0;
            }

            // add the final byte and reverse the set
            bytes.Add(buffer[bytes.Count + 1]);
            bytes.Reverse();

            // accumulate the bytes into a final value
            value = bytes.Aggregate(0, (accum, b) => (accum << 7) | b);

            return bytes.Count;
        }

        private bool DecodeSyncPacket()
        {
            // check if there are enough bytes in the buffer
            if (buffer.Count < 6)
                return false;

            var zeroByteCount = buffer.TakeWhile(b => b == 0).Count();
            if (zeroByteCount == 5 && buffer[5] == 0x80)
            {
                // pop from the buffer
                buffer.RemoveRange(0, 6);

                // create and submit packet
                OnPacketAvailable(new Packets.SyncPacket());

                return true;
            }

            // pop from the buffer
            buffer.RemoveRange(0, zeroByteCount);

            // create and submit packet
            OnPacketAvailable(new Packets.UnknownPacket(0));

            return true;
        }

        private bool DecodeOverflowPacket()
        {
            // pop from the buffer
            buffer.RemoveAt(0);

            // create and submit the packet
            OnPacketAvailable(new Packets.OverflowPacket());

            return true;
        }

        private bool DecodeLocalTimeStampPacket()
        {
            return false;
        }

        private bool DecodeGlobalTimeStamp1Packet()
        {
            return false;
        }

        private bool DecodeGlobalTimeStamp2Packet()
        {
            return false;
        }

        private bool DecodeExtensionPacket()
        {
            var value = (buffer[0] & Constants.ExtTsMask) >> Constants.ExtTsOffset;
            var source = SwoExtSource.Itm;
            var size = 1;

            // decode continuation
            if ((buffer[0] & Constants.CMask) != 0)
            {
                var tmp = 0;
                var len = DecodeConditionalPayload(out tmp);

                // not enough bytes to decode payload
                if (len == 0)
                    return false;

                value |= tmp << 3;
                size += len;
            }

            // check source field
            if ((buffer[0] & Constants.ExtSrcMask) != 0)
                source = SwoExtSource.Hardware;

            // pop from the buffer
            buffer.RemoveRange(0, size);

            // create and submit the packet
            OnPacketAvailable(new Packets.ExtensionPacket(source, value));

            return true;
        }

        private bool DecodeInstrumentationPacket()
        {
            var payloadSize = 1 << ((buffer[0] & Constants.SrcSizeMask) - 1);
            var address = (buffer[0] & Constants.SrcAddrMask) >> Constants.SrcAddrOffset;

            // not enough bytes to decode payload
            if ((payloadSize + 1) > buffer.Count)
                return false;

            // get the bytes and reverse the set
            var bytes = buffer.Skip(1).Take(payloadSize);
            bytes.Reverse();

            // accumulate the bytes into a final value
            var value = bytes.Aggregate(0, (accum, b) => (accum << 8) | b);

            // pop from the buffer
            buffer.RemoveRange(0, payloadSize + 1);

            // create and submit the packet
            OnPacketAvailable(new Packets.InstrumentationPacket(address, payloadSize, value));

            return true;
        }

        private bool DecodeHardwarePacket()
        {
            var payloadSize = 1 << ((buffer[0] & Constants.SrcSizeMask) - 1);
            var address = (buffer[0] & Constants.SrcAddrMask) >> Constants.SrcAddrOffset;

            // not enough bytes to decode payload
            if ((payloadSize + 1) > buffer.Count)
                return false;

            // get the bytes and reverse the set
            var bytes = buffer.Skip(1).Take(payloadSize);
            bytes.Reverse();

            // accumulate the bytes into a final value
            var value = bytes.Aggregate(0, (accum, b) => (accum << 8) | b);

            // pop from the buffer
            buffer.RemoveRange(0, payloadSize + 1);

            // create and submit the packet
            OnPacketAvailable(new Packets.HardwarePacket(address, payloadSize, value));

            return true;
        }

        private bool DecodeUnkownPacket()
        {
            // construct the packet
            var packet = new Packets.UnknownPacket(buffer[0]);

            // pop from the buffer
            buffer.RemoveAt(0);

            // submit the packet
            OnPacketAvailable(packet);

            return true;
        }

        private SwoPacketType DecodePacketType(byte header)
        {
            if (header == Constants.SyncHeader)
                return SwoPacketType.Sync;

            if (header == Constants.OverflowHeader)
                return SwoPacketType.Overflow;

            if ((header & ~Constants.Lts2TsMask) == 0)
                return SwoPacketType.LocalTimeStamp;

            if ((header & ~Constants.Lts1TcMask) == Constants.LtsHeader)
                return SwoPacketType.LocalTimeStamp;

            if ((header & Constants.ExtHeaderMask) == Constants.ExtHeader)
                return SwoPacketType.Extension;

            if ((header & Constants.GtsHeaderMask) == Constants.GtsHeader)
            {
                if ((header & Constants.GtsTypeMask) != 0)
                    return SwoPacketType.GlobalTimeStamp2;
                else
                    return SwoPacketType.GlobalTimeStamp1;
            }

            if ((header & Constants.SrcSizeMask) != 0)
            {
                if ((header & Constants.SrcTypeMask) != 0)
                    return SwoPacketType.Hardware;
                else
                    return SwoPacketType.Instrumentation;
            }

            return SwoPacketType.Unkown;
        }
    }
}
