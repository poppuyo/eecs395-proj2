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

        SpriteFont hudFont, hitFont, pauseFont;
        SpriteBatch spriteBatch;

        Texture2D healthBar, heart, powerBar, power, movementBar, movement;

        public int hitTimer = 101, playerTimer = 101, lastPlayerEliminated;

        protected Game1 game;

        public HUD(Game g)
            : base(g)
        {
            game = (Game1)g;
        }

        protected override void LoadContent()
        {
            hudFont = Game.Content.Load<SpriteFont>("Fonts\\hudFont");
            hitFont = Game.Content.Load<SpriteFont>("Fonts\\hitFont");
            pauseFont = Game.Content.Load<SpriteFont>("Fonts\\hitFont");
            healthBar = Game.Content.Load<Texture2D>("health bar");
            heart = Game.Content.Load<Texture2D>("heart");
            powerBar = Game.Content.Load<Texture2D>("power bar");
            power = Game.Content.Load<Texture2D>("power");
            movementBar = Game.Content.Load<Texture2D>("movement bar");
            movement = Game.Content.Load<Texture2D>("movement");

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

            switch (game.gameState)
            {
                case GameState.Menu:
                    string hudString;
                    hudString = "Welcome to Tanks 3D!\n";
                    hudString += "By: Sergey, John, Jason, Josiah\n\n";

                    hudString += "Controls:\n";
                    hudString += "--== Keys/Buttons ==--\n";
                    hudString += "Mouse (aim the turret when in aiming mode)\n";
                    hudString += "Mouse scroll (zoom the camera in and out)\n";
                    hudString += "WASD (movement)\n";
                    hudString += "T (aiming mode)\n";
                    hudString += "Spacebar (fire)\n";
                    hudString += "C (bullet view)\n";
                    hudString += "P (pause)\n";
                    hudString += "Press 'H' to play the game\n";
                    spriteBatch.DrawString(hudFont, hudString, new Vector2(25, 25), Color.DarkBlue);
                    break;
                case GameState.Play:
                    hudString = "Current Player: " + "Player " + (game.currentPlayer + 1);
                    spriteBatch.DrawString(hudFont, hudString, new Vector2(5, 5), Color.DarkBlue);

                    spriteBatch.Draw(heart, new Vector2(game.GraphicsDevice.Viewport.Width - 245, 5), Color.White);
                    spriteBatch.Draw(healthBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 210, 10, game.currentTank.health * 2, 12), Color.White);
                    spriteBatch.Draw(power, new Vector2(game.GraphicsDevice.Viewport.Width - 245, 30), Color.White);
                    spriteBatch.Draw(powerBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 210, 35, game.currentTank.power * 2, 12), Color.White);

                    int smallMoves = (int)((double)(game.currentTank.moveLimit - game.currentTank.moves) / 2.5);
                    spriteBatch.Draw(movement, new Vector2(game.GraphicsDevice.Viewport.Width - 245, 57), Color.White);
                    spriteBatch.Draw(movementBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 210, 60, smallMoves, 12), Color.White);

                    hudString = "Press 'H' to access the help menu";
                    spriteBatch.DrawString(hudFont, hudString, new Vector2(5, game.GraphicsDevice.Viewport.Height - 30), Color.DarkBlue);

                    if (playerTimer < 101)
                    {
                        DrawPlayerDeath(lastPlayerEliminated);
                        playerTimer++;
                    }

                    if (hitTimer < 101)
                    {
                        DrawHit();
                        hitTimer++;
                    }

                    game.DoSpriteBatchFix();
                    break;
                case GameState.Pause:
                    hudString = "Press 'P' to unpause, or 'Escape' to quit.\n";
                    spriteBatch.DrawString(pauseFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2 - game.GraphicsDevice.Viewport.Width / 4),
                        (game.GraphicsDevice.Viewport.Height - game.GraphicsDevice.Viewport.Height / 2)), Color.DarkBlue);
                    break;
                case GameState.End:
                    hudString = "Player " + (game.currentPlayer + 1) + " has won the game!\n";
                    hudString += "Press 'Escape' to quit the game.\n";
                    spriteBatch.DrawString(hudFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2 - game.GraphicsDevice.Viewport.Width / 4),
                        (game.GraphicsDevice.Viewport.Height - game.GraphicsDevice.Viewport.Height / 2)), Color.DarkBlue);
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

        private void DrawHit()
        {
            string hitString;
            hitString = "HIT!";
            spriteBatch.DrawString(hitFont, hitString, new Vector2((game.GraphicsDevice.Viewport.Width / 2),
                        (game.GraphicsDevice.Viewport.Height - game.GraphicsDevice.Viewport.Height / 2)), Color.YellowGreen);
        }

        public void DrawPlayerDeath(int num)
        {
            string deathString;
            deathString = "Player " + (num + 1) + " has been eliminated.";
            spriteBatch.DrawString(hitFont, deathString, new Vector2((game.GraphicsDevice.Viewport.Width / 2),
                        (game.GraphicsDevice.Viewport.Height - game.GraphicsDevice.Viewport.Height / 2) - 25), Color.DarkBlue);
        }
    }
}
