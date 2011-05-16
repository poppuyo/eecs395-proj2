using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitives3D;

namespace tanks3d
{
    public class DrawUtils : DrawableGameComponent
    {
        private Game1 game;

        /*
        private SpriteBatch spriteBatch;
        private Texture2D whiteTexture;
        */

        public List<IPrimitive> primitives = new List<IPrimitive>();

        public interface IPrimitive
        {
            void Draw();
            
            bool Equals(IPrimitive other);
        }

        public class PrimitiveEqualityComparer : EqualityComparer<IPrimitive>
        {
            public override bool Equals(IPrimitive p1, IPrimitive p2)
            {
                return p1.Equals(p2);
            }

            public override int GetHashCode(IPrimitive p)
            {
                return p.GetHashCode();
            }
        }

        private struct Cube : IPrimitive, IEquatable<IPrimitive>
        {
            public GeometricPrimitive primitive;
            public Game1 game;
            public Matrix worldMatrix;
            public Vector3 position;
            public Color color;
            public float size;
            public bool initialized;

            public Cube(Vector3 pos, float size, Color color, Game1 g)
            {
                game = g;
                position = pos;
                this.size = size;
                this.color = color;

                initialized = false;
                worldMatrix = Matrix.Identity;
                primitive = null;
            }

            public void LoadContent()
            {
                primitive = new CubePrimitive(game.GraphicsDevice, size);
                worldMatrix = Matrix.CreateTranslation(position);
                initialized = true;
            }

            public void Draw()
            {
                if (!initialized)
                {
                    LoadContent();
                }

                primitive.Draw(worldMatrix, game.worldCamera.ViewMatrix, game.worldCamera.ProjectionMatrix, color);
            }

            public bool Equals(IPrimitive otherPrimitive)
            {
                if (otherPrimitive is Cube)
                {
                    Cube other = (Cube)otherPrimitive;

                    return (this.position == other.position && this.size == other.size && this.color == other.color);
                }

                return false;
            }
        }

        private struct Sphere : IPrimitive, IEquatable<IPrimitive>
        {
            public GeometricPrimitive primitive;
            public Game1 game;
            public Matrix worldMatrix;
            public Vector3 position;
            public Color color;
            public float size;
            public bool initialized;

            public Sphere(Vector3 pos, float size, Color color, Game1 g)
            {
                game = g;
                position = pos;
                worldMatrix = Matrix.CreateTranslation(pos);
                this.color = color;
                this.size = size;

                initialized = false;
                primitive = null;
            }

            public void LoadContent()
            {
                primitive = new SpherePrimitive(game.GraphicsDevice, size, 24);
                initialized = true;
            }

            public void Draw()
            {
                if (!initialized)
                {
                    LoadContent();
                }

                primitive.Draw(worldMatrix, game.worldCamera.ViewMatrix, game.worldCamera.ProjectionMatrix, color);
            }

            public bool Equals(IPrimitive otherPrimitive)
            {
                if (otherPrimitive is Sphere)
                {
                    Sphere other = (Sphere)otherPrimitive;

                    return (this.position == other.position && this.size == other.size && this.color == other.color);
                }

                return false;
            }
        }

        private struct Cylinder : IPrimitive, IEquatable<IPrimitive>
        {
            public GeometricPrimitive primitive;
            public Game1 game;
            public Matrix worldMatrix;
            public Vector3 position;
            public Color color;
            public float height;
            public float diameter;
            public bool initialized;

            public Cylinder(Vector3 CenterPosition, float height, float diameter, Color color, Game1 g)
            {
                game = g;
                position = CenterPosition;
                worldMatrix = Matrix.CreateTranslation(CenterPosition);
                this.color = color;
                this.height = height;
                this.diameter = diameter;
                
                initialized = false;
                primitive = null;
            }

            public void LoadContent()
            {
                primitive = new CylinderPrimitive(game.GraphicsDevice, height, diameter, 24);
                initialized = true;
            }

            /*
            public Cylinder(Vector3 CenterOfFirstBase, Vector3 CenterOfSecondBase, float diameter, Color color, Game1 g)
            {
                game = g;

                Vector3 diff = CenterOfSecondBase - CenterOfFirstBase;
                float length = diff.Length();

                Vector3 projYZ = new Vector3(0.0f, diff.Y, diff.Z);
                Vector3 projXZ = new Vector3(diff.X, 0.0f, diff.Z);
                Vector3 projXY = new Vector3(diff.X, diff.Y, 0.0f);

                float xrot = (float)Math.Acos(Vector3.Dot(Vector3.Up, projYZ / projYZ.Length()));
                float yrot = (float)Math.Acos(Vector3.Dot(Vector3.UnitX, projXZ / projXZ.Length()));
                float zrot = (float)Math.Acos(Vector3.Dot(Vector3.Up, projXY / projXY.Length()));

                primitive = new CylinderPrimitive(game.GraphicsDevice, length, diameter, 24);
                position = (CenterOfFirstBase + CenterOfSecondBase) / 2.0f;
                worldMatrix = Matrix.CreateRotationX(xrot) * Matrix.CreateRotationY(yrot) * Matrix.CreateRotationZ(zrot) * Matrix.CreateTranslation(position);
                this.color = color;
            }
             * */

