namespace ProjectionBuilder
{
    partial class StartupForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.DateTB = new System.Windows.Forms.TextBox();
            this.basketball_rb = new System.Windows.Forms.RadioButton();
            this.football_rb = new System.Windows.Forms.RadioButton();
            this.start_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date";
            // 
            // DateTB
            // 
            this.DateTB.Location = new System.Drawing.Point(53, 55);
            this.DateTB.Name = "DateTB";
            this.DateTB.Size = new System.Drawing.Size(182, 20);
            this.DateTB.TabIndex = 1;
            // 
            // basketball_rb
            // 
            this.basketball_rb.AutoSize = true;
            this.basketball_rb.Location = new System.Drawing.Point(53, 102);
            this.basketball_rb.Name = "basketball_rb";
            this.basketball_rb.Size = new System.Drawing.Size(74, 17);
            this.basketball_rb.TabIndex = 2;
            this.basketball_rb.TabStop = true;
            this.basketball_rb.Text = "Basketball";
            this.basketball_rb.UseVisualStyleBackColor = true;
            // 
            // football_rb
            // 
            this.football_rb.AutoSize = true;
            this.football_rb.Location = new System.Drawing.Point(53, 125);
            this.football_rb.Name = "football_rb";
            this.football_rb.Size = new System.Drawing.Size(62, 17);
            this.football_rb.TabIndex = 3;
            this.football_rb.TabStop = true;
            this.football_rb.Text = "Football";
            this.football_rb.UseVisualStyleBackColor = true;
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(82, 161);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(113, 34);
            this.start_btn.TabIndex = 4;
            this.start_btn.Text = "Generate";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.Start_btn_Click);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 276);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.football_rb);
            this.Controls.Add(this.basketball_rb);
            this.Controls.Add(this.DateTB);
            this.Controls.Add(this.label1);
            this.Name = "StartupForm";
            this.Text = "StartupForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DateTB;
        private System.Windows.Forms.RadioButton basketball_rb;
        private System.Windows.Forms.RadioButton football_rb;
        private System.Windows.Forms.Button start_btn;
    }
}