using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Manager
{
    public partial class Form2 : Form
    {
        string list;
        int mode;
        public Form2(string message, string capture, int value)
        {
            InitializeComponent();
            this.Text = capture;
            list = message;
            mode = value;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (string item in list.Split('/'))
                if (item != "")
                {
                    if (mode == 1) listBox1.Items.Add(item + ".exe");
                    else listBox1.Items.Add(item);
                }
        }
    }
}
