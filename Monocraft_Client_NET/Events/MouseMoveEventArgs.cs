﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Events
{
    /// <summary>
    /// Provides the event data for mouse movement events.
    /// </summary>
    public class MouseMoveEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Gets the X coordinate delta for the event.
        /// </summary>
        public int DeltaX { get; private set; }

        /// <summary>
        /// Gets the Y coordinate delta for the event.
        /// </summary>
        public int DeltaY { get; private set; }

        /// <summary>
        /// Creates new mouse movement event data.
        /// </summary>
        /// <param name="x">The X coordinate for the event.</param>
        /// <param name="y">The Y coordinate for the event.</param>
        /// <param name="deltaX">The X coordinate delta for the event.</param>
        /// <param name="deltaY">The Y coordinate delta for the event.</param>
        public MouseMoveEventArgs(int x, int y, int deltaX, int deltaY) : base(x, y)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public override string ToString()
        {
            return $"{"{X: "}{X}{"; Y: "}{Y}{"; DeltaX: "}{DeltaX}{"; DeltaY: "}{DeltaY}{"}"}";
        }
    }
}
