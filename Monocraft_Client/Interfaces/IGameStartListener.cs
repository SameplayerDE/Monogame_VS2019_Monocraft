using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Interfaces
{
    public interface IGameStartListener : IGameListener
    {
        void OnGameStartEvent(EventArgs args);
    }
}
