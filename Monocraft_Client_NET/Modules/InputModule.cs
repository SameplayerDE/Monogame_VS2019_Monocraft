using Microsoft.Xna.Framework;
using Monocraft_Client_NET.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Module
{
    public abstract class InputModule : IInputModule
    {
        public bool Enabled => throw new NotImplementedException();

        public int UpdateOrder => throw new NotImplementedException();

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public virtual bool OnKeyDownEvent(GameTime gameTime, KeyboardKeyEventArgs e)
        {
            return false;
        }

        public virtual bool OnKeyUpEvent(GameTime gameTime, KeyboardKeyEventArgs e)
        {
            return false;
        }

        public virtual bool OnMouseMoveEvent(GameTime gameTime, MouseMoveEventArgs e)
        {
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
