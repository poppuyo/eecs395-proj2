using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d.Terrains
{
    public class SimpleGridTerrain : DrawableGameComponent, ITerrain
    {
        private Game1 game;
        public int Width;
        public int Height;
        public int Rows;
        public int Columns;
        public Vector3 Position;
        List<VertexPositionColor> vertices;
        BasicEffect effect;

        public SimpleGridTerrain(Game1 g)
            : base(g) 
        {
            game = g;
            Position = new Vector3(0, 0, 0);
            Width = 1000;
            Height = 1000;
            Rows = 1000;
            Columns = 1000;

            vertices = new List<VertexPositionColor>();
            int xDiff = this.Width / this.Columns;
            int zDiff = this.Height / this.Rows;
            float xBase = this.Position.X - this.Width / 2f;
            float zBase = this.Position.Z - this.Height / 2f;
            float yBase = this.Position.Y;
            for (int i = 0; i <= this.Rows; i++)
            {
                vertices.Add(new VertexPositionColor(new Vector3(xBase + i * xDiff, yBase, zBase), Color.Green)); vertices.Add(new VertexPositionColor(new Vector3(xBase + i * xDiff, yBase, zBase + this.Height), Color.Green));
            }
            for (int i = 0; i <= this.Columns; i++)
            {
                vertices.Add(new VertexPositionColor(new Vector3(xBase, yBase, zBase + i * zDiff), Color.Green));
                vertices.Add(new VertexPositionColor(new Vector3(xBase + this.Width, yBase, zBase + i * zDiff), Color.Green));
            }
        }

        protected override void LoadContent()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.VertexColorEnabled = true;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.View = game.worldCamera.ViewMatrix;
                effect.Projection = game.worldCamera.ProjectionMatrix;

                //Draw vertices as Primitive
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices.ToArray(), 0, vertices.Count / 2);
            }
        }

        public float GetHeightAt(int x, int z)
        {
            return Position.Y;
        }

        public float GetHeightAt(float x, float z)
        {
            return Position.Y;
        }

        public float GetHeightAt(Vector2 pos)
        {
            return Position.Y;
        }

        public float GetHeightAt(Vector3 pos)
        {
            return Position.Y;
        }
    }
}
