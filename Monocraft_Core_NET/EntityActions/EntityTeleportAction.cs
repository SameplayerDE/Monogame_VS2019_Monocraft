using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET.EntityActions
{
    public class EntityTeleportAction : EntityAction
    {

        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly double Yaw;
        public readonly double Pitch;
        public readonly bool OnGround;

        public EntityTeleportAction(int entityID, double x, double y, double z, float yaw, float pitch, bool onGround) : base(entityID)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

    }
}
