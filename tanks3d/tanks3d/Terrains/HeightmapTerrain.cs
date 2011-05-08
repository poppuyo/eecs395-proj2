/*
 * ****************************************************************************
 * 
 * This code is based on Riemer's "Terrain Creation Basics" Tutorial, found at:
 *     http://www.riemers.net/eng/Tutorials/XNA/Csharp/series1.php
 *     
 * This terrain is loaded from a heightmap stored in a bitmap file.
 * 
 * ****************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d.Terrains
{
    public class HeightmapTerrain : DrawableGameComponent, ITerrain
    {
        private Game1 game;

        private Effect effect;

        public static int terrainWidth = 4;
        public static int terrainHeight = 3;

        private VertexPositionColorNormal[] vertices;
        private VertexBuffer vertexBuffer;
        private Int16[] indices;
        private IndexBuffer indexBuffer;
        private float[,] heightData;

        public HeightmapTerrain(Game1 g)
            : base(g) 
        {
            this.game = g;
        }

        protected override void LoadContent()
        {
            effect = game.Content.Load<Effect>("effects");

            Texture2D heightMap = game.Content.Load<Texture2D>("heightmap");
            LoadHeightData(heightMap);
            SetUpVertices();
            SetUpIndices();
            CalculateNormals();
            CopyToBuffers();
        }

        private void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int z = 0; z < terrainHeight; z++)
                    heightData[x, z] = heightMapColors[x + z * terrainWidth].R / 5.0f;
        }

        private void SetUpVertices()
        {
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int z = 0; z < terrainHeight; z++)
                {
                    if (heightData[x, z] < minHeight)
                        minHeight = heightData[x, z];
                    if (heightData[x, z] > maxHeight)
                        maxHeight = heightData[x, z];
                }
            }

            vertices = new VertexPositionColorNormal[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int z = 0; z < terrainHeight; z++)
                {
                    vertices[x + z * terrainWidth].Position = new Vector3(x, heightData[x, z], -z);

                    if (heightData[x, z] < minHeight + (maxHeight - minHeight) / 4)
                        vertices[x + z * terrainWidth].Color = Color.Blue;
                    else if (heightData[x, z] < minHeight + (maxHeight - minHeight) * 2 / 4)
                        vertices[x + z * terrainWidth].Color = Color.Green;
                    else if (heightData[x, z] < minHeight + (maxHeight - minHeight) * 3 / 4)
                        vertices[x + z * terrainWidth].Color = Color.Brown;
                    else
                        vertices[x + z * terrainWidth].Color = Color.White;
                }
            }
        }

        private void SetUpIndices()
        {
            int numIndices = (terrainWidth - 1) * (terrainHeight - 1) * 6;
            indices = new Int16[numIndices];
            int counter = 0;
            for (int z = 0; z < terrainHeight - 1; z++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + z * terrainWidth;
                    int lowerRight = (x + 1) + z * terrainWidth;
                    int topLeft = x + (z + 1) * terrainWidth;
                    int topRight = (x + 1) + (z + 1) * terrainWidth;

                    indices[counter++] = (Int16)topLeft;
                    indices[counter++] = (Int16)lowerRight;
                    indices[counter++] = (Int16)lowerLeft;

                    indices[counter++] = (Int16)topLeft;
                    indices[counter++] = (Int16)topRight;
                    indices[counter++] = (Int16)lowerRight;
                }
            }
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        private void CopyToBuffers()
        {
            vertexBuffer = new VertexBuffer(game.GraphicsDevice, VertexPositionColorNormal.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(Int16), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = game.GraphicsDevice;

            float angle = 0.0f;
            Matrix worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f) * Matrix.CreateRotationY(angle) * Matrix.CreateScale(3.0f);
            effect.CurrentTechnique = effect.Techniques["Colored"];
            effect.Parameters["xView"].SetValue(game.worldCamera.ViewMatrix);
            effect.Parameters["xProjection"].SetValue(game.worldCamera.ProjectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            Vector3 lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            effect.Parameters["xLightDirection"].SetValue(lightDirection);
            effect.Parameters["xAmbient"].SetValue(0.1f);
            effect.Parameters["xEnableLighting"].SetValue(true);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.Indices = indexBuffer;
                device.SetVertexBuffer(vertexBuffer);
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, indices.Length / 3);
            }
        }

        public float GetHeightAt(int x, int z)
        {
            return heightData[x, z];
        }

        public float GetHeightAt(float x, float z)
        {
            // Interpolate

            int x1 = (int)x;
            int x2 = (int)x + 1;

            int z1 = (int)z;
            int z2 = (int)z + 1;

            float height_ll = heightData[x1, z1];
            float height_lh = heightData[x1, z2];
            float height_hl = heightData[x2, z1];

            float height_x = (height_ll + height_hl) / 2.0f;
            float height_y = (height_ll + height_lh) / 2.0f;

            return (height_x + height_y) / 2.0f;
        }

        public float GetHeightAt(Vector2 pos)
        {
            return GetHeightAt(pos.X, pos.Y);
        }

        public float GetHeightAt(Vector3 pos)
        {
            return GetHeightAt(pos.X, pos.Z);
        }
    }
}
