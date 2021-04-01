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

        public abstract Book CreateBookForResultData(string filePath = null);

        protected abstract bool PositionToFirstRow(Page page);
        protected abstract bool GetFirstOrNextColumns(Page page, ref Dictionary<string, int> columnNumbers);
        protected abstract Dictionary<string, List<Dictionary<int, string>>> GatherValuesFromPage(Page page, Dictionary<string, int> columnNumbers);


        protected virtual Book CreateBookForResultData(Dictionary<string, Dictionary<int, string>> titlesOfPages, string filePath)
        {
            List<string> keys = new List<string>();
            foreach (string key in titlesOfPages.Keys)
                keys.Add(key);

            Book resultBook;

            if (filePath == null)
            {
                resultBook = Book.Create(keys);
                foreach (string key in keys)
                {
                    resultBook.Pages[key].CurrentRow = 1;
                    resultBook.Pages[key].WriteLine(titlesOfPages[key], true);
                }
            }
            else
            {
                resultBook = Book.Open(filePath);
                foreach (string key in keys)
                {
                    if (resultBook.HasPage(key))
                    {
                        resultBook.Pages[key].Measure();
                        resultBook.Pages[key].CurrentRow = resultBook.Pages[key].TotalRows + 1;
                    }
                    else
                    {
                        resultBook.AddNewPage(key);
                        resultBook.Pages[key].CurrentRow = 1;
                        resultBook.Pages[key].WriteLine(titlesOfPages[key], true);
                    }
                }
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

        public virtual bool DoParsingOfIncomingPage(string bookName, Page incomPage, ParsingArgs args)
        {
            Dictionary<string, int> columnNumbers = null;
            Book resultBook = CreateBookForResultData(args.TemplateFile);
            while (GetFirstOrNextColumns(incomPage, ref columnNumbers))
                if (PositionToFirstRow(incomPage))
                    do
                    {
                        if (args.WorkerArgs.Cancel) return false;
                        Dictionary<string, List<Dictionary<int, string>>> values = GatherValuesFromPage(incomPage, columnNumbers);
                        if (values != null)
                            WriteDataToBook(resultBook, values);
                        this.OnSetProgressValue(incomPage);
                        incomPage.CurrentRow++;
                    }
                    while (!incomPage.IsEndPosition());
            if (!args.WorkerArgs.Cancel)
                SaveCurrentBook(resultBook, args.ResultPath, bookName, incomPage.Name);

            return true;
        }

        public virtual bool DoParsingWithDivisionVolumesOfIncomingPage(string bookName, Page incomPage, ParsingArgs args)
        {
            int tovarPos = 1;
            int volumeNum = 1;
            Dictionary<string, int> columnNumbers = null;
            Book resultBook = CreateBookForResultData(args.TemplateFile);
            while (GetFirstOrNextColumns(incomPage, ref columnNumbers))
                if (PositionToFirstRow(incomPage))
                    do
                    {
                        if (args.WorkerArgs.Cancel) return false;
                        Dictionary<string, List<Dictionary<int, string>>> values = GatherValuesFromPage(incomPage, columnNumbers);
                        if (values != null)
                            WriteDataToBook(resultBook, values);
                        this.OnSetProgressValue(incomPage);
                        incomPage.CurrentRow++;

                        if (tovarPos == args.VolumeSize)
                        {
                            SaveCurrentBook(resultBook, args.ResultPath, bookName, incomPage.Name, volumeNum++, args.DigitNum);
                            resultBook = CreateBookForResultData(args.TemplateFile);
                            tovarPos = 1;
                        }
                        else tovarPos++;
                    }
                    while (!incomPage.IsEndPosition());
            if (!args.WorkerArgs.Cancel)
            {
                if (tovarPos > 1)
                    SaveCurrentBook(resultBook, args.ResultPath, bookName, incomPage.Name, volumeNum, args.DigitNum);
                else
                    resultBook.Close();
            }
            
            return true;
        }

        private void SaveCurrentBook(Book book, string basePath, string bookName, string pageName, int num = 0, int digit = 3)
        {
            foreach (string key in book.Pages.Keys)
                book.Pages[key].AutoFitColumns();

            if (!basePath.EndsWith("\\"))
                basePath += "\\";
            string format = num > 0 ? "{0}{1}-{2}-{3:d" + digit + "}.xlsx" : "{0}{1}-{2}.xlsx";
            string filePath = String.Format(format, basePath, bookName, pageName, num);

            if (File.Exists(filePath))
                File.Delete(filePath);
            book.Save(filePath);
            book.Close();
        }

        #region Реализация события для индикатора обработки

        public delegate void SetProgressValueHandler(int NewValue);
        public event SetProgressValueHandler SetProgressValue;

        protected void OnSetProgressValue(Page page)
        {
            if (SetProgressValue != null)
                SetProgressValue(page.CurrentRow);
        }

        #endregion

    }
}