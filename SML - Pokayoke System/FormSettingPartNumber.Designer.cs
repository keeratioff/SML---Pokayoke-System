
namespace SML___Pokayoke_System
{
    partial class FormSettingPartNumber
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
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxfilename = new System.Windows.Forms.TextBox();
            this.comboBoxsheet = new System.Windows.Forms.ComboBox();
            this.buttonbrowse1 = new System.Windows.Forms.Button();
            this.metroGrid1 = new MetroFramework.Controls.MetroGrid();
            this.Buttonsave = new SML___Pokayoke_System.RJ_Button.RJButton();
            this.Buttonclear = new SML___Pokayoke_System.RJ_Button.RJButton();
            this.label3 = new System.Windows.Forms.Label();
            this.buttoncleardatabase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 13);
            this.label2.TabIndex = 65;
            this.label2.Text = "Add vin number for import excel file.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.DarkBlue;
            this.label9.Location = new System.Drawing.Point(29, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(361, 13);
            this.label9.TabIndex = 64;
            this.label9.Text = "___________________________________________________________";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(28, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 24);
            this.label10.TabIndex = 63;
            this.label10.Text = "Setting Partlist";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(56, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 24);
            this.label8.TabIndex = 66;
            this.label8.Text = "File Name : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(98, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 67;
            this.label1.Text = "Sheet : ";
            // 
            // textBoxfilename
            // 
            this.textBoxfilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.textBoxfilename.Location = new System.Drawing.Point(186, 67);
            this.textBoxfilename.Multiline = true;
            this.textBoxfilename.Name = "textBoxfilename";
            this.textBoxfilename.Size = new System.Drawing.Size(487, 30);
            this.textBoxfilename.TabIndex = 68;
            // 
            // comboBoxsheet
            // 
            this.comboBoxsheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.comboBoxsheet.FormattingEnabled = true;
            this.comboBoxsheet.Location = new System.Drawing.Point(186, 103);
            this.comboBoxsheet.Name = "comboBoxsheet";
            this.comboBoxsheet.Size = new System.Drawing.Size(207, 32);
            this.comboBoxsheet.TabIndex = 69;
            this.comboBoxsheet.SelectedIndexChanged += new System.EventHandler(this.comboBoxsheet_SelectedIndexChanged);
            // 
            // buttonbrowse1
            // 
            this.buttonbrowse1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonbrowse1.Location = new System.Drawing.Point(679, 65);
            this.buttonbrowse1.Name = "buttonbrowse1";
            this.buttonbrowse1.Size = new System.Drawing.Size(109, 32);
            this.buttonbrowse1.TabIndex = 70;
            this.buttonbrowse1.Text = "Browse a file";
            this.buttonbrowse1.UseVisualStyleBackColor = true;
            this.buttonbrowse1.Click += new System.EventHandler(this.buttonbrowse1_Click);
            // 
            // metroGrid1
            // 
            this.metroGrid1.AllowUserToResizeRows = false;
            this.metroGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.metroGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroGrid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.metroGrid1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.metroGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metroGrid1.DefaultCellStyle = dataGridViewCellStyle2;
            this.metroGrid1.EnableHeadersVisualStyles = false;
            this.metroGrid1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid1.Location = new System.Drawing.Point(12, 198);
            this.metroGrid1.Name = "metroGrid1";
            this.metroGrid1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.metroGrid1.RowHeadersVisible = false;
            this.metroGrid1.RowHeadersWidth = 10;
            this.metroGrid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.metroGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metroGrid1.Size = new System.Drawing.Size(1232, 474);
            this.metroGrid1.TabIndex = 71;
            // 
            // Buttonsave
            // 
            this.Buttonsave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.Buttonsave.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.Buttonsave.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Buttonsave.BorderRadius = 40;
            this.Buttonsave.BorderSize = 0;
            this.Buttonsave.FlatAppearance.BorderSize = 0;
            this.Buttonsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buttonsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.Buttonsave.ForeColor = System.Drawing.Color.White;
            this.Buttonsave.Location = new System.Drawing.Point(908, 49);
            this.Buttonsave.Name = "Buttonsave";
            this.Buttonsave.Size = new System.Drawing.Size(150, 100);
            this.Buttonsave.TabIndex = 72;
            this.Buttonsave.Text = "Save";
            this.Buttonsave.TextColor = System.Drawing.Color.White;
            this.Buttonsave.UseVisualStyleBackColor = false;
            this.Buttonsave.Click += new System.EventHandler(this.Buttonsave_Click);
            // 
            // Buttonclear
            // 
            this.Buttonclear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.Buttonclear.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(48)))), ((int)(((byte)(71)))));
            this.Buttonclear.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Buttonclear.BorderRadius = 40;
            this.Buttonclear.BorderSize = 0;
            this.Buttonclear.FlatAppearance.BorderSize = 0;
            this.Buttonclear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buttonclear.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.Buttonclear.ForeColor = System.Drawing.Color.White;
            this.Buttonclear.Location = new System.Drawing.Point(1064, 49);
            this.Buttonclear.Name = "Buttonclear";
            this.Buttonclear.Size = new System.Drawing.Size(150, 100);
            this.Buttonclear.TabIndex = 72;
            this.Buttonclear.Text = "Clear";
            this.Buttonclear.TextColor = System.Drawing.Color.White;
            this.Buttonclear.UseVisualStyleBackColor = false;
            this.Buttonclear.Click += new System.EventHandler(this.Buttonclear_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(755, 692);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(363, 13);
            this.label3.TabIndex = 74;
            this.label3.Text = "หมายเหตุ : ลบข้อมูลส่วนของ Part list ทั้งหมด กรณีมีการแก้ไขข้อมูลจำนวนมาก";
            // 
            // buttoncleardatabase
            // 
            this.buttoncleardatabase.BackColor = System.Drawing.Color.Red;
            this.buttoncleardatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttoncleardatabase.ForeColor = System.Drawing.Color.White;
            this.buttoncleardatabase.Location = new System.Drawing.Point(1124, 682);
            this.buttoncleardatabase.Name = "buttoncleardatabase";
            this.buttoncleardatabase.Size = new System.Drawing.Size(120, 23);
            this.buttoncleardatabase.TabIndex = 75;
            this.buttoncleardatabase.Text = "Clear Database is all";
            this.buttoncleardatabase.UseVisualStyleBackColor = false;
            this.buttoncleardatabase.Click += new System.EventHandler(this.buttoncleardatabase_Click);
            // 
            // FormSettingPartNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1256, 709);
            this.Controls.Add(this.buttoncleardatabase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Buttonclear);
            this.Controls.Add(this.Buttonsave);
            this.Controls.Add(this.metroGrid1);
            this.Controls.Add(this.buttonbrowse1);
            this.Controls.Add(this.comboBoxsheet);
            this.Controls.Add(this.textBoxfilename);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSettingPartNumber";
            this.Text = "FormSettingPartNumber";
            this.Load += new System.EventHandler(this.FormSettingPartNumber_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxfilename;
        private System.Windows.Forms.ComboBox comboBoxsheet;
        private System.Windows.Forms.Button buttonbrowse1;
        private MetroFramework.Controls.MetroGrid metroGrid1;
        private RJ_Button.RJButton Buttonsave;
        private RJ_Button.RJButton Buttonclear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttoncleardatabase;
    }
}