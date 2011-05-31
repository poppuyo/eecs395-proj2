using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tanks3d.Weapons
{
    public enum WeaponTypes
    {
        Weapon1,
    }

    public class WeaponManager
    {
        public Dictionary<WeaponTypes, Weapon> Weapons = new Dictionary<WeaponTypes, Weapon>();

        protected Game1 game;

        public WeaponManager(Game1 game)
        {
            this.game = game;

            Weapons.Add(WeaponTypes.Weapon1, new Weapon1(game));
        }
    }
}
