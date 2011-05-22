using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tanks3d.ParticleSystems;
using Microsoft.Xna.Framework;

namespace tanks3d.Weapons
{
    public class BulletManager : DrawableGameComponent
    {
        private Game1 game;

        List<TestBullet> bullets = new List<TestBullet>();
        
        TimeSpan timeToNextProjectile = TimeSpan.Zero;

        ParticleSystem explosionParticles;
        ParticleSystem explosionSmokeParticles;
        ParticleSystem projectileTrailParticles;

        public BulletManager(Game1 g)
            : base(g)
        {
            game = g;
        }

        protected override void LoadContent()
        {
            explosionParticles = new ExplosionParticleSystem(game, game.Content);
            explosionSmokeParticles = new ExplosionSmokeParticleSystem(game, game.Content);
            projectileTrailParticles = new ProjectileTrailParticleSystem(game, game.Content);

            // Set the draw order so the explosions and fire
            // will appear over the top of the smoke.
            explosionSmokeParticles.DrawOrder = 200;
            projectileTrailParticles.DrawOrder = 300;
            explosionParticles.DrawOrder = 400;

            game.Components.Add(explosionParticles);
            game.Components.Add(explosionSmokeParticles);
            game.Components.Add(projectileTrailParticles);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateExplosions(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Helper for updating the explosions effect.
        /// </summary>
        void UpdateExplosions(GameTime gameTime)
        {
            timeToNextProjectile -= gameTime.ElapsedGameTime;

            if (timeToNextProjectile <= TimeSpan.Zero)
            {
                // Create a new projectile once per second. The real work of moving
                // and creating particles is handled inside the Projectile class.
                TestBullet bullet = new TestBullet(game,
                                           explosionParticles,
                                           explosionSmokeParticles,
                                           projectileTrailParticles);
                bullets.Add(bullet);
                game.Components.Add(bullet);

                timeToNextProjectile += TimeSpan.FromSeconds(1);
            }
        }

        public void RemoveBullet(TestBullet bullet)
        {
            bullets.Remove(bullet);
        }
    }
}
