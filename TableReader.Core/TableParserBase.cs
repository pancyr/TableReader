using System;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TableReader.Core
{
    public abstract class TableParserBase
    {
        public DoWorkEventArgs WorkerArgs { get; set; }

        public abstract Book CreateBookForResultData();

        protected abstract bool PositionToFirstRow(Page page);
        protected abstract bool GetFirstOrNextColumns(Page page, ref Dictionary<string, int> columnNumbers);
        protected abstract Dictionary<string, List<Dictionary<int, string>>> GatherValuesFromPage(Page page, Dictionary<string, int> columnNumbers);


        protected virtual Book CreateBookForResultData(Dictionary<string, Dictionary<int, string>> titlesOfPages)
        {
            List<string> keys = new List<string>();
            foreach (string key in titlesOfPages.Keys)
                keys.Add(key);
            Book resultBook = Book.Create(keys);
            foreach (string key in keys)
            {
                resultBook.Pages[key].CurrentRow = 1;
                resultBook.Pages[key].WriteLine(titlesOfPages[key], true);
            }
            return resultBook;
        }

        protected virtual bool WriteDataToBook(Book book, Dictionary<string, List<Dictionary<int, string>>> argsData)
        {
            foreach (string key in argsData.Keys)
            {
                Page page = book.Pages[key];
                List<Dictionary<int, string>> listOfRows = argsData[key];
                foreach (Dictionary<int, string> values in listOfRows)
                    WriteLineToPage(page, values);
            }
            return true;
        }

        protected virtual bool WriteLineToPage(Page page, Dictionary<int, string> values)
        {
            return page.WriteLine(values);
        }

        public virtual bool DoParsingOfIncomingPage(Page incomPage, Book resultBook, DoWorkEventArgs workerArgs)
        {
            Dictionary<string, int> columnNumbers = null;
            while (GetFirstOrNextColumns(incomPage, ref columnNumbers))
                if (PositionToFirstRow(incomPage))
                    do
                    {
                        if (workerArgs.Cancel) return false;
                        Dictionary<string, List<Dictionary<int, string>>> values = GatherValuesFromPage(incomPage, columnNumbers);
                        if (values != null)
                            WriteDataToBook(resultBook, values);
                        this.OnSetProgressValue(incomPage);
                        incomPage.CurrentRow++;
                    }
                    while (!incomPage.IsEndPosition());
            return true;
        }

        #region Реализация события для индикатора обработки

        public delegate void SetProgressValueHandler(int NewValue);
        public event SetProgressValueHandler SetProgressValue;

        protected void OnSetProgressValue(Page page)
        {
            if (SetProgressValue != null)
            {
                double row = page.CurrentRow;
                double allRows = page.TotalRows;
                double percent = row / allRows * 100;
                SetProgressValue((int)percent);
            }
        }

        #endregion

    }
}