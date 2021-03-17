using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public void SetSheetCaption(int currentSheet, int totalSheet)
        {
            this.lblPageCount.Text = String.Format("Лист {0} из {1}", currentSheet, totalSheet);
        }

        public void SetWorkBookTitle(string wBookName)
        {
            this.Text = "Книга: " + wBookName;
        }
    }
}
