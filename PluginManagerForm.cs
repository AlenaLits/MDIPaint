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
    public partial class PluginManagerForm : Form
    {
        private List<PluginInfo> plugins;
        public PluginManagerForm(List<PluginInfo> plugins)
        {
            InitializeComponent();
            this.plugins = plugins;
        }
        private void PluginManagerForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            foreach (var plugin in plugins)
            {
                dataGridView1.Rows.Add(plugin.Name, plugin.Author, plugin.Version, plugin.Enabled);
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                plugins[i].Enabled = Convert.ToBoolean(dataGridView1.Rows[i].Cells[3].Value);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
