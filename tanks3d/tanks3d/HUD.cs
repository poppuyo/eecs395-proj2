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

        SpriteFont hudFont, hitFont, pauseFont, GUIFont;
        SpriteBatch spriteBatch;

        Texture2D healthBar, heart, powerBar, power, movementBar,
            movement, hudTop, hudBottomRight, tankBackground;

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
            GUIFont = Game.Content.Load<SpriteFont>("Fonts\\GUIFont");
            healthBar = Game.Content.Load<Texture2D>("health bar");
            heart = Game.Content.Load<Texture2D>("heart");
            powerBar = Game.Content.Load<Texture2D>("power bar");
            power = Game.Content.Load<Texture2D>("power");
            movementBar = Game.Content.Load<Texture2D>("movement bar");
            movement = Game.Content.Load<Texture2D>("movement");
            hudTop = Game.Content.Load<Texture2D>("hudTop");
            hudBottomRight = Game.Content.Load<Texture2D>("HUDBottomRight");
            tankBackground = Game.Content.Load<Texture2D>("Images\\TankBackground");

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
            string hudString;

            spriteBatch.Begin();

            switch (game.gameState)
            {
                case GameState.Menu:
                    spriteBatch.Draw(tankBackground, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
                    hudString = "\n\nWelcome to Tanks 3D!\n";
                    hudString += "By: Sergey, John, Jason, Josiah\n\n";
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - hudString.Length * 3, 0), Color.DarkGreen);
                    
                    hudString = "\n\n\n\n\n\n-=Keys=-\n";
                    hudString += "WASD\n";
                    hudString += "T\n";
                    hudString += "Space\n";
                    hudString += "P\n";

                    hudString += "\n";
                    hudString += "Mouse\n";
                    hudString += "Mouse Wheel";

                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 250, 0), Color.DarkGreen);

                    hudString = "\n\n\n\n\n\n-=Action=-\n";
                    hudString += "(movement)\n";
                    hudString += "(enter aiming mode)\n";
                    hudString += "(fire)\n";
                    hudString += "(unpause)\n";

                    hudString += "\n";
                    hudString += "(aim turret, when applicable)\n";
                    hudString += "(zoom, when applicable)";

                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) + 0, 0), Color.DarkGreen);
                    break;
                case GameState.Play:
                    spriteBatch.Draw(hudBottomRight, new Rectangle((game.GraphicsDevice.Viewport.Width / 2) - 150, 0, 300, 30), Color.White);
                    hudString = "Current Player: " + "Player " + (game.currentPlayer + 1);
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 127, 5), new Color(255, 243, 141));

                    spriteBatch.Draw(heart, new Vector2(game.GraphicsDevice.Viewport.Width - 237, 5), Color.White);
                    spriteBatch.Draw(healthBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 205, 10, game.currentTank.health * 2, 12), Color.White);
                    spriteBatch.Draw(power, new Vector2(game.GraphicsDevice.Viewport.Width - 237, 30), Color.White);
                    spriteBatch.Draw(powerBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 205, 35, game.currentTank.power * 2, 12), Color.White);

                    int smallMoves = (int)((double)(game.currentTank.moveLimit - game.currentTank.moves) / 2.5);
                    spriteBatch.Draw(movement, new Vector2(game.GraphicsDevice.Viewport.Width - 237, 57), Color.White);
                    spriteBatch.Draw(movementBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 205, 60, smallMoves, 12), Color.White);

                    hudString = "Press 'H' to access the help menu";
                    spriteBatch.Draw(hudBottomRight, new Rectangle(0, (Game.GraphicsDevice.Viewport.Height - 30),
                        (11 * hudString.Length), 30), Color.White);
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2(13, (Game.GraphicsDevice.Viewport.Height - 
                        25)), new Color(255, 243, 141));

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
                    spriteBatch.Draw(tankBackground, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
                    hudString = "\n\nPress 'P' to unpause, or 'Escape' to quit.\n";
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - hudString.Length * 5, 0), Color.DarkGreen);
                    
                    hudString = "\n\n\n\n\n\n-=Keys=-\n";
                    hudString += "WASD\n";
                    hudString += "T\n";
                    hudString += "Space\n";
                    hudString += "P\n";

                    hudString += "\n";
                    hudString += "Mouse\n";
                    hudString += "Mouse Wheel";

                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 300, 0), Color.DarkGreen);

                    hudString = "\n\n\n\n\n\n-=Action=-\n";
                    hudString += "(movement)\n";
                    hudString += "(enter aiming mode)\n";
                    hudString += "(fire)\n";
                    hudString += "(unpause)\n";

                    hudString += "\n";
                    hudString += "(aim turret, when applicable)\n";
                    hudString += "(zoom, when applicable)";

                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) + 0, 0), Color.DarkGreen);         
                    break;
                case GameState.End:
                    for (int i = 0; i < game.numPlayers; i++)
                    {
                        if (game.tanks[i].IsAlive)
                            game.currentPlayer = i;
                    }
                    hudString = "Player " + (game.currentPlayer + 1) + " has won the game!\n";
                    hudString += "Press 'Escape' to quit the game.\n";
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2 - game.GraphicsDevice.Viewport.Width / 4),
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
