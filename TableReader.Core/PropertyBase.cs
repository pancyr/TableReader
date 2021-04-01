using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReader.Core
{
    public abstract class PropertyBase
    {
        public PropertyBase(string name, string group, int column): this(name, group, new List<int> { column }) { }

        public PropertyBase(string name, string group, List<int> columns)
        {
            this.Name = name;
            this.Group = group;
            this.Columns = columns;
        }

        public string Name { get; set; }
        public string Group { get; set; }
        public List<int> Columns { get; set; }
        public string Value { get; set; }

        public abstract string ParseVaue(string value);

        public virtual string ReadValue(Page page)
        {
            this.Value = "";
            foreach(int col in this.Columns)
            {
                if (this.Value.Length > 0)
                    this.Value += " ";
                this.Value += page.ReadValue(col);
            }
            return this.Value;
        }

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
