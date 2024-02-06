using Braintree;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JobPortal.Web
{
    public class BraintreeConfiguration : IBraintreeConfiguration
    {
        public string Environment { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        private IBraintreeGateway BraintreeGateway { get; set; }

        public IBraintreeGateway CreateGateway()
        {
            Environment = System.Environment.GetEnvironmentVariable("BraintreeEnvironment");
            MerchantId = System.Environment.GetEnvironmentVariable("BraintreeMerchantId");
            PublicKey = System.Environment.GetEnvironmentVariable("BraintreePublicKey");
            PrivateKey = System.Environment.GetEnvironmentVariable("BraintreePrivateKey");

            if (MerchantId == null || PublicKey == null || PrivateKey == null)
            {
                Environment = GetConfigurationSetting("BraintreeEnvironment");
                MerchantId = GetConfigurationSetting("BraintreeMerchantId");
                PublicKey = GetConfigurationSetting("BraintreePublicKey");
                PrivateKey = GetConfigurationSetting("BraintreePrivateKey");
            }

            return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
        }

        public string GetConfigurationSetting(string setting)
        {
            return ConfigService.Instance.GetConfigValue(setting);
        }

        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }
    }
}