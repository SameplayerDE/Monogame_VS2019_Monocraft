using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET.Packets.Clientbound.Play
{
    public class SpawnLivingEntity : ClientboundPacket
    {

        public readonly int EntityID;
        public readonly Guid UUID;
        public readonly EntityType Type;
        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly sbyte Yaw;
        public readonly sbyte Pitch;
        public readonly sbyte HeadPitch;
        public readonly short VelocityX;
        public readonly short VelocityY;
        public readonly short VelocityZ;

        public SpawnLivingEntity() : base(0x02) { }
        public SpawnLivingEntity(DataReader reader) : base(0x02) 
        {
            EntityID = reader.ReadVarInt();
            UUID = reader.ReadUUID();
            Type = (EntityType)reader.ReadVarInt();
            X = reader.ReadDouble();
            Y = reader.ReadDouble();
            Z = reader.ReadDouble();
            Yaw = reader.ReadAngle();
            Pitch = reader.ReadAngle();
            HeadPitch = reader.ReadAngle();
            VelocityX = reader.ReadShort();
            VelocityY = reader.ReadShort();
            VelocityZ = reader.ReadShort();
        }
        public SpawnLivingEntity(byte[] data) : base(0x02) { }
        public SpawnLivingEntity(int entityID, Guid uuid, EntityType type, double x, double y, double z, sbyte yaw, sbyte pitch, sbyte headPitch, short velocityX, short velocityY, short velocityZ) : base(0x02) 
        {
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            return writer.ToArray();
        }
    }
}
