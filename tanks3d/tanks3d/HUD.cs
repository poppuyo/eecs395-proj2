using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace tanks3d
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HUD : DrawableGameComponent
    {

        SpriteFont hudFont;
        SpriteBatch spriteBatch;

        protected Game1 game;
        protected Cameras.FPSCamera camera;

        public HUD(Game g)
            : base(g)
        {
            game = (Game1)g;
        }

        protected override void LoadContent()
        {
            hudFont = Game.Content.Load<SpriteFont>("hudFont");
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            string text;
            string temp;
            string tab = "     ";

            spriteBatch.Begin();

            /*
            temp = String.Format("{0:000},{1:000},{2:000}", game.worldCamera.Position.X, game.worldCamera.Position.Y, game.worldCamera.Position.Z);
            text = "Camera Position: (" + temp + ")\n";
            spriteBatch.DrawString(hudFont, text, new Vector2(0, 0), Color.Black);

            temp = String.Format("{0:000},{1:000},{2:000}", game.worldCamera.LookAt.X, game.worldCamera.LookAt.Y, game.worldCamera.LookAt.Z);
            text = "LookAt Position: (" + temp + ")\n";
            spriteBatch.DrawString(hudFont, text, new Vector2(0, 20), Color.Black);

            temp = String.Format("{0:000},{1:000},{2:000}", MathHelper.ToDegrees(game.worldCamera.Angle.X), MathHelper.ToDegrees(game.worldCamera.Angle.Y), MathHelper.ToDegrees(game.worldCamera.Angle.Z));
            text = "Rotation (Angle): (" + temp + ")\n";
            spriteBatch.DrawString(hudFont, text, new Vector2(0, 40), Color.Black);
             */ 

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}
