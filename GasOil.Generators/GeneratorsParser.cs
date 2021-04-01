using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TableReader.Core;

namespace GasOil.Generators
{
    public class GeneratorsParser : TableParserBase
    {
        //private const string CATEGORY_PAGE_NAME = "Category";
        private const string PRODUCTS_PAGE_NAME = "Products";
        private const string ATTRIBUTES_PAGE_NAME = "Attributes";

        private const string MAIN_CATEGORY = "272";
        private const string DISEL_CATEGORY = "272,273";
        private const string BENSIN_CATEGORY = "272,274";
        private const string GAZ_CATEGORY = "272,275";
        private const string SVAR_CATEGORY = "272,276";

        public override Book CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, string>> titles =
                new Dictionary<string, Dictionary<int, string>>();

            /*titles.Add(CATEGORY_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "category_id",
                [2] = "category_name"
            });*/
            titles.Add(PRODUCTS_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "name",
                [3] = "categories",
                [4] = "main_categories",
                [5] = "sku",
                [6] = "upc",
                [7] = "ean",
                [8] = "jan",
                [9] = "isbn",
                [10] = "mpn",
                [11] = "location",
                [12] = "quantity",
                [13] = "model",
                [14] = "manufacturer",
                [15] = "image_name",
                [16] = "requires shipping",
                [17] = "price",
                [18] = "points",
                [19] = "date_added",
                [20] = "date_modified",
                [21] = "date_available",
                [22] = "weight",
                [23] = "unit",
                [24] = "length",
                [25] = "width",
                [26] = "height",
                [27] = "length unit",
                [28] = "status enabled",
                [29] = "tax_class_id",
                [30] = "viewed",
                [31] = "language_id",
                [32] = "seo_keyword",
                [33] = "description",
                [34] = "meta_description",
                [35] = "meta_keywords",
                [36] = "seo_title",
                [37] = "seo_h1",
                [38] = "stock_status_id",
                [39] = "store_ids",
                [40] = "layout",
                [41] = "related_ids",
                [42] = "tags",
                [43] = "sort_order",
                [44] = "subtract",
                [45] = "minimum"
            });
            titles.Add(ATTRIBUTES_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id",
                [2] = "language_id",
                [3] = "attribute_group",
                [4] = "attribute_name",
                [5] = "text"
            });
            return CreateBookForResultData(titles, filePath);
        }

        protected override bool PositionToFirstRow(Page page)
        {
            /*page.CurrentRow = 1;
            while (page.ReadValue(3).Length > 0)
                page.CurrentRow++;
            page.TotalRows = page.CurrentRow - 1;*/
            page.TotalRows = 51;
            page.CurrentRow = 2;
            return !page.IsEndPosition();
        }

        protected override bool WriteLineToPage(Page page, Dictionary<int, string> values)
        {
            return base.WriteLineToPage(page, values);
        }

        protected override bool GetFirstOrNextColumns(Page page, ref Dictionary<string, int> columnNumbers)
        {
            if (columnNumbers == null)
            {
                columnNumbers = new Dictionary<string, int>();
            }
            else return false;
            return true;
        }

        protected override Dictionary<string, List<Dictionary<int, string>>> GatherValuesFromPage(Page page, Dictionary<string, int> columnNumbers)
        {
            Dictionary<string, List<Dictionary<int, string>>> result
                = new Dictionary<string, List<Dictionary<int, string>>>();

            Dictionary<int, string> dataCommon = new Dictionary<int, string>();
            List<Dictionary<int, string>> dataAttributes = new List<Dictionary<int, string>>();

            string textName = page.ReadText(3);

            if (textName.Length > 0)
            {
                // заполняем массив данных для страницы Products
                int productID = Int32.Parse(page.ReadText(1)) + 600;
                dataCommon.Add(1, productID.ToString());
                dataCommon.Add(2, textName);

                string categName = page.ReadText(2);
                if (categName.ToLower().Contains("бензиновые генераторы"))
                    dataCommon.Add(3, BENSIN_CATEGORY);
                else if (categName.ToLower().Contains("газовые генераторы"))
                    dataCommon.Add(3, GAZ_CATEGORY);
                else if (categName.ToLower().Contains("дизельные генераторы"))
                    dataCommon.Add(3, DISEL_CATEGORY);
                else if (categName.ToLower().Contains("сварочные генераторы"))
                    dataCommon.Add(3, SVAR_CATEGORY);

                dataCommon.Add(4, MAIN_CATEGORY);
                dataCommon.Add(12, "1000");

                string manuf = page.ReadText(5);
                string model = page.ReadText(11);
                string image_URL = page.ReadText(23);

                dataCommon.Add(13, textName);
                dataCommon.Add(14, manuf);
                dataCommon.Add(15, image_URL);
                dataCommon.Add(16, "yes");

                /*if (model.StartsWith(manuf))
                    model = model.Substring(manuf.Length, model.Length - manuf.Length).Trim();*/

                Double price;
                bool tryPrice = Double.TryParse(page.ReadText(21), out price);
                
                if (tryPrice)
                {
                    price *= 1.2;
                    dataCommon.Add(17, ((int)price).ToString());
                }

                dataCommon.Add(18, "0");

                // вес и ед. измер
                dataCommon.Add(22, page.ReadText(20));
                dataCommon.Add(23, "кг");

                //габариты
                string metrage = page.ReadText(19);
                Regex m_reg = new Regex(@"\d+");
                MatchCollection matches = m_reg.Matches(metrage);

                if (matches.Count == 3)
                {
                    dataCommon.Add(24, matches[0].ToString());
                    dataCommon.Add(25, matches[1].ToString());
                    dataCommon.Add(26, matches[2].ToString());
                    dataCommon.Add(27, "см");
                }

                //dataCommon.Add(27, GetRegValue(page.ReadText(22), @"\D+"));
                dataCommon.Add(28, "true");
                dataCommon.Add(31, "1");
                dataCommon.Add(39, "0");
                dataCommon.Add(43, "1");
                dataCommon.Add(44, "true");
                dataCommon.Add(45, "1");
                dataCommon.Add(32, Transliteration.Front(page.ReadText(3)).Replace(" ", "-"));
                dataCommon.Add(33, page.ReadText(22));

                // поле Meta_Description
                dataCommon.Add(34, textName + " — купить от Газнефтесервис в Уфе. Гарантия до 5 лет. Сервис 24 часа в сутки.");

                // поле Meta_KeyWords
                dataCommon.Add(35, String.Format("{0}, {1}, купить в Уфе", textName, categName));

                // поле Seo_Title
                dataCommon.Add(36, textName);

                // поле Seo_H1
                dataCommon.Add(37, String.Format("{0} купить в Уфе от Газнефтесервис по лучшей цене c гарантией", textName));

                // заполнение страницы атрибутов
                List<PropertyBase> properties = GetListOfAttributes();
                foreach (PropertyBase prop in properties)
                {
                    prop.ReadValue(page);
                    dataAttributes.Add(prop.MakeDictionary(productID));
                }

                result.Add(PRODUCTS_PAGE_NAME, new List<Dictionary<int, string>> { dataCommon });
                result.Add(ATTRIBUTES_PAGE_NAME, dataAttributes);
                return result;
            }
            return null;
        }

        private string GetRegValue(string inputString, string regular)
        {
            Regex ex = new Regex(regular, 0);
            Match match = ex.Match(inputString.Trim());

            if (match != null)
                return match.ToString().Trim();

            return null;
        }

        private List<PropertyBase> GetListOfAttributes()
        {
            List<PropertyBase> result = new List<PropertyBase>();
            string group = "Генераторы";
            result.Add(new StringProperty("Страна", group, 4));
            result.Add(new DecimalProperty("Мощность номинальная (кВт)", group, new List<int> { 6, 7}));
            result.Add(new StringProperty("Частота вращения", group, 15));
            result.Add(new StringProperty("Напряжение (в)", group, 8));
            result.Add(new StringProperty("Исполнение", group, 9));
            result.Add(new StringProperty("Объём бака (л)", group, 10));
            result.Add(new StringProperty("Производитель двигателя", group, 11));
            result.Add(new StringProperty("Модель двигателя", group, 12));
            result.Add(new StringProperty("Топливо", group, 13));
            result.Add(new StringProperty("Норма расхода", group, 14));
            result.Add(new StringProperty("Система охлаждния", group, 16));
            result.Add(new StringProperty("Уровень шума (дБ)", group, 17));

            return result;
        }
    }
}
