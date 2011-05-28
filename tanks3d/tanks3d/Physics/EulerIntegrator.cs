using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    public class EulerIntegrator : PhysicsIntegrator
    {
        public override State integrate(State state, float dt, Vector3 accel)
        {
            state.Position = state.Position + state.Velocity * dt;
            state.Velocity = state.Velocity + accel * dt;
            return state;
        }
    }
}
