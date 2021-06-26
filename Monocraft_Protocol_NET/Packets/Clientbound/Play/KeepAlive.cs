using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET.Packets.Clientbound.Play
{
    public class KeepAlive : ClientboundPacket
    {
        public readonly long KeepAliveID;

        public KeepAlive() : base(0x1F) { }
        public KeepAlive(DataReader reader) : base(0x1F)
        {
            KeepAliveID = reader.ReadLong();
        }
        public KeepAlive(byte[] data) : base(0x1F)
        {
            DataReader reader = new DataReader(data);
            KeepAliveID = reader.ReadLong();
        }
        public KeepAlive(long keepAliveID) : base(0x1F)
        {
            KeepAliveID = keepAliveID;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            writer.WriteLong(KeepAliveID);
            return writer.ToArray();
        }
    }
}
