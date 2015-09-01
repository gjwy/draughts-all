using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace checkers_wf
{
    public partial class OptionForm : Form
    {

        // while its open, have ref to options
        Dictionary<string, string> options;

        public OptionForm()
        {
            InitializeComponent();
        }

        public OptionForm(Dictionary<string, string> options)
        {
            InitializeComponent();
            this.options = options;
            this.label_ip.Text = "Remote IP";
            this.textbox_ip.Text = this.options["Remote Ip"];
            this.label_port.Text = "Remote Port";
            this.textbox_port.Text = this.options["Remote Port"];
        }

        private void OptionForm_Load(object sender, EventArgs e)
        {
            // since its created when its loaded nothing needs to be done here (its done in the constructor)
        }

        // red x behaviour (disabled control box)
        // ~ used for cancel button
        private void optionCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void apply_button_Click(object sender, EventArgs e)
        {
            // apply the settings back to the options variable on apply
            // TODO: check the input is valid 
            this.options["Remote Ip"] = this.textbox_ip.Text;
            this.options["Remote Port"] = this.textbox_port.Text;
            this.Close();
        }
    }
}
