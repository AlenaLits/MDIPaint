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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MDIPaint
{
    public partial class DocumentForm : Form
    {
        public enum Tool
        {
            Pen,
            Line,
            Ellipse,
            Eraser,
            FillEllipse,
            Text,
            Fill,
            Polygon
        }

        public static Tool CurrentTool { get; set; } = Tool.Pen;
        private Bitmap bitmapPreview; // Временный холст для предварительного просмотра
        private bool isDrawing = false; // Флаг, идет ли рисование
        private bool isModified = false;
        public static bool FillShapes { get; set; } = false;
        private bool drawing = false;
        private Point startPoint, endPoint;

        private string filePath = null;
        private int x, y;
        private Bitmap bitmap;
        private string currentFilePath = null;
        public int BitmapWidth => bitmap.Width;
        public int BitmapHeight => bitmap.Height;
        private float scaleFactor = 1.0f; // Текущий масштаб
        private const float scaleStep = 1.2f; // Коэффициент увеличения
        private const float minScale = 0.5f; // Минимальный масштаб
        private const float maxScale = 5.0f; // Максимальный масштаб
        private string currentText = ""; // Введённый текст
        private Point textPosition; // Координаты для текста
        private bool isEnteringText = false; // Флаг режима ввода текста
        private Font textFont = new Font("Arial", 16); // Шрифт текста
        private SolidBrush textBrush = new SolidBrush(Color.Black); // Цвет текста
        private int polygonSides = 5; // По умолчанию пятиугольник
        private Point startPolygon; // Начальная точка
        private bool drawingPolygon = false; // Флаг рисования

        public Bitmap GetBitmap()
        {
            return bitmap;
        }
        public void SetBitmap(Bitmap newBitmap)
        {
            if (bitmap != null)
                bitmap.Dispose();

            bitmap = newBitmap;
            Invalidate(); // Запросить перерисовку формы
        }
        public DocumentForm()
        {
            InitializeComponent();
            bitmap = new Bitmap(800, 600); // Основной холст
            bitmapPreview = new Bitmap(800, 600); // Холст для предварительного просмотра
            this.FormClosing += DocumentForm_FormClosing;
        }
        public DocumentForm(Bitmap image)
        {
            InitializeComponent();
            bitmap = new Bitmap(image);
            Invalidate();
        }

        private void DocumentForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            
                if (CurrentTool == Tool.Pen || CurrentTool == Tool.Eraser)
                {
                    using (Graphics g = Graphics.FromImage(bitmap)) // Рисуем сразу на bitmap!
                    {
                        Pen pen = (CurrentTool == Tool.Eraser)
                            ? new Pen(Color.White, 20) // Ластик — белый цвет
                            : new Pen(MainForm.Color, MainForm.Width);

                        g.DrawLine(pen, x, y, e.X, e.Y);
                    }

                    x = e.X;
                    y = e.Y;
                    Invalidate(); // Обновляем экран
                    isModified = true;
                }
                else
                {
                    bitmapPreview = (Bitmap)bitmap.Clone(); // Для фигур используем превью
                    using (Graphics g = Graphics.FromImage(bitmapPreview))
                    {
                        Pen pen = new Pen(MainForm.Color, MainForm.Width);
                        Brush brush = new SolidBrush(MainForm.Color);

                        switch (CurrentTool)
                        {
                            case Tool.Line:
                                g.DrawLine(pen, x, y, e.X, e.Y);
                                break;

                            case Tool.Ellipse:
                                int width = Math.Abs(e.X - x);
                                int height = Math.Abs(e.Y - y);
                                g.DrawEllipse(pen, Math.Min(x, e.X), Math.Min(y, e.Y), width, height);
                                break;
                            case Tool.FillEllipse:
                                width = Math.Abs(e.X - x);
                                height = Math.Abs(e.Y - y);
                                g.FillEllipse(brush, Math.Min(x, e.X), Math.Min(y, e.Y), width, height);
                                break;
                            case Tool.Polygon:
                                // Вычисление вершин правильного многоугольника
                                Point[] polygonPoints = new Point[polygonSides];
                                float radius = Math.Min(Math.Abs(e.X - x), Math.Abs(e.Y - y)); // Радиус многоугольника
                                float angleStep = (float)(2 * Math.PI / polygonSides); // Угловое расстояние между вершинами

                                for (int i = 0; i < polygonSides; i++)
                                {
                                    float angle = i * angleStep; // Угол для каждой вершины
                                    int px = (int)(x + radius * Math.Cos(angle)); // Координаты X вершины
                                    int py = (int)(y + radius * Math.Sin(angle)); // Координаты Y вершины
                                    polygonPoints[i] = new Point(px, py); // Добавляем точку в массив
                                }

                                // Рисуем многоугольник
                                g.DrawPolygon(pen, polygonPoints);
                                break;

                        }
                    }
                    isModified = true;
                    Invalidate();
                }
        }

        private void DocumentForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
                isDrawing = true;

                if (CurrentTool == Tool.Pen || CurrentTool == Tool.Eraser)
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        Pen pen = (CurrentTool == Tool.Eraser)
                            ? new Pen(Color.White, MainForm.Width)  // Ластик рисует белым
                            : new Pen(MainForm.Color, MainForm.Width);

                        g.DrawLine(pen, x, y, x + 1, y + 1); // Рисуем маленькую точку
                    }
                    isModified = true;
                    Invalidate(); // Обновляем экран
                }
                else
                {
                    bitmapPreview = (Bitmap)bitmap.Clone(); // Для фигур используем превью
                }
            }
            if (CurrentTool == Tool.Text && e.Button == MouseButtons.Left)
            {
                textPosition = e.Location;
                isEnteringText = true;

                // Запрос ввода текста
                using (var inputBox = new FormText())
                {
                    if (inputBox.ShowDialog() == DialogResult.OK)
                    {
                        currentText = inputBox.EnteredText;
                        isEnteringText = false;

                        // Отрисовываем текст
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.DrawString(currentText, textFont, textBrush, textPosition);
                        }
                        Invalidate();
                    }
                }
            }
            if (CurrentTool == Tool.Fill && e.Button == MouseButtons.Left)
            {
                Fill(e.X, e.Y, MainForm.Color); // Вызываем заливку
                Invalidate(); // Перерисовываем
            }
            if (CurrentTool == Tool.Polygon && e.Button == MouseButtons.Left)
            {
                startPolygon = e.Location; // Запоминаем начальную точку
                drawingPolygon = true;
                bitmapPreview = (Bitmap)bitmap.Clone(); // Создаём копию изображения для превью
            }
        }


        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    Pen pen = new Pen(MainForm.Color, MainForm.Width);
                    Brush brush = new SolidBrush(MainForm.Color);

                    switch (CurrentTool)
                    {
                        case Tool.Line:
                            g.DrawLine(pen, x, y, e.X, e.Y);
                            break;

                        case Tool.Ellipse:
                            int width = Math.Abs(e.X - x);
                            int height = Math.Abs(e.Y - y);
                            g.DrawEllipse(pen, Math.Min(x, e.X), Math.Min(y, e.Y), width, height);
                            break;
                        case Tool.FillEllipse:
                            width = Math.Abs(e.X - x);
                            height = Math.Abs(e.Y - y);
                            g.FillEllipse(brush, Math.Min(x, e.X), Math.Min(y, e.Y), width, height);
                            break;
                        case Tool.Polygon:
                            // Вычисление вершин правильного многоугольника
                            Point[] polygonPoints = new Point[polygonSides];
                            float radius = Math.Min(Math.Abs(e.X - x), Math.Abs(e.Y - y)); // Радиус многоугольника
                            float angleStep = (float)(2 * Math.PI / polygonSides); // Угловое расстояние между вершинами

                            for (int i = 0; i < polygonSides; i++)
                            {
                                float angle = i * angleStep; // Угол для каждой вершины
                                int px = (int)(x + radius * Math.Cos(angle)); // Координаты X вершины
                                int py = (int)(y + radius * Math.Sin(angle)); // Координаты Y вершины
                                polygonPoints[i] = new Point(px, py); // Добавляем точку в массив
                            }

                            // Рисуем многоугольник
                            g.DrawPolygon(pen, polygonPoints);
                            break;
                    }
                }

                isDrawing = false;
                isModified = true;
                Invalidate(); // Обновляем экран
            }
            isModified = true; // Фиксируем изменения
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (bitmap != null)
            {
                try
                {
                    e.Graphics.DrawImage(bitmap, 0, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отрисовке изображения:\n" + ex.Message);
                }
            }
            //base.OnPaint(e);
            //// Включаем качественное масштабирование
            //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //// Масштабируем и рисуем изображение
            //e.Graphics.ScaleTransform(scaleFactor, scaleFactor);

            //if (bitmap != null)
            //    e.Graphics.DrawImage(bitmap, 0, 0);

            //if (isDrawing && (CurrentTool == Tool.Line || CurrentTool == Tool.Ellipse || CurrentTool == Tool.FillEllipse || CurrentTool == Tool.Polygon) && bitmapPreview != null)
            //    e.Graphics.DrawImage(bitmapPreview, 0, 0);
        }

        public void SaveAs()
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg, *.jpg)|*.jpeg;*.jpg";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(dlg.FileName);
                filePath = dlg.FileName;
            }
        }
        public void Save()
        {
            if (filePath == null)
            {
                SaveAs();
            }
            else
            {
                bitmap.Save(filePath);
            }
        }
        
        public void ResizeCanvas(int newWidth, int newHeight)
        {
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.Clear(Color.White);
                g.DrawImage(bitmap, 0, 0);
            }
            bitmap = newBitmap;
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            switch (CurrentTool)
            {
                case Tool.Pen:
                    Cursor = Cursors.Cross;
                    break;
                case Tool.Line:
                    Cursor = Cursors.Cross;
                    break;
                case Tool.Ellipse:
                    Cursor = Cursors.Hand;
                    break;
                case Tool.FillEllipse:
                    Cursor = Cursors.Hand;
                    break;
                case Tool.Eraser:
                    Cursor = Cursors.No;
                    break;
            }
        }
        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModified)
            {
                DialogResult result = MessageBox.Show(
                    "Сохранить изменения перед выходом?",
                    "Подтверждение",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    SaveAs(); // Добавь диалог сохранения вместо пути
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // Отмена закрытия
                }
            }
        }
        public void ZoomIn()
        {
            if (scaleFactor < maxScale)
            {
                scaleFactor *= scaleStep;
                Invalidate(); // Перерисовка
            }
        }

        public void ZoomOut()
        {
            if (scaleFactor > minScale)
            {
                scaleFactor /= scaleStep;
                Invalidate(); // Перерисовка
            }
        }
        public void SetTool(Tool tool)
        {
            CurrentTool = tool;
        }
        private void Fill(int x, int y, Color newColor)
        {
            Color oldColor = bitmap.GetPixel(x, y); // Получаем цвет пикселя по координатам

            if (oldColor.ToArgb() == newColor.ToArgb()) return; // Если цвет такой же – выходим

            Queue<Point> pixels = new Queue<Point>();
            pixels.Enqueue(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point p = pixels.Dequeue();

                if (p.X < 0 || p.Y < 0 || p.X >= bitmap.Width || p.Y >= bitmap.Height)
                    continue;

                if (bitmap.GetPixel(p.X, p.Y).ToArgb() == oldColor.ToArgb())
                {
                    bitmap.SetPixel(p.X, p.Y, newColor);

                    pixels.Enqueue(new Point(p.X + 1, p.Y));
                    pixels.Enqueue(new Point(p.X - 1, p.Y));
                    pixels.Enqueue(new Point(p.X, p.Y + 1));
                    pixels.Enqueue(new Point(p.X, p.Y - 1));
                }
            }
        }
        private void DrawPolygon(Bitmap bmp, Point center, Point edge, int sides, Color color, int width)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Pen pen = new Pen(color, width);
                float radius = (float)Math.Sqrt(Math.Pow(edge.X - center.X, 2) + Math.Pow(edge.Y - center.Y, 2));
                PointF[] points = new PointF[sides];

                for (int i = 0; i < sides; i++)
                {
                    float angle = (float)(2 * Math.PI * i / sides);
                    points[i] = new PointF(
                        center.X + radius * (float)Math.Cos(angle),
                        center.Y + radius * (float)Math.Sin(angle)
                    );
                }

                g.DrawPolygon(pen, points);
            }
        }
        public void SetPolygonSides(int sides)
        {
            polygonSides = Math.Max(3, sides); // Минимум треугольник
        }

    }
}
