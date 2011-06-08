using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using tanks3d.ParticleSystems;
using tanks3d.Physics;
using tank3d;

namespace tanks3d.Weapons
{
    public enum BulletState
    {
        /// <summary>
        /// Flying in the air, possible emitting smoke particles, but not yet exploded.
        /// </summary>
        Unexploded,

        /// <summary>
        /// In the process of exploding (likely generating fire particles).
        /// </summary>
        Exploding,

        /// <summary>
        /// The explosion animation has finished.
        /// </summary>
        Dead
    }

    public class Bullet : DrawableGameComponent, IPhysicsObject
    {
        private Game1 game;

        public Vector3 position;
        public Vector3 velocity;
        float age;

        private float explosionAge;
        private float explosionLifetime;  // How long the explosion state lasts.
        private Vector3 explosionLocation;

        /// <summary>
        /// Age since the bullet has exploded. This field is only valid when the bullet
        /// state is BulletState.Exploding.
        /// </summary>
        public float ExplosionAge
        {
            get
            {
                if (bulletState == BulletState.Exploding)
                {
                    return explosionAge;
                }

                throw new InvalidOperationException("The ExplosionAge field is only valid when the bullet state is BulletState.Exploding.");
            }
        }

        /// <summary>
        /// Returns the location where the bullet exploded. This field is only valid when
        /// the bullet state is BulletState.Exploding.
        /// </summary>
        public Vector3 ExplosionLocation
        {
            get
            {
                if (bulletState == BulletState.Exploding)
                {
                    return explosionLocation;
                }

                throw new InvalidOperationException("The ExplosionLocation field is only valid when the bullet state is BulletState.Exploding.");
            }
        }


        public BulletState bulletState;

        ParticleSystem explosionParticles;
        ParticleSystem explosionSmokeParticles;
        ParticleEmitter trailEmitter;

        SoundEffect explosion;

        #region Constants

        const float trailParticlesPerSecond = 200;
        const int numExplosionParticles = 90;
        const int numExplosionSmokeParticles = 150;
        const float projectileLifespan = 1.5f;
        const float sidewaysVelocityRange = 60;
        const float verticalVelocityRange = 40;

        #endregion

        public Bullet(Game1 g, ParticleSystem explosionParticles, ParticleSystem explosionSmokeParticles,
            ParticleSystem projectileTrailParticles, Vector3 origin, Vector3 initialVelocity, SoundEffect Explosion)
            : base(g)
        {
            game = g;
            bulletState = BulletState.Unexploded;

            this.position = origin;
            this.velocity = initialVelocity;

            game.physicsEngine.AddPhysicsObject(this);

            this.explosionParticles = explosionParticles;
            this.explosionSmokeParticles = explosionSmokeParticles;

            this.explosionAge = float.NaN;
            this.explosionLifetime = (float)explosionParticles.Settings.Duration.TotalSeconds + explosionParticles.Settings.DurationRandomness;

            // Use the particle emitter helper to output our trail particles.
            trailEmitter = new ParticleEmitter(projectileTrailParticles,
                                               trailParticlesPerSecond, position);

            explosion = Explosion;
        }

        public void LoadContent(ContentManager content)
        {
            explosion = content.Load<SoundEffect>("Audio\\Barrel Exploding");
        }

        public override void Update(GameTime gameTime)
        {
            switch (game.gameState)
            {
                case GameState.Play:

                    float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    switch (bulletState)
                    {
                        case BulletState.Unexploded:

                            age += elapsedTime;

                            // Update the particle emitter, which will create our particle trail.
                            trailEmitter.Update(gameTime, position);

                            // Perform collision detection with any tanks present in the game.
                            foreach (Tank tank in game.tanks)
                            {
                                if (tank.boundingBox.Intersects(this.GetBoundingBox()))
                                {
                                    tank.GetHit(this);
                                    this.StartExplosion();
                                    break;
                                }
                            }

                            // Check if we're outside the bounds of the terrain. (Note that collision
                            // detection with the terrain is handled in the HandleCollisionWithTerrain()
                            // method below).
                            if (!game.terrain.heightMapInfo.IsOnHeightmap(position))
                            {
                                StartExplosion();
                                break;
                            }

                            break;

                        case BulletState.Exploding:
                            if (this == game.bulletManager.ActiveBullet)
                            {
                                //game.worldCamera.CurrentBehavior = game.previousBehavior;
                                
                            }
                            explosionAge += elapsedTime;
                            if (explosionAge >= explosionLifetime)
                            {
                                game.bulletManager.RemoveBullet(this);
                                bulletState = BulletState.Dead;
                                // Switch tanks
                                game.switchCurrentTank();
                            }

                            break;

                        case BulletState.Dead:
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            switch (game.gameState)
            {
                case GameState.Play:
                    switch (bulletState)
                    {
                        case BulletState.Unexploded:
                            game.drawUtils.DrawSphere(position, 5.0f, Color.Black);
              		        tanks3d.Utility.BoundingBoxRenderer.Render(game, this.GetBoundingBox(), game.GraphicsDevice, game.worldCamera.ViewMatrix, game.worldCamera.ProjectionMatrix, Color.Violet);
                            break;
                        case BulletState.Exploding:
                            break;
                        case BulletState.Dead:
                            break;
                        default:
                            break;
                    }

                    base.Draw(gameTime);
                    break;
            }
        }

        /// <summary>
        /// If the bullet hit the terrain, explode.
        /// </summary>
        public void HandleCollisionWithTerrain()
        {
            switch (bulletState)
            {
                case BulletState.Unexploded:
                    StartExplosion();
                    game.terrain.AddExplosionDecal(this.position);
                    break;
                case BulletState.Exploding:
                    break;
                case BulletState.Dead:
                    break;
                default:
                    break;
            }
        }

        public void StartExplosion()
        {
            switch (bulletState)
            {
                case BulletState.Unexploded:

                    bulletState = BulletState.Exploding;
                    explosionAge = 0.0f;
                    explosionLocation = this.position;

                    Vector3 explosionVelocity = 0.25f * new Vector3(velocity.X, 5.0f, velocity.Z);

                    explosion.Play();

                    for (int i = 0; i < numExplosionParticles; i++)
                        explosionParticles.AddParticle(position, explosionVelocity);

                    for (int i = 0; i < numExplosionSmokeParticles; i++)
                        explosionSmokeParticles.AddParticle(position, explosionVelocity);

                    // moved to the post-explosion to prevent people from doing stuff until the explosion is over
                    // game.switchCurrentTank();

                    break;
                case BulletState.Exploding:
                    break;
                case BulletState.Dead:
                    break;
                default:
                    break;
            }
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            position = newPosition;
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }

        public void UpdateVelocity(Vector3 newVelocity)
        {
            velocity = newVelocity;
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(position - new Vector3(-5, -5, -5), position + new Vector3(5, 5, 5));
        }

        public bool DoBoundsCheck()
        {
            return false;    // We handle it ourselves.
        }
    }
}
