using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tanks3d
{
    public enum PlayerState
    {
        Start,
        Aim,
        Fired,
        Transition,
        Dying
    }

    public class Player
    {
        protected Game1 game;

        public PlayerState myState;

        public Player(Game1 game)
        {
            this.game = game;
            myState = PlayerState.Start;
        }
    }
}
