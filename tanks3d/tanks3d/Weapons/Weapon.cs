using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Weapons
{
    public abstract class Weapon
    {
        protected Game1 game;

        public Weapon(Game1 game)
        {
            this.game = game;
        }

        public abstract Bullet Fire(float VelocityScale);
    }
}
