namespace checkers_wf
{
    partial class AboutForm
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
            this.aboutCloseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // aboutCloseButton
            // 
            this.aboutCloseButton.Location = new System.Drawing.Point(110, 226);
            this.aboutCloseButton.Name = "aboutCloseButton";
            this.aboutCloseButton.Size = new System.Drawing.Size(80, 23);
            this.aboutCloseButton.TabIndex = 0;
            this.aboutCloseButton.Text = "Close";
            this.aboutCloseButton.UseVisualStyleBackColor = true;
            this.aboutCloseButton.Click += new System.EventHandler(this.aboutCloseButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 160);
            this.label1.TabIndex = 1;
            this.label1.Text = "Checkers\r\nVersion 1.1 - gui\r\nGareth Wilson 2015\r\n\r\nTodo: finish gui, create logic" +
    ", add gui and model to \r\nlogic.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImage = global::checkers_wf.Properties.Resources.banner;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 43);
            this.panel1.TabIndex = 2;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.aboutCloseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AboutForm";
            this.Text = "About";
            this.Load += new System.EventHandler(this.aboutForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button aboutCloseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}