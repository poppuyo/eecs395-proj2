﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tanks3d.ParticleSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace tanks3d.Weapons
{
    public class BulletManager : DrawableGameComponent
    {
        private Game1 game;

        /// <summary>
        /// List of all the bullets that are currently present and active in the game, either
        /// firing or exploding.
        /// </summary>
        List<Bullet> bullets = new List<Bullet>();

        public ParticleSystem explosionParticles;
        public ParticleSystem explosionSmokeParticles;
        public ParticleSystem projectileTrailParticles;

        SoundEffect explosionSFX;

        public Bullet ActiveBullet;

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

            explosionSFX = game.Content.Load<SoundEffect>("Audio\\Barrel Exploding");
        }

        /*
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        */

        public Bullet SpawnBullet(Vector3 origin, Vector3 initialVelocity)
        {
            Bullet bullet = new Bullet(game,
                                           explosionParticles,
                                           explosionSmokeParticles,
                                           projectileTrailParticles,
                                           origin,
                                           initialVelocity,
                                           explosionSFX);
            bullets.Add(bullet);
            game.Components.Add(bullet);

            ActiveBullet = bullet;

            return bullet;
        }

        public void RemoveBullet(Bullet bullet)
        {
            bullets.Remove(bullet);
        }
    }
}
