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

        ITerrain terrain;

        public Cameras.FPSCamera worldCamera;
        public HUD mainHUD;

        Texture2D texture;
        BasicEffect quadEffect;

        Tank tank1;
        Bullet bullet1;

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
            ground = new TexturedQuad.Quad[1];
            ground[0] = new TexturedQuad.Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 64f, 64f);

            worldCamera = new Cameras.FPSCamera(graphics.GraphicsDevice.Viewport,
                new Vector3(-100f, 100f, 100f), 0.0f, 0.0f);
            mainHUD = new HUD(this);

            //terrain = new Terrains.SimpleGridTerrain(this);
            terrain = new Terrains.HeightmapTerrain(this);
            Components.Add(terrain);

            tank1 = new Tank(this);
            Components.Add(tank1);

            bullet1 = new Bullet(this);
            Components.Add(bullet1);

            Reticle reticle = new Reticle(this);
            Components.Add(reticle);

            //View = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up);
            //aCamera.Position = new Vector3(-100, 100, 100);
            //aCamera.View = Matrix.CreateLookAt(new Vector3(-100, 100, 100), Vector3.Zero, Vector3.Up);

            //Components.Add(worldCamera);
            Components.Add(mainHUD);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
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

            // TODO: Add your drawing code here

            foreach(EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, ground[0].Vertices, 0, 4, ground[0].Indexes, 0, 2);
                //GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, ground[1].Vertices, 0, 4, ground[1].Indexes, 0, 2);
            }
            base.Draw(gameTime);
        }
    }
}
