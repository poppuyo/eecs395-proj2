using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tank3d;

namespace tanks3d
{
    public class Indicator : DrawableGameComponent
    {
        private Game1 game;

        public Tank owner;

        private Model model;

        public float t = 0.0f;
        public float angle = 0.0f;
        public float timeVaryingHeightOffset = 0.0f;
        public Vector3 offset = new Vector3(0, 150, 0);

        public Indicator(Game1 g, Tank owner)
            : base(g)
        {
            game = g;
            this.owner = owner;
        }

        public void LoadContentDamnit()
        {
            LoadContent();
        }

        protected override void LoadContent()
        {
            model = game.Content.Load<Model>("pyramid");
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            angle += 2.0f * elapsedSeconds;

            t += elapsedSeconds;
            float sint = (float) Math.Sin(5.0f * t);
            timeVaryingHeightOffset = 10.0f * sint * sint;
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix worldMatrix = Matrix.CreateScale(0.25f)
                * Matrix.CreateRotationX((float)Math.PI)
                * Matrix.CreateRotationY(angle)
                * Matrix.CreateTranslation(owner.Position + offset + timeVaryingHeightOffset * Vector3.Up);

            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = game.worldCamera.ViewMatrix;
                    effect.Projection = game.worldCamera.ProjectionMatrix;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.AmbientLightColor = owner.playerColor;
                    effect.DiffuseColor = owner.playerColor;
                    effect.SpecularColor = owner.playerColor;
                }
                mesh.Draw();
            }
        }
    }
}
