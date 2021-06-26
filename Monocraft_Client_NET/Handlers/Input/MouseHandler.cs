using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocraft_Client_NET.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Input
{
    public class MouseHandler : GameComponent
    {

        public event EventHandler<MouseMoveEventArgs> OnMove;

        private MouseState _mouseState = Mouse.GetState();

        public MouseHandler(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _mouseState = Process(Mouse.GetState());
            base.Update(gameTime);
        }

        private MouseState Process(MouseState current)
        {
            if (_mouseState.Position - current.Position != Point.Zero)
            {
                Point delta = current.Position - _mouseState.Position;
                MouseMoveEventArgs mouseEventArgs = new MouseMoveEventArgs(current.X, current.Y, delta.X, delta.Y);
                OnMove?.Invoke(this, mouseEventArgs);
                //FileConsole.WriteLine("MouseMoveEvent: " + mouseEventArgs.ToString());
            }

            return current;
        }

    }
}
