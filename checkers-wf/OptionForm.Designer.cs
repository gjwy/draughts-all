namespace checkers_wf
{
    partial class OptionForm
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
            this.label_ip = new System.Windows.Forms.Label();
            this.label_port = new System.Windows.Forms.Label();
            this.textbox_ip = new System.Windows.Forms.TextBox();
            this.textbox_port = new System.Windows.Forms.TextBox();
            this.apply_button = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Location = new System.Drawing.Point(46, 50);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(35, 13);
            this.label_ip.TabIndex = 0;
            this.label_ip.Text = "label1";
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(46, 77);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(35, 13);
            this.label_port.TabIndex = 1;
            this.label_port.Text = "label2";
            // 
            // textbox_ip
            // 
            this.textbox_ip.Location = new System.Drawing.Point(118, 50);
            this.textbox_ip.Name = "textbox_ip";
            this.textbox_ip.Size = new System.Drawing.Size(100, 20);
            this.textbox_ip.TabIndex = 2;
            // 
            // textbox_port
            // 
            this.textbox_port.Location = new System.Drawing.Point(118, 77);
            this.textbox_port.Name = "textbox_port";
            this.textbox_port.Size = new System.Drawing.Size(100, 20);
            this.textbox_port.TabIndex = 3;
            // 
            // apply_button
            // 
            this.apply_button.Location = new System.Drawing.Point(168, 209);
            this.apply_button.Name = "apply_button";
            this.apply_button.Size = new System.Drawing.Size(75, 23);
            this.apply_button.TabIndex = 4;
            this.apply_button.Text = "Apply";
            this.apply_button.UseVisualStyleBackColor = true;
            this.apply_button.Click += new System.EventHandler(this.apply_button_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(49, 209);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.optionCloseButton_Click);
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.apply_button);
            this.Controls.Add(this.textbox_port);
            this.Controls.Add(this.textbox_ip);
            this.Controls.Add(this.label_port);
            this.Controls.Add(this.label_ip);
            this.Name = "OptionForm";
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.TextBox textbox_ip;
        private System.Windows.Forms.TextBox textbox_port;
        private System.Windows.Forms.Button apply_button;
        private System.Windows.Forms.Button button2;
    }
}