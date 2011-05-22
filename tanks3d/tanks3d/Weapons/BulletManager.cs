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

        /// <summary>
        /// List of all the bullets that are currently present and active in the game, either
        /// firing or exploding.
        /// </summary>
        List<TestBullet> bullets = new List<TestBullet>();

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

        /*
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        */

        public TestBullet SpawnBullet(Vector3 origin, Vector3 initialVelocity)
        {
            TestBullet bullet = new TestBullet(game,
                                           explosionParticles,
                                           explosionSmokeParticles,
                                           projectileTrailParticles,
                                           origin,
                                           initialVelocity);
            bullets.Add(bullet);
            game.Components.Add(bullet);

            return bullet;
        }

        public void RemoveBullet(TestBullet bullet)
        {
            bullets.Remove(bullet);
        }
    }
}
