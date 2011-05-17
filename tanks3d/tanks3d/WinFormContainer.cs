using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

            // Set the focus to the game (away from any controls).
            pctSurface.Focus();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCameraPositionInfo();
        }

        private void UpdateCameraPositionInfo()
        {
            if (!CameraPositionX_TextBox.Focused)
            {
                CameraPositionX_TextBox.Text = String.Format("{0:F2}", game.worldCamera.Position.X);
            }

            if (!CameraPositionY_TextBox.Focused)
            {
                CameraPositionY_TextBox.Text = String.Format("{0:F2}", game.worldCamera.Position.Y);
            }

            if (!CameraPositionZ_TextBox.Focused)
            {
                CameraPositionZ_TextBox.Text = String.Format("{0:F2}", game.worldCamera.Position.Z);
            }
        }

        private void HandleNumericTextBox(object sender, KeyPressEventArgs e)
        {
            // If Enter is pressed, set the focus away from the control so that it triggers the
            // Leave event and causes the game to update based on the new value in the control.
            if (e.KeyChar == (char)Keys.Return)
            {
                pctSurface.Focus();
                e.Handled = true;
                return;   
            }
            
            // Handle illegal keys
            if (!char.IsControl(e.KeyChar)
                 && !char.IsDigit(e.KeyChar)
                 && e.KeyChar != '.'
                 && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void CameraPositionX_TextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                float newX = (float)Convert.ToDouble(CameraPositionX_TextBox.Text);
                game.worldCamera.Position = new Vector3(newX, game.worldCamera.Position.Y, game.worldCamera.Position.Z);
            }
            catch (FormatException)
            {
                return;
            }
        }

        private void CameraPositionY_TextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                float newY = (float)Convert.ToDouble(CameraPositionY_TextBox.Text);
                game.worldCamera.Position = new Vector3(game.worldCamera.Position.X, newY, game.worldCamera.Position.Z);
            }
            catch (FormatException)
            {
                return;
            }
        }

        private void CameraPositionZ_TextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                float newZ = (float)Convert.ToDouble(CameraPositionZ_TextBox.Text);
                game.worldCamera.Position = new Vector3(game.worldCamera.Position.X, game.worldCamera.Position.Y, newZ);
            }
            catch (FormatException)
            {
                return;
            }
        }
    }
}
