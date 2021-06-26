using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Client_NET.Rendering.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Rendering.Renderer
{
    public class EntityRenderer : Renderer<ColoredCubeMesh>
    {
        public EntityRenderer() { }

        public void DrawModel(ColoredCubeMesh mesh, Vector3 position, Matrix view, Matrix projection, GameTime gameTime)
        {

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {

                Matrix world = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation.X) * Matrix.CreateRotationY(_rotation.Y) * Matrix.CreateRotationZ(_rotation.Z) * Matrix.CreateTranslation(position);

                _effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
                _effect.Parameters["World"].SetValue(world);

                pass.Apply();

                if (!mesh.Indexed)
                {

                    MonocraftGame.Instance.GraphicsDevice.DrawUserIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        mesh.VertexData,
                        0,
                        mesh.VerticiesCount,
                        mesh.IndexData,
                        0,
                        mesh.FacesCount
                    );

                }
                else
                {
                    MonocraftGame.Instance.GraphicsDevice.DrawUserPrimitives(
                        PrimitiveType.TriangleList,
                        mesh.VertexData,
                        0,
                        mesh.FacesCount
                    );
                }
            }
        }

        public override void DrawModel(ColoredCubeMesh mesh, Matrix view, Matrix projection, GameTime gameTime)
        {

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {

                Matrix world = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation.X) * Matrix.CreateRotationY(_rotation.Y) * Matrix.CreateRotationZ(_rotation.Z) * Matrix.CreateTranslation(_position);

                _effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
                _effect.Parameters["World"].SetValue(world);

                pass.Apply();

                if (!mesh.Indexed)
                {

                    MonocraftGame.Instance.GraphicsDevice.DrawUserIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        mesh.VertexData,
                        0,
                        mesh.VerticiesCount,
                        mesh.IndexData,
                        0,
                        mesh.FacesCount
                    );

                }
                else
                {
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
}
