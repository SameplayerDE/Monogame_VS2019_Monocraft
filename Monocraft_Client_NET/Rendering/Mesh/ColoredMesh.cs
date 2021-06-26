using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Monocraft_Client_NET.Rendering.Mesh
{
    public class ColoredMesh : IMesh
    {

        protected VertexPositionColorNormal[] _verticies;
        protected int[] _indicies;
        protected int _faceCount = 0;
        protected int _vertexCount = 0;
        protected int _indexCount = 0;
        protected bool _indexed = false;

        public VertexPositionColorNormal[] VertexData { get { return _verticies; } }
        public int[] IndexData { get { return _indicies; } }
        public int IndiciesCount { get { return _indexCount; } }
        public int VerticiesCount { get { return _vertexCount; } }
        public int FacesCount { get { return _faceCount; } }
        public bool Indexed { get { return _indexed; } set { _indexed = value; } }

        public ColoredMesh(Vector3[] verticisPosition, int[][] indicies, bool indexed, Color color)
        {

            _faceCount = indicies.GetLength(0);
            _vertexCount = verticisPosition.Length;
            _indexCount = _faceCount * 3;

            _indexed = indexed;
            _verticies = new VertexPositionColorNormal[_vertexCount];
            _indicies = new int[_indexCount];

            for (int face = 0; face < _faceCount; face++)
            {
                _indicies[face * 3 + 0] = indicies[face][0];
                _indicies[face * 3 + 1] = indicies[face][1];
                _indicies[face * 3 + 2] = indicies[face][2];
            }

            if (!indexed)
            {
                for (int vertex = 0; vertex < _vertexCount; vertex++)
                {
                    Vector3 v0;

                    v0 = verticisPosition[vertex];
                    _verticies[vertex] = new VertexPositionColorNormal(v0, color);
                }
            }
            else
            {
                _verticies = new VertexPositionColorNormal[_indexCount];
                for (int face = 0; face < FacesCount; face++)
                {
                    Vector3[] verticies = {
                        verticisPosition[indicies[face][0]],
                        verticisPosition[indicies[face][1]],
                        verticisPosition[indicies[face][2]]
                    };

                    Vector3 v0, v1, v2;
                    v0 = verticies[0];
                    v1 = verticies[1];
                    v2 = verticies[2];

                    _verticies[face * 3 + 0] = new VertexPositionColorNormal(v0, color);
                    _verticies[face * 3 + 1] = new VertexPositionColorNormal(v1, color);
                    _verticies[face * 3 + 2] = new VertexPositionColorNormal(v2, color);
                }
            }
            VertexPositionColorNormal.CalculateFaceNormals(_verticies);
        }
    }
}
