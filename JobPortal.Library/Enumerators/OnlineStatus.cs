using System.ComponentModel;

namespace JobPortal.Library.Enumerators
{
    public enum OnlineStatus
    {
        [Description("Online")]
        Online,
        [Description("Away")]
        Away,
        [Description("Do Not Disturb")]
        DoNotDisturb,
        [Description("Invisible")]
        Invisible,
        [Description("Offline")]
        Offline
    }
}
