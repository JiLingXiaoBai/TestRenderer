using System.Windows.Forms;

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
            this.RotateX = new System.Windows.Forms.TrackBar();
            this.RotateY = new System.Windows.Forms.TrackBar();
            this.RotateZ = new System.Windows.Forms.TrackBar();
            this.ScaleValue = new System.Windows.Forms.TrackBar();
            this.lightingSelect = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleValue)).BeginInit();
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
            // lightingSelect
            // 
            this.lightingSelect.FormattingEnabled = true;
            this.lightingSelect.Items.AddRange(new object[] {
            "逐平面光照",
            "逐顶点光照",
            "逐片元光照"});
            this.lightingSelect.Location = new System.Drawing.Point(584, 40);
            this.lightingSelect.Name = "lightingSelect";
            this.lightingSelect.Size = new System.Drawing.Size(120, 76);
            this.lightingSelect.TabIndex = 5;
            this.lightingSelect.SetItemChecked(0, true);
            this.lightingSelect.SelectionMode = SelectionMode.One;
            this.lightingSelect.SelectedIndexChanged += new System.EventHandler(this.ChangeLightingType);
            // 
            // RotateX
            // 
            this.RotateX.Location = new System.Drawing.Point(470, 150);
            this.RotateX.Maximum = 360;
            this.RotateX.Name = "RotateX";
            this.RotateX.Size = new System.Drawing.Size(250, 45);
            this.RotateX.TabIndex = 6;
            this.RotateX.Scroll += new System.EventHandler(this.RotateX_Scroll);
            // 
            // RotateY
            // 
            this.RotateY.Location = new System.Drawing.Point(470, 200);
            this.RotateY.Maximum = 360;
            this.RotateY.Name = "RotateY";
            this.RotateY.Size = new System.Drawing.Size(250, 45);
            this.RotateY.TabIndex = 7;
            this.RotateY.Scroll += new System.EventHandler(this.RotateY_Scroll);
            // 
            // RotateZ
            // 
            this.RotateZ.Location = new System.Drawing.Point(470, 250);
            this.RotateZ.Maximum = 360;
            this.RotateZ.Name = "RotateZ";
            this.RotateZ.Size = new System.Drawing.Size(250, 45);
            this.RotateZ.TabIndex = 8;
            this.RotateZ.Scroll += new System.EventHandler(this.RotateZ_Scroll);
            // 
            // ScaleValue
            // 
            this.ScaleValue.Location = new System.Drawing.Point(470, 300);
            this.ScaleValue.Maximum = 250;
            this.ScaleValue.Minimum = 0;
            this.ScaleValue.Name = "ScaleValue";
            this.ScaleValue.Size = new System.Drawing.Size(250, 45);
            this.ScaleValue.TabIndex = 9;
            this.ScaleValue.Value = 100;
            this.ScaleValue.Scroll += new System.EventHandler(this.ScaleValue_Scroll);
            
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.lightingSelect);
            this.Controls.Add(this.IsLine);
            this.Controls.Add(this.IsOrtho);
            this.Controls.Add(this.IsZBuffer);
            this.Controls.Add(this.IsDiffuseTex);
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
        private CheckBox IsOrtho;
        private CheckBox IsZBuffer;
        private CheckBox IsDiffuseTex;
        private TrackBar RotateX;
        private TrackBar RotateY;
        private TrackBar RotateZ;
        private TrackBar ScaleValue;
        private CheckedListBox lightingSelect;
    }
}