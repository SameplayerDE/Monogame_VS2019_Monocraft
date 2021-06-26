using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Status
{
    public class Response : ClientboundPacket
    {

        public readonly string Json;

        public Response() : base(0x00) { }
        public Response(DataReader reader) : base(0x00)
        {
            Json = reader.ReadString();
        }
        public Response(byte[] data) : base(0x00)
        {
            DataReader reader = new DataReader(data);
            Json = reader.ReadString();
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            writer.WriteString(Json);
            return writer.ToArray();
        }
    }
}
