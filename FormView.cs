using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using warehouse.Modals;

namespace warehouse
{
    enum RefrehType
    {
        OnSelect,
        Full
    }
    public partial class FormView : Form, IView
    {
        public event EventHandler<EventArgs> SetCargo;
        public event EventHandler<EventArgs> SetPlatforms;
        bool rectangleFlag = false;
        int counterPlatform = 0;
        Point firstPicketButton, secondPicketButton;
        List<Rectangle> rectangles = new List<Rectangle>();
        public FormView()
        {
            InitializeComponent();
            InitializeTableLayoutSettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void InitializeTableLayoutSettings()
        {
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.ColumnCount = 4;

            float percentWidth = 100 / tableLayoutPanel1.RowCount;
            float percentHeight = 100 / tableLayoutPanel1.ColumnCount;

            int counterPickets = 0;
            // Populate the GridStrip control with ToolStripButton controls.
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                {
                    // Create a new ToolStripButton control.
                    tableLayoutPanel1.RowStyles.Add(new RowStyle());
                    Button buttonPicket = new Button();
                    buttonPicket.FlatStyle = FlatStyle.Flat;
                    buttonPicket.Dock = DockStyle.Fill;
                    buttonPicket.Click += new EventHandler(ButtonPicketClick);
                    buttonPicket.MouseEnter += new EventHandler(OnMouseEnterButtonPicket);

                    // Add the new ToolStripButton control to the GridStrip.


                    tableLayoutPanel1.Controls.Add(buttonPicket, i, j);
                    // If this is the ToolStripButton control at cell (0,0),
                    // assign it as the empty cell button.
                    counterPickets++;
                }
            }

            TableLayoutColumnStyleCollection columnStyle = tableLayoutPanel1.ColumnStyles;
            TableLayoutRowStyleCollection rowStyle = tableLayoutPanel1.RowStyles;

            foreach (ColumnStyle style in columnStyle)
            {
                style.SizeType = SizeType.Percent;
                style.Width = percentWidth;
            }

            foreach (RowStyle style in rowStyle)
            {
                style.SizeType = SizeType.Percent;
                style.Height = percentHeight;
            }

            tableLayoutPanel1.Enabled = false;
        }

        private void RefrehsGrid(RefrehType type)
        {
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                {
                    if (type == RefrehType.OnSelect)
                        if (tableLayoutPanel1.GetControlFromPosition(i, j).Tag == null)
                            tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White;
                    if (type == RefrehType.Full)
                    {
                        Button button = tableLayoutPanel1.GetControlFromPosition(i, j) as Button;
                        button.BackColor = Color.White;
                        button.Tag = null;
                    }
                }
        }

        private void ButtonPicketClick(object sender, EventArgs e)
        {
            bool intersect = false;
            Button button = (Button)sender;
            Point point = new Point(tableLayoutPanel1.GetColumn(button), tableLayoutPanel1.GetRow(button));

            Rectangle currentRect = new Rectangle(firstPicketButton.X, firstPicketButton.Y,
                                                          point.X - firstPicketButton.X, point.Y - firstPicketButton.Y);

            if (rectangleFlag)
            {
                secondPicketButton = point;
                rectangleFlag = false;
                foreach (Rectangle rec in rectangles)
                    if (intersect = IntersectsRectangles(currentRect, rec))
                        break;

                if (!intersect)
                {
                    FillRectangle(firstPicketButton, point, GetRandomColor(counterPlatform++));
                    rectangles.Add(currentRect);
                    if (GridIsFilled())
                    {
                        DialogResult result = MessageBox.Show(
                                                    "Платформы выделены. Начать заново?",
                                                    "Сообщение",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Information,
                                                    MessageBoxDefaultButton.Button1,
                                                    MessageBoxOptions.DefaultDesktopOnly);

                        if (result == DialogResult.Yes)
                            StartSelectPlatforms();
                        else
                        {
                            SetPlatforms(this, EventArgs.Empty);
                            tableLayoutPanel1.Enabled = false;
                        }
                            
                    }
                }                  
            }
            else
            {
                firstPicketButton = point;
                rectangleFlag = true;
            }
        }

