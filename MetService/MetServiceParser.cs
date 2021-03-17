using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TableReader.Core;

namespace MetService
{
    public sealed class MetServiceParser : TableParserBase
    {
        private const string RESULT_PAGE_NAME = "Result";

        private const string NAME_COLUMN = "Name";
        private const string OPTIONS_COLUMN = "Options";
        private const string UNIT_COLUMN = "Unit";
        private const string PRICE_COLUMN = "Price";

        private string groupTitle;

        public override Book CreateBookForResultData()
        {
            Dictionary<string, Dictionary<int, string>> titles =
                new Dictionary<string, Dictionary<int, string>>();
            titles.Add(RESULT_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "Наименование",
                [2] = "Ед. измер.",
                [3] = "Цена"
                
            });
            titles.Add("English", new Dictionary<int, string>
            {
                [1] = "На английском"
            });
            return CreateBookForResultData(titles);
        }

        protected override bool PositionToFirstRow(Page page)
        {
            page.CurrentRow = 4;
            return !page.IsEndPosition();
        }

        protected override bool WriteLineToPage(Page page, Dictionary<int, string> values)
        {
            if (values.Count == 1)
            {
                page.MergeColumns(1, 3);
                page.CentrateCell(1);
                page.WriteLine(values, true);
            }
            return base.WriteLineToPage(page, values);
        }

        protected override bool GetFirstOrNextColumns(Page page, ref Dictionary<string, int> columnNumbers)
        {
            if (columnNumbers == null)
            {
                columnNumbers = new Dictionary<string, int>();
                columnNumbers.Add(NAME_COLUMN, 1);
                columnNumbers.Add(OPTIONS_COLUMN, 2);
                columnNumbers.Add(UNIT_COLUMN, 3);
                columnNumbers.Add(PRICE_COLUMN, 4);
            }
            else if (columnNumbers[PRICE_COLUMN] + 5 <= page.TotalColumns)
            {
                Dictionary<string, int> result = new Dictionary<string, int>();
                foreach (string key in columnNumbers.Keys)
                    result.Add(key, columnNumbers[key] + 5);
                columnNumbers = result;
            }
            else return false;
            return true;
        }

        protected override Dictionary<string, List<Dictionary<int, string>>> GatherValuesFromPage(Page page, Dictionary<string, int> columnNumbers)
        {
            Dictionary<string, List<Dictionary<int, string>>> result 
                = new Dictionary<string, List<Dictionary<int, string>>>();

            string cellName = page.ReadText(columnNumbers[NAME_COLUMN]);
            string cellPrice = page.ReadText(columnNumbers[PRICE_COLUMN]);

            List<Dictionary<int, string>> rusRows = new List<Dictionary<int, string>>();
            List<Dictionary<int, string>> latRows = new List<Dictionary<int, string>>();

            if (page.IsVerticalBorderInLine(columnNumbers[NAME_COLUMN], columnNumbers[PRICE_COLUMN])
                & page.CountMargeColumns(columnNumbers[PRICE_COLUMN]) == 1
                & cellPrice.IndexOf("Цена") == -1)
            {
                // товарная позиция
                string cellOptions = page.ReadText(columnNumbers[OPTIONS_COLUMN]);
                
                if (cellOptions.Trim().Length > 0)
                {
                    Regex r = new Regex(@"\w+[.,]?\w*", 0);
                    MatchCollection mc = r.Matches(cellOptions.Trim());

                    foreach (Match m in mc)
                    {
                        Dictionary<int, string> rus = new Dictionary<int, string>();
                        rus.Add(1, String.Format("{0} {1} {2}", groupTitle, cellName, m.Captures[0].ToString()));
                        rus.Add(2, page.ReadText(columnNumbers[UNIT_COLUMN]));
                        rus.Add(3, cellPrice);
                        rusRows.Add(rus);

                        Dictionary<int, string> lat = new Dictionary<int, string>();
                        lat.Add(1, Transliteration.Front(rus[1]));
                        latRows.Add(lat);
                    }
                }
                else
                {
                    Dictionary<int, string> rus = new Dictionary<int, string>();
                    rus.Add(1, String.Format("{0} {1}", groupTitle, cellName));
                    rus.Add(2, page.ReadText(columnNumbers[UNIT_COLUMN]));
                    rus.Add(3, cellPrice);
                    rusRows.Add(rus);

                    Dictionary<int, string> lat = new Dictionary<int, string>();
                    lat.Add(1, Transliteration.Front(rus[1]));
                    latRows.Add(lat);
                }
                result.Add(RESULT_PAGE_NAME, rusRows);
                result.Add("English", latRows);
                return result;
            }
            else if (cellName.Length > 0
                & page.CountMargeColumns(columnNumbers[NAME_COLUMN]) == 4
                & (cellName.IndexOf("(прод)") == -1))
            {
                // заголовок группы
                groupTitle = cellName;
                Dictionary<int, string> rus = new Dictionary<int, string>();
                rus.Add(1, groupTitle);
                rusRows.Add(rus);

                Dictionary<int, string> lat = new Dictionary<int, string>();
                lat.Add(1, Transliteration.Front(rus[1]));
                latRows.Add(lat);

                result.Add(RESULT_PAGE_NAME, rusRows);
                result.Add("English", latRows);
                return result;
            }
            return null;
        }
    }
}
