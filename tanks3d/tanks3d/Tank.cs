#region File Description
//-----------------------------------------------------------------------------
// Tank.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using tanks3d;
using tanks3d.Physics;
using tanks3d.Weapons;
using tanks3d.Terrain;
using tanks3d.Utility;
#endregion

namespace tank3d
{
    public class Tank : DrawableGameComponent, IPhysicsObject
    {
        #region Constants

        // This constant controls how quickly the tank can move forward and backward
        const float TankVelocity = 1.5f;

        // The radius of the tank's wheels. This is used when we calculate how fast they
        // should be rotating as the tank moves.
        const float TankWheelRadius = 30;

        // Controls how quickly the tank can turn from side to side.
        const float TankTurnSpeed = .012f;

        public const float TurretLength = 185.0f;

        const float TurretLeftBound = -145f;
        const float TurretRightBound = 145f;
        const float TurretUpBound = 115f;
        const float TurretDownBound = -15f;

        const float TankSize = 75f;

        public int moveLimit = 500;
        public int moves = 0;
        public int thisTankNumber;

        #endregion

        #region Properties

        /// <summary>
        /// The position of the tank. The camera will use this value to position itself.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
        }
        public Vector3 position;

        public Vector3 playerColor;

        /// <summary>
        /// The direction that the tank is facing, in radians. This value will be used
        /// to position and and aim the camera.
        /// </summary>
        public float FacingDirection
        {
            get { return facingDirection; }
        }
        private float facingDirection;

        // Handles 
        private Vector3 OriginalMousePos { get; set; }
        private Vector3 TurretDiff { get; set; }
        private Vector3 TurretDirection { get; set; }

        public Vector3 GetTurretDirection()
        {
            Matrix m0 = orientation;
            Matrix m1 = Matrix.CreateRotationY((float)-(TurretDirection.X * .01));
            Matrix m2 = Matrix.CreateRotationX((float)-(TurretDirection.Y * .01));

            return Vector3.Transform(Vector3.UnitZ, m2 * m1 * orientation);
        }

        /// <summary>
        /// Location of the muzzle of the turret. This should be the spawn
        /// point of the projectiles.
        /// </summary>
        public Vector3 TurretEndPosition
        {
            get
            {
                Matrix worldMatrix = Matrix.CreateScale(0.1f) * orientation * Matrix.CreateTranslation(Position);
                Matrix barrel = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, TurretLength));
                Matrix turret = Matrix.CreateRotationY((float)-(TurretDirection.X * .01)) * turretTransform;
                Matrix canon = Matrix.CreateRotationX((float)-(TurretDirection.Y * .01)) * Matrix.CreateScale(2.0f) * canonTransform;

                Matrix combined = barrel * canon * turret  * worldMatrix;

