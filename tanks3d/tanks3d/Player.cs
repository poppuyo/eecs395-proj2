using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tanks3d
{
    public class Player
    {
        protected Game1 game;
        private int health, power;

        public enum playerState
        {
            Init,
            Move,
            Aim,
            Fired,
            Final
        }

        public playerState myState;

        public Player(Game1 game)
        {
            this.game = game;
            health = 100;
            power = 50;
            myState = playerState.Init;
        }


    }
}
