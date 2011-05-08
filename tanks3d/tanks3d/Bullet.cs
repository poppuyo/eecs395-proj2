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
    class Bullet : DrawableGameComponent
    {
         public Bullet(Game1 g)
            : base(g)
        {
        }

        Model bullet;
        private Vector3 Position = Vector3.One;
        private float Zoom = 10;
        private float RotationY = 0.0f;
        private float RotationX = 0.0f;
        private Matrix gameWorldRotation;

        protected override void LoadContent()
        {
            bullet = Game.Content.Load<Model>("Models\\tank");
        }

        public override void Update(GameTime gameTime)
        {
            gameWorldRotation =
                Matrix.CreateRotationX(MathHelper.ToRadians(RotationX)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(RotationY));
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[bullet.Bones.Count];
            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            bullet.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                aspectRatio, 1.0f, 10000.0f);
            Matrix view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, Zoom),
                Vector3.Zero, Vector3.Up);

            foreach (ModelMesh mesh in bullet.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = gameWorldRotation *
                        transforms[mesh.ParentBone.Index] *
                        Matrix.CreateTranslation(Position);
                }
                mesh.Draw();
            }
        }

    }
}
