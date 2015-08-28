using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers_wf
{
    partial class OptionForm
    {
        public OptionForm(Dictionary<string, string> options)
        {
            InitializeComponent();

            this.label1.Text = "Remote IP";
            this.textBox1.Text = options["Remote Ip"];
            this.label2.Text = "Remote Port";
            this.textBox2.Text = options["Remote Port"];
        }
    }
}
