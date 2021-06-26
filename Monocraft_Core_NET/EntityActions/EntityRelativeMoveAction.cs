using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET.EntityActions
{
    public class EntityRelativeMoveAction : EntityAction
    {

        public readonly double DeltaX;
        public readonly double DeltaY;
        public readonly double DeltaZ;
        public readonly bool OnGround;

        public EntityRelativeMoveAction(int entityID, double deltaX, double deltaY, double deltaZ, bool onGround) : base(entityID)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
            OnGround = onGround;
        }

    }
}
