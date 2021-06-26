using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocraft_Client_NET.Events;

namespace Monocraft_Client_NET.Interfaces
{
    public interface IEntityDamageListener : IEntityListener
    {
        void OnEntityDamageEvent(EntityDamageEventArgs args);
    }
}
