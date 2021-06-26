using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Play
{
    public class PlayerPositionAndLook : ClientboundPacket
    {

        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly float Yaw;
        public readonly float Pitch;
        public readonly sbyte Flags;
        public readonly int TeleportID;

        public PlayerPositionAndLook() : base(0x34) { }
        public PlayerPositionAndLook(DataReader reader) : base(0x34)
        {
            X = reader.ReadDouble();
            Y = reader.ReadDouble();
            Z = reader.ReadDouble();
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            Flags = reader.ReadByte();
            TeleportID = reader.ReadVarInt();
        }
        public PlayerPositionAndLook(byte[] data) : base(0x34) 
        {
            DataReader reader = new DataReader(data);
            X = reader.ReadDouble();
            Y = reader.ReadDouble();
            Z = reader.ReadDouble();
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            Flags = reader.ReadByte();
            TeleportID = reader.ReadVarInt();
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            writer.WriteVarInt(PacketID);
            return writer.ToArray();
        }
    }
}
