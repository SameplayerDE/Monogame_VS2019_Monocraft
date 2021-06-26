using Monocraft_Client_NET.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Events
{
    public class EntityDamageEventArgs : EntityEventArgs
    {

        public double Damage { get; private set; }

        public EntityDamageEventArgs(Entity entity, double damage) : base(entity)
        {
            Damage = damage;
        }
    }
}
