using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    public class TestPhysicsObject : DrawableGameComponent, IPhysicsObject
    {
        private Game1 game;

        public Vector3 Position;
        public Vector3 Velocity;
        float diameter;

        public TestPhysicsObject(Game1 g, Vector3 initPosition, Vector3 initVelocity)
            : base(g)
        {
            game = g;
            diameter = 10.0f;
            Position = initPosition;
            Velocity = initVelocity;
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
            float radius = diameter / 2.0f;
            Vector3 extent = new Vector3(radius, radius, radius);
            return new BoundingBox(Position - extent, Position + extent);
        }

        public override void Draw(GameTime gameTime)
        {
            game.drawUtils.DrawSphere(Position, diameter, Color.Gold);

            base.Draw(gameTime);
        }
    }
}
