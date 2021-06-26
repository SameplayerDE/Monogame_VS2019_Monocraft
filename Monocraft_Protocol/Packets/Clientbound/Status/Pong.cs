using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Status
{
    public class Pong : ClientboundPacket
    {
        public readonly long Payload;

        public Pong() : base(0x01) { }
        public Pong(DataReader reader) : base(0x01) 
        {
            Payload = reader.ReadLong();
        }
        public Pong(byte[] data) : base(0x01) { }
        public Pong(long payLoad) : base(0x01)
        {
            Payload = payLoad;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            writer.WriteLong(Payload);
            return writer.ToArray();
        }
    }
}
