using Microsoft.Xna.Framework;
using Monocraft_Client_NET.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Client_NET.Handlers.GameLogic
{
    public class GameHandler : GameComponent
    {

        private World _world;

        public event EventHandler<EntityDamageEventArgs> OnEntityDamage;

        public GameHandler(MonocraftGame game) : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            Process();
            base.Update(gameTime);
        }

        private void Process()
        {

        }

    }
}
