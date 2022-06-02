
namespace SML___Pokayoke_System
{
    partial class FormMainSettingAddStock
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Buttonstockon = new SML___Pokayoke_System.RJ_Button.RJButton();
            this.Buttoneditdatabaseon = new SML___Pokayoke_System.RJ_Button.RJButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Buttoneditdatabaseon);
            this.panel1.Controls.Add(this.Buttonstockon);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1256, 50);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1256, 660);
            this.panel2.TabIndex = 1;
            // 
            // Buttonstockon
            // 
            this.Buttonstockon.BackColor = System.Drawing.Color.ForestGreen;
            this.Buttonstockon.BackgroundColor = System.Drawing.Color.ForestGreen;
            this.Buttonstockon.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Buttonstockon.BorderRadius = 40;
            this.Buttonstockon.BorderSize = 0;
            this.Buttonstockon.FlatAppearance.BorderSize = 0;
            this.Buttonstockon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buttonstockon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Buttonstockon.ForeColor = System.Drawing.Color.White;
            this.Buttonstockon.Location = new System.Drawing.Point(12, 8);
            this.Buttonstockon.Name = "Buttonstockon";
            this.Buttonstockon.Size = new System.Drawing.Size(150, 35);
            this.Buttonstockon.TabIndex = 114;
            this.Buttonstockon.Text = "Add  Stock";
            this.Buttonstockon.TextColor = System.Drawing.Color.White;
            this.Buttonstockon.UseVisualStyleBackColor = false;
            this.Buttonstockon.Click += new System.EventHandler(this.Buttonstockon_Click);
            // 
            // Buttoneditdatabaseon
            // 
            this.Buttoneditdatabaseon.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.Buttoneditdatabaseon.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.Buttoneditdatabaseon.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Buttoneditdatabaseon.BorderRadius = 40;
            this.Buttoneditdatabaseon.BorderSize = 0;
            this.Buttoneditdatabaseon.FlatAppearance.BorderSize = 0;
            this.Buttoneditdatabaseon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buttoneditdatabaseon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Buttoneditdatabaseon.ForeColor = System.Drawing.Color.White;
            this.Buttoneditdatabaseon.Location = new System.Drawing.Point(168, 8);
            this.Buttoneditdatabaseon.Name = "Buttoneditdatabaseon";
            this.Buttoneditdatabaseon.Size = new System.Drawing.Size(150, 35);
            this.Buttoneditdatabaseon.TabIndex = 115;
            this.Buttoneditdatabaseon.Text = "Edit Database";
            this.Buttoneditdatabaseon.TextColor = System.Drawing.Color.White;
            this.Buttoneditdatabaseon.UseVisualStyleBackColor = false;
            this.Buttoneditdatabaseon.Click += new System.EventHandler(this.Buttoneditdatabaseon_Click);
            // 
            // FormMainSettingAddStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1256, 709);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMainSettingAddStock";
            this.Text = "FormMainSettingAddStock";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private RJ_Button.RJButton Buttonstockon;
        private RJ_Button.RJButton Buttoneditdatabaseon;
    }
}