using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Office.Interop.Excel;

namespace TableReader.Core
{
    public class Book
    {
        public Book(Workbook _book)
        {
            this._excelBook = _book;
            this.Styles = _excelBook.Styles;
        }

        protected Workbook _excelBook; // рабочая книга
        public readonly Styles Styles;

        public string Name { get; set; }

        private Dictionary<string, Page> _pages;
        public Dictionary<string, Page> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new Dictionary<string, Page>();
                return _pages;
            }
        }

        public static Book Create(List<string> pageNames)
        {
            Workbook _workbook = TableReaderApp.GetExcelApp().Workbooks.Add(Type.Missing);
            int count = _workbook.Worksheets.Count;
            for (int i = count; i > 1; i--)
                ((Worksheet)_workbook.Worksheets[i]).Delete();

            ((Worksheet)_workbook.Worksheets[1]).Name = pageNames[0];
            Book result = new Book(_workbook);
            result.Pages.Add(pageNames[0], new Page((Worksheet)_workbook.Worksheets[1], _workbook.Styles));

            for (int i = 1; i < pageNames.Count; i++)
                result.AddNewPage(pageNames[i]);

            return result;
        }

        public static Book Open(string path)
        {
            Workbook _workbook = TableReaderApp.GetExcelApp().Workbooks.Open(path,
                Type.Missing, true, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            int pos = path.LastIndexOf("\\") + 1;
            Book result = new Book(_workbook);
            string fileName = path.Substring(pos, path.Length - pos);
            int pset = fileName.LastIndexOf(".");
            result.Name = fileName.Substring(0, pset);

            int count = _workbook.Worksheets.Count;
            for (int i = 1; i <= count; i++)
            {
                Worksheet sheet = (Worksheet)_workbook.Worksheets[i];
                result.Pages.Add(sheet.Name, new Page(sheet));
            }

            return result;
        }

        public Page AddNewPage(string pageName)
        {
            int totalSheets = _excelBook.Worksheets.Count;
            Worksheet sheet = (Worksheet)_excelBook.Worksheets.Add(Type.Missing, _excelBook.Worksheets[totalSheets], Type.Missing, Type.Missing);
            sheet.Name = pageName;
            Page result = new Page(sheet, Styles);
            Pages.Add(sheet.Name, result);
            return result;
        }

        public void Save(string savePath)
        {
            _excelBook.Sheets[1].Select();
            _excelBook.SaveAs(savePath, XlFileFormat.xlWorkbookNormal, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlShared,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        public void Close()
        {
            _excelBook.Close(false, Type.Missing, Type.Missing);
        }
    }
}
