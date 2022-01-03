namespace DMPAplikaceProSŠ
{
    partial class Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.panelBarva = new System.Windows.Forms.Panel();
            this.lbBarvaPozadi = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.lbSSystem = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // panelBarva
            // 
            this.panelBarva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBarva.Location = new System.Drawing.Point(192, 122);
            this.panelBarva.Name = "panelBarva";
            this.panelBarva.Size = new System.Drawing.Size(25, 25);
            this.panelBarva.TabIndex = 9;
            this.panelBarva.Visible = false;
            this.panelBarva.Click += new System.EventHandler(this.panelBarva_Click);
            // 
            // lbBarvaPozadi
            // 
            this.lbBarvaPozadi.AutoSize = true;
            this.lbBarvaPozadi.Location = new System.Drawing.Point(115, 129);
            this.lbBarvaPozadi.Name = "lbBarvaPozadi";
            this.lbBarvaPozadi.Size = new System.Drawing.Size(71, 13);
            this.lbBarvaPozadi.TabIndex = 8;
            this.lbBarvaPozadi.Text = "Barva pozadí";
            this.lbBarvaPozadi.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(118, 169);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Možnosti";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbSSystem
            // 
            this.lbSSystem.Location = new System.Drawing.Point(102, 88);
            this.lbSSystem.Name = "lbSSystem";
            this.lbSSystem.Size = new System.Drawing.Size(103, 23);
            this.lbSSystem.TabIndex = 6;
            this.lbSSystem.Text = "Souřadný systém";
            this.lbSSystem.UseVisualStyleBackColor = true;
            this.lbSSystem.Click += new System.EventHandler(this.lbSSystem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "Procvičování analytické geometrie";
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 321);
            this.Controls.Add(this.panelBarva);
            this.Controls.Add(this.lbBarvaPozadi);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lbSSystem);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelBarva;
        private System.Windows.Forms.Label lbBarvaPozadi;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button lbSSystem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}