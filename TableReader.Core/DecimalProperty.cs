using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TableReader.Core
{
    public class DecimalProperty : PropertyBase
    {
        public DecimalProperty(string name, string group, int column) : base(name, group, column) { }

        public override string ParseVaue(string value)
        {
            List<decimal> array = new List<decimal>();
            value = value.Replace(".", ",");
            Regex _regular = new Regex(@"\d+[,]?\d*", 0);
            MatchCollection matches = _regular.Matches(value);

            foreach (Match m in matches)
            {
                decimal dec = Decimal.Parse(m.ToString());
                if (array.IndexOf(dec) == -1)
                    array.Add(dec);
            }

            string result = "";
            if (array.Count == 0)
                return result;
            else
                result = array[0].ToString();

            for (int i = 1; i < array.Count; i++)
                result += "/" + array[i].ToString();

            return result;
        }
    }
}
