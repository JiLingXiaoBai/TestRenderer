using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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
            this.IsLine = new System.Windows.Forms.CheckBox();
            this.IsOrtho = new System.Windows.Forms.CheckBox();
            this.IsZBuffer = new System.Windows.Forms.CheckBox();
            this.IsDiffuseTex = new System.Windows.Forms.CheckBox();
            this.IsFlatLit = new System.Windows.Forms.RadioButton();
            this.IsVertexLit = new System.Windows.Forms.RadioButton();
            this.IsPixelLit = new System.Windows.Forms.RadioButton();
            this.ResetAxisScale = new System.Windows.Forms.Button();
            this.RotateX = new System.Windows.Forms.TrackBar();
            this.RotateY = new System.Windows.Forms.TrackBar();
            this.RotateZ = new System.Windows.Forms.TrackBar();
            this.ScaleValue = new System.Windows.Forms.TrackBar();
            this.ScaleValueX = new System.Windows.Forms.TrackBar();
            this.ScaleValueY = new System.Windows.Forms.TrackBar();
            this.ScaleValueZ = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValueX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValueY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValueZ)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(40, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // IsLine
            // 
            this.IsLine.AutoSize = true;
            this.IsLine.Checked = true;
            this.IsLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsLine.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.IsLine.Location = new System.Drawing.Point(480, 40);
            this.IsLine.Name = "IsLine";
            this.IsLine.Size = new System.Drawing.Size(75, 21);
            this.IsLine.TabIndex = 1;
            this.IsLine.Text = "线框模式";
            this.IsLine.UseVisualStyleBackColor = true;
            // 
            // IsOrtho
            // 
            this.IsOrtho.AutoSize = true;
            this.IsOrtho.Checked = true;
            this.IsOrtho.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsOrtho.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.IsOrtho.Location = new System.Drawing.Point(480, 70);
            this.IsOrtho.Name = "IsOrtho";
            this.IsOrtho.Size = new System.Drawing.Size(75, 21);
            this.IsOrtho.TabIndex = 2;
            this.IsOrtho.Text = "正交投影";
            this.IsOrtho.UseVisualStyleBackColor = true;
            // 
            // IsZBuffer
            // 
            this.IsZBuffer.AutoSize = true;
            this.IsZBuffer.Checked = true;
            this.IsZBuffer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsZBuffer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.IsZBuffer.Location = new System.Drawing.Point(480, 100);
            this.IsZBuffer.Name = "IsZBuffer";
            this.IsZBuffer.Size = new System.Drawing.Size(75, 21);
            this.IsZBuffer.TabIndex = 3;
            this.IsZBuffer.Text = "深度缓冲";
            this.IsZBuffer.UseVisualStyleBackColor = true;
            // 
            // IsDiffuseTex
            // 
            this.IsDiffuseTex.AutoSize = true;
            this.IsDiffuseTex.Checked = true;
            this.IsDiffuseTex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsDiffuseTex.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.IsDiffuseTex.Location = new System.Drawing.Point(480, 130);
            this.IsDiffuseTex.Name = "IsDiffuseTex";
            this.IsDiffuseTex.Size = new System.Drawing.Size(75, 21);
            this.IsDiffuseTex.TabIndex = 4;
            this.IsDiffuseTex.Text = "基础纹理";
            this.IsDiffuseTex.UseVisualStyleBackColor = true;
            // 
            // IsFlatLit
            // 
            this.IsFlatLit.AutoSize = true;
            this.IsFlatLit.Checked = true;
            this.IsFlatLit.Location = new System.Drawing.Point(580, 40);
            this.IsFlatLit.Name = "IsFlatLit";
            this.IsFlatLit.Size = new System.Drawing.Size(86, 21);
            this.IsFlatLit.TabIndex = 5;
            this.IsFlatLit.TabStop = true;
            this.IsFlatLit.Text = "逐平面光照";
            this.IsFlatLit.UseVisualStyleBackColor = true;
            this.IsFlatLit.Click += new System.EventHandler(this.ChangeLightingType);
            // 
            // IsVertexLit
            // 
            this.IsVertexLit.AutoSize = true;
            this.IsVertexLit.Location = new System.Drawing.Point(580, 70);
            this.IsVertexLit.Name = "IsVertexLit";
            this.IsVertexLit.Size = new System.Drawing.Size(86, 21);
            this.IsVertexLit.TabIndex = 6;
            this.IsVertexLit.Text = "逐顶点光照";
            this.IsVertexLit.UseVisualStyleBackColor = true;
            this.IsVertexLit.Click += new System.EventHandler(this.ChangeLightingType);
            // 
            // IsPixelLit
            // 
            this.IsPixelLit.AutoSize = true;
            this.IsPixelLit.Location = new System.Drawing.Point(580, 100);
            this.IsPixelLit.Name = "IsPixelLit";
            this.IsPixelLit.Size = new System.Drawing.Size(86, 21);
            this.IsPixelLit.TabIndex = 7;
            this.IsPixelLit.Text = "逐片元光照";
            this.IsPixelLit.UseVisualStyleBackColor = true;
            this.IsPixelLit.Click += new System.EventHandler(this.ChangeLightingType);
            // 
            // ResetAxisScale
            // 
            this.ResetAxisScale.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ResetAxisScale.Location = new System.Drawing.Point(576, 126);
            this.ResetAxisScale.Name = "ResetAxisScale";
            this.ResetAxisScale.Size = new System.Drawing.Size(90, 25);
            this.ResetAxisScale.TabIndex = 8;
            this.ResetAxisScale.Text = "重置轴向缩放";
            this.ResetAxisScale.UseVisualStyleBackColor = true;
            this.ResetAxisScale.Click += new System.EventHandler(this.ResetAxisScale_Click);
            // 
            // RotateX
            // 
            this.RotateX.Location = new System.Drawing.Point(470, 150);
            this.RotateX.Maximum = 360;
            this.RotateX.Name = "RotateX";
            this.RotateX.Size = new System.Drawing.Size(250, 45);
            this.RotateX.TabIndex = 9;
            this.RotateX.Scroll += new System.EventHandler(this.RotateX_Scroll);
            // 
            // RotateY
            // 
            this.RotateY.Location = new System.Drawing.Point(470, 200);
            this.RotateY.Maximum = 360;
            this.RotateY.Name = "RotateY";
            this.RotateY.Size = new System.Drawing.Size(250, 45);
            this.RotateY.TabIndex = 10;
            this.RotateY.Scroll += new System.EventHandler(this.RotateY_Scroll);
            // 
            // RotateZ
            // 
            this.RotateZ.Location = new System.Drawing.Point(470, 250);
            this.RotateZ.Maximum = 360;
            this.RotateZ.Name = "RotateZ";
            this.RotateZ.Size = new System.Drawing.Size(250, 45);
            this.RotateZ.TabIndex = 11;
            this.RotateZ.Scroll += new System.EventHandler(this.RotateZ_Scroll);
            // 
            // ScaleValue
            // 
            this.ScaleValue.Location = new System.Drawing.Point(470, 300);
            this.ScaleValue.Maximum = 250;
            this.ScaleValue.Minimum = 1;
            this.ScaleValue.Name = "ScaleValue";
            this.ScaleValue.Size = new System.Drawing.Size(250, 45);
            this.ScaleValue.TabIndex = 12;
            this.ScaleValue.Value = 125;
            this.ScaleValue.Scroll += new System.EventHandler(this.ScaleValue_Scroll);
            // 
            // ScaleValueX
            // 
            this.ScaleValueX.Location = new System.Drawing.Point(470, 350);
            this.ScaleValueX.Maximum = 200;
            this.ScaleValueX.Minimum = 1;
            this.ScaleValueX.Name = "ScaleValueX";
            this.ScaleValueX.Size = new System.Drawing.Size(250, 45);
            this.ScaleValueX.TabIndex = 13;
            this.ScaleValueX.Value = 100;
            this.ScaleValueX.Scroll += new System.EventHandler(this.ScaleValueX_Scroll);
            // 
            // ScaleValueY
            // 
            this.ScaleValueY.Location = new System.Drawing.Point(470, 390);
            this.ScaleValueY.Maximum = 200;
            this.ScaleValueY.Minimum = 1;
            this.ScaleValueY.Name = "ScaleValueY";
            this.ScaleValueY.Size = new System.Drawing.Size(250, 45);
            this.ScaleValueY.TabIndex = 14;
            this.ScaleValueY.Value = 100;
            this.ScaleValueY.Scroll += new System.EventHandler(this.ScaleValueY_Scroll);
            // 
            // ScaleValueZ
            // 
            this.ScaleValueZ.Location = new System.Drawing.Point(470, 430);
            this.ScaleValueZ.Maximum = 200;
            this.ScaleValueZ.Minimum = 1;
            this.ScaleValueZ.Name = "ScaleValueZ";
            this.ScaleValueZ.Size = new System.Drawing.Size(250, 45);
            this.ScaleValueZ.TabIndex = 15;
            this.ScaleValueZ.Value = 100;
            this.ScaleValueZ.Scroll += new System.EventHandler(this.ScaleValueZ_Scroll);
            
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.ResetAxisScale);
            this.Controls.Add(this.IsLine);
            this.Controls.Add(this.IsOrtho);
            this.Controls.Add(this.IsZBuffer);
            this.Controls.Add(this.IsDiffuseTex);
            this.Controls.Add(this.IsFlatLit);
            this.Controls.Add(this.IsVertexLit);
            this.Controls.Add(this.IsPixelLit);
            this.Controls.Add(this.ScaleValue);
            this.Controls.Add(this.ScaleValueX);
            this.Controls.Add(this.ScaleValueY);
            this.Controls.Add(this.ScaleValueZ);
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
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValueX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValueY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValueZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private CheckBox IsLine;
        private CheckBox IsOrtho;
        private CheckBox IsZBuffer;
        private CheckBox IsDiffuseTex;
        private RadioButton IsFlatLit;
        private RadioButton IsVertexLit;
        private RadioButton IsPixelLit;
        private TrackBar RotateX;
        private TrackBar RotateY;
        private TrackBar RotateZ;
        private TrackBar ScaleValue;
        private TrackBar ScaleValueX;
        private TrackBar ScaleValueY;
        private TrackBar ScaleValueZ;
        private Button ResetAxisScale;
    }
}