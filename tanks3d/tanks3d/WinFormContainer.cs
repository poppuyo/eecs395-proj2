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
using tanks3d.Weapons;

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
                //game.worldCamera.UpdateViewport(game.GraphicsDevice.Viewport);
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
            UpdateCameraTargetInfo();
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

        private void UpdateCameraTargetInfo()
        {
            if (!CameraViewDirX_TextBox.Focused)
            {
                CameraViewDirX_TextBox.Text = String.Format("{0:F2}", game.worldCamera.ViewDirection.X);
            }

            if (!CameraViewDirY_TextBox.Focused)
            {
                CameraViewDirY_TextBox.Text = String.Format("{0:F2}", game.worldCamera.ViewDirection.Y);
            }

            if (!CameraViewDirZ_TextBox.Focused)
            {
                CameraViewDirZ_TextBox.Text = String.Format("{0:F2}", game.worldCamera.ViewDirection.Z);
            }
        }

        private void HandleNumericTextBox(object sender, KeyPressEventArgs e)
        {
            // Handle enter key
            if (e.KeyChar == (char)Keys.Return)
            {
                // For target position controls, simulate clicking the "Set target" button
                if ((sender as TextBox).Name.Contains("CameraTarget"))
                {
                    SetTargetButton_Click(null, null);
                }

                // If Enter is pressed, set the focus away from the control so that it triggers the
                // Leave event and causes the game to update based on the new value in the control.
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

        private void SetTargetButton_Click(object sender, EventArgs e)
        {
            try
            {
                float newTargetX = (float)Convert.ToDouble(CameraTargetX_TextBox.Text);
                float newTargetY = (float)Convert.ToDouble(CameraTargetY_TextBox.Text);
                float newTargetZ = (float)Convert.ToDouble(CameraTargetZ_TextBox.Text);
                game.worldCamera.LookAt(game.worldCamera.Position, new Vector3(newTargetX, newTargetY, newTargetZ), Vector3.Up);
                CameraMessageLabel.Visible = false;
            }
            catch (FormatException)
            {
                CameraMessageLabel.Text = "Invalid target position.";
                CameraMessageLabel.Visible = true;
                return;
            }
        }

        private WeaponTypes selectedWeaponType;

        private void WeaponTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = WeaponTypesComboBox.SelectedItem.ToString();

            if (Enum.IsDefined(typeof(WeaponTypes), selected))
            {
                selectedWeaponType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), selected);
                weaponMessageLabel.Visible = false;
            }
            else
            {
                weaponMessageLabel.Text = "Invalid weapon type selected.";
                weaponMessageLabel.Visible = true;
            }
        }

        private void WinFormContainer_Load(object sender, EventArgs e)
        {
            // Set up weapon types
            InitWeaponTypesComboBox();
            if (WeaponTypesComboBox.Items.Count >= 1)
            {
                WeaponTypesComboBox.SelectedIndex = 0;
            }

            // Create a default turret
            TurretPosition = new Vector3(0, -15, 0);
            TurretAim = new Vector3(0, -1, 0);
            WeaponPower = 100.0f;
            UpdateTurretInfo();

            // Initialize game states
            InitGameState();
            UpdateGameState();
        }

        private void UpdateTurretInfo()
        {
            if (TurretPosition.HasValue)
            {
                TurretPositionX_TextBox.Text = TurretPosition.Value.X.ToString();
                TurretPositionY_TextBox.Text = TurretPosition.Value.Y.ToString();
                TurretPositionZ_TextBox.Text = TurretPosition.Value.Z.ToString();
            }

            if (TurretAim.HasValue)
            {
                TurretAimX_TextBox.Text = TurretAim.Value.X.ToString();
                TurretAimY_TextBox.Text = TurretAim.Value.Y.ToString();
                TurretAimZ_TextBox.Text = TurretAim.Value.Z.ToString();
            }

            if (WeaponPower.HasValue)
            {
                WeaponPowerTextBox.Text = WeaponPower.Value.ToString();
            }
        }

        private void InitWeaponTypesComboBox()
        {
            foreach (string weaponName in Enum.GetNames(typeof(WeaponTypes)))
            {
                WeaponTypesComboBox.Items.Add(weaponName);
            }
        }

        private Vector3? TurretPosition;
        private Vector3? TurretAim;
        private float? WeaponPower;

        private void SetTurretPosition_Click(object sender, EventArgs e)
        {
            try
            {
                float turretX = (float)Convert.ToDouble(TurretPositionX_TextBox.Text);
                float turretY = (float)Convert.ToDouble(TurretPositionY_TextBox.Text);
                float turretZ = (float)Convert.ToDouble(TurretPositionZ_TextBox.Text);
                TurretPosition = new Vector3(turretX, turretY, turretZ);
                weaponMessageLabel.Visible = false;
            }
            catch (FormatException)
            {
                TurretPosition = null;
                weaponMessageLabel.Text = "Invalid turret position.";
                weaponMessageLabel.Visible = true;
                return;
            }
        }

        private void DrawTurret()
        {
            if (TurretPosition.HasValue)
            {
                game.drawUtils.DrawSphere(TurretPosition.Value, 5.0f, Microsoft.Xna.Framework.Color.Green);
                
                if (TurretAim.HasValue)
                {
                    Vector3 start = TurretPosition.Value;
                    Vector3 end = TurretPosition.Value + (5.0f * TurretAim.Value) + TurretAim.Value;

                    game.drawUtils.DrawLine(start, end, Microsoft.Xna.Framework.Color.Green);

                    // Draw a smaller sphere at the end
                    game.drawUtils.DrawSphere(end, 1.0f, Microsoft.Xna.Framework.Color.Green);
                }
            }
        }

        private void DrawTurretTimer_Tick(object sender, EventArgs e)
        {
            DrawTurret();
        }

        private void SetTurretAim_Click(object sender, EventArgs e)
        {
            try
            {
                float turretAimX = (float)Convert.ToDouble(TurretAimX_TextBox.Text);
                float turretAimY = (float)Convert.ToDouble(TurretAimY_TextBox.Text);
                float turretAimZ = (float)Convert.ToDouble(TurretAimZ_TextBox.Text);

                if (TurretAim == Vector3.Zero)
                {
                    TurretAim = null;
                    weaponMessageLabel.Text = "Invalid turret aim.";
                    weaponMessageLabel.Visible = true;
                    return;
                }
                else
                {
                    // Normalize the direction
                    Vector3 TurretAimNormalized = new Vector3(turretAimX, turretAimY, turretAimZ);
                    TurretAimNormalized.Normalize();
                    TurretAim = TurretAimNormalized;

                    // But don't update the form because that might result in long ugly numbers
                    // with too many decimal points.
                    //TurretAimX_TextBox.Text = TurretAim.Value.X.ToString();
                    //TurretAimY_TextBox.Text = TurretAim.Value.Y.ToString();
                    //TurretAimZ_TextBox.Text = TurretAim.Value.Z.ToString();

                    weaponMessageLabel.Visible = false;
                }
            }
            catch (FormatException)
            {
                TurretAim = null;
                weaponMessageLabel.Text = "Invalid turret aim.";
                weaponMessageLabel.Visible = true;
                return;
            }
        }

        private void FireButton_Click(object sender, EventArgs e)
        {
            if (!TurretPosition.HasValue)
            {
                weaponMessageLabel.Text = "Turret position needs a value";
                weaponMessageLabel.Visible = true;
                return;
            }

            if (!TurretAim.HasValue)
            {
                weaponMessageLabel.Text = "Turret aim needs a value";
                weaponMessageLabel.Visible = true;
                return;
            }

            try
            {
                float power = (float)Convert.ToDouble(WeaponPowerTextBox.Text);
                WeaponPower = power;
                weaponMessageLabel.Visible = false;
            }
            catch (FormatException)
            {
                WeaponPower = null;
                weaponMessageLabel.Text = "Invalid power";
                weaponMessageLabel.Visible = true;
                return;
            }

            weaponMessageLabel.Visible = false;

            // This line would cause the tank to fire, but it ignores turret position and aim
            // that is set in the form:
            //game.weaponManager.Weapons[selectedWeaponType].Fire();

            game.bulletManager.SpawnBullet(TurretPosition.Value, TurretAim.Value * WeaponPower.Value);
        }

        private void LocateTurretPositionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!TurretPosition.HasValue)
            {
                weaponMessageLabel.Text = "Turret position is not set";
                weaponMessageLabel.Visible = true;
            }
            else
            {
                Vector3 newCameraPosition = TurretPosition.Value;
                newCameraPosition += 5.0f * new Vector3(5.0f, 5.0f, 5.0f);

                game.worldCamera.LookAt(newCameraPosition, TurretPosition.Value, Vector3.Up);

                weaponMessageLabel.Visible = false;
            }
        }

        private void InitGameState()
        {
            foreach (string gameState in Enum.GetNames(typeof(tanks3d.GameState)))
            {
                gameStateComboBox.Items.Add(gameState);
            }

            foreach (string playerState in Enum.GetNames(typeof(tanks3d.PlayerState)))
            {
                playerStateComboBox.Items.Add(playerState);
            }

            foreach (string cameraState in Enum.GetNames(typeof(tanks3d.Cameras.QuaternionCamera.Behavior)))
            {
                cameraStateComboBox.Items.Add(cameraState);
            }
        }

        private void gameStateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update Game State (GameState1)
            string selected = gameStateComboBox.SelectedItem.ToString();
            if (Enum.IsDefined(typeof(tanks3d.GameState), selected))
            {
                gameState = (tanks3d.GameState)Enum.Parse(typeof(tanks3d.GameState), selected);
                game.gameState = gameState;
                stateMessageLabel.Visible = false;
            }
            else
            {
                stateMessageLabel.Text = "Unrecognized game state.";
                stateMessageLabel.Visible = true;
            }
        }

        private void playerStateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update Player State (GameState)
            string selected = playerStateComboBox.SelectedItem.ToString();
            if (Enum.IsDefined(typeof(tanks3d.PlayerState), selected))
            {
                playerState = (tanks3d.PlayerState)Enum.Parse(typeof(tanks3d.PlayerState), selected);
                game.currentTank.currentPlayerState = playerState;
                stateMessageLabel.Visible = false;

                if (playerState == PlayerState.Aim)
                {
                    game.currentTank.ChangeToAim();
                }
            }
            else
            {
                stateMessageLabel.Text = "Unrecognized player state.";
                stateMessageLabel.Visible = true;
            }
        }

        private void cameraStateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update camera state
            string selected = cameraStateComboBox.SelectedItem.ToString();
            if (Enum.IsDefined(typeof(tanks3d.Cameras.QuaternionCamera.Behavior), selected))
            {
                cameraState = (tanks3d.Cameras.QuaternionCamera.Behavior)Enum.Parse(typeof(tanks3d.Cameras.QuaternionCamera.Behavior), selected);
                game.worldCamera.CurrentBehavior = cameraState;
                stateMessageLabel.Visible = false;
            }
            else
            {
                stateMessageLabel.Text = "Unrecognized camera state.";
                stateMessageLabel.Visible = true;
            }
        }

        private void UpdateStateTimer_Tick(object sender, EventArgs e)
        {
            UpdateGameState();
        }

        private tanks3d.GameState gameState;
        private tanks3d.PlayerState playerState;
        private tanks3d.Cameras.QuaternionCamera.Behavior cameraState;

        private void UpdateGameState()
        {
            // Update game state
            if (!gameStateComboBox.DroppedDown)
            {
                tanks3d.GameState gameState = game.gameState;
                string gameStateString = System.Enum.GetName(typeof(tanks3d.GameState), gameState);
                if (gameStateComboBox.Items.Contains(gameStateString))
                {
                    gameStateComboBox.SelectedIndex = gameStateComboBox.Items.IndexOf(gameStateString);
                    stateMessageLabel.Visible = false;
                }
                else
                {
                    stateMessageLabel.Text = "Unknown game state: \"" + gameStateString + "\".";
                    stateMessageLabel.Visible = true;
                }
            }

            // Update player state
            if (!playerStateComboBox.DroppedDown)
            {
                tanks3d.PlayerState playerState = game.currentTank.currentPlayerState;
                string playerStateString = System.Enum.GetName(typeof(tanks3d.PlayerState), playerState);
                if (playerStateComboBox.Items.Contains(playerStateString))
                {
                    playerStateComboBox.SelectedIndex = playerStateComboBox.Items.IndexOf(playerStateString);
                }
                else
                {
                    stateMessageLabel.Text = "Unknown player state: \"" + playerStateString + "\".";
                    stateMessageLabel.Visible = true;
                }
            }

            // Update camera state
            if (!cameraStateComboBox.DroppedDown)
            {
                tanks3d.Cameras.QuaternionCamera.Behavior cameraState = game.worldCamera.CurrentBehavior;
                string cameraStateString = System.Enum.GetName(typeof(tanks3d.Cameras.QuaternionCamera.Behavior), cameraState);
                if (cameraStateComboBox.Items.Contains(cameraStateString))
                {
                    cameraStateComboBox.SelectedIndex = cameraStateComboBox.Items.IndexOf(cameraStateString);
                }
                else
                {
                    stateMessageLabel.Text = "Unknown camera state: \"" + cameraStateString + "\".";
                    stateMessageLabel.Visible = true;
                }
            }

            UpdateActivePlayer();
        }

        private void UpdateActivePlayer()
        {
            if (!activePlayerComboBox.DroppedDown)
            {
                string previousNumPlayers = numPlayersLabel.Text;
                numPlayersLabel.Text = game.numPlayers.ToString();

                if (previousNumPlayers != numPlayersLabel.Text)
                {
                    RepopulateActivePlayerComboBox(game.numPlayers);
                }

                if (activePlayerComboBox.Items.Contains(game.currentPlayer.ToString()))
                {
                    activePlayerComboBox.SelectedIndex = activePlayerComboBox.Items.IndexOf(game.currentPlayer.ToString());
                }
                else
                {
                    stateMessageLabel.Text = "Currently active player is >= num players";
                    stateMessageLabel.Visible = true;
                }
            }
        }

        private void RepopulateActivePlayerComboBox(int numPlayers)
        {
            activePlayerComboBox.Items.Clear();

            for (int i = 0; i < numPlayers; i++)
            {
                activePlayerComboBox.Items.Add(i.ToString());
            }
        }

        private void pctSurface_Click(object sender, EventArgs e)
        {
            pctSurface.Focus();
        }

        private void activePlayerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                game.currentPlayer = int.Parse(activePlayerComboBox.SelectedItem.ToString());
                stateMessageLabel.Visible = false;
            }
            catch (Exception ex)
            {
                stateMessageLabel.Text = "Failed to change active player: " + ex.Message;
                stateMessageLabel.Visible = true;
            }
        }

        private void WinFormContainer_Resize(object sender, EventArgs e)
        {
            game.Window_ClientSizeChanged(null, null);
        }
    }
}
