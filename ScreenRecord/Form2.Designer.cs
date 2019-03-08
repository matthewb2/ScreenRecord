namespace ScreenRecord
{
    partial class Form2
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
            this.resizeNode2 = new ScreenRecord.ResizeNode();
            this.resizeNode1 = new ScreenRecord.ResizeNode();
            this.SuspendLayout();
            // 
            // resizeNode2
            // 
            this.resizeNode2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resizeNode2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.resizeNode2.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.resizeNode2.Location = new System.Drawing.Point(624, 363);
            this.resizeNode2.Name = "resizeNode2";
            this.resizeNode2.Size = new System.Drawing.Size(21, 23);
            this.resizeNode2.TabIndex = 1;
            this.resizeNode2.Text = "resizeNode2";
            // 
            // resizeNode1
            // 
            this.resizeNode1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resizeNode1.BackColor = System.Drawing.Color.SaddleBrown;
            this.resizeNode1.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.resizeNode1.Location = new System.Drawing.Point(652, 41);
            this.resizeNode1.Name = "resizeNode1";
            this.resizeNode1.Size = new System.Drawing.Size(15, 15);
            this.resizeNode1.TabIndex = 0;
            this.resizeNode1.Text = "resizeNode";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(641, 382);
            this.ControlBox = false;
            this.Controls.Add(this.resizeNode2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(100, 100);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "form2";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ResizeNode resizeNode1;
        private ResizeNode resizeNode2;
    }
}