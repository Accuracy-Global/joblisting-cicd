using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Web
{
    public interface IBraintreeConfiguration
    {
        IBraintreeGateway CreateGateway();
        string GetConfigurationSetting(string setting);
        IBraintreeGateway GetGateway();
    }
}
