using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingInvitationService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            InvitationService service = new InvitationService();
            service.InviteFriends();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new InvitationService() 
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
