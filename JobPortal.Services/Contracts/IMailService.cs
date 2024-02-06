using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services.Contracts
{
    public interface IMailService
    {
        bool Send(string subject, string from, string to, string body);
    }
}
