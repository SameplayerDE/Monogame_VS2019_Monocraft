using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Login
{
    public class EncryptionResponse : ServerboundPacket
    {
        public EncryptionResponse() : base(0x01) { }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            return writer.ToArray();
        }
    }
}
