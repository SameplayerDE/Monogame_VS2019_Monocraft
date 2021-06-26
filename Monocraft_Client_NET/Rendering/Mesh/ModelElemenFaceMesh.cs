using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Core_NET.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Client_NET.Rendering.Mesh
{
    public class ModelElemenFaceMesh
    {
        // Down 0, 1, 2, 3
        // Up 4, 5, 6, 7
        // North 0, 1, 4, 5
        // East 1, 3, 5, 7
        // South 3, 2, 6, 7
        // West 2, 0, 4, 6

        private readonly Dictionary<string, int[]> VERTEX_INDICIES = new Dictionary<string, int[]>()
        {
            { "down", new int[4] { 1, 0, 3, 2 } },
            { "up", new int[4] { 6, 7, 4, 5 } },
            { "north", new int[4] { 0, 1, 5, 4 } },
            { "east", new int[4] { 1, 2, 6, 5 } },
            { "south", new int[4] { 2, 3, 7, 6 } },
            { "west", new int[4] { 3, 0, 4, 7 } },
        };

        private readonly short[] INDICIES = new short[]
        {
            1, 2, 3,
            3, 4, 1
        };

        protected VertexPositionTexture[] _verticies;
        protected string _direction;
        protected Face _face;
        protected Vector3[] _vectors;
        public VertexPositionTexture[] VertexData { get { return _verticies; } }
        public string Direction { get { return _direction; } }

        public Face Face { get { return _face; } }

        public ModelElemenFaceMesh(string direction, Face face, Vector3[] vectors)
        {
            _direction = direction;
            _face = face;
            _vectors = vectors;

            List<Vector2> uvs = new List<Vector2>();

            if (face.UV == null)
            {
                uvs.Add(new Vector2(16, 16));
                uvs.Add(new Vector2(0, 16));
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(16, 0));
            }
            else
            {
                uvs.Add(new Vector2(face.UV[2], face.UV[3]));
                uvs.Add(new Vector2(face.UV[0], face.UV[3]));
                uvs.Add(new Vector2(face.UV[0], face.UV[1]));
                uvs.Add(new Vector2(face.UV[2], face.UV[1]));
            }
            
            GenerateVertexPositions(uvs);

        }

        private void GenerateVertexPositions(List<Vector2> uvs)
        {
            if (VERTEX_INDICIES.ContainsKey(_direction))
            {
                _verticies = new VertexPositionTexture[4];
                List<Vector3> vertexData = new List<Vector3>();
                for (int i = 0; i < 4; i++)
                {
                    vertexData.Add(_vectors[VERTEX_INDICIES[_direction][i]]);
                }

                List<int[]> indicies = new List<int[]>();

                for (int i = 0; i < 6; i += 3)
                {
                    int[] triangle = new int[6];
                    triangle[0] = INDICIES[i];
                    triangle[1] = INDICIES[i + 1];
                    triangle[2] = INDICIES[i + 2];
                    triangle[3] = INDICIES[i];
                    triangle[4] = INDICIES[i + 1];
                    triangle[5] = INDICIES[i + 2];
                    indicies.Add(triangle);
                }

                _verticies = new VertexPositionTexture[indicies.Count * 3];

                for (int face = 0; face < indicies.Count; face++)
                {
                    Vector3[] verticies = {
                        vertexData[indicies[face][0] - 1] / 16f,
                        vertexData[indicies[face][1] - 1] / 16f,
                        vertexData[indicies[face][2] - 1] / 16f
                    };

                    Vector2[] uvPositions = {
                            uvs[indicies[face][3] - 1] / 16f,
                            uvs[indicies[face][4] - 1] / 16f,
                            uvs[indicies[face][5] - 1] / 16f
                        };

                    /*Vector3[] tempNormals = {
                            normals[indicies[face][6] - 1],
                            normals[indicies[face][7] - 1],
                            normals[indicies[face][8] - 1]
                        };*/

                    Vector3 v0, v1, v2;
                    //Vector3 vn0, vn1, vn2;
                    Vector2 vt0, vt1, vt2;

                    v0 = verticies[0];
                    v1 = verticies[1];
                    v2 = verticies[2];

                    vt0 = uvPositions[0];
                    vt1 = uvPositions[1];
                    vt2 = uvPositions[2];

                    /*vn0 = tempNormals[0];
                    vn1 = tempNormals[1];
                    vn2 = tempNormals[2];*/

                    _verticies[face * 3 + 0] = new VertexPositionTexture(v0, vt0);
                    _verticies[face * 3 + 1] = new VertexPositionTexture(v1, vt1);
                    _verticies[face * 3 + 2] = new VertexPositionTexture(v2, vt2);
                }

            }
        }
    }
}
