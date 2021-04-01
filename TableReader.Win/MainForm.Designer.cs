
namespace TableReader.Win
{
    partial class MainForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastModifiedDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastModifiedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bgPriceWorker = new System.ComponentModel.BackgroundWorker();
            this.cmdLoadFile = new System.Windows.Forms.Button();
            this.txtResultFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileName,
            this.Type,
            this.LastModifiedDate,
            this.LastModifiedTime});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 45);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(578, 393);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // FileName
            // 
            this.FileName.Text = "Имя";
            // 
            // Type
            // 
            this.Type.Text = "Тип";
            // 
            // LastModifiedDate
            // 
            this.LastModifiedDate.Text = "Дата";
            // 
            // LastModifiedTime
            // 
            this.LastModifiedTime.Text = "Время";
            // 
            // bgPriceWorker
            // 
            this.bgPriceWorker.WorkerReportsProgress = true;
            this.bgPriceWorker.WorkerSupportsCancellation = true;
            this.bgPriceWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgPriceWorker_DoWork);
            this.bgPriceWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgPriceWorker_ProgressChanged);
            this.bgPriceWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgPriceWorker_RunWorkerCompleted);
            // 
            // cmdLoadFile
            // 
            this.cmdLoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLoadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdLoadFile.Location = new System.Drawing.Point(596, 12);
            this.cmdLoadFile.Name = "cmdLoadFile";
            this.cmdLoadFile.Size = new System.Drawing.Size(192, 32);
            this.cmdLoadFile.TabIndex = 11;
            this.cmdLoadFile.Text = "Загрузить файл";
            this.cmdLoadFile.UseVisualStyleBackColor = true;
            this.cmdLoadFile.Click += new System.EventHandler(this.cmdLoadFile_Click);
            // 
            // txtResultFile
            // 
            this.txtResultFile.Location = new System.Drawing.Point(101, 12);
            this.txtResultFile.Name = "txtResultFile";
            this.txtResultFile.ReadOnly = true;
            this.txtResultFile.Size = new System.Drawing.Size(444, 20);
            this.txtResultFile.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Выходной файл";
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdBrowse.Location = new System.Drawing.Point(551, 7);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(39, 32);
            this.cmdBrowse.TabIndex = 14;
            this.cmdBrowse.Text = "...";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResultFile);
            this.Controls.Add(this.cmdLoadFile);
            this.Controls.Add(this.listView1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader LastModifiedDate;
        private System.Windows.Forms.ColumnHeader LastModifiedTime;
        private System.ComponentModel.BackgroundWorker bgPriceWorker;
        private System.Windows.Forms.Button cmdLoadFile;
        private System.Windows.Forms.TextBox txtResultFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdBrowse;
    }
}

