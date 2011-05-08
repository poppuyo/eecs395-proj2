using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Camera : DrawableGameComponent
    {
        public Matrix View, Projection;

        public Vector3 Position;


        public Camera(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            Position.X = -100f;
            Position.Y = 100f;
            Position.Z = 100f;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Down);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Up))
                Position.Y++;
            if (keyboard.IsKeyDown(Keys.Down))
                Position.Y--;
            if (keyboard.IsKeyDown(Keys.Right))
                Position.X++;
            if (keyboard.IsKeyDown(Keys.Left))
                Position.X--;
            if (keyboard.IsKeyDown(Keys.PageUp))
                Position.Z++;
            if (keyboard.IsKeyDown(Keys.PageDown))
                Position.Z--;

            View = Matrix.CreateLookAt(Position, Vector3.Zero, Vector3.Down);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
