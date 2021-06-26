using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Monocraft_Client_NET.Events;

namespace Monocraft_Client_NET.Module
{
    public interface IInputModule : IUpdateable
    {

        bool OnKeyDownEvent(GameTime gameTime, KeyboardKeyEventArgs e);
        bool OnKeyUpEvent(GameTime gameTime, KeyboardKeyEventArgs e);
        bool OnMouseMoveEvent(GameTime gameTime, MouseMoveEventArgs e);

    }
}
