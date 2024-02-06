using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingBirthdayService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

#if DEBUG
            BirthdayService service = new BirthdayService();
            service.BirthdayWish();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new BirthdayService() 
            };
            ServiceBase.Run(ServicesToRun);
#endif

        }
    }
}
