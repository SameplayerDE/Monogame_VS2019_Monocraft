using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Client_NET.Rendering.Mesh;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Client_NET.Rendering.Renderer
{
    public class BlockClusterRenderer : Renderer<BlockClusterModel>
    {

        public override void DrawModel(BlockClusterModel mesh, Vector3 position, Matrix view, Matrix projection, GameTime gameTime)
        {
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                Matrix world = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation.X) * Matrix.CreateRotationY(_rotation.Y) * Matrix.CreateRotationZ(_rotation.Z) * Matrix.CreateTranslation(position);

                _effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
                _effect.Parameters["World"].SetValue(world);
                _effect.Parameters["Texture"].SetValue(mesh.Texture);

                pass.Apply();

                MonocraftGame.Instance.GraphicsDevice.DrawUserPrimitives(
                        PrimitiveType.TriangleList,
                        mesh.VertexData,
                        0,
                        mesh.FacesCount
                    );
            }
        }

    }
}
