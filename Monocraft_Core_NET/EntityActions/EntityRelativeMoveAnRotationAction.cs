using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET.EntityActions
{
    public class EntityRelativeMoveAnRotationAction : EntityAction
    {

        public readonly double DeltaX;
        public readonly double DeltaY;
        public readonly double DeltaZ;
        public readonly float Yaw;
        public readonly float Pitch;
        public readonly bool OnGround;

        public EntityRelativeMoveAnRotationAction(int entityID, double deltaX, double deltaY, double deltaZ, float yaw, float pitch, bool onGround) : base(entityID)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

    }
}
