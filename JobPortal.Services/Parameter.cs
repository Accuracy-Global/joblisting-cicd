using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public class Parameter
    {
        private string name;
        public Parameter() { }
        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public string Name
        {
            get
            {
                return string.Format("@{0}", name);
            }
            set 
            { 
                name = value; 
            }
        }
        public object Value { get; set; }
    }
}
