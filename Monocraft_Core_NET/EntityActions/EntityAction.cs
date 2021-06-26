using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET.EntityActions
{
    public abstract class EntityAction
    {
        public readonly int EntityID;

        public EntityAction(int entityID)
        {
            EntityID = entityID;
        }

    }
}
