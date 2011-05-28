namespace tanks3d
{
    partial class WinFormContainer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pctSurface = new System.Windows.Forms.PictureBox();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.CameraGroupBox = new System.Windows.Forms.GroupBox();
            this.CameraMessageLabel = new System.Windows.Forms.Label();
            this.SetTargetButton = new System.Windows.Forms.Button();
            this.CameraTargetZ_TextBox = new System.Windows.Forms.TextBox();
            this.CameraTargetY_TextBox = new System.Windows.Forms.TextBox();
            this.CameraTargetX_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CameraViewDirZ_TextBox = new System.Windows.Forms.TextBox();
            this.CameraViewDirY_TextBox = new System.Windows.Forms.TextBox();
            this.CameraViewDirX_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CameraPositionZ_TextBox = new System.Windows.Forms.TextBox();
            this.CameraPositionY_TextBox = new System.Windows.Forms.TextBox();
            this.CameraPositionX_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LocateTurretPositionLink = new System.Windows.Forms.LinkLabel();
            this.FireButton = new System.Windows.Forms.Button();
            this.WeaponPowerTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SetTurretAim = new System.Windows.Forms.Button();
            this.TurretAimZ_TextBox = new System.Windows.Forms.TextBox();
            this.TurretAimY_TextBox = new System.Windows.Forms.TextBox();
            this.TurretAimX_TextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SetTurretPosition = new System.Windows.Forms.Button();
            this.TurretPositionZ_TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TurretPositionY_TextBox = new System.Windows.Forms.TextBox();
            this.weaponMessageLabel = new System.Windows.Forms.Label();
            this.TurretPositionX_TextBox = new System.Windows.Forms.TextBox();
            this.WeaponTypesComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DrawTurretTimer = new System.Windows.Forms.Timer(this.components);
            this.wireframeRadioButton = new System.Windows.Forms.RadioButton();
            this.texturedRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
            this.CameraGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pctSurface
            // 
            this.pctSurface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pctSurface.Location = new System.Drawing.Point(12, 12);
            this.pctSurface.Name = "pctSurface";
            this.pctSurface.Size = new System.Drawing.Size(760, 411);
            this.pctSurface.TabIndex = 0;
            this.pctSurface.TabStop = false;
            this.pctSurface.SizeChanged += new System.EventHandler(this.pctSurface_SizeChanged);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // CameraGroupBox
            // 
            this.CameraGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraGroupBox.Controls.Add(this.texturedRadioButton);
            this.CameraGroupBox.Controls.Add(this.wireframeRadioButton);
            this.CameraGroupBox.Controls.Add(this.CameraMessageLabel);
            this.CameraGroupBox.Controls.Add(this.SetTargetButton);
            this.CameraGroupBox.Controls.Add(this.CameraTargetZ_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraTargetY_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraTargetX_TextBox);
            this.CameraGroupBox.Controls.Add(this.label3);
            this.CameraGroupBox.Controls.Add(this.CameraViewDirZ_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraViewDirY_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraViewDirX_TextBox);
            this.CameraGroupBox.Controls.Add(this.label2);
            this.CameraGroupBox.Controls.Add(this.CameraPositionZ_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraPositionY_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraPositionX_TextBox);
            this.CameraGroupBox.Controls.Add(this.label1);
            this.CameraGroupBox.Location = new System.Drawing.Point(12, 429);
            this.CameraGroupBox.Name = "CameraGroupBox";
            this.CameraGroupBox.Size = new System.Drawing.Size(231, 206);
            this.CameraGroupBox.TabIndex = 1;
            this.CameraGroupBox.TabStop = false;
            this.CameraGroupBox.Text = "Camera";
            // 
            // CameraMessageLabel
            // 
            this.CameraMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraMessageLabel.ForeColor = System.Drawing.Color.Maroon;
            this.CameraMessageLabel.Location = new System.Drawing.Point(6, 183);
            this.CameraMessageLabel.Name = "CameraMessageLabel";
            this.CameraMessageLabel.Size = new System.Drawing.Size(219, 20);
            this.CameraMessageLabel.TabIndex = 13;
            this.CameraMessageLabel.Text = "[ Message ]";
            this.CameraMessageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.CameraMessageLabel.Visible = false;
            // 
            // SetTargetButton
            // 
            this.SetTargetButton.Location = new System.Drawing.Point(159, 94);
            this.SetTargetButton.Name = "SetTargetButton";
            this.SetTargetButton.Size = new System.Drawing.Size(65, 23);
            this.SetTargetButton.TabIndex = 12;
            this.SetTargetButton.Text = "Set target";
            this.SetTargetButton.UseVisualStyleBackColor = true;
            this.SetTargetButton.Click += new System.EventHandler(this.SetTargetButton_Click);
            // 
            // CameraTargetZ_TextBox
            // 
            this.CameraTargetZ_TextBox.Location = new System.Drawing.Point(111, 96);
            this.CameraTargetZ_TextBox.Name = "CameraTargetZ_TextBox";
            this.CameraTargetZ_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraTargetZ_TextBox.TabIndex = 11;
            this.CameraTargetZ_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // CameraTargetY_TextBox
            // 
            this.CameraTargetY_TextBox.Location = new System.Drawing.Point(63, 96);
            this.CameraTargetY_TextBox.Name = "CameraTargetY_TextBox";
            this.CameraTargetY_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraTargetY_TextBox.TabIndex = 10;
            this.CameraTargetY_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // CameraTargetX_TextBox
            // 
            this.CameraTargetX_TextBox.Location = new System.Drawing.Point(15, 96);
            this.CameraTargetX_TextBox.Name = "CameraTargetX_TextBox";
            this.CameraTargetX_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraTargetX_TextBox.TabIndex = 9;
            this.CameraTargetX_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Target Position (look at):";
            // 
            // CameraViewDirZ_TextBox
            // 
            this.CameraViewDirZ_TextBox.Location = new System.Drawing.Point(182, 49);
            this.CameraViewDirZ_TextBox.Name = "CameraViewDirZ_TextBox";
            this.CameraViewDirZ_TextBox.ReadOnly = true;
            this.CameraViewDirZ_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraViewDirZ_TextBox.TabIndex = 7;
            // 
            // CameraViewDirY_TextBox
            // 
            this.CameraViewDirY_TextBox.Location = new System.Drawing.Point(134, 49);
            this.CameraViewDirY_TextBox.Name = "CameraViewDirY_TextBox";
            this.CameraViewDirY_TextBox.ReadOnly = true;
            this.CameraViewDirY_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraViewDirY_TextBox.TabIndex = 6;
            // 
            // CameraViewDirX_TextBox
            // 
            this.CameraViewDirX_TextBox.Location = new System.Drawing.Point(86, 49);
            this.CameraViewDirX_TextBox.Name = "CameraViewDirX_TextBox";
            this.CameraViewDirX_TextBox.ReadOnly = true;
            this.CameraViewDirX_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraViewDirX_TextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "View Direction:";
            // 
            // CameraPositionZ_TextBox
            // 
            this.CameraPositionZ_TextBox.Location = new System.Drawing.Point(182, 24);
            this.CameraPositionZ_TextBox.Name = "CameraPositionZ_TextBox";
            this.CameraPositionZ_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraPositionZ_TextBox.TabIndex = 3;
            this.CameraPositionZ_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            this.CameraPositionZ_TextBox.Leave += new System.EventHandler(this.CameraPositionZ_TextBox_Leave);
            // 
            // CameraPositionY_TextBox
            // 
            this.CameraPositionY_TextBox.Location = new System.Drawing.Point(134, 24);
            this.CameraPositionY_TextBox.Name = "CameraPositionY_TextBox";
            this.CameraPositionY_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraPositionY_TextBox.TabIndex = 2;
            this.CameraPositionY_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            this.CameraPositionY_TextBox.Leave += new System.EventHandler(this.CameraPositionY_TextBox_Leave);
            // 
            // CameraPositionX_TextBox
            // 
            this.CameraPositionX_TextBox.Location = new System.Drawing.Point(86, 24);
            this.CameraPositionX_TextBox.Name = "CameraPositionX_TextBox";
            this.CameraPositionX_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraPositionX_TextBox.TabIndex = 1;
            this.CameraPositionX_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            this.CameraPositionX_TextBox.Leave += new System.EventHandler(this.CameraPositionX_TextBox_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Location:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.LocateTurretPositionLink);
            this.groupBox1.Controls.Add(this.FireButton);
            this.groupBox1.Controls.Add(this.WeaponPowerTextBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.SetTurretAim);
            this.groupBox1.Controls.Add(this.TurretAimZ_TextBox);
            this.groupBox1.Controls.Add(this.TurretAimY_TextBox);
            this.groupBox1.Controls.Add(this.TurretAimX_TextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.SetTurretPosition);
            this.groupBox1.Controls.Add(this.TurretPositionZ_TextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TurretPositionY_TextBox);
            this.groupBox1.Controls.Add(this.weaponMessageLabel);
            this.groupBox1.Controls.Add(this.TurretPositionX_TextBox);
            this.groupBox1.Controls.Add(this.WeaponTypesComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(249, 429);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 206);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weapon Test";
            // 
            // LocateTurretPositionLink
            // 
            this.LocateTurretPositionLink.AutoSize = true;
            this.LocateTurretPositionLink.Location = new System.Drawing.Point(85, 52);
            this.LocateTurretPositionLink.Name = "LocateTurretPositionLink";
            this.LocateTurretPositionLink.Size = new System.Drawing.Size(73, 13);
            this.LocateTurretPositionLink.TabIndex = 26;
            this.LocateTurretPositionLink.TabStop = true;
            this.LocateTurretPositionLink.Text = "click to locate";
            this.LocateTurretPositionLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LocateTurretPositionLink_LinkClicked);
            // 
            // FireButton
            // 
            this.FireButton.Location = new System.Drawing.Point(116, 140);
            this.FireButton.Name = "FireButton";
            this.FireButton.Size = new System.Drawing.Size(114, 23);
            this.FireButton.TabIndex = 25;
            this.FireButton.Text = "Fire!";
            this.FireButton.UseVisualStyleBackColor = true;
            this.FireButton.Click += new System.EventHandler(this.FireButton_Click);
            // 
            // WeaponPowerTextBox
            // 
            this.WeaponPowerTextBox.Location = new System.Drawing.Point(52, 142);
            this.WeaponPowerTextBox.Name = "WeaponPowerTextBox";
            this.WeaponPowerTextBox.Size = new System.Drawing.Size(58, 20);
            this.WeaponPowerTextBox.TabIndex = 24;
            this.WeaponPowerTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Power:";
            // 
            // SetTurretAim
            // 
            this.SetTurretAim.Location = new System.Drawing.Point(164, 110);
            this.SetTurretAim.Name = "SetTurretAim";
            this.SetTurretAim.Size = new System.Drawing.Size(64, 23);
            this.SetTurretAim.TabIndex = 22;
            this.SetTurretAim.Text = "Set";
            this.SetTurretAim.UseVisualStyleBackColor = true;
            this.SetTurretAim.Click += new System.EventHandler(this.SetTurretAim_Click);
            // 
            // TurretAimZ_TextBox
            // 
            this.TurretAimZ_TextBox.Location = new System.Drawing.Point(116, 112);
            this.TurretAimZ_TextBox.Name = "TurretAimZ_TextBox";
            this.TurretAimZ_TextBox.Size = new System.Drawing.Size(42, 20);
            this.TurretAimZ_TextBox.TabIndex = 21;
            this.TurretAimZ_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // TurretAimY_TextBox
            // 
            this.TurretAimY_TextBox.Location = new System.Drawing.Point(68, 112);
            this.TurretAimY_TextBox.Name = "TurretAimY_TextBox";
            this.TurretAimY_TextBox.Size = new System.Drawing.Size(42, 20);
            this.TurretAimY_TextBox.TabIndex = 20;
            this.TurretAimY_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // TurretAimX_TextBox
            // 
            this.TurretAimX_TextBox.Location = new System.Drawing.Point(20, 112);
            this.TurretAimX_TextBox.Name = "TurretAimX_TextBox";
            this.TurretAimX_TextBox.Size = new System.Drawing.Size(42, 20);
            this.TurretAimX_TextBox.TabIndex = 19;
            this.TurretAimX_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Turret Aim (direction vector):";
            // 
            // SetTurretPosition
            // 
            this.SetTurretPosition.Location = new System.Drawing.Point(164, 66);
            this.SetTurretPosition.Name = "SetTurretPosition";
            this.SetTurretPosition.Size = new System.Drawing.Size(64, 23);
            this.SetTurretPosition.TabIndex = 17;
            this.SetTurretPosition.Text = "Set";
            this.SetTurretPosition.UseVisualStyleBackColor = true;
            this.SetTurretPosition.Click += new System.EventHandler(this.SetTurretPosition_Click);
            // 
            // TurretPositionZ_TextBox
            // 
            this.TurretPositionZ_TextBox.Location = new System.Drawing.Point(116, 68);
            this.TurretPositionZ_TextBox.Name = "TurretPositionZ_TextBox";
            this.TurretPositionZ_TextBox.Size = new System.Drawing.Size(42, 20);
            this.TurretPositionZ_TextBox.TabIndex = 16;
            this.TurretPositionZ_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Turret Position:";
            // 
            // TurretPositionY_TextBox
            // 
            this.TurretPositionY_TextBox.Location = new System.Drawing.Point(68, 68);
            this.TurretPositionY_TextBox.Name = "TurretPositionY_TextBox";
            this.TurretPositionY_TextBox.Size = new System.Drawing.Size(42, 20);
            this.TurretPositionY_TextBox.TabIndex = 15;
            this.TurretPositionY_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // weaponMessageLabel
            // 
            this.weaponMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.weaponMessageLabel.ForeColor = System.Drawing.Color.Maroon;
            this.weaponMessageLabel.Location = new System.Drawing.Point(13, 183);
            this.weaponMessageLabel.Name = "weaponMessageLabel";
            this.weaponMessageLabel.Size = new System.Drawing.Size(215, 20);
            this.weaponMessageLabel.TabIndex = 14;
            this.weaponMessageLabel.Text = "[ Message ]";
            this.weaponMessageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.weaponMessageLabel.Visible = false;
            // 
            // TurretPositionX_TextBox
            // 
            this.TurretPositionX_TextBox.Location = new System.Drawing.Point(20, 68);
            this.TurretPositionX_TextBox.Name = "TurretPositionX_TextBox";
            this.TurretPositionX_TextBox.Size = new System.Drawing.Size(42, 20);
            this.TurretPositionX_TextBox.TabIndex = 14;
            this.TurretPositionX_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // WeaponTypesComboBox
            // 
            this.WeaponTypesComboBox.FormattingEnabled = true;
            this.WeaponTypesComboBox.Location = new System.Drawing.Point(90, 24);
            this.WeaponTypesComboBox.Name = "WeaponTypesComboBox";
            this.WeaponTypesComboBox.Size = new System.Drawing.Size(138, 21);
            this.WeaponTypesComboBox.TabIndex = 1;
            this.WeaponTypesComboBox.SelectedIndexChanged += new System.EventHandler(this.WeaponTypesComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Weapon Type:";
            // 
            // DrawTurretTimer
            // 
            this.DrawTurretTimer.Enabled = true;
            this.DrawTurretTimer.Interval = 10;
            this.DrawTurretTimer.Tick += new System.EventHandler(this.DrawTurretTimer_Tick);
            // 
            // wireframeRadioButton
            // 
            this.wireframeRadioButton.AutoSize = true;
            this.wireframeRadioButton.Location = new System.Drawing.Point(9, 131);
            this.wireframeRadioButton.Name = "wireframeRadioButton";
            this.wireframeRadioButton.Size = new System.Drawing.Size(73, 17);
            this.wireframeRadioButton.TabIndex = 14;
            this.wireframeRadioButton.Text = "Wireframe";
            this.wireframeRadioButton.UseVisualStyleBackColor = true;
            this.wireframeRadioButton.CheckedChanged += new System.EventHandler(this.wireframeRadioButton_CheckedChanged);
            // 
            // texturedRadioButton
            // 
            this.texturedRadioButton.AutoSize = true;
            this.texturedRadioButton.Checked = true;
            this.texturedRadioButton.Location = new System.Drawing.Point(9, 154);
            this.texturedRadioButton.Name = "texturedRadioButton";
            this.texturedRadioButton.Size = new System.Drawing.Size(67, 17);
            this.texturedRadioButton.TabIndex = 15;
            this.texturedRadioButton.TabStop = true;
            this.texturedRadioButton.Text = "Textured";
            this.texturedRadioButton.UseVisualStyleBackColor = true;
            this.texturedRadioButton.CheckedChanged += new System.EventHandler(this.texturedRadioButton_CheckedChanged);
            // 
            // WinFormContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 647);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CameraGroupBox);
            this.Controls.Add(this.pctSurface);
            this.Name = "WinFormContainer";
            this.Text = "WinFormContainer";
            this.Load += new System.EventHandler(this.WinFormContainer_Load);
            this.Shown += new System.EventHandler(this.WinFormContainer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
            this.CameraGroupBox.ResumeLayout(false);
            this.CameraGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pctSurface;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.GroupBox CameraGroupBox;
        private System.Windows.Forms.TextBox CameraPositionX_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CameraPositionZ_TextBox;
        private System.Windows.Forms.TextBox CameraPositionY_TextBox;
        private System.Windows.Forms.TextBox CameraViewDirZ_TextBox;
        private System.Windows.Forms.TextBox CameraViewDirY_TextBox;
        private System.Windows.Forms.TextBox CameraViewDirX_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CameraTargetZ_TextBox;
        private System.Windows.Forms.TextBox CameraTargetY_TextBox;
        private System.Windows.Forms.TextBox CameraTargetX_TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SetTargetButton;
        private System.Windows.Forms.Label CameraMessageLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox WeaponTypesComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label weaponMessageLabel;
        private System.Windows.Forms.TextBox TurretPositionZ_TextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TurretPositionY_TextBox;
        private System.Windows.Forms.TextBox TurretPositionX_TextBox;
        private System.Windows.Forms.Button SetTurretPosition;
        private System.Windows.Forms.Timer DrawTurretTimer;
        private System.Windows.Forms.Button SetTurretAim;
        private System.Windows.Forms.TextBox TurretAimZ_TextBox;
        private System.Windows.Forms.TextBox TurretAimY_TextBox;
        private System.Windows.Forms.TextBox TurretAimX_TextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox WeaponPowerTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button FireButton;
        private System.Windows.Forms.LinkLabel LocateTurretPositionLink;
        private System.Windows.Forms.RadioButton texturedRadioButton;
        private System.Windows.Forms.RadioButton wireframeRadioButton;
    }
}