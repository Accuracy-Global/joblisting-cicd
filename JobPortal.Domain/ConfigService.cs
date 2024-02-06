using System.Linq;
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Domain
{
    public class ConfigService
    {
        private static volatile ConfigService instance;
        private static readonly object sync = new object();

        private ConfigService()
        {
        }

        /// <summary>
        ///     Single Instance of JobPortalService
        /// </summary>
        public static ConfigService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new ConfigService();
                        }
                    }
                }
                return instance;
            }
        }

        public string GetConfigValue(string parameter)
        {
            string value = string.Empty;
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                value = dataHelper.Get<ConfigParameter>().SingleOrDefault(x=>x.Name.Equals(parameter)).Value;
            }
            return value;
        }
    }
}