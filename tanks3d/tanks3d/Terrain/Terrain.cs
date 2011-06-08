using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d.Terrain
{
    public class Terrain : DrawableGameComponent
    {
        private Game1 game;

        Model terrain;
        Texture2D terrainTexture;
        Texture2D decalTexture;
        BasicEffect terrainEffect;
        public HeightMapInfo heightMapInfo;
        Sky sky;

        Effect terrainDecalEffect;

        private List<Vector3> explosionDecalLocations;

        public const int MaxDecals = 10;

        public Terrain(Game1 g) : base(g) 
        {
            game = g;
            explosionDecalLocations = new List<Vector3>();
        }

        protected override void LoadContent()
        {
            terrain = game.Content.Load<Model>("terrain");
            terrainTexture = game.Content.Load<Texture2D>("rocks");
            decalTexture = game.Content.Load<Texture2D>("explosion_decal");

            LoadTerrainEffects();

            foreach (ModelMesh mesh in terrain.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Set the terrain texture
                    // TODO: This part will have to go in the draw loop if we want the terrain to have
                    // multiple textures. See: http://forums.create.msdn.com/forums/p/70607/432670.aspx
                    // In fact, right now this next line of code is kind of stupid because it will result
                    // the global terrainEffect to have the texture associated with the last mesh part
                    // in the terrain mesh (because it keeps getting overwritten in the loop). Setting the
                    // texture for the current mesh part is something that should be done when you
                    // draw the terrain, as described in above forum post.
                    terrainEffect.Texture = ((BasicEffect)meshPart.Effect).Texture;
                    terrainEffect.TextureEnabled = true;

                    meshPart.Effect = terrainEffect.Clone();
                }
            }

            // The terrain processor attached a HeightMapInfo to the terrain model's
            // Tag. We'll save that to a member variable now, and use it to
            // calculate the terrain's heights later.
            heightMapInfo = terrain.Tag as HeightMapInfo;
            if (heightMapInfo == null)
            {
                string message = "The terrain model did not have a HeightMapInfo " +
                    "object attached. Are you sure you are using the " +
                    "TerrainProcessor?";
                throw new InvalidOperationException(message);
            }

            sky = game.Content.Load<Sky>("sky");
        }

        protected void LoadTerrainEffects()
        {
            // Load the regular terrain effect

            terrainEffect = new BasicEffect(GraphicsDevice);
            terrainEffect.EnableDefaultLighting();

            // Set the specular lighting to match the sky color.
            terrainEffect.SpecularColor = new Vector3(0.6f, 0.4f, 0.2f);
            terrainEffect.SpecularPower = 8;

            // Set the fog to match the distant mountains
            // that are drawn into the sky texture.
            terrainEffect.FogEnabled = false;
            terrainEffect.FogColor = new Vector3(0.15f);
            terrainEffect.FogStart = 100 * 2;
            terrainEffect.FogEnd = 320 * 5;

            // Load the terrain effect for rendering decals
            terrainDecalEffect = game.Content.Load<Effect>("decals");
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawTerrain(game.worldCamera.ViewMatrix, game.worldCamera.ProjectionMatrix);

            if (!game.WireframeMode)
            {
                sky.Draw(game.worldCamera.ViewMatrix, game.worldCamera.ProjectionMatrix);
            }
        }

        /// <summary>
        /// Helper for drawing the terrain model.
        /// </summary>
        void DrawTerrain(Matrix view, Matrix projection)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;

            foreach (ModelMesh mesh in terrain.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    //terrainEffect.Texture = terrainTexture;
                    //terrainEffect.TextureEnabled = true;

                    terrainEffect.View = view;
                    terrainEffect.Projection = projection;
                    meshPart.Effect = terrainEffect;
                }

                mesh.Draw();
            }

            ApplyDecals(view, projection);
        }

        void ApplyDecals(Matrix view, Matrix projection)
        {
            if (explosionDecalLocations.Any())
            {
                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                terrainDecalEffect.CurrentTechnique = terrainDecalEffect.Techniques["Decal"];

                terrainDecalEffect.Parameters["View"].SetValue(game.worldCamera.ViewMatrix);
                terrainDecalEffect.Parameters["Projection"].SetValue(game.worldCamera.ProjectionMatrix);
                terrainDecalEffect.Parameters["World"].SetValue(Matrix.Identity);
                terrainDecalEffect.Parameters["LightDirection"].SetValue(new Vector3(-0.45f, -0.25f, -1.0f));
                terrainDecalEffect.Parameters["Ambient"].SetValue(0.1f);
                terrainDecalEffect.Parameters["EnableLighting"].SetValue(true);
                terrainDecalEffect.Parameters["Texture"].SetValue(terrainTexture);

                foreach (Vector3 explosionLocation in explosionDecalLocations)
                {
                    terrainDecalEffect.Parameters["DecalTexture"].SetValue(decalTexture);
                    terrainDecalEffect.Parameters["DecalCenter"].SetValue(explosionLocation);
                    terrainDecalEffect.Parameters["DecalRadius"].SetValue(100.0f);

                    foreach (ModelMesh mesh in terrain.Meshes)
                    {
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = terrainDecalEffect;
                        }

                        mesh.Draw();
                    }
                }
            }
        }

        /// <summary>
        /// Overlays an explosion decal over the terrain at the specified location. The Y-coordinate of
        /// the location is ignored since it will be read from the terrain heightmap. If the number of
        /// decals on the terrain exceeds MaxDecals, then the oldest decal will be removed.
        /// </summary>
        public void AddExplosionDecal(Vector3 location)
        {
            if (explosionDecalLocations.Count >= MaxDecals)
            {
                explosionDecalLocations.RemoveAt(0);
            }

            explosionDecalLocations.Add(location);
        }

        /// <summary>
        /// This function adjusts the given input position so that it is above the terrain (by at
        /// least the amount specified by the second argument). If the given position is already above
        /// the terrain, or it is not on the terrain at all, then it is returned unmodified.
        /// </summary>
        public Vector3 AdjustForTerrainHeight(Vector3 position, float minDistanceAboveTerrain = 0.0f)
        {
            if (heightMapInfo.IsOnHeightmap(position))
            {
                float height;
                Vector3 normal;
                heightMapInfo.GetHeightAndNormal(position, out height, out normal);

                if (position.Y <= height + minDistanceAboveTerrain)
                {
                    return new Vector3(position.X, height + minDistanceAboveTerrain, position.Z);
                }
            }

            return position;
        }
    }
}
