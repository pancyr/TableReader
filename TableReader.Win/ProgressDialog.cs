using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableReader.Core;

namespace TableReader.Win
{
    public partial class ProgressDialog : Form
    {
        private MainForm formMain;

        public ProgressDialog(MainForm form)
        {
            InitializeComponent();
            this.formMain = form;
        }

        public Book CurrentBook { get; set; }
        public Page CurrentPage { get; set; }
        public int PageNum { get; set; }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.formMain.CancelPocessing();
        }

        public void ResetParams()
        {
            this.Text = "Обработка...";
            this.lblPageCount.Text = string.Empty;
            this.progressBar.Value = 0;
        }

        public void SetProgressParams(int currentRow)
        {
            this.Text = "Книга: " + CurrentBook.Name;
            this.lblPageCount.Text = String.Format("Лист {0} из {1}", PageNum, CurrentBook.Pages.Count);
            this.lblPagePos.Text = String.Format("Обработка строки {0} из {1}", currentRow, CurrentPage.TotalRows);
            double percent = (double)currentRow / CurrentPage.TotalRows * 100;
            progressBar.Value = (int)percent;
        }
    }
}
