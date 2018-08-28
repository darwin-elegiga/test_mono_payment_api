namespace VerifyString
{
    partial class Form1
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
            this.buttonGo1 = new System.Windows.Forms.Button();
            this.buttonGo2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGo1
            // 
            this.buttonGo1.Location = new System.Drawing.Point(12, 12);
            this.buttonGo1.Name = "buttonGo1";
            this.buttonGo1.Size = new System.Drawing.Size(75, 23);
            this.buttonGo1.TabIndex = 0;
            this.buttonGo1.Text = "Go 1";
            this.buttonGo1.UseVisualStyleBackColor = true;
            this.buttonGo1.Click += new System.EventHandler(this.buttonGo1_Click);
            // 
            // buttonGo2
            // 
            this.buttonGo2.Location = new System.Drawing.Point(12, 41);
            this.buttonGo2.Name = "buttonGo2";
            this.buttonGo2.Size = new System.Drawing.Size(75, 23);
            this.buttonGo2.TabIndex = 1;
            this.buttonGo2.Text = "Go 2";
            this.buttonGo2.UseVisualStyleBackColor = true;
            this.buttonGo2.Click += new System.EventHandler(this.buttonGo2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 315);
            this.Controls.Add(this.buttonGo2);
            this.Controls.Add(this.buttonGo1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGo1;
        private System.Windows.Forms.Button buttonGo2;
    }
}

