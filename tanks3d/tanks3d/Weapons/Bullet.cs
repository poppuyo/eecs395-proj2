using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tanks3d.ParticleSystems;
using tanks3d.Physics;

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
    }

    public class Bullet : DrawableGameComponent, IPhysicsObject
    {
        private Game1 game;

        public Vector3 position;
        public Vector3 velocity;
        float age;

        private float explosionAge;
        private float explosionLifetime;  // How long the explosion state lasts.

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
                else
                {
                    throw new InvalidOperationException("The ExplosionAge field is only valid when the bullet state is BulletState.Exploding.");
                }
            }
        }
        

        public BulletState bulletState;

        ParticleSystem explosionParticles;
        ParticleSystem explosionSmokeParticles;
        ParticleEmitter trailEmitter;

        #region Constants

        const float trailParticlesPerSecond = 200;
        const int numExplosionParticles = 30;
        const int numExplosionSmokeParticles = 50;
        const float projectileLifespan = 1.5f;
        const float sidewaysVelocityRange = 60;
        const float verticalVelocityRange = 40;

        #endregion

        public Bullet(Game1 g, ParticleSystem explosionParticles, ParticleSystem explosionSmokeParticles,
            ParticleSystem projectileTrailParticles, Vector3 origin, Vector3 initialVelocity)
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
        }

        public override void Update(GameTime gameTime)
        {
            switch (game.currentState1)
            {
                case Game1.GameState1.Play:
                    float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    switch (bulletState)
                    {
                        case BulletState.Unexploded:

                            age += elapsedTime;

                            // Update the particle emitter, which will create our particle trail.
                            trailEmitter.Update(gameTime, position);

                            // If enough time has passed, explode! Note how we pass our velocity
                            // in to the AddParticle method: this lets the explosion be influenced
                            // by the speed and direction of the projectile which created it.
                            /*
                            if (age > projectileLifespan)
                            {
                                bulletState = BulletState.Exploding;

                                for (int i = 0; i < numExplosionParticles; i++)
                                    explosionParticles.AddParticle(position, velocity);

                                for (int i = 0; i < numExplosionSmokeParticles; i++)
                                    explosionSmokeParticles.AddParticle(position, velocity);
                            }
                            */

                            break;
                        case BulletState.Exploding:
                            if (this == game.bulletManager.ActiveBullet)
                            {
                                //game.worldCamera.CurrentBehavior = game.previousBehavior;
                                game.worldCamera.CurrentBehavior = Cameras.QuaternionCamera.Behavior.FollowT;
                            }
                            explosionAge += elapsedTime;
                            if (explosionAge >= explosionLifetime)
                            {
                                game.bulletManager.RemoveBullet(this);
                            }

                            break;
                        default:
                            break;
                    }

                    base.Update(gameTime);
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            switch (game.currentState1)
            {
                case Game1.GameState1.Play:
                    switch (bulletState)
                    {
                        case BulletState.Unexploded:
                            game.drawUtils.DrawSphere(position, 5.0f, Color.Green);
                            break;
                        case BulletState.Exploding:
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
            StartExplosion();
        }

        public void StartExplosion()
        {
            switch (bulletState)
            {
                case BulletState.Unexploded:

                    bulletState = BulletState.Exploding;
                    explosionAge = 0.0f;

                    Vector3 explosionVelocity = new Vector3(velocity.X, 0.0f, velocity.Z);

                    for (int i = 0; i < numExplosionParticles; i++)
                        explosionParticles.AddParticle(position, explosionVelocity);

                    for (int i = 0; i < numExplosionSmokeParticles; i++)
                        explosionSmokeParticles.AddParticle(position, explosionVelocity);

                    break;
                case BulletState.Exploding:
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
    }
}
