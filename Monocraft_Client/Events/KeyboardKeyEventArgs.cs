using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Events
{
    public class KeyboardKeyEventArgs : KeyboardEventArgs
    {
        /// <summary>
        /// Gets the key for the event.
        /// </summary>
        public Keys Key { get; private set; }

        /// <summary>
        /// Gets whether the key was pressed or released.
        /// </summary>
        public bool IsPressed { get; private set; }

        /// <summary>
        /// Creates new keyboard key event data.
        /// </summary>
        /// <param name="key">The key for the event.</param>
        /// <param name="isPressed">Whether the key was pressed or released.</param>
        public KeyboardKeyEventArgs(Keys key, bool isPressed)
        {
            Key = key;
            IsPressed = isPressed;
        }

        public override string ToString()
        {
            return $"{"{Key: "}{Key}{"; IsPressed: "}{IsPressed}{"}"}";
        }

    }
}
