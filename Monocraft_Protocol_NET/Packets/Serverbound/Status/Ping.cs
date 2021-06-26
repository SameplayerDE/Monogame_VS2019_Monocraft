using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET.Packets.Serverbound.Status
{
    public class Ping : ServerboundPacket
    {

        public readonly long Payload;

        public Ping(long payLoad) : base(0x01) 
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
