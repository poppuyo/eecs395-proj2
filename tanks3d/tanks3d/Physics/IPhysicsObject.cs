using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    /// <summary>
    /// An object that undergoes collisions and is influenced by gravity. In order for an
    /// object to be influenced by physics in this game, it must implement this interface
    /// and be added to the physics engine.
    /// </summary>
    public interface IPhysicsObject
    {
        Vector3 GetPosition();
        void UpdatePosition(Vector3 newPosition);

        Vector3 GetVelocity();
        void UpdateVelocity(Vector3 newVelocity);

        BoundingBox GetBoundingBox();

        /// <summary>
        /// The physics engine will call this method when the object collides with the terrain.
        /// </summary>
        void HandleCollisionWithTerrain();
    }
}
