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
            this.CameraPositionZ_TextBox = new System.Windows.Forms.TextBox();
            this.CameraPositionY_TextBox = new System.Windows.Forms.TextBox();
            this.CameraPositionX_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CameraLookAtZ_TextBox = new System.Windows.Forms.TextBox();
            this.CameraLookAtY_TextBox = new System.Windows.Forms.TextBox();
            this.CameraLookAtX_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            this.pctSurface.Size = new System.Drawing.Size(760, 416);
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
            this.CameraGroupBox.Controls.Add(this.CameraLookAtZ_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraLookAtY_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraLookAtX_TextBox);
            this.CameraGroupBox.Controls.Add(this.label2);
            this.CameraGroupBox.Controls.Add(this.CameraPositionZ_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraPositionY_TextBox);
            this.CameraGroupBox.Controls.Add(this.CameraPositionX_TextBox);
            this.CameraGroupBox.Controls.Add(this.label1);
            this.CameraGroupBox.Location = new System.Drawing.Point(12, 434);
            this.CameraGroupBox.Name = "CameraGroupBox";
            this.CameraGroupBox.Size = new System.Drawing.Size(207, 116);
            this.CameraGroupBox.TabIndex = 1;
            this.CameraGroupBox.TabStop = false;
            this.CameraGroupBox.Text = "Camera";
            // 
            // CameraPositionZ_TextBox
            // 
            this.CameraPositionZ_TextBox.Location = new System.Drawing.Point(155, 24);
            this.CameraPositionZ_TextBox.Name = "CameraPositionZ_TextBox";
            this.CameraPositionZ_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraPositionZ_TextBox.TabIndex = 3;
            this.CameraPositionZ_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            this.CameraPositionZ_TextBox.Leave += new System.EventHandler(this.CameraPositionZ_TextBox_Leave);
            // 
            // CameraPositionY_TextBox
            // 
            this.CameraPositionY_TextBox.Location = new System.Drawing.Point(107, 24);
            this.CameraPositionY_TextBox.Name = "CameraPositionY_TextBox";
            this.CameraPositionY_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraPositionY_TextBox.TabIndex = 2;
            this.CameraPositionY_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            this.CameraPositionY_TextBox.Leave += new System.EventHandler(this.CameraPositionY_TextBox_Leave);
            // 
            // CameraPositionX_TextBox
            // 
            this.CameraPositionX_TextBox.Location = new System.Drawing.Point(59, 24);
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
            // CameraLookAtZ_TextBox
            // 
            this.CameraLookAtZ_TextBox.Location = new System.Drawing.Point(155, 49);
            this.CameraLookAtZ_TextBox.Name = "CameraLookAtZ_TextBox";
            this.CameraLookAtZ_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraLookAtZ_TextBox.TabIndex = 7;
            this.CameraLookAtZ_TextBox.TextChanged += new System.EventHandler(this.CameraLookAtZ_TextBox_TextChanged);
            this.CameraLookAtZ_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // CameraLookAtY_TextBox
            // 
            this.CameraLookAtY_TextBox.Location = new System.Drawing.Point(107, 49);
            this.CameraLookAtY_TextBox.Name = "CameraLookAtY_TextBox";
            this.CameraLookAtY_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraLookAtY_TextBox.TabIndex = 6;
            this.CameraLookAtY_TextBox.TextChanged += new System.EventHandler(this.CameraLookAtY_TextBox_TextChanged);
            this.CameraLookAtY_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            // 
            // CameraLookAtX_TextBox
            // 
            this.CameraLookAtX_TextBox.Location = new System.Drawing.Point(59, 49);
            this.CameraLookAtX_TextBox.Name = "CameraLookAtX_TextBox";
            this.CameraLookAtX_TextBox.Size = new System.Drawing.Size(42, 20);
            this.CameraLookAtX_TextBox.TabIndex = 5;
            this.CameraLookAtX_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleNumericTextBox);
            this.CameraLookAtX_TextBox.Leave += new System.EventHandler(this.CameraLookAtX_TextBox_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target:";
            // 
            // WinFormContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
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
        private System.Windows.Forms.TextBox CameraLookAtZ_TextBox;
        private System.Windows.Forms.TextBox CameraLookAtY_TextBox;
        private System.Windows.Forms.TextBox CameraLookAtX_TextBox;
        private System.Windows.Forms.Label label2;
    }
}