using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Data
{
    public enum ParameterComparisonTypes
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanEquals,
        LessThan,
        LessThanEqual,
        Contains
    }
    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterComparisonTypes Comparison { get; set; }
    }
}
