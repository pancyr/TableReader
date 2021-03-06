using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableReader.Core;

namespace TableReader.Win
{
    public partial class MainForm : Form
    {
        private ProgressDialog formDialog;

        public BackgroundWorker PriceProcess
        {
            get
            {
                return this.bgPriceWorker;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void FillListOfFiles()
        {
            string sResultDirName = String.Format("{0}\\Result", Application.StartupPath);
            DirectoryInfo dirInfo = new DirectoryInfo(@sResultDirName);

            listView1.Items.Clear();
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                ListViewItem item = new ListViewItem(file.Name, 1);
                ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "Прайслист"),
                     new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString()),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortTimeString())};

                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.FillListOfFiles();
        }

        

        private void cmdLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.Filter = "Excel (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|Все файлы (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                //ParseSingleFile(openDialog.FileName);
                formDialog = new ProgressDialog(this);
                Object[] args = new Object[] { 1, openDialog.FileName };
                bgPriceWorker.RunWorkerAsync(args);
                formDialog.ResetParams();
                formDialog.ShowDialog();
            }
        }

        #region Функции потока обработки файлов

        private void bgPriceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int argType = Int32.Parse(((Object[])e.Argument)[0].ToString());
            /*TableReaderApp.SetProgressDelegate = 
                new TableParserBase.SetProgressValueHandler(bgPriceWorker.ReportProgress);*/
            TableReaderApp.WorkerArgs = e;
            switch (argType)
            {
                case 1: ParseSingleFile(((Object[])e.Argument)[1].ToString()); break;
                case 2: ParseDirectory(((Object[])e.Argument)[1].ToString()); break;
            }
        }

        private void bgPriceWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            formDialog.SetProgressParams(e.ProgressPercentage);
        }

        private void bgPriceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            formDialog.Close();
            FillListOfFiles();
        }

        private void ParseSingleFile(string fileName)
        {
            formDialog.CurrentBook = Book.Open(fileName);
            formDialog.PageNum = 0;
            foreach (string key in formDialog.CurrentBook.Pages.Keys)
            {
                TableParserBase tableParser = TableReaderApp.SelectParserObjectFromAvailableModules
                    (formDialog.CurrentBook.Pages[key], Application.StartupPath + "\\Modules");
                if (tableParser != null)
                {
                    tableParser.SetProgressValue += new TableParserBase.SetProgressValueHandler(bgPriceWorker.ReportProgress);
                    formDialog.CurrentPage = formDialog.CurrentBook.Pages[key];
                    formDialog.PageNum++;
                    string templateFile = (txtResultFile.Text.Length > 0) ? txtResultFile.Text : null;
                    ParsingArgs args = new ParsingArgs(templateFile,
                        Application.StartupPath + "\\Result", TableReaderApp.WorkerArgs, 500, 2);
                    tableParser.DoParsingWithDivisionVolumesOfIncomingPage
                        (formDialog.CurrentBook.Name, formDialog.CurrentBook.Pages[key], args);
                    
                }
            }
            formDialog.CurrentBook.Close();
        }

        private void ParseDirectory(string sDirName)
        {
            /*DirectoryInfo dirInfo = new DirectoryInfo(@sDirName);
            foreach (FileInfo file in dirInfo.GetFiles("*.xls"))
            {
                MakePriceFromFile(file.FullName);
                if (PriceResult.WorkerArgs.Cancel)
                    return;
            }
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                MakeAllPricesFromFolder(dir.FullName);
                if (PriceResult.WorkerArgs.Cancel)
                    return;
            }*/
        }

        public void CancelPocessing()
        {
            PriceProcess.CancelAsync();
            TableReaderApp.WorkerArgs.Cancel = true;
        }

        #endregion

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.Filter = "Excel (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|Все файлы (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
                txtResultFile.Text = openDialog.FileName;
        }
    }
}
