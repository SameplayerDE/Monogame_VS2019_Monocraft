using Microsoft.Xna.Framework;
using Monocraft_Protocol_NET;
using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Monocraft_Core_NET
{
    public class Chunk
    {

        private const int _chunkSectionCount = 16;

        private int _chunkX = 0;
        private int _chunkZ = 0;

        private ChunkSection[] _chunkSections = new ChunkSection[_chunkSectionCount]; // ChunkSection = 16x16x16 Blocks

        public int ChunkX { get { return _chunkX; } }
        public int ChunkZ { get { return _chunkZ; } }

        public List<ChunkSection> ChunkSectionsAsList { get { return _chunkSections.ToList(); } }

        public Chunk() 
        {
            // set owner and set y position of section
            for (int chunkSection = 0; chunkSection < _chunkSectionCount; chunkSection++)
            {
                _chunkSections[chunkSection] = new ChunkSection(this, chunkSection);
            }
        }
        public Chunk(int chunkX, int chunkZ) : this()
        {
            _chunkX = chunkX;
            _chunkZ = chunkZ;
        }

        public void Populate(int bitMask, PacketStream chunkStream)
        {
            for (int chunkSectionIndex = 0; chunkSectionIndex < _chunkSectionCount; chunkSectionIndex++)
            {
                int index = 0;
                if ((bitMask & (1 << chunkSectionIndex)) != 0)
                {

                    short blockCount = chunkStream.ReadSignedShort();
                    byte bitsPerBlock = chunkStream.ReadUnsignedByte();

                    int paletteLength = chunkStream.ReadVarInt();
                    int[] palette = new int[paletteLength];
                    for (int i = 0; i < paletteLength; i++)
                    {
                        palette[i] = chunkStream.ReadVarInt();
                    }

                    int dataArrayLength = chunkStream.ReadVarInt();
                    long[] dataArray = new long[dataArrayLength];
                    for (int i = 0; i < dataArrayLength; i++)
                    {
                        dataArray[i] = chunkStream.ReadLong();
                    }

                    ChunkSection chunkSection = _chunkSections[chunkSectionIndex];
                    int blocksRead = 0;
                    for (int y = 0; y < 16; y++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            for (int x = 0; x < 16; x++)
                            {

                                int bitsRead = 0;
                                int offset = blocksRead * bitsPerBlock;
                                int bitsLeft = (64 - offset);

                                if (bitsLeft < bitsPerBlock)
                                {
                                    index++;
                                    if (index >= dataArray.Length)
                                    {
                                        return;
                                    }
                                    blocksRead = 0;
                                    offset = 0;
                                    bitsLeft = 64;
                                }

                                long value = dataArray[index];

                                string binaryString = Convert.ToString(value, 2).PadLeft(64, '0');
                                char[] array = binaryString.ToCharArray();
                                Array.Reverse(array);
                                binaryString = new string(array);

                                // read bitsperblock from long
                                // save read value in int
                                // index += bitsperblock

                                char[] blockBinary = new char[bitsPerBlock];
                                for (int i = 0; i < bitsPerBlock; i++)
                                {
                                    if (bitsLeft >= bitsPerBlock)
                                    {
                                        blockBinary[i] = binaryString[offset + i];
                                        bitsRead++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (bitsRead == bitsPerBlock)
                                {
                                    //Console.Write(".");
                                    Array.Reverse(blockBinary);

                                    int paletteID = Convert.ToInt32(new string(blockBinary), 2);
                                    int blockID = palette[Math.Clamp(paletteID, 0, palette.Length - 1)];
                                    chunkSection.SetBlockAt(x, y, z, blockID);
                                    blocksRead++;
                                }
                                else
                                {
                                    //index++;
                                    //if (index >= data.Length)
                                    //{
                                    //    return;
                                    //}
                                    //blocksRead = 0;
                                }
                            }
                            //Console.WriteLine();
                        }
                        //Console.WriteLine();
                        //Console.WriteLine();
                    }
                }
            }
        }

        public bool IsPositionInChunk(int x, int z)
        {
            Vector3 min = GetMinBlockPosition();
            Vector3 max = GetMaxBlockPosition();
            if (x >= min.X && z >= min.Z)
            {
                if (x <= max.X && z <= max.Z)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPositionInChunk(float x, float z)
        {
            return IsPositionInChunk((int)x, (int)z);
        }

        public bool IsPositionInChunk(Vector3 position)
        {
            return IsPositionInChunk(position.X, position.Z);
        }

        public Rectangle GetChunkArea()
        {
            return new Rectangle(ChunkX * 16, ChunkZ * 16, ChunkX * 16 + 15, ChunkZ * 16 + 15);
        }

        public Vector3 GetMinBlockPosition()
        {
            return new Vector3(ChunkX << 4, 0, ChunkZ << 4);
        }

        public Vector3 GetMaxBlockPosition()
        {
            return new Vector3(ChunkX + 1 << 4, 0, ChunkZ + 1 << 4) - new Vector3(1, 0, 1);
        }

        private void test(long data, int bitsPerBlock, int[] palette)
        {
            string binaryString = Convert.ToString(data, 2).PadLeft(64, '0');
            List<int> blocks = new List<int>();

            for (int i = 0; i < binaryString.Length;)
            {
                int rest = binaryString.Length - i;
                char[] block = new char[bitsPerBlock];
                if (rest >= bitsPerBlock)
                {
                    for (int j = 0; j < bitsPerBlock; j++)
                    {
                    
                        block[j] = binaryString[i + j];
                    }
                    blocks.Add(Convert.ToInt32(block.ToString(), 2));
                    i += bitsPerBlock;
                }
                else
                {
                    i = binaryString.Length;
                }
            }

            foreach (int block in blocks) {
                Console.Write($"{block} ");
            }

        }
    }
}
