#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Models
{
    public class Recipient
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'RecipientTypes' could not be found (are you missing a using directive or an assembly reference?)
        public RecipientTypes Type { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'RecipientTypes' could not be found (are you missing a using directive or an assembly reference?)
    }
}