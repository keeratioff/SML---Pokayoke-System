
namespace SML___Pokayoke_System
{
    partial class FormReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReport));
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.metroDateTimestart = new MetroFramework.Controls.MetroDateTime();
            this.metroDateTimeend = new MetroFramework.Controls.MetroDateTime();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.metroGridReport = new MetroFramework.Controls.MetroGrid();
            this.Buttonsubmit = new SML___Pokayoke_System.RJ_Button.RJButton();
            this.Buttonclear = new SML___Pokayoke_System.RJ_Button.RJButton();
            this.Buttonexport = new SML___Pokayoke_System.RJ_Button.RJButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.metroGridReport)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(85, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "End Date :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(77, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Start Date :";
            // 
            // metroDateTimestart
            // 
            this.metroDateTimestart.Location = new System.Drawing.Point(186, 87);
            this.metroDateTimestart.MinimumSize = new System.Drawing.Size(0, 29);
            this.metroDateTimestart.Name = "metroDateTimestart";
            this.metroDateTimestart.Size = new System.Drawing.Size(288, 29);
            this.metroDateTimestart.TabIndex = 10;
            // 
            // metroDateTimeend
            // 
            this.metroDateTimeend.Location = new System.Drawing.Point(186, 135);
            this.metroDateTimeend.MinimumSize = new System.Drawing.Size(0, 29);
            this.metroDateTimeend.Name = "metroDateTimeend";
            this.metroDateTimeend.Size = new System.Drawing.Size(288, 29);
            this.metroDateTimeend.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(12, 262);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1232, 32);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "                                                                                 " +
    "                         Search ( Vin.No ) ";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // metroGridReport
            // 
            this.metroGridReport.AllowUserToResizeRows = false;
            this.metroGridReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.metroGridReport.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGridReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroGridReport.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.metroGridReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGridReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.metroGridReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metroGridReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.metroGridReport.EnableHeadersVisualStyles = false;
            this.metroGridReport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroGridReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGridReport.Location = new System.Drawing.Point(12, 298);
            this.metroGridReport.Name = "metroGridReport";
            this.metroGridReport.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGridReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.metroGridReport.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.metroGridReport.RowTemplate.Height = 30;
            this.metroGridReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metroGridReport.Size = new System.Drawing.Size(1232, 399);
            this.metroGridReport.TabIndex = 14;
            // 
            // Buttonsubmit
            // 
            this.Buttonsubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.Buttonsubmit.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.Buttonsubmit.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Buttonsubmit.BorderRadius = 40;
            this.Buttonsubmit.BorderSize = 0;
            this.Buttonsubmit.FlatAppearance.BorderSize = 0;
            this.Buttonsubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buttonsubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.Buttonsubmit.ForeColor = System.Drawing.Color.White;
            this.Buttonsubmit.Image = ((System.Drawing.Image)(resources.GetObject("Buttonsubmit.Image")));
            this.Buttonsubmit.Location = new System.Drawing.Point(629, 74);
            this.Buttonsubmit.Name = "Buttonsubmit";
            this.Buttonsubmit.Size = new System.Drawing.Size(170, 110);
            this.Buttonsubmit.TabIndex = 15;
            this.Buttonsubmit.Text = "Submit";
            this.Buttonsubmit.TextColor = System.Drawing.Color.White;
            this.Buttonsubmit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Buttonsubmit.UseVisualStyleBackColor = false;
            this.Buttonsubmit.Click += new System.EventHandler(this.Buttonsubmit_Click);
            // 
            // Buttonclear
            // 
            this.Buttonclear.BackColor = System.Drawing.Color.Firebrick;
            this.Buttonclear.BackgroundColor = System.Drawing.Color.Firebrick;
            this.Buttonclear.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Buttonclear.BorderRadius = 40;
            this.Buttonclear.BorderSize = 0;
            this.Buttonclear.FlatAppearance.BorderSize = 0;
            this.Buttonclear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buttonclear.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.Buttonclear.ForeColor = System.Drawing.Color.White;
            this.Buttonclear.Image = ((System.Drawing.Image)(resources.GetObject("Buttonclear.Image")));
            this.Buttonclear.Location = new System.Drawing.Point(815, 74);
            this.Buttonclear.Name = "Buttonclear";
            this.Buttonclear.Size = new System.Drawing.Size(170, 110);
            this.Buttonclear.TabIndex = 16;
            this.Buttonclear.Text = "Clear";
            this.Buttonclear.TextColor = System.Drawing.Color.White;
            this.Buttonclear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Buttonclear.UseVisualStyleBackColor = false;
            this.Buttonclear.Click += new System.EventHandler(this.Buttonclear_Click);
            // 
            // Buttonexport
            // 
            this.Buttonexport.BackColor = System.Drawing.Color.DarkGreen;
            this.Buttonexport.BackgroundColor = System.Drawing.Color.DarkGreen;
            this.Buttonexport.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Buttonexport.BorderRadius = 40;
            this.Buttonexport.BorderSize = 0;
            this.Buttonexport.FlatAppearance.BorderSize = 0;
            this.Buttonexport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buttonexport.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.Buttonexport.ForeColor = System.Drawing.Color.White;
            this.Buttonexport.Image = ((System.Drawing.Image)(resources.GetObject("Buttonexport.Image")));
            this.Buttonexport.Location = new System.Drawing.Point(1006, 79);
            this.Buttonexport.Name = "Buttonexport";
            this.Buttonexport.Size = new System.Drawing.Size(170, 110);
            this.Buttonexport.TabIndex = 17;
            this.Buttonexport.Text = "Export";
            this.Buttonexport.TextColor = System.Drawing.Color.White;
            this.Buttonexport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Buttonexport.UseVisualStyleBackColor = false;
            this.Buttonexport.Click += new System.EventHandler(this.Buttonexport_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.DarkBlue;
            this.label9.Location = new System.Drawing.Point(29, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(361, 13);
            this.label9.TabIndex = 59;
            this.label9.Text = "___________________________________________________________";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(28, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 24);
            this.label10.TabIndex = 58;
            this.label10.Text = "Description";
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1256, 709);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Buttonexport);
            this.Controls.Add(this.Buttonclear);
            this.Controls.Add(this.Buttonsubmit);
            this.Controls.Add(this.metroGridReport);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.metroDateTimeend);
            this.Controls.Add(this.metroDateTimestart);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormReport";
            this.Text = "FormReport";
            this.Load += new System.EventHandler(this.FormReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroGridReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private MetroFramework.Controls.MetroDateTime metroDateTimestart;
        private MetroFramework.Controls.MetroDateTime metroDateTimeend;
        private System.Windows.Forms.TextBox textBox1;
        private MetroFramework.Controls.MetroGrid metroGridReport;
        private RJ_Button.RJButton Buttonsubmit;
        private RJ_Button.RJButton Buttonclear;
        private RJ_Button.RJButton Buttonexport;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}