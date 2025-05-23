using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MDIPaint.DocumentForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PluginInterface;
using System.Reflection;
using System.IO;
using System.Threading;

namespace MDIPaint
{
    public partial class MainForm : Form
    {
        private Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        private CancellationTokenSource cts;
        private bool isFiltering = false;

        private List<PluginInfo> allPlugins;
        private string pluginConfigPath = "plugins.config.json";
        public static Color Color { get; set; }
        public static int Width { get; set; }
        private void UpdateCursor()
        {
            switch (DocumentForm.CurrentTool)
            {
                case Tool.Pen:
                    this.Cursor = Cursors.Cross;
                    break;
                case Tool.Eraser:
                    this.Cursor = Cursors.Hand;
                    break;
                case Tool.Line:
                case Tool.Ellipse:
                    this.Cursor = Cursors.Cross;
                    break;
            }
        }
        private ToolStripMenuItem filtersToolStripMenuItem;
        public MainForm()
        {
            InitializeComponent();
            Color = Color.Black;
            Width = 3;
            LoadPlugins();
            //CreatePluginsMenu();
            this.MdiChildActivate += MainForm_MdiChildActivate;
        }
        private void LoadPlugins()
        {
            allPlugins = PluginLoader.LoadPlugins(pluginConfigPath);
            plugins.Clear();

            foreach (var plugin in allPlugins)
            {
                if (plugin.Enabled)
                {
                    plugins.Add(plugin.Name, plugin.Instance);
                }
            }
        }
        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            CreatePluginsMenu();
        }

        private void CreatePluginsMenu()
        {
            filtersToolStripMenuItem.DropDownItems.Clear();

            bool hasActiveDocument = this.ActiveMdiChild is DocumentForm;

            foreach (var p in plugins)
            {
                var item = filtersToolStripMenuItem.DropDownItems.Add(p.Value.Name);
                item.Click += OnPluginClick;
                item.Enabled = hasActiveDocument;
            }

            filtersToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

            var managerItem = filtersToolStripMenuItem.DropDownItems.Add("Управление плагинами...");
            managerItem.Click += OpenPluginManager;
        }
        private void OpenPluginManager(object sender, EventArgs e)
        {
            var dialog = new PluginManagerForm(allPlugins);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PluginLoader.SavePluginConfig(allPlugins, pluginConfigPath);
                LoadPlugins();
                CreatePluginsMenu();
            }
        }
        private async void OnPluginClick(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            buttonCancelFilter.Enabled = true;

            if (isFiltering)
            {
                MessageBox.Show("Фильтрация уже выполняется.");
                return;
            }

            if (this.ActiveMdiChild is DocumentForm doc)
            {
                var plugin = plugins[((ToolStripMenuItem)sender).Text];

                Bitmap original = doc.GetBitmap();
                cts = new CancellationTokenSource();
                isFiltering = true;

                await Task.Delay(1);

                try
                {
                    Bitmap result;

                    if (plugin is IAsyncPlugin asyncPlugin)
                    {
                        result = await asyncPlugin.TransformAsync(original, cts.Token, new Progress<int>(percent =>
                        {
                            progressBar1.BeginInvoke(new Action(() => progressBar1.Value = percent));
                        }));
                    }
                    else
                    {
                        result = await Task.Run(() => plugin.Transform(original));
                    }

                    doc.SetBitmap(result);
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("Фильтрация отменена.");
                }
                finally
                {
                    isFiltering = false;
                    buttonCancelFilter.Enabled = false;
                    progressBar1.Value = 0;
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmAbout = new AboutForm();
            frmAbout.ShowDialog();

        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new DocumentForm();
            frm.MdiParent = this;
            frm.Show();

        }

        private void рисунокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled=!(ActiveMdiChild==null);
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                var frm = new CanvasSizeForm(d.BitmapWidth, d.BitmapHeight);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    d.ResizeCanvas(frm.CanvasWidth, frm.CanvasHeight);
                }
            }
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Red;
        }

        private void синToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Blue;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Green;
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                Color = cd.Color;

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            d?.Save();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Изображения (*.bmp, *.jpg, *.jpeg)|*.bmp;*.jpg;*.jpeg|Все файлы (*.*)|*.*"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bmp = new Bitmap(dlg.FileName);
                    var frm = new DocumentForm(bmp);
                    frm.MdiParent = this;
                    frm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;

            сохранитьКакToolStripMenuItem.Enabled = d != null;
            сохранитьToolStripMenuItem.Enabled = d != null;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Width = 1;
        }

        private void Толщина2_Click(object sender, EventArgs e)
        {
            Width = 5;
        }

        private void толщина3_Click(object sender, EventArgs e)
        {
            Width = 10;
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            d?.SaveAs();
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocumentForm.CurrentTool = Tool.Line;
            UpdateCursor();
        }

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocumentForm.CurrentTool = Tool.Ellipse;
            UpdateCursor();
        }

        private void toolStripButtonPen_Click(object sender, EventArgs e)
        {
            DocumentForm.CurrentTool = Tool.Pen;
            UpdateCursor();
        }

        private void toolStripButtonEraser_Click(object sender, EventArgs e)
        {
            DocumentForm.CurrentTool = Tool.Eraser;
            UpdateCursor();
        }

        private void незакрашенныйЭллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocumentForm.CurrentTool = Tool.FillEllipse;
            UpdateCursor();
        }

        private void масштабувелtoolStripButton_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.ZoomIn();
            }

        }

        private void масштабуменьtoolStripButton_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.ZoomOut();
            }
        }

        private void toolStripButtonText_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetTool(Tool.Text);
            }
        }

        private void toolStripButtonFilling_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetTool(Tool.Fill);
            }
        }

        private void правильныйМногоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetTool(Tool.Polygon);
            }
        }
       
        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(3);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(4);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(5);
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(6);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(7);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(8);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(9);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(10);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(11);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            var d = ActiveMdiChild as DocumentForm;
            if (d != null)
            {
                d.SetPolygonSides(12);
            }
        }

        private void buttonCancelFilter_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }
    }
}
