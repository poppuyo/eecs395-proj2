//
// Borrowed from the XNA Wiki:
// http://www.xnawiki.com/index.php?title=Rendering_Bounding_Boxes
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d.Utility
{
    /// <summary>
    /// Provides a set of methods for the rendering BoundingBoxs.
    /// </summary>
    public static class BoundingBoxRenderer
    {
        #region Fields

        static VertexPositionColor[] verts = new VertexPositionColor[8];
        static Int16[] indices = new Int16[]
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            0, 4,
            1, 5,
            2, 6,
            3, 7,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
        };

        static BasicEffect effect;

        #endregion

        /// <summary>
        /// Renders the bounding box for debugging purposes.
        /// </summary>
        /// <param name="box">The box to render.</param>
        /// <param name="graphicsDevice">The graphics device to use when rendering.</param>
        /// <param name="view">The current view matrix.</param>
        /// <param name="projection">The current projection matrix.</param>
        /// <param name="color">The color to use drawing the lines of the box.</param>
        public static void Render(
            Game1 game,
            BoundingBox box,
            GraphicsDevice graphicsDevice,
            Matrix view,
            Matrix projection,
            Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(graphicsDevice);
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
            }

            
            Vector3[] corners = box.GetCorners();
            for (int i = 0; i < 4; i++)
            {
                int j = i + 1;
                if (i == 3)
                {
                    j = 0;
                }
                game.drawUtils.DrawLine(corners[i], corners[j], color);
                game.drawUtils.DrawLine(corners[i], corners[i + 4], color);
            }

            for (int i = 4; i < 8; i++)
            {
                int j = i + 1;
                if (i == 7)
                {
                    j = 4;
                }
                game.drawUtils.DrawLine(corners[i], corners[j], color);
            }

            
        }

        /*
        static readonly SamplerState samplerState = new SamplerState
        {
            AddressU = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
        };
        */
    }
}
