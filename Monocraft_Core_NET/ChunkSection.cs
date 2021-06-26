using Microsoft.Xna.Framework;
using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monocraft_Core_NET
{
    public class ChunkSection
    {
        private const int _width = 16; // x
        private const int _height = 16; // y
        private const int _depth = 16; // z
        private const int _blockCount = _width * _height * _depth;
        private int[] _blocks = new int[_blockCount];
        //private List<Block> _blocks = new List<Block>();

        public List<int> BlocksAsList { get { return _blocks.ToList(); } }
        public int[] BlocksAsArray { get { return _blocks; } }
        public bool IsOneBlockType { get { return (DifferentBlockCount() == 1); } }
        public bool IsEmpty { get { return (IsOneBlockType && DifferentBlockTypes()[0] == 0); } }

        public readonly int ChunkSectionY;
        public readonly Chunk Owner;

        public ChunkSection(Chunk owner, int chunkSectionY)
        {
            Owner = owner;
            ChunkSectionY = chunkSectionY;
        }

        public int DifferentBlockCount()
        {
            return DifferentBlockTypes().Length;
        }

        public int[] DifferentBlockTypes()
        {
            return BlocksAsList.Distinct().ToArray();
        }

        /*public Material[] DifferentBlockTypes()
        {
            List<Material> blockTypes = new List<Material>();
            foreach (Block block in _blocks)
            {
                if (!blockTypes.Contains(block.Material))
                {
                    blockTypes.Add(block.Material);
                }
            }
            return blockTypes.ToArray();
        }*/

        public bool IsPositionInSection(int x, int y, int z)
        {
            Vector3 min = GetMinBlockPosition();
            Vector3 max = GetMaxBlockPosition();
            if (x >= min.X && z >= min.Z && y >= min.Y)
            {
                if (x <= max.X && z <= max.Z && y <= max.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPositionInSection(float x, float y, float z)
        {
            return IsPositionInSection((int)x, (int)y, (int)z);
        }

        public bool IsPositionInSection(Vector3 position)
        {
            return IsPositionInSection(position.X, position.Y, position.Z);
        }

        public Vector3 GetMinBlockPosition()
        {
            Vector3 position = Owner.GetMinBlockPosition();
            return new Vector3(position.X, ChunkSectionY << 4, position.Z);
        }

        public Vector3 GetMaxBlockPosition()
        {
            Vector3 position = Owner.GetMaxBlockPosition();
            return new Vector3(position.X, ChunkSectionY + 1 << 4, position.Z) - new Vector3(0, 1, 0);
        }

        public void SetBlockAt(int x, int y, int z, int blockID)
        {

            x = x % _width;
            z = z % _width;
            y = y % _height;

            if (x < 0)
            {
                x += 16;
            }
            if (z < 0)
            {
                z += 16;
            }

            int i = x + _width * y + _width * _height * z;
            _blocks[i] = blockID;
            /*if (HasBlockAt(x, y, z))
            {
                Block block = GetBlockAt(x, y, z);
                block.Material = (Material)blockID;
            }
            else
            {
                Block block = new Block(x, y, z, (Material)blockID);
                _blocks.Add(block);
            }*/
        }

        public bool HasBlockAt(int x, int y, int z)
        {
            /*foreach (Block block in _blocks)
            {
                if (block.X == x && block.Y == y && block.Z == z)
                {
                    return true;
                }
            }*/
            return false;
        }

        public int GetBlockAt(int x, int y, int z)
        {
            x = x % _width;
            z = z % _width;
            y = y % _height;
            int i = x + _width * y + _width * _height * z;
            return _blocks[i];
            /*foreach (Block block in _blocks)
            {
                if (block.X == x && block.Y == y && block.Z == z)
                {
                    return block;
                }
            }
            return new Block(x, y, z, Material.Air);*/
        }
    }
}