using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    public class PhysicsEngine : GameComponent
    {
        private Game1 game;

        private List<IPhysicsObject> physicsObjects;

        public PhysicsEngine(Game1 g)
            : base(g)
        {
            game = g;
            physicsObjects = new List<IPhysicsObject>();
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        public void AddPhysicsObject(IPhysicsObject obj)
        {
            physicsObjects.Add(obj);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (IPhysicsObject physicsObject in physicsObjects)
            {
                // Move all objects according to their current velocities
                Vector3 oldPosition = physicsObject.GetPosition();
                Vector3 newPosition = oldPosition + physicsObject.GetVelocity() * elapsedSeconds;
                physicsObject.UpdatePosition(newPosition);

                Gravity(physicsObject, elapsedSeconds);

                DoCollisionDetectionWithTerrain(physicsObject);

                System.Console.WriteLine(physicsObject.GetPosition());
            }

            base.Update(gameTime);
        }

        private void DoCollisionDetectionWithTerrain(IPhysicsObject physicsObject)
        {
            Vector3 position = physicsObject.GetPosition();

            if (game.heightMapInfo.IsOnHeightmap(position))
            {
                float terrainElevation;
                Vector3 terrainNormal;
                game.heightMapInfo.GetHeightAndNormal(position, out terrainElevation, out terrainNormal);
                float objectBottom = physicsObject.GetBoundingBox().Min.Y;

                if (objectBottom <= terrainElevation)
                {
                    HandleCollisionWithSurface(physicsObject, terrainNormal, 0.7f);
                }
            }
        }

        /// <summary>
        /// Handles a collision between the physics object and a surface described by a surface
        /// normal. The object's velocity will change such that the angle of incidence and the 
        /// angle of reflection are equal with respect to the surface normal. Note that this
        /// method does not check whether any sort of a collision actually occurred - it trusts
        /// the caller to determine that.
        /// </summary>
        /// <param name="physicsObject">The object whose velocity will change as a result of the
        /// collision.</param>
        /// <param name="surfaceNormal">The surface normal of the surface against which the object
        /// collided.</param>
        /// <param name="restitution">Coefficient of restitution. 1.0f means an elastic collision (no
        /// energy lost), so the object will bounce back as forcefully as it went in. For 0.0f, the
        /// object will effectively stop at the surface.</param>
        private void HandleCollisionWithSurface(IPhysicsObject physicsObject, Vector3 surfaceNormal,
            float restitution)
        {
            Vector3 oldVelocity = physicsObject.GetVelocity();

            // Project velocity vector onto the surface normal
            float v_dot_n = Vector3.Dot(oldVelocity, surfaceNormal);

            // Calculate the normal force
            Vector3 normalForce = -(1.0f + restitution) * v_dot_n * surfaceNormal;

            // Add the normal force to the motion vector to get the new velocity
            Vector3 newVelocity = oldVelocity + normalForce;

            physicsObject.UpdateVelocity(newVelocity);
        }

        /// <summary>
        /// Update the given object's velocity based on the effect of gravity.
        /// </summary>
        /// <param name="physicsObject"></param>
        private static void Gravity(IPhysicsObject physicsObject, float elapsedSeconds)
        {
            Vector3 oldVelocity = physicsObject.GetVelocity();
            Vector3 newVelocity = oldVelocity + PhysicsUtil.GravityConstant * Vector3.Down * elapsedSeconds;
            physicsObject.UpdateVelocity(newVelocity);
        }
    }
}
