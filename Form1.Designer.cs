namespace TestRenderer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.RotateX = new System.Windows.Forms.TrackBar();
            this.RotateY = new System.Windows.Forms.TrackBar();
            this.RotateZ = new System.Windows.Forms.TrackBar();
            this.ScaleValue = new System.Windows.Forms.TrackBar();
            this.IsLine = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValue)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(40, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // IsLine
            // 
            this.IsLine.AutoSize = true;
            this.IsLine.Checked = true;
            this.IsLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsLine.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.IsLine.Location = new System.Drawing.Point(480, 70);
            this.IsLine.Name = "IsLine";
            this.IsLine.Size = new System.Drawing.Size(75, 21);
            this.IsLine.TabIndex = 1;
            this.IsLine.Text = "线框模式";
            this.IsLine.UseVisualStyleBackColor = true;
            // 
            // RotateX
            // 
            this.RotateX.Location = new System.Drawing.Point(470, 150);
            this.RotateX.Maximum = 360;
            this.RotateX.Minimum = 0;
            this.RotateX.Name = "RotateX";
            this.RotateX.Size = new System.Drawing.Size(250, 45);
            this.RotateX.TabIndex = 2;
            this.RotateX.Value = 0;
            this.RotateX.Scroll += new System.EventHandler(this.RotateX_Scroll);
            // 
            // RotateY
            // 
            this.RotateY.Location = new System.Drawing.Point(470, 200);
            this.RotateY.Maximum = 360;
            this.RotateY.Minimum = 0;
            this.RotateY.Name = "RotateY";
            this.RotateY.Size = new System.Drawing.Size(250, 45);
            this.RotateY.TabIndex = 3;
            this.RotateY.Value = 0;
            this.RotateY.Scroll += new System.EventHandler(this.RotateY_Scroll);
            // 
            // RotateZ
            // 
            this.RotateZ.Location = new System.Drawing.Point(470, 250);
            this.RotateZ.Maximum = 360;
            this.RotateZ.Minimum = 0;
            this.RotateZ.Name = "RotateZ";
            this.RotateZ.Size = new System.Drawing.Size(250, 45);
            this.RotateZ.TabIndex = 4;
            this.RotateZ.Value = 0;
            this.RotateZ.Scroll += new System.EventHandler(this.RotateZ_Scroll);
            // 
            // ScaleValue
            // 
            this.ScaleValue.Location = new System.Drawing.Point(470, 300);
            this.ScaleValue.Maximum = 250;
            this.ScaleValue.Minimum = 100;
            this.ScaleValue.Name = "ScaleValue";
            this.ScaleValue.Size = new System.Drawing.Size(250, 45);
            this.ScaleValue.TabIndex = 5;
            this.ScaleValue.Value = 100;
            this.ScaleValue.Scroll += new System.EventHandler(this.ScaleValue_Scroll);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.IsLine);
            this.Controls.Add(this.ScaleValue);
            this.Controls.Add(this.RotateZ);
            this.Controls.Add(this.RotateY);
            this.Controls.Add(this.RotateX);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private CheckBox IsLine;
        private TrackBar RotateX;
        private TrackBar RotateY;
        private TrackBar RotateZ;
        private TrackBar ScaleValue;
        
    }
}