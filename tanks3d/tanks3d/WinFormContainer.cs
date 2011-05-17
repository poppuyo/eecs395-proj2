using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;

namespace tanks3d
{
    public partial class WinFormContainer : Form
    {
        public Game1 game;

        public WinFormContainer()
        {
            InitializeComponent();
        }

        public IntPtr getDrawSurface()
        {
            return pctSurface.Handle;
        }

        private void pctSurface_SizeChanged(object sender, EventArgs e)
        {
            if (game != null)
            {
                //game.GraphicsDevice.Viewport = new Microsoft.Xna.Framework.Graphics.Viewport(pctSurface.Left, pctSurface.Top, pctSurface.Width, pctSurface.Height);
                //game.GraphicsDevice.SetRenderTarget(new Microsoft.Xna.Framework.Graphics.RenderTarget2D(game.GraphicsDevice, pctSurface.Width, pctSurface.Height));
                //game.GraphicsDevice.Reset();

                PresentationParameters newParams = game.GraphicsDevice.PresentationParameters;
                newParams.BackBufferWidth = pctSurface.Width;
                newParams.BackBufferHeight = pctSurface.Height;
                game.GraphicsDevice.Reset(newParams);
                game.worldCamera.UpdateViewport(game.GraphicsDevice.Viewport);
            }
        }

        private void WinFormContainer_Shown(object sender, EventArgs e)
        {
            // Force the reticle (crosshair thingy) to be aligned with the mouse.
            pctSurface_SizeChanged(null, null);
        }
    }
}