                return Vector3.Transform(Vector3.Zero, combined);
            }
        }

        public int angle = 0 ;

        public PlayerState currentPlayerState = PlayerState.Aim;

        public int health = 100, power = 0;

        Vector3 bBoxMinPoint;
        Vector3 bBoxMaxPoint;

        public BoundingBox boundingBox
        {
            get
            {
                Matrix m0 = Matrix.CreateTranslation(position);
                Matrix m2 = Matrix.CreateScale(TankSize);

                if (IsAlive)
                {
                    bBoxMinPoint = new Vector3(-0.5f, -0.5f, -0.5f);
                    bBoxMinPoint = Vector3.Transform(bBoxMinPoint, m2 * m0);
                    bBoxMaxPoint = new Vector3(0.5f, 0.5f, 0.5f);
                    bBoxMaxPoint = Vector3.Transform(bBoxMaxPoint, m2 * m0);
                }
                else
                {
                    bBoxMinPoint = new Vector3(-1000f, -1000f, -1000f);
                    bBoxMaxPoint = new Vector3(-1000f, -1000f, -1000f);
                }

                return new BoundingBox(bBoxMinPoint, bBoxMaxPoint);
            }
        }

        public bool IsAlive = false;

        /// <summary>
        /// Length of the turret's barrel.
        /// </summary>

        #endregion

        #region Fields

        private Game1 game;

        // The tank's model - a fearsome sight.
        Model model;

        // How is the tank oriented? We'll calculate this based on the user's input and
        // the heightmap's normals, and then use it when drawing.
        public Matrix orientation = Matrix.Identity;

        // We'll use this value when making the wheels roll. It's calculated based on 
        // the distance moved.
        Matrix wheelRollMatrix = Matrix.Identity;

        // The Simple Animation Sample at creators.xna.com explains the technique that 
        // we will be using in order to roll the tanks wheels. In this technique, we
        // will keep track of the ModelBones that control the wheels, and will manually
        // set their transforms. These next eight fields will be used for this
        // technique.
        ModelBone leftBackWheelBone;
        ModelBone rightBackWheelBone;
        ModelBone leftFrontWheelBone;
        ModelBone rightFrontWheelBone;
        ModelBone turretBone;
        ModelBone canonBone;

        Matrix leftBackWheelTransform;
        Matrix rightBackWheelTransform;
        Matrix leftFrontWheelTransform;
        Matrix rightFrontWheelTransform;
        Matrix turretTransform;
        Matrix canonTransform;

        SoundEffect moving;
        SoundEffectInstance movingInstance;

        #endregion

        #region Initialization

        public Indicator tankIndicator;

        public Tank(Game1 game, Vector3 pos, int num, Vector3 playerColor)
            : base(game)
        {
            position = pos;
            this.game = game;
            thisTankNumber = num;
            this.playerColor = playerColor;
            ChangeToAim();
        }

        private Effect experimentalEffect;

        /// <summary>
        /// Called when the Game is loading its content. Pass in a ContentManager so the
        /// tank can load its model.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>("Models//tank");

            experimentalEffect = content.Load<Effect>("effects");

            tankIndicator = new Indicator(game, this);
            tankIndicator.LoadContentDamnit();
            game.Components.Add(tankIndicator);

            moving = content.Load<SoundEffect>("Audio\\Tank");
            movingInstance = moving.CreateInstance();
            movingInstance.IsLooped = true;
           
            // As discussed in the Simple Animation Sample, we'll look up the bones
            // that control the wheels.
            leftBackWheelBone = model.Bones["l_back_wheel_geo"];
            rightBackWheelBone = model.Bones["r_back_wheel_geo"];
            leftFrontWheelBone = model.Bones["l_front_wheel_geo"];
            rightFrontWheelBone = model.Bones["r_front_wheel_geo"];

            // Also, we'll store the original transform matrix for each animating bone.
            leftBackWheelTransform = leftBackWheelBone.Transform;
            rightBackWheelTransform = rightBackWheelBone.Transform;
            leftFrontWheelTransform = leftFrontWheelBone.Transform;
            rightFrontWheelTransform = rightFrontWheelBone.Transform;

            turretBone = model.Bones["turret_geo"];
            turretTransform = turretBone.Transform;

            canonBone = model.Bones["canon_geo"];
            canonTransform = canonBone.Transform;
        }

        #endregion


        #region Handle input and draw

        /// <summary>
        /// This function is called when the game is Updating in response to user input.
        /// It'll move the tank around the heightmap, and update all of the tank's 
        /// necessary state.
        /// </summary>


        public void HandleInput(GamePadState currentGamePadState,
                                KeyboardState currentKeyboardState, 
                                MouseState currentMouseState,
                                HeightMapInfo heightMapInfo,
                                GameTime gameTime)
        {
            //Recalculates turretDirection based on current mouse position
            if (currentPlayerState == PlayerState.Aim)
            {
                TurretDiff = new Vector3(currentMouseState.X, currentMouseState.Y, 0) - OriginalMousePos;
                TurretDiff = new Vector3(TurretDiff.X, -TurretDiff.Y, TurretDiff.Z);

                TurretDirection += TurretDiff;
                OriginalMousePos = new Vector3(currentMouseState.X, currentMouseState.Y, 0);

                if (TurretDirection.X > TurretRightBound) TurretDirection = new Vector3(TurretRightBound, TurretDirection.Y, TurretDirection.Z);
                else if (TurretDirection.X < TurretLeftBound) TurretDirection = new Vector3(TurretLeftBound, TurretDirection.Y, TurretDirection.Z);

                if (TurretDirection.Y > TurretUpBound) TurretDirection = new Vector3(TurretDirection.X, TurretUpBound, TurretDirection.Z);
                else if (TurretDirection.Y < TurretDownBound) TurretDirection = new Vector3(TurretDirection.X, TurretDownBound, TurretDirection.Z);
            }

            // First, we want to check to see if the tank should turn. turnAmount will 
            // be an accumulation of all the different possible inputs.
            float turnAmount = -currentGamePadState.ThumbSticks.Left.X;
            if (currentKeyboardState.IsKeyDown(Keys.A) || currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                turnAmount += 5;
            }

            if (currentKeyboardState.IsKeyDown(Keys.D) || currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                turnAmount -= 5;
            }

            // clamp the turn amount between -1 and 1, and then use the finished
            // value to turn the tank.
            turnAmount = MathHelper.Clamp(turnAmount, -1, 1);
            if (game.currentTank.moves < moveLimit)
                facingDirection += turnAmount * TankTurnSpeed;

            bool canMove = !TankCollision();
            // Next, we want to move the tank forward or back. to do this, 
            // we'll create a Vector3 and modify use the user's input to modify the Z
            // component, which corresponds to the forward direction.
            Vector3 movement = Vector3.Zero;
            movement.Z = -currentGamePadState.ThumbSticks.Left.Y;


            if ((currentKeyboardState.IsKeyDown(Keys.W) || currentGamePadState.DPad.Up == ButtonState.Pressed))
            {
                if (!canMove)
                {
                    movement.Z = -4;
                }
                else
                {
                    movement.Z = 5;
                    game.currentTank.moves++;
                }
            }

            if ((currentKeyboardState.IsKeyDown(Keys.S) || currentGamePadState.DPad.Down == ButtonState.Pressed))
            {
                if (!canMove)
                {
                    movement.Z = 4;
                }
                else
                {
                    movement.Z = -5;
                    game.currentTank.moves++;
                }
            }

            // Next, we'll create a rotation matrix from the direction the tank is 
            // facing, and use it to transform the vector.
            orientation = Matrix.CreateRotationY(FacingDirection);
            Vector3 velocity = Vector3.Transform(movement, orientation);
            velocity *= TankVelocity;

            if (game.currentTank.moves > moveLimit)
                velocity = Vector3.Zero;

            // Now we know how much the user wants to move. We'll construct a temporary
            // vector, newPosition, which will represent where the user wants to go. If
            // that value is on the heightmap, we'll allow the move.
            Vector3 newPosition = Position + velocity;
            if (heightMapInfo.IsOnHeightmap(newPosition))
            {
                // Now that we know we're on the heightmap, we need to know the correct
                // height and normal at this position.
                Vector3 normal;
                heightMapInfo.GetHeightAndNormal(newPosition,
                                                 out newPosition.Y,
                                                 out normal);

                // As discussed in the doc, we'll use the normal of the heightmap
                // and our desired forward direction to recalculate our orientation
                // matrix. It's important to normalize, as well.
                orientation.Up = normal;

                orientation.Right = Vector3.Cross(orientation.Forward, orientation.Up);
                orientation.Right = Vector3.Normalize(orientation.Right);

                orientation.Forward = Vector3.Cross(orientation.Up, orientation.Right);
                orientation.Forward = Vector3.Normalize(orientation.Forward);

                // Now we need to roll the tank's wheels "forward." to do this, we'll
                // calculate how far they have rolled, and from there calculate how much
                // they must have rotated.
                float distanceMoved = Vector3.Distance(Position, newPosition);
                float theta = distanceMoved / TankWheelRadius;
                int rollDirection = movement.Z > 0 ? 1 : -1;

                wheelRollMatrix *= Matrix.CreateRotationX(theta * rollDirection);

                // once we've finished all computations, we can set our position to the
                // new position that we calculated.
                position = newPosition;

            }

            movingInstance.Volume = 0.20f;
            movingInstance.Play();
        }


        public void FixGravity(HeightMapInfo heightMapInfo)
        {
            Vector3 newPosition = Position;
            if (heightMapInfo.IsOnHeightmap(newPosition))
            {
                // now that we know we're on the heightmap, we need to know the correct
                // height and normal at this position.
                Vector3 normal;
                heightMapInfo.GetHeightAndNormal(newPosition,
                                                 out newPosition.Y,
                                                 out normal);

                // As discussed in the doc, we'll use the normal of the heightmap
                // and our desired forward direction to recalculate our orientation
                // matrix. It's important to normalize, as well.
                orientation.Up = normal;

                orientation.Right = Vector3.Cross(orientation.Forward, orientation.Up);
                orientation.Right = Vector3.Normalize(orientation.Right);

                orientation.Forward = Vector3.Cross(orientation.Up, orientation.Right);
                orientation.Forward = Vector3.Normalize(orientation.Forward);

                // Once we've finished all computations, we can set our position to the
                // new position that we calculated.
                position = newPosition;
            }
        }

		public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            game.GraphicsDevice.BlendState = BlendState.Opaque;

            // Apply matrices to the relevant bones, as discussed in the Simple 
            // Animation Sample.
            leftBackWheelBone.Transform = wheelRollMatrix * leftBackWheelTransform;
            rightBackWheelBone.Transform = wheelRollMatrix * rightBackWheelTransform;
            leftFrontWheelBone.Transform = wheelRollMatrix * leftFrontWheelTransform;
            rightFrontWheelBone.Transform = wheelRollMatrix * rightFrontWheelTransform;


            turretBone.Transform = Matrix.CreateRotationY((float)-(TurretDirection.X * .01)) *
                                   turretTransform;

            canonBone.Transform = Matrix.CreateRotationX((float)-(TurretDirection.Y * .01)) *
                                  Matrix.CreateScale(2.0f) *
                                  canonTransform;

            // now that we've updated the wheels' transforms, we can create an array
            // of absolute transforms for all of the bones, and then use it to draw.
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            // calculate the tank's world matrix, which will be a combination of our
            // orientation and a translation matrix that will put us at at the correct
            // position.
            Matrix worldMatrix = Matrix.CreateScale(0.1f) * orientation * Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    Matrix world = boneTransforms[mesh.ParentBone.Index] * worldMatrix;

                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(game.worldCamera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(game.worldCamera.ProjectionMatrix);
                    effect.Parameters["LightDirection"].SetValue(new Vector3(-0.45f, -0.25f, -1.0f));
                    effect.Parameters["LightColor"].SetValue(new Vector3(0.6f, 0.4f, 0.2f) * 0.5f);
                    effect.Parameters["AmbientColor"].SetValue(new Vector3(1.0f, 1.0f, 1.0f) * 0.5f);
                    effect.Parameters["TintColor"].SetValue(playerColor);
                }

                mesh.Draw();
            }

            //BoundingBoxRenderer.Render(game, boundingBox, game.GraphicsDevice, viewMatrix, projectionMatrix, Color.Red);
        }

        #endregion

        public void ChangeToAim()
        {
            MouseState mouseState = Mouse.GetState();
            OriginalMousePos = new Vector3(mouseState.X, mouseState.Y, 0);
        }

        /// <summary>
        /// This method gets called when a bullet hits the tank.
        /// </summary>
        public void GetHit(Bullet bullet)
        {
            health -= 40;
            game.mainHUD.hitTimer = 0;
            if (health <= 0)
            {
                Dies();
            }
        }

        private void Dies()
        {
            IsAlive = false;
            health = 0;
            game.mainHUD.lastPlayerEliminated = thisTankNumber;
            game.numPlayersAlive -= 1;
            game.mainHUD.playerTimer = 0;

            currentPlayerState = PlayerState.Dying;
            deathTimer = 6.0f;
            game.bulletViewCamera.deadTank = this;
            float height;
            Vector3 normal;
            game.terrain.heightMapInfo.GetHeightAndNormal(position, out height, out normal);
            velocity = 20.0f*normal;
            playerColor = new Vector3(-2, -2, -2);
            game.Shake();
        }

        public void CompleteDeath()
        {
            game.bulletViewCamera.deadTank = null;
            Game.Components.Remove(this);
            game.ExitBulletView();

            if (game.numPlayersAlive == 1)
                game.gameState = GameState.End;
        }

        private bool TankCollision()
        {
            Vector3[] testLowerBounds = this.boundingBox.GetCorners();
            /*for (int i = 0; i < testLowerBounds; i++)
                testLowerBounds[i].*/


            foreach (Tank tank in game.tanks)
            {
                if (tank.boundingBox.Intersects(this.boundingBox) && tank != this)
                    return true;
            }
            return false;
        }

        public float smokeAndFireGeneratorTimer = 0.5f;
        public float smokeAndFireGeneratorFrequency = 0.5f;
        public float baseSmokeParticles = 0.3f;
        public float baseFireParticles = 0.02f;

        public float deathTimer;

        public void GenerateSmokeAndFire(float elapsedSeconds)
        {
            smokeAndFireGeneratorTimer -= elapsedSeconds;
            if (smokeAndFireGeneratorTimer <= 0.0f)
            {
                Vector3 basePositionOffset = Vector3.Transform(new Vector3(0.0f, 20.0f, 0.0f), orientation);

                Vector3 v = new Vector3(0.0f, 5.0f, 0.0f);
                int numSmokeParticles = (int)((100 - health) * baseSmokeParticles);
                int numFireParticles = (int)((100 - health) * baseFireParticles);

                for (int i = 0; i < numSmokeParticles; i++)
                    game.bulletManager.explosionSmokeParticles.AddParticle(position + basePositionOffset, v);

                for (int i = 0; i < numFireParticles; i++)
                    game.bulletManager.explosionParticles.AddParticle(position + basePositionOffset, v);

                smokeAndFireGeneratorTimer = smokeAndFireGeneratorFrequency;
            }
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            position = newPosition;
        }

        // lame hack
        public Vector3 velocity = new Vector3(0, 0, 0);

        public Vector3 GetVelocity()
        {
            return velocity;
        }

        public void UpdateVelocity(Vector3 newVelocity)
        {
            // lame hack
            velocity = newVelocity;
            position += 0.01f*velocity;
        }

        public BoundingBox GetBoundingBox()
        {
            return boundingBox;
        }

        public void HandleCollisionWithTerrain()
        {
            return;
        }
    }
}
