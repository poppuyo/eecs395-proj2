using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Weapons
{
    public class Weapon1 : Weapon
    {
        public Weapon1(Game1 game) : base(game) { }

        public override void Fire(float VelocityScale)
        {
            game.bulletManager.SpawnBullet(game.currentTank.TurretEndPosition, VelocityScale * game.currentTank.GetTurretDirection());
        }
    }
}
