using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monocraft_Client_NET.Rendering
{
    public struct VertexPositionNormal : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;

        public VertexPositionNormal(Vector3 Position)
        {
            this.Position = Position;
            this.Normal = Vector3.UnitZ;
        }

        public VertexPositionNormal(VertexPositionNormal vertex, Vector3 Normal)
        {
            this.Position = vertex.Position;
            this.Normal = Normal;
        }

        public static void CalculateFaceNormals(VertexPositionNormal[] data)
        {

            for (int i = 0; i < data.Length / 3; i++)
            {
                Vector3 ab = data[i * 3].Position - data[i * 3 + 1].Position;
                Vector3 cb = data[i * 3 + 2].Position - data[i * 3 + 1].Position;

                ab.Normalize();
                cb.Normalize();

                Vector3 normal = Vector3.Cross(ab, cb);

                data[i * 3] = new VertexPositionNormal(data[i * 3], normal);
                data[i * 3 + 1] = new VertexPositionNormal(data[i * 3 + 1], normal);
                data[i * 3 + 2] = new VertexPositionNormal(data[i * 3 + 2], normal);

            }

        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

    }
}
