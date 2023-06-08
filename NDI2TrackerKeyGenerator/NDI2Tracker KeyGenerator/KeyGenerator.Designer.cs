namespace NDI2Tracker
{
    partial class KeyGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyGenerator));
            label1 = new System.Windows.Forms.Label();
            TxtMachineId = new System.Windows.Forms.TextBox();
            TxtLicense = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            BtnGenerate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(121, 30);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(67, 15);
            label1.TabIndex = 0;
            label1.Text = "MachineID:";
            // 
            // TxtMachineId
            // 
            TxtMachineId.Location = new System.Drawing.Point(208, 27);
            TxtMachineId.Name = "TxtMachineId";
            TxtMachineId.Size = new System.Drawing.Size(235, 23);
            TxtMachineId.TabIndex = 0;
            // 
            // TxtLicense
            // 
            TxtLicense.Location = new System.Drawing.Point(208, 72);
            TxtLicense.Name = "TxtLicense";
            TxtLicense.Size = new System.Drawing.Size(235, 23);
            TxtLicense.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(121, 75);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(71, 15);
            label2.TabIndex = 2;
            label2.Text = "License Key:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(103, 99);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // BtnGenerate
            // 
            BtnGenerate.Location = new System.Drawing.Point(251, 113);
            BtnGenerate.Name = "BtnGenerate";
            BtnGenerate.Size = new System.Drawing.Size(123, 30);
            BtnGenerate.TabIndex = 2;
            BtnGenerate.Text = "&Generate";
            BtnGenerate.UseVisualStyleBackColor = true;
            BtnGenerate.Click += BtnRegister_Click;
            // 
            // KeyGenerator
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(454, 172);
            Controls.Add(BtnGenerate);
            Controls.Add(pictureBox1);
            Controls.Add(TxtLicense);
            Controls.Add(label2);
            Controls.Add(TxtMachineId);
            Controls.Add(label1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "KeyGenerator";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "NDI2Tracker KeyGenerator";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtMachineId;
        private System.Windows.Forms.TextBox TxtLicense;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BtnGenerate;
    }
}