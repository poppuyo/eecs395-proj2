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
using tanks3d.Physics;
using tanks3d.Weapons;
using tanks3d.ParticleSystems;
using tank3d;

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
        //public Texture2D terrainTexture;
        BasicEffect terrainEffect;
        public HeightMapInfo heightMapInfo;
        Sky sky;

        public Cameras.QuaternionCameraComponent worldCamera;
        public Cameras.QuaternionCamera.Behavior previousBehavior;

        public PhysicsEngine physicsEngine;
        public TestPhysicsObject testPhysicsObject;

        public WeaponManager weaponManager;
        public BulletManager bulletManager;

        public HUD mainHUD;

        Texture2D texture;
        BasicEffect quadEffect;

        public Tank tank1;

        public DrawUtils drawUtils;

        public WinFormContainer winFormContainer = null;
        public IntPtr drawSurface;

        public KeyboardState previousKeyboardState;

        private int timeOut = 0;

        public enum GameState
        {
            Start,
            Move,
            Aim,
            Fired,
            Aftermath,
            Transition
        }

        public GameState currentState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Make the window resizable
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        public Game1(WinFormContainer winFormContainer)
            : this()
        {
            // Set the drawing surface to be the picture box inside the WinForm.
            this.winFormContainer = winFormContainer;
            this.drawSurface = winFormContainer.getDrawSurface();
            graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged +=
                new EventHandler(Game1_VisibleChanged);
            Mouse.WindowHandle = drawSurface;
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

            worldCamera = new Cameras.QuaternionCameraComponent(this);
            worldCamera.Perspective(90.0f, 16.0f / 9.0f, 0.5f, 10000.0f);
            worldCamera.Position = new Vector3(-88, -300, 195);
            worldCamera.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
            worldCamera.ClickAndDragMouseRotation = true;
            worldCamera.CurrentBehavior = Cameras.QuaternionCamera.Behavior.Spectator;
            worldCamera.MovementSpeed = 100.0f;
            Components.Add(worldCamera);

            physicsEngine = new PhysicsEngine(this);
            Components.Add(physicsEngine);
            
            mainHUD = new HUD(this);
            Components.Add(mainHUD);

            drawUtils = new DrawUtils(this);
            Components.Add(drawUtils);

            //terrain = new Terrains.SimpleGridTerrain(this);
            //terrain = new Terrains.HeightmapTerrain(this);
            //Components.Add(terrain);

            tank1 = new Tank(this);

            weaponManager = new WeaponManager(this);

            bulletManager = new BulletManager(this);
            Components.Add(bulletManager);

            Reticle reticle = new Reticle(this);
            Components.Add(reticle);

            testPhysicsObject = new TestPhysicsObject(this, new Vector3(54, 0, 64), new Vector3(0, 0, 0));
            Components.Add(testPhysicsObject);
            physicsEngine.AddPhysicsObject(testPhysicsObject);

            //View = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up);
            //aCamera.Position = new Vector3(-100, 100, 100);
            //aCamera.View = Matrix.CreateLookAt(new Vector3(-100, 100, 100), Vector3.Zero, Vector3.Up);

            base.Initialize();
        }

        protected void LoadTerrainEffect()
        {
            terrainEffect = new BasicEffect(GraphicsDevice);

            terrainEffect.EnableDefaultLighting();
            //effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f); 

            // Set the specular lighting to match the sky color.
            terrainEffect.SpecularColor = new Vector3(0.6f, 0.4f, 0.2f);
            terrainEffect.SpecularPower = 8;

            // Set the fog to match the distant mountains
            // that are drawn into the sky texture.
            terrainEffect.FogEnabled = false;
            terrainEffect.FogColor = new Vector3(0.15f);
            terrainEffect.FogStart = 100 * 2;
            terrainEffect.FogEnd = 320 * 5;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            terrain = Content.Load<Model>("terrain");
            //terrainTexture = Content.Load<Texture2D>("64x64");

            LoadTerrainEffect();

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

            sky = Content.Load<Sky>("sky");

            tank1.LoadContent(Content);

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
            //MediaPlayer.Play(mySong);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Components.Remove(tank1);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;

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

            tank1.Draw(worldCamera.ViewMatrix, worldCamera.ProjectionMatrix);

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
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;

            foreach (ModelMesh mesh in terrain.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = view;
                    effect.Projection = projection;
                    //effect.Texture = terrainTexture;
                    //effect.TextureEnabled = true;
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// This gets called when the game window is resized.
        /// </summary>
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // Make changes to handle the new window size.

        }

        /// <summary>
        /// Event capturing the construction of a draw surface and makes sure this gets redirected to
        /// a predesignated drawsurface marked by pointer drawSurface
        /// </summary>
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            if (winFormContainer != null)
            {
                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
            }
        }

        /// <summary>
        /// Occurs when the original gamewindows' visibility changes and makes sure it stays invisible
        /// </summary>
        private void Game1_VisibleChanged(object sender, EventArgs e)
        {
            if (winFormContainer != null)
            {
                if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
                {
                    System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
                }
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState currentMouseState = Mouse.GetState();

            // Allows the game to exit
            if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                    currentGamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Changes Camera View
            if (previousKeyboardState.IsKeyDown(Keys.Space))
            {
                if (currentKeyboardState.IsKeyUp(Keys.Space))
                {
                    if (worldCamera.CurrentBehavior == Cameras.QuaternionCamera.Behavior.FirstPerson)
                    {
                        worldCamera.CurrentBehavior = Cameras.QuaternionCamera.Behavior.FollowT;
                    }
                    else
                    {
                        worldCamera.CurrentBehavior = Cameras.QuaternionCamera.Behavior.FirstPerson;
                    }
                }
            }

            // Fires bullets
            if (previousKeyboardState.IsKeyDown(Keys.F))
            {
                if (currentKeyboardState.IsKeyUp(Keys.F))
                {
                    weaponManager.Weapons[WeaponTypes.Weapon1].Fire();
                }
            }
            if(previousKeyboardState.IsKeyDown(Keys.C))
            {
                if (currentKeyboardState.IsKeyUp(Keys.C))
                {
                    weaponManager.Weapons[WeaponTypes.Weapon1].Fire();
                    previousBehavior = this.worldCamera.CurrentBehavior;
                    this.worldCamera.CurrentBehavior = Cameras.QuaternionCamera.Behavior.FollowActiveBullet;
                }
            }

            // Shakes screen
            if (currentKeyboardState.IsKeyDown(Keys.G))
                timeOut = 45;

            if (timeOut != 0)
            {
                Shake();
                timeOut--;
            }

            // Changes Gamestate from Move to Aim
            if (previousKeyboardState.IsKeyDown(Keys.T))
            {
                if (currentKeyboardState.IsKeyUp(Keys.T))
                {
                    if (currentState == GameState.Move)
                    {
                        currentState = GameState.Aim;
                        tank1.ChangeToAim();
                    }
                    else
                    {
                        currentState = GameState.Move;
                        tank1.ChangeToMove();
                    }
                }
            }

            tank1.HandleInput(currentGamePadState, 
                              currentKeyboardState, 
                              currentMouseState, 
                              heightMapInfo,
                              gameTime);

            previousKeyboardState = currentKeyboardState;

        }

        public static readonly Random random = new Random();

        private void Shake()
        {
            worldCamera.Rotate(RandomFloat(), RandomFloat(), RandomFloat());
        }

        private float RandomFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }
    }
}
