using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d
{
    public class Tank : DrawableGameComponent
    {
        Model tank;
        float aspectRatio;

        // Set the position of the model in world space, and set the rotation.
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;

        // Set the position of the camera in world space, for our view matrix.
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);

        private Game1 game;

        public Tank(Game1 g)
            : base(g)
        {
            game = g;
        }

        public override void Update(GameTime gameTime)
        {
           modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
           base.Update(gameTime);
        }

        protected override void LoadContent()
        {
 	        base.LoadContent();
            tank = Game.Content.Load<Model>("Models\\p1_wedge");
            aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        }

        public override void  Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[tank.Bones.Count];
            tank.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in tank.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * 
                        Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(modelPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition, 
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio, 
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
       }
    }
}
