using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Client_NET.Rendering.Mesh;
using Monocraft_Core_NET.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Client_NET.Rendering.Renderer
{
    public class BlockyModelRenderer : Renderer<BlockyModelMesh>
    {
        public override void DrawModel(BlockyModelMesh mesh, Vector3 position, Matrix view, Matrix projection, GameTime gameTime)
        {
            Matrix world = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation.X) * Matrix.CreateRotationY(_rotation.Y) * Matrix.CreateRotationZ(_rotation.Z) * Matrix.CreateTranslation(position);

            _effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
            _effect.Parameters["World"].SetValue(world);
            string texture = "Not_Found";
            foreach (ModelElementMesh modelElementMesh in mesh.ModelElementMeshes)
            {
                
                foreach (KeyValuePair<string, List<VertexPositionTexture>> modelElemenFaceMesh in modelElementMesh.Faces)
                {

                    texture = modelElemenFaceMesh.Key.Replace("minecraft:", "");
                    
                    _effect.Parameters["Texture"].SetValue(MonocraftGame.TextureManager.Textures[texture]);
                    foreach (var pass in _effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        MonocraftGame.Instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, modelElemenFaceMesh.Value.ToArray(), 0, modelElemenFaceMesh.Value.Count / 3);
                    }
                }

            }
            /*foreach (var pass in _effect.CurrentTechnique.Passes)
            {

                Matrix world = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation.X) * Matrix.CreateRotationY(_rotation.Y) * Matrix.CreateRotationZ(_rotation.Z) * Matrix.CreateTranslation(position);

                _effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
                _effect.Parameters["World"].SetValue(world);
                _effect.Parameters["Texture"].SetValue(MonocraftGame.TextureManager.Textures[texture.Replace("minecraft:", "")]);

                pass.Apply();

                MonocraftGame.Instance.GraphicsDevice.DrawUserPrimitives(
                        PrimitiveType.TriangleList,
                        vertexBuffer.ToArray(),
                        0,
                        vertexBuffer.ToArray().Length / 3
                    );
            }*/
            //Console.WriteLine($"Drawcalls: {count}");
        }
    }
}
