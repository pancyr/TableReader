using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TableReader.Core;

namespace GasOilService
{
    public class GasOilServiceParser : TableParserBase
    {
        private const string CATEGORY_PAGE_NAME = "Category";
        private const string PRODUCTS_PAGE_NAME = "Products";
        private const string ATTRIBUTES_PAGE_NAME = "Attributes";

        private const string MAIN_CATEGORY = "115";
        private const string DIRECT_DRIVE_CATEGORY = "115,215,221";
        private const string BELT_DRIVE_CATEGORY = "115,215,222";

        private int productID;

        public override Book CreateBookForResultData(string filePath = null)
        {
            Dictionary<string, Dictionary<int, string>> titles =
                new Dictionary<string, Dictionary<int, string>>();
            titles.Add(PRODUCTS_PAGE_NAME, new Dictionary<int, string>
            {
                [1] = "product_id", [2] = "name", [3] = "categories", [4] = "main_categories",
                [5] = "sku", [6] = "upc", [7] = "ean", [8] = "jan", [9] = "isbn", [10] = "mpn",
                [11] = "location", [12] = "quantity",
                [13] = "model", [14] = "manufacturer",
                [15] = "image_name", [16] = "requires shipping",
                [17] = "price", [18] = "points",
                [19] = "date_added", [20] = "date_modified", [21] = "date_available",
                [22] = "weight", [23] = "unit", [24] = "length", [25] = "width", [26] = "height", [27] = "length unit",
                [28] = "status enabled", [29] = "tax_class_id", [30] = "viewed", [31] = "language_id", 
                [32] = "seo_keyword", [33] = "description", [34] = "meta_description", [35] = "meta_keywords", [36] = "seo_title", [37] = "seo_h1", [38] = "stock_status_id",
                [39] = "store_ids", [40] = "layout", [41] = "related_ids", [42] = "tags", [43] = "sort_order", [44] = "subtract", [45] = "minimum",

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
            this.productID = 2200;
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

            string textName = page.ReadText(2);
            string textDrive = page.ReadText(16);

            if (textName.Length > 0)
            {
                Dictionary<int, string> dataCommon = new Dictionary<int, string>();
                List<Dictionary<int, string>> dataAttributes = new List<Dictionary<int, string>>();

                // заполняем массив данных для страницы Products
                dataCommon.Add(1, productID.ToString());
                dataCommon.Add(2, textName);
                dataCommon.Add(13, textName);
                dataCommon.Add(4, MAIN_CATEGORY);
                dataCommon.Add(12, "1000");
                dataCommon.Add(14, page.ReadText(8));
                dataCommon.Add(16, "yes");
                dataCommon.Add(17, page.ReadText(3));

                if (textDrive.ToLower().Contains("прям"))
                    dataCommon.Add(3, DIRECT_DRIVE_CATEGORY);
                else if (textDrive.ToLower().Contains("рем"))
                    dataCommon.Add(3, BELT_DRIVE_CATEGORY);

                dataCommon.Add(22, GetRegValue(page.ReadText(23), @"\d+"));
                dataCommon.Add(23, GetRegValue(page.ReadText(23), @"\D+"));
                dataCommon.Add(24, GetRegValue(page.ReadText(20), @"\d+"));
                dataCommon.Add(25, GetRegValue(page.ReadText(21), @"\d+"));
                dataCommon.Add(26, GetRegValue(page.ReadText(22), @"\d+"));
                dataCommon.Add(27, GetRegValue(page.ReadText(22), @"\D+"));
                dataCommon.Add(28, "true");
                dataCommon.Add(32, Transliteration.Front(textName).Replace(" ", "-"));

                // заполнение страницы атрибутов
                List<PropertyBase> properties = GetListOfAttributes();
                foreach (PropertyBase prop in properties)
                {
                    prop.ReadValue(page);
                    dataAttributes.Add(prop.MakeDictionary(productID));
                }

                result.Add(PRODUCTS_PAGE_NAME, new List<Dictionary<int, string>> { dataCommon });
                result.Add(ATTRIBUTES_PAGE_NAME, dataAttributes);
                productID++;
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
            string group = "Компрессоры";
            result.Add(new StringProperty("Страна", group, 7));
            result.Add(new StringProperty("Вид компрессора", group, 9));
            result.Add(new DecimalProperty("Производительность (л/мин)", group, 10));
            result.Add(new DecimalProperty("Объём ресивера (л)", group, 11));
            result.Add(new StringProperty("Расположение ресивера", group, 12));
            result.Add(new DecimalProperty("Рабочее давление (атм)", group, 13));
            result.Add(new DecimalProperty("Мощность (кВт)", group, 14));
            result.Add(new DecimalProperty("Питание (В)", group, 15));
            result.Add(new StringProperty("Тип привода", group, 16));
            result.Add(new StringProperty("Тип двигателя", group, 17));
            result.Add(new StringProperty("Малошумный", group, 18));
            result.Add(new StringProperty("Спиральный", group, 19));
            result.Add(new StringProperty("Частотный привод", group, 24));
            result.Add(new StringProperty("Безмасляный", group, 25));
            result.Add(new StringProperty("С осушителем", group, 26));
            result.Add(new StringProperty("На шасси", group, 27));
            result.Add(new StringProperty("Тип охлаждения", group, 30));
            result.Add(new StringProperty("Манометр", group, 36));
            result.Add(new StringProperty("Уровень шума", group, 37));
            result.Add(new StringProperty("Трёхфазный", group, 60));
            return result;
        }
    }
}
