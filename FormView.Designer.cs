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
            this.buttonSetCargo = new System.Windows.Forms.Button();
            this.buttonReformingPlatform = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.platformId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cargoCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(130, 142);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(416, 301);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonSelectPlatform
            // 
            this.buttonSelectPlatform.Location = new System.Drawing.Point(12, 142);
            this.buttonSelectPlatform.Name = "buttonSelectPlatform";
            this.buttonSelectPlatform.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectPlatform.TabIndex = 1;
            this.buttonSelectPlatform.Text = "button1";
            this.buttonSelectPlatform.UseVisualStyleBackColor = true;
            this.buttonSelectPlatform.Click += new System.EventHandler(this.ButtonSelectPlatform_Click);
            // 
            // buttonSetCargo
            // 
            this.buttonSetCargo.Location = new System.Drawing.Point(12, 365);
            this.buttonSetCargo.Name = "buttonSetCargo";
            this.buttonSetCargo.Size = new System.Drawing.Size(75, 23);
            this.buttonSetCargo.TabIndex = 3;
            this.buttonSetCargo.Text = "Очистка склада";
            this.buttonSetCargo.UseVisualStyleBackColor = true;
            this.buttonSetCargo.Click += new System.EventHandler(this.buttonSetCargo_Click);
            // 
            // buttonReformingPlatform
            // 
            this.buttonReformingPlatform.Location = new System.Drawing.Point(12, 271);
            this.buttonReformingPlatform.Name = "buttonReformingPlatform";
            this.buttonReformingPlatform.Size = new System.Drawing.Size(75, 23);
            this.buttonReformingPlatform.TabIndex = 4;
            this.buttonReformingPlatform.Text = "buttonReformingPlatform";
            this.buttonReformingPlatform.UseVisualStyleBackColor = true;
            this.buttonReformingPlatform.Click += new System.EventHandler(this.buttonReformingPlatform_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.platformId,
            this.cargoCol});
            this.dataGridView1.Location = new System.Drawing.Point(605, 142);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(290, 144);
            this.dataGridView1.TabIndex = 5;
            // 
            // platformId
            // 
            this.platformId.HeaderText = "ID";
            this.platformId.Name = "platformId";
            // 
            // cargoCol
            // 
            this.cargoCol.HeaderText = "Груз";
            this.cargoCol.Name = "cargoCol";
            // 
            // FormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1140, 616);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonReformingPlatform);
            this.Controls.Add(this.buttonSetCargo);
            this.Controls.Add(this.buttonSelectPlatform);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormView";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.FormView_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonSelectPlatform;
        private System.Windows.Forms.Button buttonSetCargo;
        private System.Windows.Forms.Button buttonReformingPlatform;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn platformId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cargoCol;
    }
}

