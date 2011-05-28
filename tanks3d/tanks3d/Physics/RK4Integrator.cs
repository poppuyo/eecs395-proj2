using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    public class RK4Integrator : PhysicsIntegrator
    {
        public override State integrate(State state, float t, float dt, Vector3 accel)
        {
            Derivative a = evaluate(state, t, 0.0f, new Derivative(), accel);
            Derivative b = evaluate(state, t + dt * 0.5f, dt * 0.5f, a, accel);
            Derivative c = evaluate(state, t + dt * 0.5f, dt * 0.5f, b, accel);
            Derivative d = evaluate(state, t + dt, dt, c, accel);

            Vector3 dxdt = 1.0f / 6.0f * (a.dx + 2.0f * (b.dx + c.dx) + d.dx);
            Vector3 dvdt = 1.0f / 6.0f * (a.dv + 2.0f * (b.dv + c.dv) + d.dv);

            Vector3 finalPosition = state.Position + dxdt * dt;
            Vector3 finalVelocity = state.Velocity + dvdt * dt;

            return new State(finalPosition, finalVelocity);
        }
    }
}
