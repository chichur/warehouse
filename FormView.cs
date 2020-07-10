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
    public partial class FormView : Form, IView
    {
        public event EventHandler<EventArgs> SetCargo;
        public event EventHandler<EventArgs> SetPlatforms;
        public event EventHandler<EventArgs> DeletePlatforms;
        public event EventHandler<EventArgs> GetHistory;
        bool rectangleFlag = false;
        ReformingStates States = ReformingStates.Nothing;
        int counterPlatform = 0;
        int?[][] platforms;
        Point firstPicketButton, secondPicketButton;
        List<Rectangle> rectangles = new List<Rectangle>();
        List<int?[]> platformsToDelete = new List<int?[]>();
        Dictionary<int?[], int?> cargo = new Dictionary<int?[], int?>();
        public FormView()
        {
            InitializeComponent();
            InitializeTableLayoutSettings();
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
                    buttonPicket.MouseLeave += new EventHandler(OnMouseLeaveButtonPicket);
                    buttonPicket.FlatAppearance.BorderSize = 2;

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
            {
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                {
                    Button button = tableLayoutPanel1.GetControlFromPosition(i, j) as Button;

                    if (type == RefrehType.OnlyEnable)
                    {
                        button.Enabled = true;
                    }

                    if (type == RefrehType.OnSelect)
                        if (button.Tag == null)
                            button.BackColor = Color.White;
                        if (button.FlatAppearance.BorderColor == Color.Red)
                            button.FlatAppearance.BorderColor = Color.Black;
                    if (type == RefrehType.Full)
                    {
                        button.BackColor = Color.White;
                        button.Tag = null;
                        button.Enabled = true;
                    }
                    if (type == RefrehType.Partially)
                    {
                        foreach (int?[] platform in platformsToDelete)
                            for (int k = 0; k < platform.Length; k++)
                                if (Int32.Parse(button.Text) == platform[k])
                                {
                                    button.BackColor = Color.White;
                                    button.Tag = null;
                                    goto exit;
                                }

                        button.Enabled = false;
                    }
                    exit: continue;
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

            if (States == ReformingStates.Select)
            {
                for (int i = 0; i < platforms.Count(); i++)
                    for (int j = 0; j < platforms[i].Count(); j++)
                        if (button.Text == platforms[i][j].ToString())
                        {
                            for (int k = 0; k < platforms[i].Count(); k++)
                            {
                                Control[] controls = tableLayoutPanel1.Controls.Find(platforms[i][k].ToString(), false);
                                if (controls != null && controls.Length > 0)
                                {
                                    Button btnFind = controls[0] as Button;
                                    btnFind.FlatAppearance.BorderColor = Color.Red;
                                }
                            }

                        }

                for (int i = 0; i < platforms.Length; i++)
                    for (int j = 0; j < platforms[i].Length; j++)
                        if (Int32.Parse(button.Text) == platforms[i][j])
                        {
                            platformsToDelete.Add(platforms[i]);
                            goto exit;
                        }
                exit:;
            }
            else if(States == ReformingStates.SettingCargo)
            {
                ShowModal(Int32.Parse(button.Text));
            }    
            else
            {
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
                            RefrehsGrid(RefrehType.OnSelect);
                            RefrehsGrid(RefrehType.OnlyEnable);
                            

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
                                platforms = ConvertRecToPlatforms();
                                SetPlatforms(this, EventArgs.Empty);
                                tableLayoutPanel1.Enabled = false;
                                buttonReformingPlatform.Enabled = true;
                                buttonSetCargo.Enabled = true;
                                buttonSelectPlatform.Enabled = true;
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
            

            Rectangle currentRect = new Rectangle(firstPicketButton.X, firstPicketButton.Y,
                                point.X - firstPicketButton.X, point.Y - firstPicketButton.Y);

            if (States == ReformingStates.Select)
            {
                for (int i = 0; i < platforms.Count(); i++)
                    for (int j = 0; j < platforms[i].Count(); j++)
                        if(button.Text == platforms[i][j].ToString())
                        {
                            for (int k = 0; k < platforms[i].Count(); k++)
                            {
                                Control[] controls = tableLayoutPanel1.Controls.Find(platforms[i][k].ToString(), false);
                                if (controls != null && controls.Length > 0)
                                {
                                    Button btnFind = controls[0] as Button;
                                    btnFind.FlatAppearance.BorderSize = 3;
                                }
                            }
                            goto exit;
                        }
                exit:;
            }
            else
            {
                if (States == ReformingStates.Select)
                    RefrehsGrid(RefrehType.Partially);
                else
                    RefrehsGrid(RefrehType.OnSelect);

                if (rectangleFlag)
                {
                    foreach (Rectangle rec in rectangles)
                        if (intersect = IntersectsRectangles(currentRect, rec))
                            break;

                    if (!intersect)
                        FillRectangle(firstPicketButton, point, Color.LimeGreen);
                }
            }
        }

        private void OnMouseLeaveButtonPicket(object sender, EventArgs e)
        {
            if (States == ReformingStates.Select)
            {
                Button btn = sender as Button;

                for (int i = 0; i < platforms.Count(); i++)
                    for (int j = 0; j < platforms[i].Count(); j++)
                        if (btn.Text == platforms[i][j].ToString())
                        {
                            for (int k = 0; k < platforms[i].Count(); k++)
                            {
                                Control[] controls = tableLayoutPanel1.Controls.Find(platforms[i][k].ToString(), false);
                                if (controls != null && controls.Length > 0)
                                {
                                    Button btnFind = controls[0] as Button;
                                    btnFind.FlatAppearance.BorderSize = 2;
                                }
                            }

                        }
            }
        }

        private bool IntersectsRectangles(Rectangle a, Rectangle b)
        {
            return (a.Y <= b.Y + b.Height && a.Y + a.Height >= b.Y &&
                a.X + a.Width >= b.X && a.X <= b.X + b.Width);
        }

        private void ButtonSelectPlatform_Click(object sender, EventArgs e)
        {
            States = ReformingStates.Nothing;
            buttonReformingPlatform.Enabled = false;
            buttonSetCargo.Enabled = false;
            ShowModal();
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
            DeletePlatforms(this, EventArgs.Empty);
            rectangles.Clear();
            RefrehsGrid(RefrehType.Full);
        }

        private int?[][] ConvertRecToPlatforms()
        {
            int?[][] platformsArr = new int?[rectangles.Count][];
            for (int i = 0; i < rectangles.Count; i++)
            {
                int counter = 0;
                platformsArr[i] = new int?[(rectangles[i].Width + 1) * (rectangles[i].Height + 1)];
                for (int j = rectangles[i].X; j < rectangles[i].X + rectangles[i].Width + 1; j++)
                    for (int k = rectangles[i].Y; k < rectangles[i].Y + rectangles[i].Height + 1; k++)
                    {
                        if (Int32.TryParse(tableLayoutPanel1.GetControlFromPosition(j, k).Text, out int x))
                            platformsArr[i][counter] = x;
                        counter++;
                    }
            }

            return platformsArr;
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
                        tableLayoutPanel1.GetControlFromPosition(j, i).Name = value[k].ToString();
                        k++;
                    }    
            }
        }

        

        public int?[][] Platforms
        {
            get 
            {
                return platforms;
            }
            set
            {
                for (int i = 0; i < value.Length; i++)
                    for (int j = 0; j < value[i].Length; j++)
                    {
                        Control[] controls = tableLayoutPanel1.Controls.Find(value[i][j].ToString(), false);
                        if (controls != null && controls.Length > 0)
                        {
                            Button btn = controls[0] as Button;
                            btn.BackColor = GetRandomColor(i);
                            btn.Tag = 1;
                        }      
                    }

                platforms = value;
            }
        }

        public Dictionary<int?[], int?> Cargo
        {
            get
            {
                return cargo;
            }
            set
            {
                
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                
                // заполнить таблицу грузов
                foreach (int?[] pickets in value.Keys)
                {
                    string[] row = { string.Join(",", pickets), value[pickets].ToString() };

                    DataGridViewRow dataGridViewRow = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    DataGridViewCellStyle style = new DataGridViewCellStyle();


                    for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                        for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                            if (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(i, j).Text) == pickets[0])
                                style.BackColor = tableLayoutPanel1.GetControlFromPosition(i, j).BackColor;

                    //style.BackColor = Color.Green; // the color change
                    dataGridViewRow.DefaultCellStyle = style;
                    dataGridViewRow.Cells[0].Value = value[pickets].ToString();
                    dataGridViewRow.Cells[1].Value = string.Join(",", pickets);
                    dataGridView1.Rows.Add(dataGridViewRow);

                    
                    //dataGridView1.Rows.Add(;
                }

                DataGridViewSelectionMode oldmode = dataGridView1.SelectionMode;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionMode = oldmode;

            }
        }

        private void buttonInputPickets_Click(object sender, EventArgs e)
        {
            ShowModal();
        }

        private void buttonSetCargo_Click(object sender, EventArgs e)
        {
            if (States == ReformingStates.SettingCargo)
            {
                States = ReformingStates.Nothing;
                buttonSetCargo.Text = "Задать груз";
                buttonReformingPlatform.Enabled = true;
                buttonSelectPlatform.Enabled = true;
                tableLayoutPanel1.Enabled = false;
            }
            else if (States == ReformingStates.Nothing)
            {
                States = ReformingStates.SettingCargo;
                buttonSetCargo.Text = "Выберите площадки";
                buttonReformingPlatform.Enabled = false;
                buttonSelectPlatform.Enabled = false;
                tableLayoutPanel1.Enabled = true;
            }

        }

        private void buttonReformingPlatform_Click(object sender, EventArgs e)
        {
            if(States == ReformingStates.Select)
            {
                RefrehsGrid(RefrehType.Partially);
                platforms = platformsToDelete.ToArray();
                DeletePlatforms(this, EventArgs.Empty);
                rectangles.Clear();
                platformsToDelete.Clear();
                States = ReformingStates.Nothing;
                buttonReformingPlatform.Text = "Расформировать платформы";
                buttonReformingPlatform.Enabled = false;
                buttonSetCargo.Enabled = false;
                buttonSelectPlatform.Enabled = false;
            }
            else if(States == ReformingStates.Nothing)
            {
                States = ReformingStates.Select;
                buttonReformingPlatform.Text = "Выбрать платформы";
                tableLayoutPanel1.Enabled = true;
            }
        }

        private void FormView_Shown(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        public void ShowModal(int picketNumber=0)
        {
            if (States == ReformingStates.Nothing)
            {
                InputPickectsModalForm Dialog = new InputPickectsModalForm();

                // 
                if (Dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // 
                    int picket = Int32.Parse(Dialog.textBox1.Text);
                    int?[] pickets = new int?[tableLayoutPanel1.Controls.Count];
                    for (int i = 0; i < pickets.Length; picket++, i++)
                        pickets[i] = picket;

                    this.InputPickets = pickets;
                }

                Dialog.Dispose();
            }
            else if (States == ReformingStates.SettingCargo)
            {
                InputCargoModalForm Dialog = new InputCargoModalForm();

                // 
                if (Dialog.ShowDialog(this) == DialogResult.OK)
                {
                    int?[] pickets = new int?[1];
                    pickets[0] = picketNumber;
                    cargo.Add(pickets, Int32.Parse(Dialog.textBox1.Text));
                    SetCargo(this, EventArgs.Empty);
                }

                Dialog.Dispose();
            }
            
        }

    }
}
