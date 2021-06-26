using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Rendering.Renderer
{
    public abstract class Renderer<T> where T : RawMesh
    {
        protected Effect _effect = null;
        protected Vector3 _position = Vector3.Zero;
        protected Vector3 _rotation = Vector3.Zero;
        protected float _scale = 1f;

        public Effect Effect { get { return _effect; } set { _effect = value; } }

        public abstract void DrawModel(T mesh, Matrix view, Matrix projection, GameTime gameTime);
    }
}
