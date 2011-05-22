using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tanks3d.ParticleSystems;

namespace tanks3d.Weapons
{
    public class TestBullet : DrawableGameComponent
    {
        private Game1 game;

        public Vector3 position;
        public Vector3 velocity;
        float age;

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
        const float gravity = 15;

        #endregion

        public TestBullet(Game1 g, ParticleSystem explosionParticles, ParticleSystem explosionSmokeParticles,
            ParticleSystem projectileTrailParticles, Vector3 origin, Vector3 initialVelocity)
            : base(g)
        {
            game = g;
            bulletState = BulletState.Unexploded;

            this.position = origin;
            this.velocity = initialVelocity;

            this.explosionParticles = explosionParticles;
            this.explosionSmokeParticles = explosionSmokeParticles;

            // Use the particle emitter helper to output our trail particles.
            trailEmitter = new ParticleEmitter(projectileTrailParticles,
                                               trailParticlesPerSecond, position);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (bulletState)
            {
                case BulletState.Unexploded:
                    
                    // Simple projectile physics.
                    position += velocity * elapsedTime;
                    velocity.Y -= elapsedTime * gravity;
                    age += elapsedTime;

                    // Update the particle emitter, which will create our particle trail.
                    trailEmitter.Update(gameTime, position);

                    // If enough time has passed, explode! Note how we pass our velocity
                    // in to the AddParticle method: this lets the explosion be influenced
                    // by the speed and direction of the projectile which created it.
                    if (age > projectileLifespan)
                    {
                        bulletState = BulletState.Exploding;

                        for (int i = 0; i < numExplosionParticles; i++)
                            explosionParticles.AddParticle(position, velocity);

                        for (int i = 0; i < numExplosionSmokeParticles; i++)
                            explosionSmokeParticles.AddParticle(position, velocity);
                    }
                    break;
                case BulletState.Exploding:
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
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
        }
    }

    public enum BulletState
    {
        Unexploded,
        Exploding
    }
}
