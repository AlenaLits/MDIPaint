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
    public partial class CanvasSizeForm : Form
    {
        public int CanvasWidth => (int)numericUpDownWidth.Value;
        public int CanvasHeight => (int)numericUpDownHeight.Value;

        public CanvasSizeForm(int currentWidth, int currentHeight)
        {
            InitializeComponent();
            numericUpDownWidth.Value = currentWidth;
            numericUpDownHeight.Value = currentHeight;
        }
    }
}
