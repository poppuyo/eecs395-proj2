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

        Texture2D healthBar, heart, powerBar, power;

        protected Game1 game;

        public HUD(Game g)
            : base(g)
        {
            game = (Game1)g;
        }

        protected override void LoadContent()
        {
            hudFont = Game.Content.Load<SpriteFont>("hudFont");
            healthBar = Game.Content.Load<Texture2D>("health bar");
            heart = Game.Content.Load<Texture2D>("heart");
            powerBar = Game.Content.Load<Texture2D>("power bar");
            power = Game.Content.Load<Texture2D>("power");

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
            spriteBatch.Begin();

            switch (game.currentState1)
            {
                case Game1.GameState1.Menu:
                    string hudString;
                    hudString = "Welcome to Tanks 3D!\n";
                    hudString += "By: Sergey, John, Jason, Josiah\n";
                    hudString += "For Ian's EECS395 Class!\n\n";

                    hudString += "Controls:\n";
                    hudString += "--== Keys/Buttons ==--\n";
                    hudString += "WASD (movement)\n";
                    hudString += "T (aim)\n";
                    hudString += "F (fire)\n";
                    hudString += "C (bullet view)\n";
                    spriteBatch.DrawString(hudFont, hudString, new Vector2(75, 125), Color.LimeGreen);
                    break;
                case Game1.GameState1.Play:
                    string text;

                    hudString = String.Format("{0:F2},{1:F2},{2:F2}", game.worldCamera.Position.X, game.worldCamera.Position.Y, game.worldCamera.Position.Z);
                    text = "Camera Position: (" + hudString + ")\n";
                    spriteBatch.DrawString(hudFont, text, new Vector2(0, 20), Color.Black);
                        
                    Vector3 LookAtDirection = game.worldCamera.ViewDirection;
                    hudString = String.Format("{0:F2},{1:F2},{2:F2}", LookAtDirection.X, LookAtDirection.Y, LookAtDirection.Z);
                    text = "LookAt Direction: (" + hudString + ")\n";
                    spriteBatch.DrawString(hudFont, text, new Vector2(0, 40), Color.Black);

                    spriteBatch.Draw(heart, new Vector2(game.GraphicsDevice.Viewport.Width - 245, 5), Color.White);
                    spriteBatch.Draw(healthBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 210, 10, game.currentTank.health * 2, 12), Color.White);
                    spriteBatch.Draw(power, new Vector2(game.GraphicsDevice.Viewport.Width - 245, 30), Color.White);
                    spriteBatch.Draw(powerBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 210, 35, game.currentTank.power * 2, 12), Color.White);

                    game.DoSpriteBatchFix();
                    break;
                case Game1.GameState1.Pause:
                    hudString = "Press 'P' to unpause, or 'Escape' to quit.\n";
                    spriteBatch.DrawString(hudFont, hudString, new Vector2(25, 220), Color.MediumVioletRed, 0.0f, new Vector2(0,0), 2.0f, SpriteEffects.None, 0.0f);
                    break;
                }

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
