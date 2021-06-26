using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Protocol_NET.Packets.Clientbound.Play
{
    public class BlockChange : ClientboundPacket
    {

        public int X, Y, Z;
        public Material Material;

        public BlockChange() : base(0x0B)
        {
        }

        public BlockChange(int x, int y, int z, Material material) : base(0x0B)
        {
            X = x;
            Y = y;
            Z = z;
            Material = material;
        }

        public void Decode(PacketStream packetStream)
        {
            int[] position = packetStream.ReadPosition();
            int blockID = packetStream.ReadVarInt();
            X = position[0];
            Y = position[1];
            Z = position[2];
            Material = (Material)blockID;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            throw new NotImplementedException();
        }
    }
}
