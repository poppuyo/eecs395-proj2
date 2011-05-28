using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    public class EulerIntegrator : PhysicsIntegrator
    {
        public override State integrate(State state, float t, float dt, Vector3 accel)
        {
            /*
            Derivative d = evaluate(state, t, dt, new Derivative(), accel);
            Vector3 dxdt = d.dx;
            Vector3 dvdt = d.dv;
            */

            state.Position = state.Position + state.Velocity * dt;
            state.Velocity = state.Velocity + accel * dt;
            return state;
        }
    }
}
