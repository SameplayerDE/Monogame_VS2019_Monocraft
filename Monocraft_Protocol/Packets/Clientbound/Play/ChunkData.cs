using fNbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Play
{
    public class ChunkData : ClientboundPacket
    {
		public int ChunkX, ChunkY, PrimaryBitmask;
		public bool FullChunk;
		public NbtCompound HeightMaps;
		public int[] Biomes;
		public Memory<byte> Data;
		public List<NbtCompound> TileEntities;

		public ChunkData() : base(0x20) 
		{
			TileEntities = new List<NbtCompound>();
		}

        public void Decode(PacketStream packetStream)
		{
			ChunkX = packetStream.ReadInt();
			ChunkY = packetStream.ReadInt();
			FullChunk = packetStream.ReadBoolean();
			PrimaryBitmask = packetStream.ReadVarInt();

			HeightMaps = packetStream.ReadNbtCompound();

			if (FullChunk)
			{
				int biomeCount = packetStream.ReadVarInt();

				int[] biomeIds = new int[biomeCount];
				for (int idx = 0; idx < biomeIds.Length; idx++)
				{
					biomeIds[idx] = packetStream.ReadVarInt();
				}

				Biomes = biomeIds;
			}

			int i = packetStream.ReadVarInt();
			Data = new Memory<byte>(new byte[i]);
			packetStream.Read(Data.ToArray(), 0, Data.Length);

			int tileEntities = packetStream.ReadVarInt();
			for (int k = 0; k < tileEntities; k++)
			{
				TileEntities.Add(packetStream.ReadNbtCompound());
			}
		}

		public override byte[] ToBytes(int compressionThreshold)
        {
            throw new NotImplementedException();
        }
    }
}
