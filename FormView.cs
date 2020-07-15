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
    // наследуем класс Form и интефейс IView
    public partial class FormView : Form, IView
    {
        #region реализация интерфейса
        // событие установки груза на площадку
        public event EventHandler<EventArgs> SetCargo;
        // событие разбиение платформы
        public event EventHandler<EventArgs> SetPlatforms;
        // событие удаления платформы
        public event EventHandler<EventArgs> DeletePlatforms;
        // событие получении истории (не используется)
        public event EventHandler<EventArgs> GetHistory;
        #endregion
        // флаг для рисования прямоугольника
        bool rectangleFlag = false;
        // перечисление режимов 
        ReformingStates States = ReformingStates.Nothing;
        int counterPlatform = 0;
        int untrackedCargo = 0;
        // переменная хранящая номер пикета площадки с которой перемещают груз
        int? firstPlatformPicket = null;
        // переменная для хранения платформ на представлении
        int?[][] platforms;
        // точки рисования прямоугольника
        Point firstPicketButton, secondPicketButton;
        // список прямоугольников
        List<Rectangle> rectangles = new List<Rectangle>();
        // список платформ к удалению
        List<int?[]> platformsToDelete = new List<int?[]>();
        // список платформ которым требуется указать груз
        List<int?[]> platformsToSetCargo = new List<int?[]>();
        // словарь для вывода значений грузов
        Dictionary<int?[], int?> cargo = new Dictionary<int?[], int?>();
        public FormView()
        {
            InitializeComponent();
            InitializeTableLayoutSettings();
        }

        // функция инициализации интерфейса
        private void InitializeTableLayoutSettings()
        {
            // задаем размерность сетки
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.ColumnCount = 4;
            
            // вычисляем в процентах размеры клеток чтобы были равномерные
            float percentWidth = 100 / tableLayoutPanel1.RowCount;
            float percentHeight = 100 / tableLayoutPanel1.ColumnCount;

            int counterPickets = 0;
            // Заполнение сетки
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                {
                    // Создание контрола кнопки в каждой ячейки с определенными параметрами
                    tableLayoutPanel1.RowStyles.Add(new RowStyle());
                    Button buttonPicket = new Button();
                    buttonPicket.FlatStyle = FlatStyle.Flat;
                    buttonPicket.Dock = DockStyle.Fill;
                    buttonPicket.Click += new EventHandler(ButtonPicketClick);
                    buttonPicket.MouseEnter += new EventHandler(OnMouseEnterButtonPicket);
                    buttonPicket.MouseLeave += new EventHandler(OnMouseLeaveButtonPicket);
                    buttonPicket.FlatAppearance.BorderSize = 2;

                    // Add the new ToolStripButton control to the GridStrip.

                    // добавляем в сетку
                    tableLayoutPanel1.Controls.Add(buttonPicket, i, j);
                    // If this is the ToolStripButton control at cell (0,0),
                    // assign it as the empty cell button.
                    counterPickets++;
                }
            }

            // создаем стили колонок и строк
            TableLayoutColumnStyleCollection columnStyle = tableLayoutPanel1.ColumnStyles;
            TableLayoutRowStyleCollection rowStyle = tableLayoutPanel1.RowStyles;

            // применяем к каждой колонке задаем размер
            foreach (ColumnStyle style in columnStyle)
            {
                style.SizeType = SizeType.Percent;
                style.Width = percentWidth;
            }

            // применяем к каждой строке задаем размер
            foreach (RowStyle style in rowStyle)
            {
                style.SizeType = SizeType.Percent;
                style.Height = percentHeight;
            }

            // отключаем сетку
            tableLayoutPanel1.Enabled = false;
        }

        // функция обновления сетки принимает аргумент перечисления режимов очистки
        // первый режим  OnlyEnable - сделать все кнопки активными
        // второй режим OnSelect - очистка при выделении прямоугольника площадки
        // третий режим Partially - очистка частичная при разбиении отдельных площадок
        // четверный режим Full - полная очистка
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
            int picketNumber = Int32.Parse(button.Text);
            Point point = new Point(tableLayoutPanel1.GetColumn(button), tableLayoutPanel1.GetRow(button));

            // текущий выделенный прямоугольник
            Rectangle currentRect = new Rectangle(firstPicketButton.X, firstPicketButton.Y,
                                                          point.X - firstPicketButton.X, point.Y - firstPicketButton.Y);

            // если режим выделения платформ
            if (States == ReformingStates.Select)
            {
                // перебрать площадки и выделить их кнопки бордером красным цветом
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

                // добавить площадки в переменную для удаления
                for (int i = 0; i < platforms.Length; i++)
                    for (int j = 0; j < platforms[i].Length; j++)
                        if (Int32.Parse(button.Text) == platforms[i][j])
                        {
                            platformsToDelete.Add(platforms[i]);
                            // оператор goto используется для выхода из вложенных циклов оператор break неуместен
                            goto exit;
                        }
                exit:;

                buttonReformingPlatform.Enabled = true;
            }
            else if(States == ReformingStates.SettingCargo)
            {
                // находим платформу по кнопке
                foreach (int?[] platform in platformsToSetCargo.ToList())
                    for (int i = 0; i < platform.Length; i++)
                        if (Int32.Parse(button.Text) == platform[i])
                        {
                            // отключаем все кнопки платформы
                            for (int j = 0; j < platform.Length; j++)
                            {
                                Control[] controls = tableLayoutPanel1.Controls.Find(platform[j].ToString(), false);
                                if (controls != null && controls.Length > 0)
                                {
                                    Button btnFind = controls[0] as Button;
                                    btnFind.Enabled = false;
                                }
                            }

                            platformsToSetCargo.Remove(platform);
                            // оператор goto используется для выхода из вложенных циклов оператор break неуместен
                            goto exit;
                        }
                    exit:;

                ShowModal(picketNumber);


                if (platformsToSetCargo.Count == 1 || untrackedCargo == 0)
                {
                    if (platformsToSetCargo.Count != 0)
                    {
                        int?[] pickets = platformsToSetCargo.First();
                        WithdrawCargo(untrackedCargo, pickets[0]);
                    }
                    untrackedCargo = 0;
                    platformsToSetCargo.Clear();
                    States = ReformingStates.Nothing;
                    buttonReformingPlatform.Enabled = true;
                    buttonSelectPlatform.Enabled = true;
                    buttonReformingPlatform.Enabled = true;
                    buttonTransferCargo.Enabled = true;
                    tableLayoutPanel1.Enabled = false;
                }
            }
            else if (States == ReformingStates.TransferCargo)
            {
                if (firstPlatformPicket != null)
                {
                    ShowModal(picketNumber);
                    firstPlatformPicket = null;
                }
                else
                    firstPlatformPicket = Int32.Parse(button.Text);
            }

            else
            {
                // блок кода отвечает за рисование прямоугольника на сетке
                if (rectangleFlag)
                {
                    // закончить отрисовку 
                    secondPicketButton = point;
                    rectangleFlag = false;
                    // проверяем на пересечение с другими прямоугольниками
                    foreach (Rectangle rec in rectangles)
                        if (intersect = IntersectsRectangles(currentRect, rec))
                            break;

                    if (!intersect)
                    {
                        // если нету пересечения заполняем цветом и добавляем в переменную значения координат
                        FillRectangle(firstPicketButton, point, GetRandomColor(counterPlatform++));
                        rectangles.Add(currentRect);

                        // если сетка заполнена
                        if (GridIsFilled())
                        {
                            // обновляем сетку
                            RefrehsGrid(RefrehType.OnSelect);
                            
                            // выводим модальное окно
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
                                // преобразуем прямоугольники в площадки
                                platforms = ConvertRecToPlatforms();
                                // добавляем все платформы в список для задания груза
                                foreach (int?[] platform in platforms)
                                    platformsToSetCargo.Add(platform);
                                // вызываем событие установки платформ,
                                SetPlatforms(this, EventArgs.Empty);
                                // переключаем на режим размещения грузов
                                States = ReformingStates.SettingCargo;
                                buttonTransferCargo.Text = "Выберите площадки";
                                // блокировка кнопок
                                buttonReformingPlatform.Enabled = false;
                                buttonSelectPlatform.Enabled = false;
                                buttonReformingPlatform.Enabled = false;
                                buttonTransferCargo.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    // начало рисования прямоугольника
                    firstPicketButton = point;
                    rectangleFlag = true;
                }
            }
            
        }

        // функция раскраски прямоугольника
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

        // обработчик вхождения курсора в кпопку пикета
        private void OnMouseEnterButtonPicket(object sender, EventArgs e)
        {
            bool intersect = false;
            Button button = (Button)sender;
            Point point = new Point(tableLayoutPanel1.GetColumn(button), tableLayoutPanel1.GetRow(button));


            Rectangle currentRect = new Rectangle(firstPicketButton.X, firstPicketButton.Y,
                                point.X - firstPicketButton.X, point.Y - firstPicketButton.Y);

            // если режим разбиения площадок то при выделении ширина бордера увеличивается
            if (States == ReformingStates.Select ||
                States == ReformingStates.SettingCargo ||
                States == ReformingStates.TransferCargo)
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
                    RefrehsGrid(RefrehType.Partially); // при разбиении частичная очистка
                else
                    RefrehsGrid(RefrehType.OnSelect); // обычная очистка

                // проверка на пересечения с прямоугольниками
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

        // обработчик события выхода курсова из кнопки пикета
        private void OnMouseLeaveButtonPicket(object sender, EventArgs e)
        {
            // если режим разбиения то возращаем прежнюю толщину границы кнопки
            if (States == ReformingStates.Select ||
                States == ReformingStates.SettingCargo ||
                States == ReformingStates.TransferCargo)
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

        // функция проверки пересечения прямоугольников
        private bool IntersectsRectangles(Rectangle a, Rectangle b)
        {
            return (a.Y <= b.Y + b.Height && a.Y + a.Height >= b.Y &&
                a.X + a.Width >= b.X && a.X <= b.X + b.Width);
        }

        // обработчик нажатия кнопки формирования склада
        private void ButtonSelectPlatform_Click(object sender, EventArgs e)
        {
            States = ReformingStates.Nothing;
            buttonReformingPlatform.Enabled = false;
            buttonTransferCargo.Enabled = false;
            ShowModal();
            StartSelectPlatforms();
        }

        // Функция получения случайного цвета, в приложении это функция работает с последовательностью
        // от 1 до n
        private Color GetRandomColor(int seed)
        {
            Random randomGen = new Random(seed);
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            return Color.FromKnownColor(randomColorName);
        }

        // функция проверки заполненности склада площадками
        private bool GridIsFilled()
        {
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                    if (tableLayoutPanel1.GetControlFromPosition(i, j).Tag == null)
                        return false;

            return true;
        }

        // процедура старта формирования склада 
        private void StartSelectPlatforms()
        {
            rectangleFlag = false;
            counterPlatform = 0;
            tableLayoutPanel1.Enabled = true;
            DeletePlatforms(this, EventArgs.Empty); // удаления платформ
            rectangles.Clear();
            RefrehsGrid(RefrehType.Full); // полная очистка сетки
        }

        // преобразование прямоугольников в площадки
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

        #region реализация интерфейса IView
        // определения свойства которое только для записи, записи последовательности пикетов с базы даннных
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

        // свойство для записи и чтения площадок
        public int?[][] Platforms
        {
            get 
            {
                return platforms;
            }
            set
            {
                // обновление представления 
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

        // свойство для чтения и записи
        // при чтении ключ словаря это массив с одним номером площадки, и значение груза площадки
        // при записи это массив всех пикетов площадки, и значение груза площадки
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
                    // инициилизируем строку
                    DataGridViewRow dataGridViewRow = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    DataGridViewCellStyle style = new DataGridViewCellStyle();

                    // определяем цвет платформы по номеру пикету площадки
                    for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                        for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                            if (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(i, j).Text) == pickets[0])
                                style.BackColor = tableLayoutPanel1.GetControlFromPosition(i, j).BackColor;

                    // применяем стиль строки (закрышиваем цветом)
                    dataGridViewRow.DefaultCellStyle = style;

                    // записываем значения в клетки
                    dataGridViewRow.Cells[0].Value = value[pickets].ToString(); // значение груза
                    dataGridViewRow.Cells[1].Value = string.Join(",", pickets); // список пикетов

                    // добавляем строку в таблицу
                    dataGridView1.Rows.Add(dataGridViewRow);
                }

                // убрать выделение верхней левой клетки
                DataGridViewSelectionMode oldmode = dataGridView1.SelectionMode;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionMode = oldmode;

                labelUntrackedCargo.Text = untrackedCargo.ToString();
                cargo = value;
            }
        }
        #endregion


        private void buttonTransferCargo_Click(object sender, EventArgs e)
        {
            if (States == ReformingStates.TransferCargo)
            {
                States = ReformingStates.Nothing;
                buttonTransferCargo.Text = "Переместить груз";
                buttonReformingPlatform.Enabled = true;
                buttonSelectPlatform.Enabled = true;
                tableLayoutPanel1.Enabled = false;
            }
            else if (States == ReformingStates.Nothing)
            {
                States = ReformingStates.TransferCargo;
                RefrehsGrid(RefrehType.OnlyEnable);
                cargo.Clear();
                firstPlatformPicket = null;
                buttonTransferCargo.Text = "Выберите площадки";
                buttonReformingPlatform.Enabled = false;
                buttonSelectPlatform.Enabled = false;
                tableLayoutPanel1.Enabled = true;
            }

        }

        // обработчик кнопки "Расформировать платформы"
        private void buttonReformingPlatform_Click(object sender, EventArgs e)
        {
            if(States == ReformingStates.Select)
            {
                RefrehsGrid(RefrehType.Partially); // частичная очистка сетки
                platforms = platformsToDelete.ToArray(); // удаление площадки
                DeletePlatformsRowsGrid(platformsToDelete);
                DeletePlatforms(this, EventArgs.Empty); // вызов события для представителя (Presenter)
                rectangles.Clear();
                platformsToDelete.Clear();
                States = ReformingStates.Nothing; // вернуть в режим ожидания
                buttonReformingPlatform.Text = "Расформировать платформы";
            }
            else if(States == ReformingStates.Nothing)
            {
                RefrehsGrid(RefrehType.OnlyEnable);
                States = ReformingStates.Select;
                buttonReformingPlatform.Text = "Выбрать платформы";
                tableLayoutPanel1.Enabled = true;
                buttonTransferCargo.Enabled = false;
                buttonSelectPlatform.Enabled = false;
                buttonReformingPlatform.Enabled = false;
            }
        }

        private void FormView_Shown(object sender, EventArgs e)
        {
            // убрать выделение ячейки
            dataGridView1.ClearSelection();
        }

        // функция для расчет количества груза которое требуется распределить между площадками
        private void WithdrawCargo(int cargoValue, int? picketNumber, bool transfer=false)
        {
            int?[] pickets = new int?[1];
            pickets[0] = picketNumber;
            int?[] picketsFirstPlatform = new int?[1];
            int?[] picketsSecondPlatform = new int?[1];

            if (transfer)
            {
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    int platformCargo = Int32.Parse(dataGridView1[0, i].FormattedValue.ToString());

                    if (dataGridView1[1, i].FormattedValue.ToString().Contains(firstPlatformPicket.ToString()) &&
                        dataGridView1[1, i].FormattedValue.ToString().Contains(picketNumber.ToString()))
                    {
                        picketsFirstPlatform[0] = firstPlatformPicket;
                        cargo.Add(picketsFirstPlatform, platformCargo);
                        break;
                    }
                    else if (dataGridView1[1, i].FormattedValue.ToString().Contains(firstPlatformPicket.ToString()))
                    {
                        if (cargoValue > platformCargo)
                        {
                            MessageBox.Show("Значени груза больше, чем на площадке",
                                            "Сообщение",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error,
                                            MessageBoxDefaultButton.Button1,
                                            MessageBoxOptions.DefaultDesktopOnly);
                            firstPlatformPicket = null;
                            break;
                        }
                        else
                        {
                            picketsFirstPlatform[0] = firstPlatformPicket;
                            cargo.Add(picketsFirstPlatform, platformCargo - cargoValue);
                        }
                    }
                    else if(dataGridView1[1, i].FormattedValue.ToString().Contains(picketNumber.ToString()))
                    {
                        picketsSecondPlatform[0] = picketNumber;
                        cargo.Add(picketsSecondPlatform, platformCargo + cargoValue);
                    }
                }
            }
            else
                if (cargoValue > untrackedCargo)
                {
                    cargo.Add(pickets, untrackedCargo);
                    untrackedCargo = 0;
                }
                else
                {
                    cargo.Add(pickets, cargoValue);
                    untrackedCargo -= cargoValue;
                }
            
            labelUntrackedCargo.Text = untrackedCargo.ToString();
            SetCargo(this, EventArgs.Empty);
        }

        // функция удаления платформ с таблицы отображения платформ
        private void DeletePlatformsRowsGrid(List<int?[]> platforms)
        {
            // список в который мы заносим индексы строк для удаления
            List<DataGridViewRow> list_rows = new List<DataGridViewRow>();

            // проходим по всем площадкам запоминаем индексы, и прибавляем к грузу значение удаленной
            // площадки
            foreach (int?[] platform in platforms)
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1[1, i].FormattedValue.ToString().Contains(string.Join(",", platform)))
                    {
                        list_rows.Add(dataGridView1.Rows[i]);
                        untrackedCargo += Int32.Parse(dataGridView1[0, i].FormattedValue.ToString());
                    }
                }

            // удаляем строки
            foreach (DataGridViewRow row in list_rows)
                dataGridView1.Rows.Remove(row);

            dataGridView1.ClearSelection();
            labelUntrackedCargo.Text = untrackedCargo.ToString();
        }

        // функция вызова модальных окон
        public void ShowModal(int picketNumber=0)
        {
            if (States == ReformingStates.Nothing)
            {
                // если режим ожидания вывести окно ввода пикета
                InputPickectsModalForm Dialog = new InputPickectsModalForm();

                // если нажата кнопка ок тогда считать значение 
                if (Dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // если не был введен номер первого пикета то принимаем "1"
                    int picket = Int32.TryParse(Dialog.textBox1.Text, out int res1) ? res1 : 1;
                    int?[] pickets = new int?[tableLayoutPanel1.Controls.Count];
                    for (int i = 0; i < pickets.Length; picket++, i++)
                        pickets[i] = picket;

                    this.InputPickets = pickets;

                    // если не был введен нераспределенный груз то принимаем "0"
                    untrackedCargo = Int32.TryParse(Dialog.textBox2.Text, out int res2) ? res2 : 0;
                }

                Dialog.Dispose();
            }
            else if (States == ReformingStates.SettingCargo)
            {
                // если ввода грузов
                InputCargoModalForm Dialog = new InputCargoModalForm();

                if (Dialog.ShowDialog(this) == DialogResult.OK)
                {
                    int cargo = Int32.TryParse(Dialog.textBox1.Text, out int res) ? res : 0;
                    WithdrawCargo(cargo, picketNumber);
                }

                Dialog.Dispose();
            }
            else if (States == ReformingStates.TransferCargo)
            {
                InputCargoModalForm Dialog = new InputCargoModalForm();

                if (Dialog.ShowDialog(this) == DialogResult.OK)
                {
                    int cargo = Int32.TryParse(Dialog.textBox1.Text, out int res) ? res : 0;
                    WithdrawCargo(cargo, picketNumber, true);
                }
            }
        }
    }
}
