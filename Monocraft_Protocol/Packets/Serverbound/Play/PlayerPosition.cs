using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Play
{
    public class PlayerPosition : ServerboundPacket
    {

        public readonly double X;
        public readonly double FeetY;
        public readonly double Z;
        public readonly bool OnGround;

        public PlayerPosition(double x, double feetY, double z, bool onGround) : base(0x12) 
        {
            X = x;
            FeetY = feetY;
            Z = z;
            OnGround = onGround;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WriteVarInt(PacketID);
            packetStream.WriteDouble(X);
            packetStream.WriteDouble(FeetY);
            packetStream.WriteDouble(Z);
            packetStream.WriteBoolean(OnGround);
            return packetStream.ToPacket();
        }
    }
}
