using System;
using System.Collections.Generic;
using System.Text;

namespace TableReader.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TableColumnAttribute : System.Attribute
    {
        public TableColumnAttribute(int iNum, string title)
        {
            this._num = iNum;
            this._title = title;
        }

        private int _num;
        public int Num
        {
            get
            {
                return _num;
            }
        }
        
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class MultyStructurePriceAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SecondaryPriceAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class NextPriceClassAttribute : System.Attribute
    {
        private string _className;
        public NextPriceClassAttribute(string className)
        {
            this.ClassName = className;
        }

        public string ClassName
        {
            get
            {
                return _className;
            }
            set
            {
                _className = value;
            }
        }
    }
}
