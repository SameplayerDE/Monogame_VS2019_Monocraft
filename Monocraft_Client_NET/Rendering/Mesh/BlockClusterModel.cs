using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Client_NET.Rendering.Mesh
{
    public class BlockClusterModel : IMesh
    {

        protected VertexPositionTexture[] _verticies;
        protected int[] _indicies;
        protected int _faceCount = 0;
        protected int _vertexCount = 0;
        protected int _indexCount = 0;

        public VertexPositionTexture[] VertexData { get { return _verticies; } }
        public int[] IndexData { get { return _indicies; } }
        public int IndiciesCount { get { return _indexCount; } }
        public int VerticiesCount { get { return _vertexCount; } }
        public int FacesCount { get { return _faceCount; } }
        public Texture2D Texture;

        public BlockClusterModel(Vector3 from, Vector3 to, string texture)
        {
            if (MonocraftGame.TextureManager.Textures.ContainsKey($"block/{texture.ToString().ToLower()}"))
            {
                Texture = MonocraftGame.TextureManager.Textures[$"block/{texture.ToString().ToLower()}"];
            }
            else
            {
                Texture = MonocraftGame.TextureManager.Textures["Not_Found"];
            }
            List<Vector3> verticies = new List<Vector3>();

            Vector3 height = new Vector3(0, to.Y - from.Y, 0);
            Vector3 width = new Vector3(to.X - from.X, 0, 0);
            Vector3 depth = new Vector3(0, 0, to.Z - from.Z);

            float xDiff = to.X - from.X;
            float yDiff = to.Y - from.Y;
            float zDiff = to.Z - from.Z;

            float maxX, maxY, maxZ;
            float minX, minY, minZ;

            maxX = Math.Max(from.X, to.X);
            maxY = Math.Max(from.Y, to.Y);
            maxZ = Math.Max(from.Z, to.Z);

            minX = Math.Min(from.X, to.X);
            minY = Math.Min(from.Y, to.Y);
            minZ = Math.Min(from.Z, to.Z);

            /*Vector3[] positions = new Vector3[]
            {
                new Vector3(minX, minY, minZ), // 0
                new Vector3(maxX, minY, minZ), // 1
                new Vector3(maxX, maxY, minZ), // 2
                new Vector3(minX, maxY, minZ), // 3

                new Vector3(minX, minY, maxZ), // 4
                new Vector3(maxX, minY, maxZ), // 5
                new Vector3(maxX, maxY, maxZ), // 6
                new Vector3(minX, maxY, maxZ), // 7
            };*/

            verticies.Add(from); // 0 
            verticies.Add(from + width); // 1 
            verticies.Add(from + (width + depth)); // 2
            verticies.Add(from + depth); // 3

            verticies.Add(from + height); // 4
            verticies.Add(from + width + height); // 5
            verticies.Add(from + (width + depth) + height); // 6
            verticies.Add(from + depth + height); // 7

            Vector2[] uvs = new Vector2[]
            {
                new Vector2(xDiff, yDiff), // 0
                new Vector2(0, yDiff), // 1
                new Vector2(0, 0), // 2
                new Vector2(xDiff, 0), // 3

                new Vector2(zDiff, yDiff), // 0
                new Vector2(0, yDiff), // 1
                new Vector2(0, 0), // 2
                new Vector2(zDiff, 0), // 3
            };

            int[][] indicies = new int[][]
            {
                new int[] { 0, 1, 5, 0, 1, 2}, // front
                new int[] { 5, 4, 0, 2, 3, 0},

                new int[] { 1, 2, 6, 0 + 4, 1 + 4, 2 + 4}, // right
                new int[] { 6, 5, 1, 2 + 4, 3 + 4, 0 + 4},

                new int[] { 2, 3, 7, 0, 1, 2 }, // back
                new int[] { 7, 6, 2, 2, 3, 0 },

                new int[] { 3, 0, 4, 0 + 4, 1 + 4, 2 + 4 }, // left
                new int[] { 4, 7, 3, 2 + 4, 3 + 4, 0 + 4 },

                new int[] { 3, 2, 1, 2, 3, 0 }, // bottom
                new int[] { 1, 0, 3, 0, 1, 2 },

                new int[] { 4, 5, 6, 2, 3, 0 }, // top
                new int[] { 6, 7, 4, 0, 1, 2 }
            };

            _faceCount = indicies.GetLength(0);
            _vertexCount = verticies.Count;
            _indexCount = _faceCount * 3;

            _indicies = new int[_indexCount];

            for (int face = 0; face < _faceCount; face++)
            {
                _indicies[face * 3 + 0] = indicies[face][0];
                _indicies[face * 3 + 1] = indicies[face][1];
                _indicies[face * 3 + 2] = indicies[face][2];
            }

            _verticies = new VertexPositionTexture[_indexCount];
            for (int face = 0; face < FacesCount; face++)
            {
                Vector3[] positions = {
                        verticies[indicies[face][0]],
                        verticies[indicies[face][1]],
                        verticies[indicies[face][2]]
                    };

                Vector2[] uv = {
                        uvs[indicies[face][3]],
                        uvs[indicies[face][4]],
                        uvs[indicies[face][5]]
                    };


                Vector3 v0, v1, v2;
                Vector2 vt0, vt1, vt2;

                v0 = positions[0];
                v1 = positions[1];
                v2 = positions[2];

                vt0 = uv[0];
                vt1 = uv[1];
                vt2 = uv[2];

                _verticies[face * 3 + 0] = new VertexPositionTexture(v0, vt0);
                _verticies[face * 3 + 1] = new VertexPositionTexture(v1, vt1);
                _verticies[face * 3 + 2] = new VertexPositionTexture(v2, vt2);
            }

        }

    }
}
