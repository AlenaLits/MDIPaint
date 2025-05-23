using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class FormText : Form
    {
        public string EnteredText { get; private set; }

        public FormText()
        {
            InitializeComponent();
            bntOK.Click += bntOK_Click;
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            EnteredText = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
