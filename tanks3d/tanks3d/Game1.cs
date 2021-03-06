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
using tanks3d.Cameras;
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

        SoundEffect firing;

        public Terrain.Terrain terrain;

        public QuaternionCameraComponent worldCamera;
        public QuaternionCameraComponent originalWorldCamera;
        public QuaternionCamera.Behavior previousBehavior;
        public PhysicsCamera bulletViewCamera;

        public PhysicsEngine physicsEngine;

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
        public int numPlayers = 10, currentPlayer = 0;

        float VelocityCount = 0;
        float VelocityCountMax = 125f;
        float VelocityMult = 5.0f;

        bool canControlTank = true;

        public GameState gameState;

        SoundEffect music;
        SoundEffectInstance musicInstance;

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
            worldCamera.CurrentBehavior = Cameras.QuaternionCamera.Behavior.AimMode;
            worldCamera.MovementSpeed = 100.0f;
            Components.Add(worldCamera);

            originalWorldCamera = worldCamera;
            bulletViewCamera = new PhysicsCamera(this);
            bulletViewCamera.Perspective(fov, 16.0f / 9.0f, 0.5f, 20000.0f);

            wireframeRasterizerState = new RasterizerState();
            wireframeRasterizerState.FillMode = FillMode.WireFrame;

            solidRasterizerState = new RasterizerState();
            solidRasterizerState.FillMode = FillMode.Solid;

            playerColors = new List<Vector3>();
            playerColors.Add(new Vector3(1.0f, 0.0f, 0.0f));
            playerColors.Add(new Vector3(0.0f, 1.0f, 0.0f));
            playerColors.Add(new Vector3(0.0f, 0.0f, 1.0f));
            playerColors.Add(new Vector3(1.0f, 1.0f, 0.0f));
            playerColors.Add(new Vector3(0.0f, 1.0f, 1.0f));
            playerColors.Add(new Vector3(1.0f, 0.0f, 1.0f));
            playerColors.Add(new Vector3(1.0f, 1.0f, 1.0f));
            playerColors.Add(new Vector3(1.0f, 0.5f, 0.5f));
            playerColors.Add(new Vector3(0.5f, 1.0f, 0.5f));
            playerColors.Add(new Vector3(0.5f, 0.5f, 1.0f));

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
                tanks[i] = new Tank(this, Vector3.Zero, i, playerColors.ElementAt(i));
            }

            numPlayersAlive = numPlayers;

            currentTank = tanks[currentPlayer];

            weaponManager = new WeaponManager(this);

            bulletManager = new BulletManager(this);
            Components.Add(bulletManager);

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

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            firing = Content.Load<SoundEffect>("Audio\\Tank Firing");

            music = Content.Load<SoundEffect>("Audio\\Music");
            musicInstance = music.CreateInstance();
            musicInstance.IsLooped = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            musicInstance.Play();

            switch (gameState)
            {
                case GameState.Menu:
                    HandleInput(gameTime);
                    break;

                case GameState.Play:
                    HandleInput(gameTime);
                    currentTank.power = (int)((VelocityCount / VelocityCountMax) * 100);

                    foreach (Tank tank in tanks)
                    {
                        if (tank.currentPlayerState == PlayerState.Dying)
                        {
                            tank.position = terrain.AdjustForTerrainHeight(tank.position);

                            tank.deathTimer -= elapsedSeconds;
                            if (tank.deathTimer <= 0)
                            {
                                tank.CompleteDeath();
                            }
                        }

                        tank.GenerateSmokeAndFire(elapsedSeconds);
                    }

                    if (enteringBulletView == true)
                    {
                        bulletViewTimer -= elapsedSeconds;
                        if (bulletViewTimer <= 0)
                        {
                            EnterBulletView();
                        }
                    }
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

            Boolean suddenDeath = false;

            // Allows the game to exit
            if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                    currentGamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (gameState)
            {
                case GameState.Menu:

                    Keys[] pressed_Key = Keyboard.GetState().GetPressedKeys();

                    for(int i = 0; i < pressed_Key.Length; i++)
                    {
                        switch (pressed_Key[i])
                        {
                            case Keys.F2:
                                suddenDeath = true;
                                numPlayers = 2;
                                gameState = GameState.Play;
                                break;
                            
                            case Keys.D2:
                                numPlayers = 2;
                                gameState = GameState.Play;
                            break;

                            case Keys.F3:
                            suddenDeath = true;
                            numPlayers = 3;
                            gameState = GameState.Play;
                            break;
                            
                            case Keys.D3:
                                numPlayers = 3;
                                gameState = GameState.Play;
                            break;

                            case Keys.F4:
                            suddenDeath = true;
                            numPlayers = 4;
                            gameState = GameState.Play;
                            break;

                            case Keys.D4:
                                numPlayers = 4;
                                gameState = GameState.Play;
                            break;

                            case Keys.F5:
                            suddenDeath = true;
                            numPlayers = 5;
                            gameState = GameState.Play;
                            break;

                            case Keys.D5:
                                numPlayers = 5;
                                gameState = GameState.Play;
                            break;

                            case Keys.F6:
                            suddenDeath = true;
                            numPlayers = 6;
                            gameState = GameState.Play;
                            break;
                            
                            case Keys.D6:
                                numPlayers = 6;
                                gameState = GameState.Play;
                            break;

                            case Keys.F7:
                                suddenDeath = true;
                                numPlayers = 7;
                                gameState = GameState.Play;
                            break;

                            case Keys.D7:
                                numPlayers = 7;
                                gameState = GameState.Play;
                            break;

                            case Keys.F8:
                                suddenDeath = true;
                                numPlayers = 8;
                                gameState = GameState.Play;
                            break;

                            case Keys.D8:
                                numPlayers = 8;
                                gameState = GameState.Play;
                            break;

                            case Keys.F9:
                                suddenDeath = true;
                                numPlayers = 9;
                                gameState = GameState.Play;
                            break;

                            case Keys.D9:
                                numPlayers = 9;
                                gameState = GameState.Play;
                            break;

                            case Keys.F10:
                                suddenDeath = true;
                                numPlayers = 10;
                                gameState = GameState.Play;
                            break;
                                
                            case Keys.D0:
                                numPlayers = 10;
                                gameState = GameState.Play;
                            break;

                            default:
                            break;
                        }
                    }

                    for (int i = 0; i < numPlayers; i++)
                    {
                        if (gameState == GameState.Play)
                        {
                            Components.Remove(tanks[9 - i]);
                            tanks[i].moveLimit += 500 - (50 * (numPlayers - 2));
                            tanks[i].IsAlive = true;
                        }
                        if (suddenDeath)
                        {
                            tanks[i].health = 1;
                        }
                    }

                    numPlayersAlive = numPlayers;

                    break;
                case GameState.Play:
                    if (currentKeyboardState.IsKeyDown(Keys.Tab))
                        mainHUD.showScoreBoard = true;
                    if (currentKeyboardState.IsKeyUp(Keys.Tab))
                        mainHUD.showScoreBoard = false;
                    if (previousKeyboardState.IsKeyDown(Keys.P))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.P))
                            gameState = GameState.Pause;
                    }
                    if (previousKeyboardState.IsKeyDown(Keys.H))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.H))
                            gameState = GameState.Pause;
                    }

                    if (canControlTank)
                    {
                        // Fires bullets
                        if (previousKeyboardState.IsKeyDown(Keys.Space))
                        {
                            if (VelocityCount < VelocityCountMax)
                            {
                                VelocityCount += .5f;
                            }

                            if (currentKeyboardState.IsKeyUp(Keys.Space))
                            {
                                firing.Play();
                                Bullet bullet = weaponManager.Weapons[WeaponTypes.Weapon1].Fire(VelocityCount * VelocityMult);
                                VelocityCount = 0;
                                //switchCurrentTank();

                                currentTank.currentPlayerState = PlayerState.Aim;
                                canControlTank = false;
                                // Switch to bullet view after a small delay
                                //
                                enteringBulletView = true;
                                followBullet = bullet;
                                bulletViewTimer = 0.2f;
                                followBulletStartPos = bullet.position;
                                followBulletStartVelocity = bullet.velocity;
                            }
                        }

                        // CannonView
                        if (previousKeyboardState.IsKeyDown(Keys.C))
                        {
                            if (currentKeyboardState.IsKeyUp(Keys.C))
                            {
                                if (worldCamera.CurrentBehavior != QuaternionCamera.Behavior.CannonView)
                                {
                                    worldCamera.CurrentBehavior = QuaternionCamera.Behavior.CannonView;
                                }
                                else
                                {
                                    currentTank.currentPlayerState = PlayerState.Aim;
                                    worldCamera.CurrentBehavior = QuaternionCamera.Behavior.AimMode;
                                }
                            }
                        }

                        if (previousKeyboardState.IsKeyDown(Keys.Enter))
                        {
                            if (currentKeyboardState.IsKeyUp(Keys.Enter))
                            {
                                switchCurrentTank();
                            }
                        }

                        currentTank.HandleInput(currentGamePadState,
                                            currentKeyboardState,
                                            currentMouseState,
                                            terrain.heightMapInfo,
                                            gameTime);
                    }

                    break;

                case GameState.Pause:
                    if (previousKeyboardState.IsKeyDown(Keys.P))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.P))
                            gameState = GameState.Play;
                    }

                    if (previousKeyboardState.IsKeyDown(Keys.H))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.H))
                            gameState = GameState.Play;
                    }
                    break;

                case GameState.End:
                    if (previousKeyboardState.IsKeyDown(Keys.P))
                    {
                        if (currentKeyboardState.IsKeyUp(Keys.P))
                            gameState = GameState.Menu;
                    }
                    break;
            }
            previousKeyboardState = currentKeyboardState;
        }

        private bool enteringBulletView = false;
        private Bullet followBullet = null;
        private float bulletViewTimer;
        private Vector3 followBulletStartPos;
        private Vector3 followBulletStartVelocity;

        public void EnterBulletView()
        {
            worldCamera.CurrentBehavior = QuaternionCamera.Behavior.AimMode;
            enteringBulletView = false;
            this.IsMouseVisible = false;
            bulletViewCamera.FollowBullet = followBullet;
            Components.Add(bulletViewCamera);
            bulletViewCamera.Position = followBulletStartPos;
            bulletViewCamera.Velocity = followBulletStartVelocity;
            physicsEngine.AddPhysicsObject(bulletViewCamera);
            worldCamera = bulletViewCamera;
        }

        public void ExitBulletView()
        {
            worldCamera = originalWorldCamera;
            Components.Remove(bulletViewCamera);
            physicsEngine.RemovePhysicsObject(bulletViewCamera);
            enteringBulletView = false;
            this.IsMouseVisible = true;
            canControlTank = true;
        }

        public static readonly Random random = new Random();

        public void Shake()
        {
            bulletViewCamera.shake = true;
        }

        public float RandomFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        private Vector3 RandomLocation()
        {
            float randomX, randomZ;
            randomX = (float)random.NextDouble() - 1/2f;
            randomX *= (terrain.heightMapInfo.terrainWidth - 100);

            randomZ = (float)random.NextDouble() - 1/2f;
            randomZ *= (terrain.heightMapInfo.terrainHeight - 100);

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

            if (!currentTank.IsAlive && numPlayersAlive > 0)
            {
                switchCurrentTank();
            }

            currentTank.moves = 0;
        }

        public List<Vector3> playerColors;
    }
}
