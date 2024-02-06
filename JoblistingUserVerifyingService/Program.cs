using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingUserVerifyingService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            UserVerifyingService service = new UserVerifyingService();
            service.ValidateEmails();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new UserVerifyingService() 
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
