using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tanks3d
{
    class Reticle : DrawableGameComponent 
    {

        private Game1 game;

        public Reticle(Game1 g)
            : base(g)
        {
            game = g;
            Position = new Vector2(0, 0);
        }
        
        public Vector2 Position { get; set; }
        private Vector2 Offset;
        Texture2D sprite;
        SpriteBatch spriteBatch;

        protected override void LoadContent()
        {
            sprite = Game.Content.Load<Texture2D>("Reticle");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Offset = new Vector2((float)sprite.Width / 2f,
                                 (float)sprite.Height / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Position = new Vector2(mouse.X, mouse.Y);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite,
                             Position,
                             null,
                             Color.White,
                             0,
                             Offset,
                             1.0f,
                             SpriteEffects.None,
                             0);
            spriteBatch.End();
            game.DoSpriteBatchFix();
        }
    }
}
