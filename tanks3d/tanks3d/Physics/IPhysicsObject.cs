using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    /// <summary>
    /// An object that undergoes collisions and is influenced by gravity.
    /// </summary>
    public interface IPhysicsObject
    {
        Vector3 GetPosition();
        void UpdatePosition(Vector3 newPosition);

        Vector3 GetVelocity();
        void UpdateVelocity(Vector3 newVelocity);

        BoundingBox GetBoundingBox();
    }
}
