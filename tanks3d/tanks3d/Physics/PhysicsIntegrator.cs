using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    public enum IntegrationMethod
    {
        /// <summary>
        /// Simple, fast, but sloppy.
        /// </summary>
        Euler,

        /// <summary>
        /// Slow, expensive, but accurate.
        /// </summary>
        // RungeKutta4,
    }

    public abstract class PhysicsIntegrator
    {
        public abstract State integrate(State state, float dt, Vector3 accel);

        protected Derivative evaluate(State initial, float dt, Derivative d, Vector3 accel)
        {
            Vector3 Position = initial.Position + d.dx * dt;
            Vector3 Velocity = initial.Position + d.dv * dt;
            State state = new State(Position, Velocity);

            Derivative output = new Derivative();
            output.dx = state.Velocity;
            output.dv = accel;
            return output;
        }
    }
}
