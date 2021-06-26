using Monocraft_Client_NET.Events;
using Monocraft_Client_NET.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Abstracts
{
    public abstract class Entity : IDamageable
    {
        public double Health { get; set; }

        public bool IsDead { get { return (Health <= 0); } }

        public void Damage(double amount)
        {
            Health -= amount;
            Orium.CallEvent(new EntityDamageEventArgs(this, amount));
        }

        public void Damage(double amount, Entity source)
        {
            Health -= amount;
            Orium.CallEvent(new EntityDamageEventArgs(this, amount));
        }
    }
}
