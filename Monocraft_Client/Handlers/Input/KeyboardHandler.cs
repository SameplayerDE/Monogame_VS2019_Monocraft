using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Monocraft_Client_NET.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Monocraft_Client_NET.Input
{
    public class KeyboardHandler : GameComponent
    {
        public event EventHandler<KeyboardKeyEventArgs> OnKeyDown;
        public event EventHandler<KeyboardKeyEventArgs> OnKeyUp;

        private KeyboardState _keyboardState = Keyboard.GetState();

        public KeyboardHandler(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Process(Keyboard.GetState());
            base.Update(gameTime);
        }

        private void TriggerKeyEvent(Keys key, bool value)
        {
            KeyboardKeyEventArgs keyboardEventArgs = new KeyboardKeyEventArgs(key, value);
            if (value)
            {
                OnKeyDown?.Invoke(this, keyboardEventArgs);
            }
            else
            {
                OnKeyUp?.Invoke(this, keyboardEventArgs);
            }
            FileConsole.WriteLine("KeyboardKeyEvent: " + keyboardEventArgs.ToString());
        }

        private KeyboardState Process(KeyboardState current)
        {
            foreach(Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (_keyboardState.IsKeyDown(key) && current.IsKeyUp(key))
                {
                    TriggerKeyEvent(key, false);
                }
                else if (_keyboardState.IsKeyUp(key) && current.IsKeyDown(key))
                {
                    TriggerKeyEvent(key, true);
                }
            }
            return current;
        }
    }
}
