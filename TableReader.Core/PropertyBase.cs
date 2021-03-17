using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReader.Core
{
    public abstract class PropertyBase
    {
        public PropertyBase(string name, string group, int column)
        {
            this.Name = name;
            this.Group = group;
            this.Column = column;
        }

        public string Name { get; set; }
        public string Group { get; set; }
        public int Column { get; set; }
        public string Value { get; set; }

        public abstract string ParseVaue(string value);

        public virtual string ReadValue(Page page) => this.Value = page.ReadValue(Column);

        public virtual Dictionary<int, string> MakeDictionary(int productID)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            values.Add(1, productID.ToString());
            values.Add(2, "1");
            values.Add(3, Group);
            values.Add(4, Name);
            values.Add(5, ParseVaue(Value));
            return values;
        }
    }
}
