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
            game = g;
        }

        private Game1 game;    

        Model bullet;
        private Vector3 Position = Vector3.One;
        private Vector3 Velocity = Vector3.One;
        private float Rotation = 0.0f;
        float aspectRatio;

        protected override void LoadContent()
        {
            bullet = Game.Content.Load<Model>("Models\\SHOTGUN_CART_FBX");
            aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;
        }

        public override void Update(GameTime gameTime)
        {
           
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[bullet.Bones.Count];
            bullet.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in bullet.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(Rotation)
                        * Matrix.CreateScale(0.1f)
                        * Matrix.CreateTranslation(Position);
                    effect.View = game.worldCamera.ViewMatrix;
                    effect.Projection = game.worldCamera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

    }
}
