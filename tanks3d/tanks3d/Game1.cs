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
    public enum GameState
    {
        Menu,
        Play,
        Pause,
        End
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Terrain.Terrain terrain;

        public Cameras.QuaternionCameraComponent worldCamera;
        public Cameras.QuaternionCamera.Behavior previousBehavior;

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

        public Player[] players;

        public Tank currentTank;
        public Tank[] tanks;

        public DrawUtils drawUtils;

        public WinFormContainer winFormContainer = null;
        public IntPtr drawSurface;

        public KeyboardState previousKeyboardState;

        public int numPlayersAlive;
        public int numPlayers = 4, currentPlayer = 0;
        
        private int timeOut = 0;

        float VelocityCount = 0;
        float VelocityCountMax = 150f;
        float VelocityMult = 10.0f;

        public GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameState = GameState.Menu;

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
            worldCamera = new Cameras.QuaternionCameraComponent(this);
            float fov = 9.0f/16.0f*this.GraphicsDevice.Viewport.AspectRatio*90.0f;
            worldCamera.Perspective(fov, 16.0f / 9.0f, 0.5f, 20000.0f);
            worldCamera.Position = new Vector3(0, -370, 160);
            worldCamera.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
            worldCamera.ClickAndDragMouseRotation = true;
            worldCamera.CurrentBehavior = Cameras.QuaternionCamera.Behavior.FollowT;
            worldCamera.MovementSpeed = 100.0f;
            Components.Add(worldCamera);

            wireframeRasterizerState = new RasterizerState();
            wireframeRasterizerState.FillMode = FillMode.WireFrame;

            solidRasterizerState = new RasterizerState();
            solidRasterizerState.FillMode = FillMode.Solid;

            terrain = new Terrain.Terrain(this);
            Components.Add(terrain);

			physicsEngine = new PhysicsEngine(this, IntegrationMethod.RungeKutta4);
            Components.Add(physicsEngine);
            
            mainHUD = new HUD(this);
            Components.Add(mainHUD);

            drawUtils = new DrawUtils(this);
            Components.Add(drawUtils);

            tanks = new Tank[numPlayers];
            players = new Player[numPlayers];

            for (int i = 0; i < numPlayers; i++)
            {
                players[i] = new Player(this);
                //tanks[i] = new Tank(this, new Vector3(RandomFloat() * 100, RandomFloat() * 100, RandomFloat() * 100));
                tanks[i] = new Tank(this, Vector3.Zero, i);
                //tanks[i] = new Tank(this, RandomLocation());
            }

            numPlayersAlive = numPlayers;

            currentTank = tanks[currentPlayer];

            weaponManager = new WeaponManager(this);

            bulletManager = new BulletManager(this);
            Components.Add(bulletManager);

            Reticle reticle = new Reticle(this);
            Components.Add(reticle);

            testPhysicsObject = new TestPhysicsObject(this, new Vector3(54, 0, 64), new Vector3(0, 0, 0));
            Components.Add(testPhysicsObject);
            physicsEngine.AddPhysicsObject(testPhysicsObject);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            for (int i = 0; i < numPlayers; i++)
            {
                tanks[i].LoadContent(Content);
                tanks[i].position = RandomLocation();
                tanks[i].FixGravity(terrain.heightMapInfo);
            }

            tanks[0].position = Vector3.Zero;
                

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Song mySong = Content.Load<Song>("Audio\\Bulls");
            //MediaPlayer.Play(mySong);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Menu:
                    HandleInput(gameTime);
                    break;

                case GameState.Play:
                    HandleInput(gameTime);
                    currentTank.power = (int)((VelocityCount / VelocityCountMax) * 100);
                    break;

                case GameState.Pause:
                    HandleInput(gameTime);
                    break;

                case GameState.End:
                    HandleInput(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Menu:
                    GraphicsDevice.Clear(Color.Black);
                    break;

                case GameState.Play:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    DrawAxes();

                    for (int i = 0; i < numPlayers; i++)
                        tanks[i].Draw(worldCamera.ViewMatrix, worldCamera.ProjectionMatrix);

                    break;

                case GameState.Pause:
                    GraphicsDevice.Clear(Color.Black);
                    break;

                case GameState.End:
                    GraphicsDevice.Clear(Color.Black);
                    break;
            }

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
        public void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // Make changes to handle the new window size.

            // Change the field of view to maintain a constant aspect ratio.
            float fov = 9.0f / 16.0f * this.GraphicsDevice.Viewport.AspectRatio * 90.0f;
            if (fov >= 130.0f) fov = 130.0f;
            worldCamera.Perspective(fov, 16.0f / 9.0f, 0.5f, 20000.0f);
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
            currentTank = tanks[currentPlayer];
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState currentMouseState = Mouse.GetState();

            // Allows the game to exit
            if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                    currentGamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (gameState)
            {
                case GameState.Menu:
                    if (previousKeyboardState.IsKeyDown(Keys.H))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.H))
                            gameState = GameState.Play;
                    }
                    break;

                case GameState.Play:
                    if (previousKeyboardState.IsKeyDown(Keys.P))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.P))
                            gameState = GameState.Pause;
                    }

                    if (previousKeyboardState.IsKeyDown(Keys.H))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.H))
                            gameState = GameState.Menu;
                    }

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
                        if (VelocityCount < VelocityCountMax)
                        {
                            VelocityCount += 1;
                        }

                        if (currentKeyboardState.IsKeyUp(Keys.F))
                        {
                            weaponManager.Weapons[WeaponTypes.Weapon1].Fire(VelocityCount * VelocityMult);
                            VelocityCount = 0;
                            switchCurrentTank();
                            //Shake();
                        }
                    }
                    if (previousKeyboardState.IsKeyDown(Keys.C))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.C))
                        {
                            weaponManager.Weapons[WeaponTypes.Weapon1].Fire(200.0f);
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
                            if (currentTank.currentPlayerState == PlayerState.Move)
                            {
                                currentTank.currentPlayerState = PlayerState.Aim;
                                currentTank.ChangeToAim();
                            }
                            else
                            {
                                currentTank.currentPlayerState = PlayerState.Move;
                            }
                        }
                    }

                    currentTank.HandleInput(currentGamePadState,
                                      currentKeyboardState,
                                      currentMouseState,
                                      terrain.heightMapInfo,
                                      gameTime);
                    break;

                case GameState.Pause:
                    if (previousKeyboardState.IsKeyDown(Keys.P))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.P))
                            gameState = GameState.Play;
                    }
                    break;                
            }
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

        private Vector3 RandomLocation()
        {
            float randomX, randomZ;
            randomX = (float)random.NextDouble() - 1/2f;
            randomX *= terrain.heightMapInfo.terrainWidth;

            randomZ = (float)random.NextDouble() - 1/2f;
            randomZ *= terrain.heightMapInfo.terrainHeight;

            return new Vector3(randomX, 0f, randomZ);

        }

        public void switchCurrentTank()
        {
            if (currentPlayer < numPlayers - 1)
            {
                currentPlayer++;
                currentTank = tanks[currentPlayer];
            }
            else
            {
                currentPlayer = 0;
                currentTank = tanks[currentPlayer];
            }
            if (!currentTank.IsAlive)
            {
                switchCurrentTank();
            }
            currentTank.moves = 0;
        }
    }
}
