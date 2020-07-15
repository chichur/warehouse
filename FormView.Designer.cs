namespace warehouse
{
    partial class FormView
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSelectPlatform = new System.Windows.Forms.Button();
            this.buttonTransferCargo = new System.Windows.Forms.Button();
            this.buttonReformingPlatform = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cargo_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.labelUntrackedCargo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 721F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(136, 40);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(419, 301);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonSelectPlatform
            // 
            this.buttonSelectPlatform.Location = new System.Drawing.Point(12, 40);
            this.buttonSelectPlatform.Name = "buttonSelectPlatform";
            this.buttonSelectPlatform.Size = new System.Drawing.Size(112, 53);
            this.buttonSelectPlatform.TabIndex = 1;
            this.buttonSelectPlatform.Text = "Cформировать склад";
            this.buttonSelectPlatform.UseVisualStyleBackColor = true;
            this.buttonSelectPlatform.Click += new System.EventHandler(this.ButtonSelectPlatform_Click);
            // 
            // buttonTransferCargo
            // 
            this.buttonTransferCargo.Location = new System.Drawing.Point(12, 263);
            this.buttonTransferCargo.Name = "buttonTransferCargo";
            this.buttonTransferCargo.Size = new System.Drawing.Size(112, 62);
            this.buttonTransferCargo.TabIndex = 3;
            this.buttonTransferCargo.Text = "Переместить груз";
            this.buttonTransferCargo.UseVisualStyleBackColor = true;
            this.buttonTransferCargo.Click += new System.EventHandler(this.buttonTransferCargo_Click);
            // 
            // buttonReformingPlatform
            // 
            this.buttonReformingPlatform.Location = new System.Drawing.Point(12, 145);
            this.buttonReformingPlatform.Name = "buttonReformingPlatform";
            this.buttonReformingPlatform.Size = new System.Drawing.Size(112, 55);
            this.buttonReformingPlatform.TabIndex = 4;
            this.buttonReformingPlatform.Text = "Расформировать площадки";
            this.buttonReformingPlatform.UseVisualStyleBackColor = true;
            this.buttonReformingPlatform.Click += new System.EventHandler(this.buttonReformingPlatform_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cargo_value,
            this.pickets});
            this.dataGridView1.Location = new System.Drawing.Point(574, 40);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(290, 301);
            this.dataGridView1.TabIndex = 5;
            // 
            // cargo_value
            // 
            this.cargo_value.HeaderText = "Груз, т";
            this.cargo_value.Name = "cargo_value";
            // 
            // pickets
            // 
            this.pickets.HeaderText = "Пикеты";
            this.pickets.Name = "pickets";
            this.pickets.Width = 300;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(169, 344);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Нераспределенный груз: ";
            // 
            // labelUntrackedCargo
            // 
            this.labelUntrackedCargo.AutoSize = true;
            this.labelUntrackedCargo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelUntrackedCargo.Location = new System.Drawing.Point(391, 344);
            this.labelUntrackedCargo.Name = "labelUntrackedCargo";
            this.labelUntrackedCargo.Size = new System.Drawing.Size(0, 20);
            this.labelUntrackedCargo.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(264, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Схема склада";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(684, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Площадки";
            // 
            // FormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(886, 371);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelUntrackedCargo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonReformingPlatform);
            this.Controls.Add(this.buttonTransferCargo);
            this.Controls.Add(this.buttonSelectPlatform);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormView";
            this.Text = "Учет склада";
            this.Shown += new System.EventHandler(this.FormView_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonSelectPlatform;
        private System.Windows.Forms.Button buttonTransferCargo;
        private System.Windows.Forms.Button buttonReformingPlatform;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cargo_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn pickets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelUntrackedCargo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

