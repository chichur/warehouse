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

namespace warehouse
{
    public partial class Form1 : Form
    {
        bool rectangleFlag = false;
        Point firstPicketButton, secondPicketButton;
        List<Rectangle> rectangles = new List<Rectangle>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeTableLayoutSettings();
        }

        private void InitializeTableLayoutSettings()
        {
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.ColumnCount = 5;

            float percentWidth = 100 / tableLayoutPanel1.RowCount;
            float percentHeight = 100 / tableLayoutPanel1.ColumnCount;

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
        }

        private void RefrehsGrid()
        {
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                    if (tableLayoutPanel1.GetControlFromPosition(i, j).Tag == null)
                        tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White;
        }

        private void ButtonPicketClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Point point = new Point(tableLayoutPanel1.GetColumn(button), tableLayoutPanel1.GetRow(button));

            Rectangle currentRect = new Rectangle(firstPicketButton.X, firstPicketButton.Y,
                                                          point.X - firstPicketButton.X, point.Y - firstPicketButton.Y);

            if (rectangleFlag)
            {
                secondPicketButton = point;
                rectangleFlag = false;
                FillRectangle(firstPicketButton, secondPicketButton, Color.Red);
                rectangles.Add(currentRect);
            }
            else
            {
                firstPicketButton = point;
                rectangleFlag = true;
            }
            Trace.WriteLine(tableLayoutPanel1.GetColumn(button).ToString() + tableLayoutPanel1.GetRow(button).ToString());
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
            RefrehsGrid();

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
    }
}
