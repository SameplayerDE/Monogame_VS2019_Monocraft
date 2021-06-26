using Monocraft_Client_NET.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Interfaces
{
    public interface IMouseButtonListener : IMouseListener
    {

        void OnMouseButtonEvent(MouseEventArgs args);

    }
}
