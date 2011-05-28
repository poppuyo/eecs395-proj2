using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tanks3d.Physics
{
    public static class PhysicsUtil
    {
        public static float GravityConstant = 50.0f;
    }

    public class State
    {
        public Vector3 Position;
        public Vector3 Velocity;

        public State(Vector3 x, Vector3 v)
        {
            this.Position = x;
            this.Velocity = v;
        }
    }

    public class Derivative
    {
        public Vector3 dx;
        public Vector3 dv;

        public Derivative()
        {
            dx = Vector3.Zero;
            dv = Vector3.Zero;
        }

        public Derivative(Vector3 dx, Vector3 dv)
        {
            this.dx = dx;
            this.dv = dv;
        }
    }
}
