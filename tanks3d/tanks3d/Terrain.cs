using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d
{
    public interface ITerrain : IGameComponent, IDrawable
    {
        /// <summary>
        /// Returns the y-coordinate of the ground at position (X,Z)
        /// </summary>
        float GetHeightAt(int x, int z);

        /// <summary>
        /// Returns the y-coordinate of the ground at position (X,Z)
        /// </summary>
        float GetHeightAt(float x, float z);

        /// <summary>
        /// Given a vector (X,Z), returns the y-coordinate of the ground at that position
        /// </summary>
        float GetHeightAt(Vector2 pos);

        /// <summary>
        /// Given a vector (X,Y,Z), returns the y-coordinate of the ground at position (X,Z).
        /// The Y component of the input is ignored.
        /// </summary>
        float GetHeightAt(Vector3 pos);
    }
}
