using System;
using Microsoft.Xna.Framework;
using tank3d;
using tanks3d.Physics;
using tanks3d.Weapons;

namespace tanks3d.Cameras
{
    public class PhysicsCamera : QuaternionCameraComponent, IPhysicsObject
    {
        public PhysicsCamera(Game1 game)
            : base(game)
        {
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            Position = newPosition;
        }

        public Vector3 GetVelocity()
        {
            return Velocity;
        }

        public void UpdateVelocity(Vector3 newVelocity)
        {
            Velocity = newVelocity;
        }

        public BoundingBox GetBoundingBox()
        {
            const float halfSize = 1000.0f;
            Vector3 extent = new Vector3(halfSize, halfSize, halfSize);
            return new BoundingBox(Position - extent, Position + extent);
        }

        public void HandleCollisionWithTerrain()
        {
            // ignore
        }

        public bool DoBoundsCheck()
        {
            return true;
        }

        public Tank deadTank;
        public bool shake = false;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check if the bullet doesn't exist anymore.
            if (FollowBullet == null)
            {
                LookAt(Position, g.currentTank.position, Vector3.Up);
                return;
            }

            Vector3 PositionAboveTerrain = g.terrain.AdjustForTerrainHeight(Position, 150.0f);

            switch (FollowBullet.bulletState)
            {
                case BulletState.Unexploded:
                    LookAt(PositionAboveTerrain, FollowBullet.position, Vector3.Up);
                    break;
                case BulletState.Exploding:
                    Vector3 target = (deadTank != null) ? deadTank.position : FollowBullet.ExplosionLocation;
                    LookAt(PositionAboveTerrain, target, Vector3.Up);
                    break;
                case BulletState.Dead:
                    if (deadTank != null)
                    {
                        LookAt(PositionAboveTerrain, deadTank.position, Vector3.Up);
                        if (shake) { this.Rotate(g.RandomFloat(), g.RandomFloat(), g.RandomFloat()); }
                    }
                    else
                    {
                        g.ExitBulletView();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}