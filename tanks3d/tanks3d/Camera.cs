using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace tanks3d
{
    public class Camera : DrawableGameComponent
    {
        private static Camera activeCamera = null;

        // View and projection
        private Matrix projection = Matrix.Identity;
        private Matrix view = Matrix.Identity;

        //
        private Vector3 position = new Vector3(0, 0, 1000);
        private Vector3 angle = new Vector3();
        private float speed = 250f;
        private float turnSpeed = 90f;

        public static Camera ActiveCamera
        {
            get { return activeCamera; }
            set { activeCamera = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public Camera(Game game)
            : base(game)
        {
            if (ActiveCamera == null)
                ActiveCamera = this;
        }

        public override void Initialize()
        {
            int centerX = Game.Window.ClientBounds.Width / 2;
            int centerY = Game.Window.ClientBounds.Width / 2;

            Mouse.SetPosition(centerX, centerY);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            float ratio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, ratio, 10, 10000);

        }

        public override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            int centerX = Game.Window.ClientBounds.Width / 2;
            int centerY = Game.Window.ClientBounds.Width / 2;

            Mouse.SetPosition(centerX, centerY);

            angle.X += MathHelper.ToRadians((mouse.Y - centerY) * turnSpeed * 0.01f); // pitch
            angle.Y += MathHelper.ToRadians((mouse.X - centerX) * turnSpeed * 0.01f); // yaw

            Vector3 forward = Vector3.Normalize(new Vector3((float)Math.Sin(-angle.Y), (float)Math.Sin(angle.X), (float)Math.Cos(-angle.Y)));
            Vector3 left = Vector3.Normalize(new Vector3((float)Math.Cos(angle.Y), 0f, (float)Math.Sin(angle.Y)));

            if (keyboard.IsKeyDown(Keys.Up))
                position -= forward * speed * delta;

            if (keyboard.IsKeyDown(Keys.Down))
                position += forward * speed * delta;

            if (keyboard.IsKeyDown(Keys.Right))
                position -= left * speed * delta;

            if (keyboard.IsKeyDown(Keys.Left))
                position += left * speed * delta;

            if (keyboard.IsKeyDown(Keys.PageUp))
                position += Vector3.Down * speed * delta;

            if (keyboard.IsKeyDown(Keys.PageDown))
                position += Vector3.Up * speed * delta;

            if (keyboard.IsKeyDown(Keys.Escape))
                Game.Exit();

            view = Matrix.Identity;
            view *= Matrix.CreateTranslation(-position);
            view *= Matrix.CreateRotationZ(angle.Z);
            view *= Matrix.CreateRotationY(angle.Y);
            view *= Matrix.CreateRotationX(angle.X);

            base.Update(gameTime);
        }
    }

}
