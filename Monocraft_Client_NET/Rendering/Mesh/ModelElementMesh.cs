using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Core_NET.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Client_NET.Rendering.Mesh
{
    public class ModelElementMesh : IMesh
    {

        //private List<ModelElemenFaceMesh> _meshes = new List<ModelElemenFaceMesh>();
        
        //public List<ModelElemenFaceMesh> ModelElemenFaceMeshes { get { return _meshes; } }

        public Dictionary<string, List<VertexPositionTexture>> Faces = new Dictionary<string, List<VertexPositionTexture>>();

        private Element _element;

        public Element Element { get { return _element; } }

        public ModelElementMesh(Element element, bool allSidesSame)
        {
            _element = element;
            List<Vector3> verticies = new List<Vector3>();

            Vector3 from = new Vector3(element.From[0], element.From[1], element.From[2]);
            Vector3 to = new Vector3(element.To[0], element.To[1], element.To[2]);

            Vector3 height = new Vector3(0, to.Y - from.Y, 0);
            Vector3 width = new Vector3(to.X - from.X, 0, 0);
            Vector3 depth = new Vector3(0, 0, to.Z - from.Z);

            float maxX, maxY, maxZ;
            float minX, minY, minZ;

            maxX = Math.Max(from.X, to.X);
            maxY = Math.Max(from.Y, to.Y);
            maxZ = Math.Max(from.Z, to.Z);

            minX = Math.Min(from.X, to.X);
            minY = Math.Min(from.Y, to.Y);
            minZ = Math.Min(from.Z, to.Z);

            verticies.Add(from); // 0 
            verticies.Add(from + width); // 1 
            verticies.Add(from + (width + depth)); // 2
            verticies.Add(from + depth); // 3

            verticies.Add(from + height); // 4
            verticies.Add(from + width + height); // 5
            verticies.Add(from + (width + depth) + height); // 6
            verticies.Add(from + depth + height); // 7


            // Down 0, 1, 2, 3
            // Up 4, 5, 6, 7
            // North 0, 1, 5, 4
            // East 1, 3, 5, 7
            // South 3, 2, 6, 7
            // West 2, 0, 4, 6

            foreach (KeyValuePair<string, Face> keyValuePair in element.Faces)
            {

                ModelElemenFaceMesh modelElemenFaceMesh = new ModelElemenFaceMesh(keyValuePair.Key, keyValuePair.Value, verticies.ToArray());
                //_meshes.Add(modelElemenFaceMesh);

                string texturePath = Element.Parent.Textures[modelElemenFaceMesh.Face.Texture.Replace("#", "")];
                if (!Faces.ContainsKey(texturePath))
                {
                    Faces.Add(texturePath, new List<VertexPositionTexture>());
                }
                Faces[texturePath].AddRange(modelElemenFaceMesh.VertexData);

            }

            //foreach (ModelElemenFaceMesh modelElemenFaceMesh in _meshes)
           // {
             //   string texturePath = Element.Parent.Textures[modelElemenFaceMesh.Face.Texture.Replace("#", "")];
              //  if (!Faces.ContainsKey(texturePath))
              //  {
              //      Faces.Add(texturePath, new List<VertexPositionTexture>());
              //  }
              //  Faces[texturePath].AddRange(modelElemenFaceMesh.VertexData);
           // }
        }

    }
}
