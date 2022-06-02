
namespace SML___Pokayoke_System
{
    partial class FormMainReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainReport));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxstock = new System.Windows.Forms.PictureBox();
            this.pictureBoxwithdraw = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxstock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxwithdraw)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBoxstock);
            this.panel1.Controls.Add(this.pictureBoxwithdraw);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1232, 79);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(115, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Report Stock";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Report Withdraw";
            // 
            // pictureBoxstock
            // 
            this.pictureBoxstock.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxstock.Image")));
            this.pictureBoxstock.Location = new System.Drawing.Point(125, 4);
            this.pictureBoxstock.Name = "pictureBoxstock";
            this.pictureBoxstock.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxstock.TabIndex = 0;
            this.pictureBoxstock.TabStop = false;
            this.pictureBoxstock.Click += new System.EventHandler(this.pictureBoxstock_Click);
            // 
            // pictureBoxwithdraw
            // 
            this.pictureBoxwithdraw.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxwithdraw.Image")));
            this.pictureBoxwithdraw.Location = new System.Drawing.Point(24, 4);
            this.pictureBoxwithdraw.Name = "pictureBoxwithdraw";
            this.pictureBoxwithdraw.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxwithdraw.TabIndex = 0;
            this.pictureBoxwithdraw.TabStop = false;
            this.pictureBoxwithdraw.Click += new System.EventHandler(this.pictureBoxwithdraw_Click);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 97);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1256, 611);
            this.panel2.TabIndex = 1;
            // 
            // FormMainReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1256, 709);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMainReport";
            this.Text = "FormMainReport";
            this.Load += new System.EventHandler(this.FormMainReport_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxstock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxwithdraw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxstock;
        private System.Windows.Forms.PictureBox pictureBoxwithdraw;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
    }
}