using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monocraft_Client_NET.Rendering
{
    public struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public VertexPositionColorNormal(Vector3 Position, Color Color)
        {
            this.Position = Position;
            this.Color = Color;
            this.Normal = Vector3.UnitZ;
        }

        public VertexPositionColorNormal(VertexPositionColorNormal vertex, Vector3 Normal)
        {
            this.Position = vertex.Position;
            this.Color = vertex.Color;
            this.Normal = Normal;
        }

        public static void CalculateFaceNormals(VertexPositionColorNormal[] data)
        {

            for (int i = 0; i < data.Length / 3; i++)
            {
                Vector3 ab = data[i * 3].Position - data[i * 3 + 1].Position;
                Vector3 cb = data[i * 3 + 2].Position - data[i * 3 + 1].Position;

                ab.Normalize();
                cb.Normalize();

                Vector3 normal = Vector3.Cross(ab, cb);

                data[i * 3] = new VertexPositionColorNormal(data[i * 3], normal);
                data[i * 3 + 1] = new VertexPositionColorNormal(data[i * 3 + 1], normal);
                data[i * 3 + 2] = new VertexPositionColorNormal(data[i * 3 + 2], normal);

            }

        }

        public static void CalculateFaceNormals(VertexPositionNormalTexture[] data)
        {

            for (int i = 0; i < data.Length / 3; i++)
            {
                Vector3 ab = data[i * 3].Position - data[i * 3 + 1].Position;
                Vector3 cb = data[i * 3 + 2].Position - data[i * 3 + 1].Position;

                ab.Normalize();
                cb.Normalize();

                Vector3 normal = Vector3.Cross(ab, cb);

                data[i * 3] = new VertexPositionNormalTexture(data[i * 3].Position, normal, data[i * 3].TextureCoordinate);
                data[i * 3 + 1] = new VertexPositionNormalTexture(data[i * 3 + 1].Position, normal, data[i * 3 + 1].TextureCoordinate);
                data[i * 3 + 2] = new VertexPositionNormalTexture(data[i * 3 + 2].Position, normal, data[i * 3 + 2].TextureCoordinate);

            }

        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

    }
}