        private void FillRectangle(Point a, Point b, Color color)
        {
            for (int i = a.X; i < b.X + 1; i++)
                for (int j = a.Y; j < b.Y + 1; j++)
                {
                    Button button = tableLayoutPanel1.GetControlFromPosition(i, j) as Button;
                    button.BackColor = color;
                    if (!rectangleFlag)
                        button.Tag = 1;
                }
        }

        private void OnMouseEnterButtonPicket(object sender, EventArgs e)
        {
            bool intersect = false;
            Button button = (Button)sender;
            Point point = new Point(tableLayoutPanel1.GetColumn(button), tableLayoutPanel1.GetRow(button));
            RefrehsGrid(RefrehType.OnSelect);

            Rectangle currentRect = new Rectangle(firstPicketButton.X, firstPicketButton.Y,
                                point.X - firstPicketButton.X, point.Y - firstPicketButton.Y);

            if (rectangleFlag)
            {
                foreach (Rectangle rec in rectangles)
                    if (intersect = IntersectsRectangles(currentRect, rec))
                        break;

                if (!intersect)
                    FillRectangle(firstPicketButton, point, Color.Green);
            }
        }

        private bool IntersectsRectangles(Rectangle a, Rectangle b)
        {
            return (a.Y <= b.Y + b.Height && a.Y + a.Height >= b.Y &&
                a.X + a.Width >= b.X && a.X <= b.X + b.Width);
        }

        private void ButtonSelectPlatform_Click(object sender, EventArgs e)
        {
            StartSelectPlatforms();
        }

        private Color GetRandomColor(int seed)
        {
            Random randomGen = new Random(seed);
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            return Color.FromKnownColor(randomColorName);
        }

        private bool GridIsFilled()
        {
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                    if (tableLayoutPanel1.GetControlFromPosition(i, j).Tag == null)
                        return false;

            return true;
        }

        private void StartSelectPlatforms()
        {
            rectangleFlag = false;
            counterPlatform = 0;
            tableLayoutPanel1.Enabled = true;
            rectangles.Clear();
            RefrehsGrid(RefrehType.Full);
        }

        public int?[] InputPickets
        {
            set
            {
                int k = 0;
                for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
                    for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                    {
                        tableLayoutPanel1.GetControlFromPosition(j, i).Text = value[k].ToString();
                        k++;
                    }
                        
            }
        }

        public int[][] Platforms
        {
            get 
            {
                int[][] platformsArr = new int[rectangles.Count][];
                for (int i = 0; i < rectangles.Count; i++)
                {
                    int counter = 0;
                    platformsArr[i] = new int[(rectangles[i].Width + 1) * (rectangles[i].Height + 1)];
                    for (int j = rectangles[i].X; j < rectangles[i].X + rectangles[i].Width + 1; j++)
                        for (int k = rectangles[i].Y; k < rectangles[i].Y + rectangles[i].Height + 1; k++)
                        {
                            platformsArr[i][counter] = Int32.Parse(tableLayoutPanel1.GetControlFromPosition(j, k).Text);
                            counter++;
                        }
                }

                return platformsArr;
            }
        }

        private void buttonInputPickets_Click(object sender, EventArgs e)
        {
            ShowMyDialogBox();
        }

        public void ShowMyDialogBox()
        {
            InputPickectsModalForm Dialog = new InputPickectsModalForm();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (Dialog.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                int picket = Int32.Parse(Dialog.textBox1.Text);
                int?[] pickets = new int?[tableLayoutPanel1.Controls.Count];
                for (int i = 0; i < pickets.Length; picket++, i++)
                    pickets[i] = picket;

                this.InputPickets = pickets;
            }

            Dialog.Dispose();
        }

    }
}
