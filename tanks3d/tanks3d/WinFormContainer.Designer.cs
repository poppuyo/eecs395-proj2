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
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
            this.CameraGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pctSurface
            // 
            this.pctSurface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pctSurface.Location = new System.Drawing.Point(12, 12);
            this.pctSurface.Name = "pctSurface";
            this.pctSurface.Size = new System.Drawing.Size(760, 412);
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
            this.CameraGroupBox.Location = new System.Drawing.Point(12, 430);
            this.CameraGroupBox.Name = "CameraGroupBox";
            this.CameraGroupBox.Size = new System.Drawing.Size(231, 170);
            this.CameraGroupBox.TabIndex = 1;
            this.CameraGroupBox.TabStop = false;
            this.CameraGroupBox.Text = "Camera";
            // 
            // CameraMessageLabel
            // 
            this.CameraMessageLabel.ForeColor = System.Drawing.Color.Maroon;
            this.CameraMessageLabel.Location = new System.Drawing.Point(6, 147);
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
            // WinFormContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 612);
            this.Controls.Add(this.CameraGroupBox);
            this.Controls.Add(this.pctSurface);
            this.Name = "WinFormContainer";
            this.Text = "WinFormContainer";
            this.Shown += new System.EventHandler(this.WinFormContainer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
            this.CameraGroupBox.ResumeLayout(false);
            this.CameraGroupBox.PerformLayout();
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
    }
}