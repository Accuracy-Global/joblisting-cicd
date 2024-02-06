using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Library.Enumerators
{
    public enum Relationships
    {
        [Description("Single")]
        SINGLE = 1,
        [Description("Married")]
        MARRIED,
        [Description("Engaged")]
        ENGAGED,
        [Description("In Relationship")]
        INRELATIONSHIP,
        [Description("Separated")]
        SEPARATED,
        [Description("Divorced")]
        DIVORCED,
        [Description("Widowed")]
        WIDOWED,
        [Description("Can't Say")]
        CANT_SAY
    }
}
