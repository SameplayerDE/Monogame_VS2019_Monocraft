using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Protocol_NET.Packets.Serverbound.Play
{
    public class PlayerRotation : ServerboundPacket
    {

        public float Yaw, Pitch;
        public bool OnGround;

        public PlayerRotation(float yaw, float pitch, bool onGround) : base(0x14)
        {
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            PacketStream packetStream = new PacketStream();
            if (compressionThreshold != -1 && compressionThreshold > 10)
            {
                packetStream.WriteByte(0x00); // No Compression
            }
            packetStream.WriteVarInt(PacketID);
            packetStream.WriteFloat(Yaw);
            packetStream.WriteFloat(Pitch);
            packetStream.WriteBoolean(OnGround);
            return packetStream.ToArray();
        }
    }
}
