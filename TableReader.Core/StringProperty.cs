using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReader.Core
{
    public class StringProperty : PropertyBase
    {
        public StringProperty(string name, string group, int column) : base(name, group, column) { }

        public override string ParseVaue(string value) => value;
    }
}
