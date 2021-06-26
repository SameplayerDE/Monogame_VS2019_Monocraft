using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET
{
    public class World
    {

        List<Chunk> _chunks = new List<Chunk>();
        List<Entity> _entities = new List<Entity>();
        public List<Chunk> Chunks { get { return new List<Chunk>(_chunks); } }
        public List<Entity> Entities { get { return new List<Entity>(_entities); } }

        public World() { }

        public void AddChunk(Chunk chunk)
        {
            if (!_chunks.Contains(chunk))
            {
                _chunks.Add(chunk);
            }
        }

        public void RemoveChunk(Chunk chunk)
        {
            if (_chunks.Contains(chunk))
            {
                _chunks.Remove(chunk);
            }
        }

        public void AddEntity(Entity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            if (_entities.Contains(entity))
            {
                _entities.Remove(entity);
            }
        }

        public bool SetBlock(int x, int y, int z, int blockID)
        {
            foreach (Chunk chunk in new List<Chunk>(_chunks))
            {
                if (chunk.IsPositionInChunk(x, z))
                {
                    foreach (ChunkSection chunkSection in chunk.ChunkSectionsAsList)
                    {
                        if (chunkSection.IsPositionInSection(x, y, z))
                        {
                            chunkSection.SetBlockAt(x, y, z, blockID);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Chunk GetChunkAt(int chunkX, int chunkZ)
        {
            foreach (Chunk chunk in new List<Chunk>(_chunks))
            {
                if (chunk.ChunkX == chunkX && chunk.ChunkZ == chunkZ)
                {
                    return chunk;
                }
            }
            return null;
        }

        public static Vector3 ToChunkCoordinates(int x, int y, int z)
        {
            x /= 16;
            y /= 16;
            z /= 16;

            if (x < 0) x += 1;
            if (z < 0) z += 1;

            return new Vector3(x, y, z);

        }

        public static Vector3 ToChunkCoordinates(float x, float y, float z)
        {
            return ToChunkCoordinates((int)x, (int)y, (int)z);
        }

        public static Vector3 ToChunkCoordinates(Vector3 position)
        {
            return ToChunkCoordinates(position.X, position.Y, position.Z);
        }

        public bool SetBlock(float x, float y, float z, int blockID)
        {
            return SetBlock((int)x, (int)y, (int)z, blockID);
        }

        public bool SetBlock(Vector3 position, int blockID)
        {
            return SetBlock(position.X, position.Y, position.Z, blockID);
        }

    }
}
