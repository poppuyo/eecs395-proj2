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

        public Terrain.Terrain terrain;

        public Cameras.QuaternionCameraComponent worldCamera;

        public PhysicsEngine physicsEngine;
        public TestPhysicsObject testPhysicsObject;

        public WeaponManager weaponManager;
        public BulletManager bulletManager;

        public HUD mainHUD;

        private bool wireframe = false;
        public bool WireframeMode
        {
            get { return wireframe; }
            set
            {
                wireframe = value;
            }
        }
        public RasterizerState wireframeRasterizerState;
        public RasterizerState solidRasterizerState;

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

            wireframeRasterizerState = new RasterizerState();
            wireframeRasterizerState.FillMode = FillMode.WireFrame;

            solidRasterizerState = new RasterizerState();
            solidRasterizerState.FillMode = FillMode.Solid;

            terrain = new Terrain.Terrain(this);
            Components.Add(terrain);

            physicsEngine = new PhysicsEngine(this);
            Components.Add(physicsEngine);
            
            mainHUD = new HUD(this);
            Components.Add(mainHUD);

            drawUtils = new DrawUtils(this);
            Components.Add(drawUtils);

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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            tank1.LoadContent(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
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
                              terrain.heightMapInfo,
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
