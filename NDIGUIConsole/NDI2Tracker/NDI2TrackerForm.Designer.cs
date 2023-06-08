using System.Resources;

namespace CGUINDIStreaming
{
    partial class NDI2TrackerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NDI2TrackerForm));
            btnTmpBrowse = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            txtTMPFilePath = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            TrackingGridView = new System.Windows.Forms.DataGridView();
            Handle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Tx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Ty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Tz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Q0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Q1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Q2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Q3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            BtnStartTracking = new System.Windows.Forms.Button();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            label1 = new System.Windows.Forms.Label();
            BtnStopTracking = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            CmbReferenceHandle = new System.Windows.Forms.ComboBox();
            CmbPortHandle = new System.Windows.Forms.ComboBox();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            TxtToolType = new System.Windows.Forms.TextBox();
            TxtManufactureID = new System.Windows.Forms.TextBox();
            TxtPartNumber = new System.Windows.Forms.TextBox();
            TxtSerialNumber = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            TxtToolRevision = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            txtCameraSerial = new System.Windows.Forms.TextBox();
            label12 = new System.Windows.Forms.Label();
            ChkHandleInitialized = new System.Windows.Forms.CheckBox();
            ChkHandleEnabled = new System.Windows.Forms.CheckBox();
            label11 = new System.Windows.Forms.Label();
            TxtRomPath = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            CmbMode = new System.Windows.Forms.ComboBox();
            BtnLoadRom = new System.Windows.Forms.Button();
            BtnUnloadRom = new System.Windows.Forms.Button();
            groupBox3 = new System.Windows.Forms.GroupBox();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrackingGridView).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnTmpBrowse
            // 
            btnTmpBrowse.Location = new System.Drawing.Point(570, 197);
            btnTmpBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnTmpBrowse.Name = "btnTmpBrowse";
            btnTmpBrowse.Size = new System.Drawing.Size(55, 24);
            btnTmpBrowse.TabIndex = 11;
            btnTmpBrowse.Text = "...";
            btnTmpBrowse.UseVisualStyleBackColor = true;
            btnTmpBrowse.Click += btnTmpBrowse_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(-205, 23);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(104, 15);
            label2.TabIndex = 10;
            label2.Text = "TMP File Location:";
            // 
            // txtTMPFilePath
            // 
            txtTMPFilePath.Location = new System.Drawing.Point(119, 197);
            txtTMPFilePath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtTMPFilePath.Name = "txtTMPFilePath";
            txtTMPFilePath.Size = new System.Drawing.Size(443, 23);
            txtTMPFilePath.TabIndex = 9;
            txtTMPFilePath.Text = "C:\\Tracker\\MevisNDI_State.tmp";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(TrackingGridView);
            groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            groupBox1.ForeColor = System.Drawing.Color.Blue;
            groupBox1.Location = new System.Drawing.Point(10, 315);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(951, 279);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Transformation";
            // 
            // TrackingGridView
            // 
            TrackingGridView.BackgroundColor = System.Drawing.Color.DarkGray;
            TrackingGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            TrackingGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            TrackingGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Handle, Tx, Ty, Tz, Q0, Q1, Q2, Q3, Error, Status });
            TrackingGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            TrackingGridView.Location = new System.Drawing.Point(4, 19);
            TrackingGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TrackingGridView.MultiSelect = false;
            TrackingGridView.Name = "TrackingGridView";
            TrackingGridView.ReadOnly = true;
            TrackingGridView.RowHeadersWidth = 30;
            TrackingGridView.RowTemplate.ReadOnly = true;
            TrackingGridView.Size = new System.Drawing.Size(943, 257);
            TrackingGridView.TabIndex = 0;
            TrackingGridView.SelectionChanged += TrackingGridView_SelectionChanged;
            // 
            // Handle
            // 
            Handle.HeaderText = "Handle";
            Handle.Name = "Handle";
            Handle.ReadOnly = true;
            Handle.Width = 90;
            // 
            // Tx
            // 
            Tx.HeaderText = "Tx";
            Tx.Name = "Tx";
            Tx.ReadOnly = true;
            Tx.Width = 90;
            // 
            // Ty
            // 
            Ty.HeaderText = "Ty";
            Ty.Name = "Ty";
            Ty.ReadOnly = true;
            Ty.Width = 90;
            // 
            // Tz
            // 
            Tz.HeaderText = "Tz";
            Tz.Name = "Tz";
            Tz.ReadOnly = true;
            Tz.Width = 90;
            // 
            // Q0
            // 
            Q0.HeaderText = "Q0";
            Q0.Name = "Q0";
            Q0.ReadOnly = true;
            Q0.Width = 90;
            // 
            // Q1
            // 
            Q1.HeaderText = "Q1";
            Q1.Name = "Q1";
            Q1.ReadOnly = true;
            Q1.Width = 90;
            // 
            // Q2
            // 
            Q2.HeaderText = "Q2";
            Q2.Name = "Q2";
            Q2.ReadOnly = true;
            Q2.Width = 90;
            // 
            // Q3
            // 
            Q3.HeaderText = "Q3";
            Q3.Name = "Q3";
            Q3.ReadOnly = true;
            Q3.Width = 90;
            // 
            // Error
            // 
            Error.HeaderText = "Error";
            Error.Name = "Error";
            Error.ReadOnly = true;
            Error.Width = 90;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Width = 90;
            // 
            // BtnStartTracking
            // 
            BtnStartTracking.Location = new System.Drawing.Point(19, 115);
            BtnStartTracking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnStartTracking.Name = "BtnStartTracking";
            BtnStartTracking.Size = new System.Drawing.Size(195, 26);
            BtnStartTracking.TabIndex = 7;
            BtnStartTracking.Text = "Start Tracking";
            BtnStartTracking.UseVisualStyleBackColor = true;
            BtnStartTracking.Click += BtnStartTracking_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 200);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(75, 15);
            label1.TabIndex = 13;
            label1.Text = "TmpFilePath:";
            // 
            // BtnStopTracking
            // 
            BtnStopTracking.Location = new System.Drawing.Point(19, 161);
            BtnStopTracking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnStopTracking.Name = "BtnStopTracking";
            BtnStopTracking.Size = new System.Drawing.Size(195, 26);
            BtnStopTracking.TabIndex = 14;
            BtnStopTracking.Text = "Stop Tracking";
            BtnStopTracking.UseVisualStyleBackColor = true;
            BtnStopTracking.Click += BtnStopTracking_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(335, 126);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(103, 15);
            label3.TabIndex = 15;
            label3.Text = "Reference Handle:";
            // 
            // CmbReferenceHandle
            // 
            CmbReferenceHandle.FormattingEnabled = true;
            CmbReferenceHandle.Location = new System.Drawing.Point(444, 123);
            CmbReferenceHandle.Name = "CmbReferenceHandle";
            CmbReferenceHandle.Size = new System.Drawing.Size(121, 23);
            CmbReferenceHandle.TabIndex = 16;
            CmbReferenceHandle.SelectedIndexChanged += CmbReferenceHandle_SelectedIndexChanged;
            // 
            // CmbPortHandle
            // 
            CmbPortHandle.FormattingEnabled = true;
            CmbPortHandle.Location = new System.Drawing.Point(119, 26);
            CmbPortHandle.Name = "CmbPortHandle";
            CmbPortHandle.Size = new System.Drawing.Size(121, 23);
            CmbPortHandle.TabIndex = 18;
            CmbPortHandle.SelectedIndexChanged += CmbPortHandle_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(13, 29);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(73, 15);
            label4.TabIndex = 17;
            label4.Text = "Port Handle:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(13, 63);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(59, 15);
            label5.TabIndex = 19;
            label5.Text = "Tool Type:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(13, 94);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(100, 15);
            label6.TabIndex = 20;
            label6.Text = "Manufacture's ID:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(13, 126);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(78, 15);
            label7.TabIndex = 21;
            label7.Text = "Part Number:";
            // 
            // TxtToolType
            // 
            TxtToolType.Enabled = false;
            TxtToolType.Location = new System.Drawing.Point(119, 60);
            TxtToolType.Name = "TxtToolType";
            TxtToolType.Size = new System.Drawing.Size(181, 23);
            TxtToolType.TabIndex = 22;
            // 
            // TxtManufactureID
            // 
            TxtManufactureID.Enabled = false;
            TxtManufactureID.Location = new System.Drawing.Point(119, 91);
            TxtManufactureID.Name = "TxtManufactureID";
            TxtManufactureID.Size = new System.Drawing.Size(181, 23);
            TxtManufactureID.TabIndex = 23;
            // 
            // TxtPartNumber
            // 
            TxtPartNumber.Enabled = false;
            TxtPartNumber.Location = new System.Drawing.Point(119, 123);
            TxtPartNumber.Name = "TxtPartNumber";
            TxtPartNumber.Size = new System.Drawing.Size(181, 23);
            TxtPartNumber.TabIndex = 24;
            // 
            // TxtSerialNumber
            // 
            TxtSerialNumber.Enabled = false;
            TxtSerialNumber.Location = new System.Drawing.Point(444, 55);
            TxtSerialNumber.Name = "TxtSerialNumber";
            TxtSerialNumber.Size = new System.Drawing.Size(181, 23);
            TxtSerialNumber.TabIndex = 26;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(338, 58);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(85, 15);
            label8.TabIndex = 25;
            label8.Text = "Serial Number:";
            // 
            // TxtToolRevision
            // 
            TxtToolRevision.Enabled = false;
            TxtToolRevision.Location = new System.Drawing.Point(444, 89);
            TxtToolRevision.Name = "TxtToolRevision";
            TxtToolRevision.Size = new System.Drawing.Size(181, 23);
            TxtToolRevision.TabIndex = 28;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(338, 92);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(73, 15);
            label9.TabIndex = 27;
            label9.Text = "Tool Version:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtCameraSerial);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(ChkHandleInitialized);
            groupBox2.Controls.Add(ChkHandleEnabled);
            groupBox2.Controls.Add(TxtToolRevision);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(TxtSerialNumber);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(btnTmpBrowse);
            groupBox2.Controls.Add(TxtRomPath);
            groupBox2.Controls.Add(TxtPartNumber);
            groupBox2.Controls.Add(txtTMPFilePath);
            groupBox2.Controls.Add(TxtManufactureID);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(TxtToolType);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(CmbMode);
            groupBox2.Controls.Add(CmbPortHandle);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(CmbReferenceHandle);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new System.Drawing.Point(17, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(643, 270);
            groupBox2.TabIndex = 29;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tool Properties";
            // 
            // txtCameraSerial
            // 
            txtCameraSerial.Location = new System.Drawing.Point(119, 234);
            txtCameraSerial.Name = "txtCameraSerial";
            txtCameraSerial.Size = new System.Drawing.Size(181, 23);
            txtCameraSerial.TabIndex = 32;
            txtCameraSerial.Text = "P9-01009";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(13, 237);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(63, 15);
            label12.TabIndex = 31;
            label12.Text = "CamSerial:";
            // 
            // ChkHandleInitialized
            // 
            ChkHandleInitialized.AutoSize = true;
            ChkHandleInitialized.Enabled = false;
            ChkHandleInitialized.Location = new System.Drawing.Point(335, 22);
            ChkHandleInitialized.Name = "ChkHandleInitialized";
            ChkHandleInitialized.Size = new System.Drawing.Size(117, 19);
            ChkHandleInitialized.TabIndex = 29;
            ChkHandleInitialized.Text = "Handle Initialized";
            ChkHandleInitialized.UseVisualStyleBackColor = true;
            // 
            // ChkHandleEnabled
            // 
            ChkHandleEnabled.AutoSize = true;
            ChkHandleEnabled.Enabled = false;
            ChkHandleEnabled.Location = new System.Drawing.Point(485, 22);
            ChkHandleEnabled.Name = "ChkHandleEnabled";
            ChkHandleEnabled.Size = new System.Drawing.Size(109, 19);
            ChkHandleEnabled.TabIndex = 30;
            ChkHandleEnabled.Text = "Handle Enabled";
            ChkHandleEnabled.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(335, 237);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(88, 15);
            label11.TabIndex = 13;
            label11.Text = "Tmp File Mode:";
            // 
            // TxtRomPath
            // 
            TxtRomPath.Enabled = false;
            TxtRomPath.Location = new System.Drawing.Point(119, 161);
            TxtRomPath.Name = "TxtRomPath";
            TxtRomPath.Size = new System.Drawing.Size(506, 23);
            TxtRomPath.TabIndex = 24;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(13, 164);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(62, 15);
            label10.TabIndex = 21;
            label10.Text = "Rom Path:";
            // 
            // CmbMode
            // 
            CmbMode.FormattingEnabled = true;
            CmbMode.Items.AddRange(new object[] { "Using Tmp File", "Using Memory Map File" });
            CmbMode.Location = new System.Drawing.Point(443, 234);
            CmbMode.Name = "CmbMode";
            CmbMode.Size = new System.Drawing.Size(121, 23);
            CmbMode.TabIndex = 18;
            CmbMode.Text = "Using Tmp File";
            CmbMode.SelectedIndexChanged += CmbMode_SelectedIndexChanged;
            // 
            // BtnLoadRom
            // 
            BtnLoadRom.Location = new System.Drawing.Point(19, 29);
            BtnLoadRom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnLoadRom.Name = "BtnLoadRom";
            BtnLoadRom.Size = new System.Drawing.Size(195, 26);
            BtnLoadRom.TabIndex = 30;
            BtnLoadRom.Text = "Load Rom";
            BtnLoadRom.UseVisualStyleBackColor = true;
            BtnLoadRom.Click += BtnLoadRom_Click;
            // 
            // BtnUnloadRom
            // 
            BtnUnloadRom.Location = new System.Drawing.Point(19, 72);
            BtnUnloadRom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnUnloadRom.Name = "BtnUnloadRom";
            BtnUnloadRom.Size = new System.Drawing.Size(195, 26);
            BtnUnloadRom.TabIndex = 30;
            BtnUnloadRom.Text = "Unload Rom";
            BtnUnloadRom.UseVisualStyleBackColor = true;
            BtnUnloadRom.Click += BtnUnloadRom_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(BtnUnloadRom);
            groupBox3.Controls.Add(BtnLoadRom);
            groupBox3.Controls.Add(BtnStopTracking);
            groupBox3.Controls.Add(BtnStartTracking);
            groupBox3.Location = new System.Drawing.Point(689, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(236, 236);
            groupBox3.TabIndex = 31;
            groupBox3.TabStop = false;
            groupBox3.Text = "System";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            toolStripStatusLabel1.Text = "Status:";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(0, 597);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(974, 22);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // NDI2TrackerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(974, 619);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(statusStrip1);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "NDI2TrackerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "NDI2Tracker";
            FormClosing += NDI2TrackerForm_FormClosing;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)TrackingGridView).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnTmpBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTMPFilePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnStartTracking;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Handle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tx;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tz;
        private System.Windows.Forms.DataGridViewTextBoxColumn Q0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Q1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Q2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Q3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnStopTracking;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BtnLoadRom;
        private System.Windows.Forms.Button BtnUnloadRom;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.TextBox TxtRomPath;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox CmbReferenceHandle;
        private System.Windows.Forms.TextBox TxtToolType;
        private System.Windows.Forms.TextBox TxtManufactureID;
        private System.Windows.Forms.TextBox TxtPartNumber;
        private System.Windows.Forms.TextBox TxtSerialNumber;
        private System.Windows.Forms.TextBox TxtToolRevision;
        private System.Windows.Forms.CheckBox ChkHandleEnabled;
        private System.Windows.Forms.CheckBox ChkHandleInitialized;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox CmbMode;
        private System.Windows.Forms.ComboBox CmbPortHandle;
        private System.Windows.Forms.TextBox txtCameraSerial;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private static System.Windows.Forms.DataGridView TrackingGridView;
    }
}