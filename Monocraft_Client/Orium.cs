using Microsoft.Xna.Framework;
using Monocraft_Client_NET.Events;
using Monocraft_Client_NET.Handlers.GameLogic;
using Monocraft_Client_NET.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET
{
    public static class Orium
    {

        public static void CallEvent(EventArgs args)
        {
            if (args is GameEventArgs)
            {

                foreach (GameComponent component in MonocraftGame.Instance.Components)
                {
                    if (component is GameHandler)
                    {
                        GameHandler gameHandler = component as GameHandler;
                        //gameHandler.OnEntityDamage?.Invoke();
                    }
                }

                if (args is EntityEventArgs)
                {

                }
            }
        }

        private static void RegisterGameListener(IListener listener, MonocraftGame game)
        {
            game.GameListener.Add(listener as IGameListener);
        }

        private static void RegisterInputListener(IListener listener, MonocraftGame game)
        {
            game.InputListener.Add(listener as IInputListener);
        }

        public static void RegisterListener(IListener listener, MonocraftGame game)
        {
            if (listener is IInputListener)
            {
                RegisterInputListener(listener, game);
            }
            else if (listener is IGameListener)
            {
                RegisterGameListener(listener, game);
            }
            
        }

    }
}
