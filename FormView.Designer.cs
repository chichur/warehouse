﻿namespace warehouse
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
            this.buttonInputPickets = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(444, 302);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonSelectPlatform
            // 
            this.buttonSelectPlatform.Location = new System.Drawing.Point(623, 188);
            this.buttonSelectPlatform.Name = "buttonSelectPlatform";
            this.buttonSelectPlatform.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectPlatform.TabIndex = 1;
            this.buttonSelectPlatform.Text = "button1";
            this.buttonSelectPlatform.UseVisualStyleBackColor = true;
            this.buttonSelectPlatform.Click += new System.EventHandler(this.ButtonSelectPlatform_Click);
            // 
            // buttonInputPickets
            // 
            this.buttonInputPickets.Location = new System.Drawing.Point(623, 258);
            this.buttonInputPickets.Name = "buttonInputPickets";
            this.buttonInputPickets.Size = new System.Drawing.Size(75, 23);
            this.buttonInputPickets.TabIndex = 2;
            this.buttonInputPickets.Text = "button1";
            this.buttonInputPickets.UseVisualStyleBackColor = true;
            this.buttonInputPickets.Click += new System.EventHandler(this.buttonInputPickets_Click);
            // 
            // FormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(761, 506);
            this.Controls.Add(this.buttonInputPickets);
            this.Controls.Add(this.buttonSelectPlatform);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormView";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonSelectPlatform;
        private System.Windows.Forms.Button buttonInputPickets;
    }
}

