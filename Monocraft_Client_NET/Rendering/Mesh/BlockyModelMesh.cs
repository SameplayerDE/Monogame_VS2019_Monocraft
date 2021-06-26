using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Core_NET.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Client_NET.Rendering.Mesh
{
    public class BlockyModelMesh : IMesh
    {

        protected List<ModelElementMesh> _meshes = new List<ModelElementMesh>();
        private ResourceModel _resourceModel;

        public List<ModelElementMesh> ModelElementMeshes { get { return _meshes; } }

        public ResourceModel ResourceModel { get { return _resourceModel; } }

        public BlockyModelMesh(ResourceModel resourceModel, bool allSidesSame)
        {
            _resourceModel = resourceModel;

            if (resourceModel != null && (resourceModel.Elements != null) && resourceModel.Textures != null)
            {

                foreach (Element element in resourceModel.Elements)
                {
                    element.Parent = resourceModel;
                    ModelElementMesh modelElementMesh = new ModelElementMesh(element, allSidesSame);
                    _meshes.Add(modelElementMesh);
                }

            }

        }

    }
}
