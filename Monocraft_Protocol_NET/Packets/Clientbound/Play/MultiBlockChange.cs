using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Protocol_NET.Packets.Clientbound.Play
{
	public class MultiBlockChange : ClientboundPacket
	{
		public long ChunkSectionPosition = 0;
		public int ChunkSectionX = 0;
		public int ChunkSectionY = 0;
		public int ChunkSectionZ = 0;
		public int AmountOfBlocks = 0;
		public long[] Blocks;
		public List<BlockChange> BlockUpdates = new List<BlockChange>();

        public MultiBlockChange() : base(0x3b) { }

		public void Decode(PacketStream packetStream)
		{
			ChunkSectionPosition = packetStream.ReadLong();
			ChunkSectionX = (int)(ChunkSectionPosition >> 42);
			ChunkSectionY = (int)(ChunkSectionPosition << 44 >> 44);
			ChunkSectionZ = (int)(ChunkSectionPosition << 22 >> 42);
			Console.WriteLine(ChunkSectionX);
			Console.WriteLine(ChunkSectionY);
			Console.WriteLine(ChunkSectionZ);
			Console.WriteLine("----");
			packetStream.ReadBoolean();
			AmountOfBlocks = packetStream.ReadVarInt();
			
			Blocks = new long[AmountOfBlocks];

			for (int block = 0; block < AmountOfBlocks; block++)
            {
				Blocks[block] = packetStream.ReadVarLong();
            }

			foreach (long blockLong in Blocks)
            {
				int blockID = (int)(blockLong >> 12);
				int blockLocalX = (int)(blockLong >> 8 & 0x0F);
				int blockLocalZ = (int)(blockLong >> 4 & 0x0F);
				int blockLocalY = (int)(blockLong & 0x0F);

				BlockUpdates.Add(new BlockChange(ChunkSectionX * 16 + blockLocalX, ChunkSectionY * 16 + blockLocalY, ChunkSectionZ * 16 + blockLocalZ, (Material)blockID));

			}

		}

		public override byte[] ToBytes(int compressionThreshold)
        {
            throw new NotImplementedException();
        }
    }
}
