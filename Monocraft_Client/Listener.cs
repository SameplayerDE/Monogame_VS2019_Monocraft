using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocraft_Client_NET.Events;
using Monocraft_Client_NET.Interfaces;

namespace Monocraft_Client_NET
{
    public class Listener : IMouseMoveListener, IKeyboardKeyListener
    {
        private MonocraftGame _game;

        public Listener(MonocraftGame game)
        {
            _game = game;
            Orium.RegisterListener(this, _game);
        }

        public void OnKeyDown(KeyboardKeyEventArgs args)
        {
            Console.WriteLine(args.ToString());
        }

        public void OnKeyUp(KeyboardKeyEventArgs args)
        {
            Console.WriteLine(args.ToString());
        }

        public void OnMouseMoveEvent(MouseMoveEventArgs args)
        {
            
        }
    }
}
