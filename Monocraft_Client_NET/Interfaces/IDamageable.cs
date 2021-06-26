using Monocraft_Client_NET.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Interfaces
{
    public interface IDamageable
    {

        void Damage(double amount);
        void Damage(double amount, Entity source);

        double Health { get; }
        bool IsDead { get; }

    }
}
