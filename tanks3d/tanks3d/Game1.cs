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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TexturedQuad.Quad[] ground;
        VertexDeclaration vertexDeclaration;

        Model terrain;
        HeightMapInfo heightMapInfo;
        Sky sky;

        public Cameras.FPSCamera worldCamera;
        public HUD mainHUD;

        Texture2D texture;
        BasicEffect quadEffect;

        Tank tank1;
        Bullet bullet1;

        public DrawUtils drawUtils;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //ground = new TexturedQuad.Quad[1];
            //ground[0] = new TexturedQuad.Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 64f, 64f);

            worldCamera = new Cameras.FPSCamera(graphics.GraphicsDevice.Viewport,
                new Vector3(64f, 0f, 64f), 0.0f, 0.0f);
            
            mainHUD = new HUD(this);
            Components.Add(mainHUD);

            drawUtils = new DrawUtils(this);
            Components.Add(drawUtils);

            //terrain = new Terrains.SimpleGridTerrain(this);
            //terrain = new Terrains.HeightmapTerrain(this);
            //Components.Add(terrain);

            tank1 = new Tank(this);
            Components.Add(tank1);

            //bullet1 = new Bullet(this);
            //Components.Add(bullet1);

            Reticle reticle = new Reticle(this);
            Components.Add(reticle);

            //View = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up);
            //aCamera.Position = new Vector3(-100, 100, 100);
            //aCamera.View = Matrix.CreateLookAt(new Vector3(-100, 100, 100), Vector3.Zero, Vector3.Up);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            terrain = Content.Load<Model>("terrain");

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

            sky = Content.Load<Sky>("sky");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("64x64");
            quadEffect = new BasicEffect(graphics.GraphicsDevice);
            quadEffect.EnableDefaultLighting();

            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;

            vertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(24, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
            });

            
            Song mySong = Content.Load<Song>("Audio\\Bulls");
            MediaPlayer.Play(mySong);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Components.Remove(tank1);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            worldCamera.Update(mouseState, keyboard);

            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;
            //worldCamera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawAxes();

            /*
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.End();
            */

            // TODO: Add your drawing code here

            /*
            foreach(EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, ground[0].Vertices, 0, 4, ground[0].Indexes, 0, 2);
                //GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, ground[1].Vertices, 0, 4, ground[1].Indexes, 0, 2);
            }
            */

            DrawTerrain(worldCamera.ViewMatrix, worldCamera.ProjectionMatrix);

            sky.Draw(worldCamera.ViewMatrix, worldCamera.ProjectionMatrix);

            base.Draw(gameTime);
        }

        public void DrawAxes()
        {
            drawUtils.DrawSphere(new Vector3(0, 0, 0), 3.0f, Color.Turquoise);
            drawUtils.DrawLine(new Vector3(0, 0, 0), new Vector3(5, 0, 0), Color.Red);
            drawUtils.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 5, 0), Color.Green);
            drawUtils.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 5), Color.Blue);
        }

        public void DoSpriteBatchFix()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        /// <summary>
        /// Helper for drawing the terrain model.
        /// </summary>
        void DrawTerrain(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in terrain.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                    //effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f); 

                    // Set the specular lighting to match the sky color.
                    effect.SpecularColor = new Vector3(0.6f, 0.4f, 0.2f);
                    effect.SpecularPower = 8;

                    // Set the fog to match the distant mountains
                    // that are drawn into the sky texture.
                    effect.FogEnabled = false;
                    effect.FogColor = new Vector3(0.15f);
                    effect.FogStart = 100*2;
                    effect.FogEnd = 320*5;
                }

                mesh.Draw();
            }
        }
    }
}
