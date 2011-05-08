// A skeleton file for creating a new DrawableGameComponent.
// Don't forget to do Components.Add().

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d
{
    class SkeletonDrawableGameComponent : DrawableGameComponent
    {
        private Game1 game;

        public SkeletonDrawableGameComponent(Game1 g) : base(g) 
        {
            game = g;
        }

        protected override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}
