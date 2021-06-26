using Monocraft_Client_NET.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Events
{
    public class EntityEventArgs : GameEventArgs
    {

        public Entity Entity { get; private set; }

        public EntityEventArgs(Entity entity)
        {
            Entity = entity;
        }

    }
}