            public void Draw()
            {
                if (!initialized)
                {
                    LoadContent();
                }

                primitive.Draw(worldMatrix, game.worldCamera.ViewMatrix, game.worldCamera.ProjectionMatrix, color);
            }

            public bool Equals(IPrimitive otherPrimitive)
            {
                if (otherPrimitive is Cylinder)
                {
                    Cylinder other = (Cylinder)otherPrimitive;
                    return (this.position == other.position
                    && this.height == other.height
                    && this.diameter == other.diameter
                    && this.color == other.color);
                }

                return false;
            }
        }

        private struct Line : IPrimitive, IEquatable<IPrimitive>
        {
            public Color color;
            public Game1 game;
            BasicEffect basicEffect;
            public VertexBuffer vertexBuffer;
            public Vector3 start;
            public Vector3 end;
            public bool initialized;

            public Line(Vector3 start, Vector3 end, Color color, Game1 g)
            {
                game = g;
                this.start = start;
                this.end = end;
                this.color = color;                

                initialized = false;
                basicEffect = null;
                vertexBuffer = null;
            }

            public void LoadContent()
            {
                basicEffect = new BasicEffect(game.GraphicsDevice);
                VertexPositionColor[] pointList = new VertexPositionColor[2];
                pointList[0] = new VertexPositionColor(start, color);
                pointList[1] = new VertexPositionColor(end, color);
                vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor),
                    2, BufferUsage.None);
                vertexBuffer.SetData(pointList.ToArray());
                initialized = true;
            }

            public void Draw()
            {
                if (!initialized)
                {
                    LoadContent();
                }

                basicEffect.LightingEnabled = false;
                basicEffect.World = Matrix.Identity;
                basicEffect.View = game.worldCamera.ViewMatrix;
                basicEffect.Projection = game.worldCamera.ProjectionMatrix;
                basicEffect.DiffuseColor = color.ToVector3();
                
                GraphicsDevice device = basicEffect.GraphicsDevice;
                device.SetVertexBuffer(vertexBuffer);

                foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();
                    device.DrawPrimitives(PrimitiveType.LineList, 0, 1);
                }
            }

            public bool Equals(IPrimitive otherPrimitive)
            {
                if (otherPrimitive is Line)
                {
                    Line other = (Line)otherPrimitive;
                    return (this.start == other.start && this.end == other.end && this.color == other.color);
                }

                return false;
            }
        }

        public DrawUtils(Game1 g) : base(g)
        {
            game = g;
        }

        public PrimitiveEqualityComparer primitiveEqualityComparer = new PrimitiveEqualityComparer();

        protected override void LoadContent()
        {
            /*
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            whiteTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
             * */
        }

        public void DrawCube(Vector3 pos, float size, Color color)
        {
            Cube cube = new Cube(pos, size, color, game);

            if (!primitives.Contains(cube, primitiveEqualityComparer))
            {
                primitives.Add(cube);
            }
        }

        public void DrawSphere(Vector3 pos, float size, Color color)
        {
            Sphere sphere = new Sphere(pos, size, color, game);

            if (!primitives.Contains(sphere, primitiveEqualityComparer))
            {
                primitives.Add(sphere);
            }
        }

        public void DrawCylinder(Vector3 pos, float height, float diameter, Color color)
        {
            Cylinder cylinder = new Cylinder(pos, height, diameter, color, game);

            if (!primitives.Contains(cylinder, primitiveEqualityComparer))
            {
                primitives.Add(cylinder);
            }
        }

        /*
        public void DrawCylinder(Vector3 CenterOfFirstBase, Vector3 CenterOfSecondBase, float diameter, Color color)
        {
            primitives.Add(new Cylinder(CenterOfFirstBase, CenterOfSecondBase, diameter, color, game));
        }
        */

        public void DrawLine(Vector3 startPos, Vector3 endPos, Color color)
        {
            Line line = new Line(startPos, endPos, color, game);

            if (!primitives.Contains(line, primitiveEqualityComparer))
            {
                primitives.Add(line);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin();

            foreach (IPrimitive primitive in primitives)
            {
                primitive.Draw();
            }

            //spriteBatch.End();
            //game.DoSpriteBatchFix();
        }
    }
}
