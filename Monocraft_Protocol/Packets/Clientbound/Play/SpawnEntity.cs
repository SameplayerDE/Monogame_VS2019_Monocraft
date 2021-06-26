using Monocraft_Protocol.Packets.Serverbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Play
{

    public class SpawnEntity : ServerboundPacket
    {

        public readonly int EntityID;

        public SpawnEntity() : base(0x00) { }
        public SpawnEntity(DataReader reader) : base(0x00) { }
        public SpawnEntity(byte[] data) : base(0x00) { }
        public SpawnEntity(int entityID) : base(0x00)
        {
            EntityID = entityID;
        }


        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            return writer.ToArray();
        }
    }
}
