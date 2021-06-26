using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Monocraft_Client_NET.Rendering.Mesh
{
    public class TexturedMesh : IMesh
    {

        protected VertexPositionTexture[] _verticies;
        protected int[] _indicies;
        protected int _faceCount = 0;
        protected int _vertexCount = 0;
        protected int _indexCount = 0;
        protected bool _indexed = false;

        public VertexPositionTexture[] VertexData { get { return _verticies; } }
        public int[] IndexData { get { return _indicies; } }
        public int IndiciesCount { get { return _indexCount; } }
        public int VerticiesCount { get { return _vertexCount; } }
        public int FacesCount { get { return _faceCount; } }
        public bool Indexed { get { return _indexed; } set { _indexed = value; } }

        //public TexturedMesh(Vector3[] verticisPosition, Vector2[] uvPosition, Vector3[] normals, int[][] indicies, bool indexed)
        //{
        public TexturedMesh(Vector3[] verticisPosition, Vector2[] uvPosition, int[][] indicies, bool indexed)
        {

            _faceCount = indicies.GetLength(0);
            _vertexCount = verticisPosition.Length;
            _indexCount = _faceCount * 3;

            _indexed = indexed;
            _verticies = new VertexPositionTexture[_vertexCount];
            _indicies = new int[_indexCount];

            for (int face = 0; face < _faceCount; face++)
            {
                _indicies[face * 3 + 0] = indicies[face][0];
                _indicies[face * 3 + 1] = indicies[face][1];
                _indicies[face * 3 + 2] = indicies[face][2];
            }

            if (!indexed)
            {
                //_verticies = new VertexPositionNormalTexture[_vertexCount];

                for (int vertex = 0; vertex < _vertexCount; vertex++)
                {
                    Vector3 v0;

                    v0 = verticisPosition[vertex];
                    _verticies[vertex] = new VertexPositionTexture(v0, uvPosition[1]);
                }

            }
            else
            {
                _verticies = new VertexPositionTexture[_indexCount];
                for (int face = 0; face < FacesCount; face++)
                {
                    Vector3[] verticies = {
                        verticisPosition[indicies[face][0]],
                        verticisPosition[indicies[face][1]],
                        verticisPosition[indicies[face][2]]
                    };

                    Vector2[] uvs = {
                        uvPosition[indicies[face][3]],
                        uvPosition[indicies[face][4]],
                        uvPosition[indicies[face][5]]
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

                    vt0 = uvs[0];
                    vt1 = uvs[1];
                    vt2 = uvs[2];

                    //vn0 = tempNormals[0];
                    //vn1 = tempNormals[1];
                    //vn2 = tempNormals[2];

                    _verticies[face * 3 + 0] = new VertexPositionTexture(v0, vt0);
                    _verticies[face * 3 + 1] = new VertexPositionTexture(v1, vt1);
                    _verticies[face * 3 + 2] = new VertexPositionTexture(v2, vt2);
                }


                /**for (int i = 0; i < _indicies.Length; i += 3)
                {
                    Vector3 v0, v1, v2;

                    v0 = verticisPosition[_indicies[i + 0]];
                    v1 = verticisPosition[_indicies[i + 1]];
                    v2 = verticisPosition[_indicies[i + 2]];

                    _verticies[i + 0] = new VertexPositionNormalTexture(v0, Vector3.UnitZ, new Vector2(0, 0));
                    _verticies[i + 1] = new VertexPositionNormalTexture(v1, Vector3.UnitZ, new Vector2(0, 0));
                    _verticies[i + 2] = new VertexPositionNormalTexture(v2, Vector3.UnitZ, new Vector2(0, 0));
                }
                VertexPositionNormalTexture.CalculateFaceNormals(_verticies);**/
            }
        }
    }
}
