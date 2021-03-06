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

        SpriteFont GUIFont;
        SpriteBatch spriteBatch;

        Texture2D healthBar, heart, powerBar, power, movementBar,
            movement, hudSmallBox, hudLargeBox, tankBackground;

        public int hitTimer = 181, playerTimer = 241, lastPlayerEliminated;

        public bool showScoreBoard = false;

        protected Game1 game;

        public HUD(Game g)
            : base(g)
        {
            game = (Game1)g;
        }

        protected override void LoadContent()
        {
            GUIFont = Game.Content.Load<SpriteFont>("Fonts\\GUIFont");
            healthBar = Game.Content.Load<Texture2D>("health bar");
            heart = Game.Content.Load<Texture2D>("heart");
            powerBar = Game.Content.Load<Texture2D>("power bar");
            power = Game.Content.Load<Texture2D>("power");
            movementBar = Game.Content.Load<Texture2D>("movement bar");
            movement = Game.Content.Load<Texture2D>("movement");
            hudSmallBox = Game.Content.Load<Texture2D>("HUDSmallBox");
            hudLargeBox = Game.Content.Load<Texture2D>("HUDLargeBox");
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
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 106, 0), Color.DarkGreen);
                    hudString = "\n\n\nBy: Sergey, John, Jason, and Josiah";
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 184, 0), Color.DarkGreen);
                    
                    hudString = "\n\n\n\n\n\n-=Controls=-\n";
                    hudString += "WASD\n";
                    hudString += "Space\n";
                    hudString += "P (or H)\n";
                    hudString += "Tab\n";
                    hudString += "Enter";

                    hudString += "\nMouse\n";
                    hudString += "\n\n\nSelect a number of players on the keyboard to begin\n";
                    hudString += "the game. Games can have between two (2) and ten (0)\nplayers.";
                    hudString += "\nSUDDEN DEATH MODE: Press F2 to F10 for a faster game!";

                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 250, 0), Color.DarkGreen);

                    hudString = "\n\n\n\n\n\n-=Action=-\n";
                    hudString += "(movement)\n";
                    hudString += "(fire)\n";
                    hudString += "(pause)\n";
                    hudString += "Display scores\n";
                    hudString += "Switch player (without firing)";

                    hudString += "\n(aim turret)\n";

                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) + 0, 0), Color.DarkGreen);
                    break;
                case GameState.Play:
                    spriteBatch.Draw(hudSmallBox, new Rectangle((game.GraphicsDevice.Viewport.Width / 2) - 150, 0, 300, 30), Color.White);
                    hudString = "Current Player: " + "Player " + (game.currentPlayer + 1);
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 127, 5), new Color(255, 243, 141));

                    spriteBatch.Draw(heart, new Vector2(game.GraphicsDevice.Viewport.Width - 237, 5), Color.White);
                    spriteBatch.Draw(healthBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 205, 10, game.currentTank.health * 2, 12), Color.White);
                    spriteBatch.Draw(power, new Vector2(game.GraphicsDevice.Viewport.Width - 237, 30), Color.White);
                    spriteBatch.Draw(powerBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 205, 35, game.currentTank.power * 2, 12), Color.White);

                    int smallMoves = (int)((double)(game.currentTank.moveLimit - game.currentTank.moves) / game.currentTank.moveLimit * 200);
                    spriteBatch.Draw(movement, new Vector2(game.GraphicsDevice.Viewport.Width - 237, 57), Color.White);
                    spriteBatch.Draw(movementBar, new Rectangle(game.GraphicsDevice.Viewport.Width - 205, 60, smallMoves, 12), Color.White);

                    hudString = "Press 'H' to access the help menu";
                    spriteBatch.Draw(hudSmallBox, new Rectangle(0, (Game.GraphicsDevice.Viewport.Height - 30),
                        (11 * hudString.Length), 30), Color.White);
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2(13, (Game.GraphicsDevice.Viewport.Height - 
                        25)), new Color(255, 243, 141));

                    if (hitTimer < 181)
                    {
                        DrawHit();
                        hitTimer++;
                    }

                    if (playerTimer < 241)
                    {
                        DrawPlayerDeath(lastPlayerEliminated);
                        playerTimer++;
                    }

                    game.DoSpriteBatchFix();
                    break;
                case GameState.Pause:
                    spriteBatch.Draw(tankBackground, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
                    hudString = "\n\nPress 'P' or 'H' to unpause, or 'Escape' to quit.\n";
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 300, 0), Color.DarkGreen);
                    
                    hudString = "\n\n\n\n\n\n-=Controls=-\n";
                    hudString += "WASD\n";
                    hudString += "Space\n";
                    hudString += "P\n";
                    hudString += "Tab\n";
                    hudString += "Enter";

                    hudString += "\nMouse\n";

                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 300, 0), Color.DarkGreen);

                    hudString = "\n\n\n\n\n\n-=Action=-\n";
                    hudString += "(movement)\n";
                    hudString += "(fire)\n";
                    hudString += "(unpause)\n";
                    hudString += "Display scores\n";
                    hudString += "Switch player (without firing)";

                    hudString += "\n(aim turret)";

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
                    spriteBatch.Draw(hudSmallBox, new Rectangle(game.GraphicsDevice.Viewport.Width / 2 - 175, game.GraphicsDevice.Viewport.Height / 2 - 30, 350, 60), Color.White);
                    spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 2 - 167),
                        game.GraphicsDevice.Viewport.Height / 2 - 22), new Color(255, 243, 141));
                    break;
                }

            if (showScoreBoard)
            {
                hudString = "";
                for (int i = 0; i < game.numPlayers; i++)
                {
                    if(game.tanks[i].health>0)
                        hudString += "Player " + (i + 1) + " : " + game.tanks[i].health + "/100 \n";
                    else
                        hudString += "Player " + (i + 1) + " : Dead! \n";
                }
                spriteBatch.Draw(hudLargeBox, new Rectangle((game.GraphicsDevice.Viewport.Width / 8),
                        (game.GraphicsDevice.Viewport.Height / 4), 200, 235), Color.White);
                spriteBatch.DrawString(GUIFont, hudString, new Vector2((game.GraphicsDevice.Viewport.Width / 8) + 5,
                        (game.GraphicsDevice.Viewport.Height / 4) + 5), new Color(255, 243, 141));
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ShowControls()
        {
            string hudString;
            hudString = "-=Keys=-\n";
            hudString += "WASD\n";
            hudString += "T\n";
            hudString += "C\n";
            hudString += "Space\n";
            hudString += "P\n";

            hudString += "\n";
            hudString += "MouseWheel";

            spriteBatch.DrawString(GUIFont, hudString, new Vector2(25f, 100f), Color.DarkBlue);

            hudString = "-=Action=-\n";
            hudString += "(movement)\n";
            hudString += "(aim)\n";
            hudString += "(turret view)\n";
            hudString += "(press and hold to fire)\n";
            hudString += "(unpause)\n";

            hudString += "\n";
            hudString += "(zoom, where applicable)";
            spriteBatch.DrawString(GUIFont, hudString, new Vector2(200f, 100f), Color.DarkBlue);
        }

        private static string GetControlsString()
        {
            string hudString = "";

            hudString += "Controls:\n";
            hudString += "--== Keys/Buttons ==--\n";
            hudString += "Mouse (aim the turret when in aiming mode)\n";
            hudString += "Mouse scroll (zoom the camera in and out)\n";
            hudString += "WASD (movement)\n";

            hudString += "T (aim)\n";
            hudString += "C (turret view)\n";
            hudString += "Spacebar (fire)\n";
            hudString += "P (or H) (pause)\n\n";

            return hudString;
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
            spriteBatch.Draw(hudSmallBox, new Rectangle(game.GraphicsDevice.Viewport.Width / 2 - 25, game.GraphicsDevice.Viewport.Height / 2 - 15, 50, 30), Color.White);
            spriteBatch.DrawString(GUIFont, hitString, new Vector2((game.GraphicsDevice.Viewport.Width / 2 - 20),
                        game.GraphicsDevice.Viewport.Height / 2 - 10), new Color(255, 243, 141));
        }

        public void DrawPlayerDeath(int num)
        {
            string deathString;
            deathString = "Player " + (num + 1) + " has been eliminated.";
            spriteBatch.Draw(hudSmallBox, new Rectangle(game.GraphicsDevice.Viewport.Width / 2 - 165, game.GraphicsDevice.Viewport.Height / 2 - 15, 330, 30), Color.White);
            spriteBatch.DrawString(GUIFont, deathString, new Vector2((game.GraphicsDevice.Viewport.Width / 2 - 160),
                        game.GraphicsDevice.Viewport.Height / 2 - 10), new Color(255, 243, 141));
        }
    }
}
