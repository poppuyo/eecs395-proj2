using System;
using Microsoft.Xna.Framework;
using tanks3d.Physics;
using tanks3d.Weapons;

namespace tanks3d.Cameras
{
    public class PhysicsCamera : QuaternionCameraComponent, IPhysicsObject
    {
        private Vector3 verticalOffset;

        public PhysicsCamera(Game1 game)
            : base(game)
        {
            verticalOffset = new Vector3(0, 100, 0);
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Velocity *= 0.999f;

            // Check if the bullet doesn't exist anymore.
            if (FollowBullet == null)
            {
                LookAt(Position, g.currentTank.position, Vector3.Up);
                return;
            }

            Position += verticalOffset;

            switch (FollowBullet.bulletState)
            {
                case BulletState.Unexploded:
                    LookAt(Position, FollowBullet.position, Vector3.Up);
                    break;
                case BulletState.Exploding:
                    LookAt(Position, FollowBullet.ExplosionLocation, Vector3.Up);
                    break;
                case BulletState.Dead:
                    LookAt(Position, g.currentTank.position, Vector3.Up);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Position -= verticalOffset;
        }
    }
}