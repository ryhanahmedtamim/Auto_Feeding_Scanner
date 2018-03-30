namespace Auto_Feeding_Scanner
{
    partial class Form1
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
            this.comPort = new System.Windows.Forms.ComboBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textPath = new System.Windows.Forms.TextBox();
            this.startScan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pathButton = new System.Windows.Forms.Button();
            this.cBoxResolution = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DebugRichTextBox = new System.Windows.Forms.RichTextBox();
            this.DispPictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DispPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comPort
            // 
            this.comPort.FormattingEnabled = true;
            this.comPort.Location = new System.Drawing.Point(93, 189);
            this.comPort.Name = "comPort";
            this.comPort.Size = new System.Drawing.Size(164, 21);
            this.comPort.TabIndex = 0;
            this.comPort.Click += new System.EventHandler(this.comPort_Click);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(277, 187);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Save To";
            // 
            // textPath
            // 
            this.textPath.Location = new System.Drawing.Point(93, 217);
            this.textPath.Name = "textPath";
            this.textPath.ReadOnly = true;
            this.textPath.Size = new System.Drawing.Size(164, 20);
            this.textPath.TabIndex = 5;
            // 
            // startScan
            // 
            this.startScan.Enabled = false;
            this.startScan.Location = new System.Drawing.Point(277, 245);
            this.startScan.Name = "startScan";
            this.startScan.Size = new System.Drawing.Size(75, 23);
            this.startScan.TabIndex = 6;
            this.startScan.Text = "Scan";
            this.startScan.UseVisualStyleBackColor = true;
            this.startScan.Click += new System.EventHandler(this.startScan_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Divice";
            // 
            // pathButton
            // 
            this.pathButton.Enabled = false;
            this.pathButton.Location = new System.Drawing.Point(277, 216);
            this.pathButton.Name = "pathButton";
            this.pathButton.Size = new System.Drawing.Size(75, 23);
            this.pathButton.TabIndex = 8;
            this.pathButton.Text = "Path";
            this.pathButton.UseVisualStyleBackColor = true;
            this.pathButton.Click += new System.EventHandler(this.pathButton_Click);
            // 
            // cBoxResolution
            // 
            this.cBoxResolution.FormattingEnabled = true;
            this.cBoxResolution.Location = new System.Drawing.Point(93, 247);
            this.cBoxResolution.Name = "cBoxResolution";
            this.cBoxResolution.Size = new System.Drawing.Size(164, 21);
            this.cBoxResolution.TabIndex = 27;
            this.cBoxResolution.Text = "320x240";
            this.cBoxResolution.SelectedIndexChanged += new System.EventHandler(this.cBoxResolution_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Resulation";
            // 
            // DebugRichTextBox
            // 
            this.DebugRichTextBox.Location = new System.Drawing.Point(12, 283);
            this.DebugRichTextBox.Name = "DebugRichTextBox";
            this.DebugRichTextBox.Size = new System.Drawing.Size(340, 81);
            this.DebugRichTextBox.TabIndex = 29;
            this.DebugRichTextBox.Text = "";
            // 
            // DispPictureBox1
            // 
            this.DispPictureBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.DispPictureBox1.Location = new System.Drawing.Point(15, 4);
            this.DispPictureBox1.Name = "DispPictureBox1";
            this.DispPictureBox1.Size = new System.Drawing.Size(337, 177);
            this.DispPictureBox1.TabIndex = 30;
            this.DispPictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 376);
            this.Controls.Add(this.DispPictureBox1);
            this.Controls.Add(this.DebugRichTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cBoxResolution);
            this.Controls.Add(this.pathButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startScan);
            this.Controls.Add(this.textPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.comPort);
            this.Name = "Form1";
            this.Text = "Auto Feeding Scanner";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DispPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comPort;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPath;
        private System.Windows.Forms.Button startScan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button pathButton;
        private System.Windows.Forms.ComboBox cBoxResolution;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox DebugRichTextBox;
        private System.Windows.Forms.PictureBox DispPictureBox1;
    }
}

