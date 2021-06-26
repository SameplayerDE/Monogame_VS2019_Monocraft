using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Events
{
    public class MouseEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the X coordinate for the event.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the Y coordinate for the event.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Creates new mouse event data.
        /// </summary>
        /// <param name="x">The X coordinate for the event.</param>
        /// <param name="y">The Y coordinate for the event.</param>
        public MouseEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{"{X: "}{X}{"; Y: "}{Y}{"}"}";
        }

    }
}
