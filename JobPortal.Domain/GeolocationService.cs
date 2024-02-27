using JobPortal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JobPortal.Domain
{
    public class GeolocationService
    {
        private readonly string geolocationApi, geolocationFreeApi;
        private readonly bool isLogCreate;
        public GeolocationService()
        {
            this.geolocationApi = Convert.ToString(ConfigurationManager.AppSettings["GeolocationApi"]);
            this.geolocationFreeApi = Convert.ToString(ConfigurationManager.AppSettings["GeolocationFreeApi"]);
            this.isLogCreate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogCreate"]);
        }

        public string GetUser_IP()
        {
            string visitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                visitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                LogEntry($"GetUser_IP Request.ServerVariables['HTTP_X_FORWARDED_FOR'] () => Public Ip: {visitorsIPAddr}");
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                visitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
                LogEntry($"GetUser_IP Request.UserHostAddress () => Public Ip: {visitorsIPAddr}");
            }
            //visitorsIPAddr = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString();

            if (visitorsIPAddr == "::1" || visitorsIPAddr.Contains("localhost"))
            {
                visitorsIPAddr = new System.Net.WebClient().DownloadString("https://api.ipify.org");
                LogEntry($"GetUser_IP https://api.ipify.org () => Public Ip: {visitorsIPAddr}");
            }

            if (visitorsIPAddr.Contains(":"))
            {
                visitorsIPAddr = visitorsIPAddr.Substring(0, visitorsIPAddr.IndexOf(':')).Trim();
                LogEntry($"GetUser_IP After Remove Port Number () => Public Ip: {visitorsIPAddr}");
            }

            return visitorsIPAddr;
        }

        public IpGeolocation GetUserCountryByIpFree(string ip)
        {
            GeolocationService locationService = new GeolocationService();
            IpGeolocation ipInfo = new IpGeolocation();
            try
            {
                string apiUrl = string.Format(this.geolocationFreeApi, ip);
                locationService.LogEntry($"GeoLocation Free Api => {apiUrl}");
                string info = new WebClient().DownloadString(apiUrl);
                ipInfo = JsonConvert.DeserializeObject<IpGeolocation>(info);

                if (ipInfo != null && string.IsNullOrEmpty(ipInfo.country_name))
                {
                    GetUserCountryByIpFree(ip); 
                }
                else if (ipInfo == null)
                {
                    GetUserCountryByIpFree(ip);
                }

                locationService.LogEntry($"GetUserCountryByIpFree() => {JsonConvert.SerializeObject(ipInfo)}");
            }
            catch (Exception ex)
            {
                string exp = ex.ToString();
                locationService.LogEntry($"GetUserCountryByIpFree  Exception: => {exp}");
                GetUserCountryByIpFree(ip);
            }

            return ipInfo;
        }

        public IpGeolocation GetUserCountryByIp(string ip)
        {
            IpGeolocation ipInfo = new IpGeolocation();
            GeoLocationError ipError = new GeoLocationError();
            try
            {
                string apiUrl = string.Format(this.geolocationApi, ip);
                LogEntry($"GeoLocation Api => {apiUrl}");
                string info = new WebClient().DownloadString(apiUrl);
                ipInfo = JsonConvert.DeserializeObject<IpGeolocation>(info);
                ipError = JsonConvert.DeserializeObject<GeoLocationError>(info);

                if (ipError.error == null)
                {
                    LogEntry($"GetUserCountryByIp() => {JsonConvert.SerializeObject(ipInfo)}");

                    if (ipInfo != null && string.IsNullOrEmpty(ipInfo.country_name))
                    {
                        return GetUserCountryByIp(ip);
                    }
                    else if (ipInfo == null)
                    {
                        return GetUserCountryByIp(ip);
                    }
                }
                else if (ipError.error.info.ToLower() == "your monthly usage limit has been reached. please upgrade your subscription plan.")
                {
                    LogEntry($"GetUserCountryByIp() => {JsonConvert.SerializeObject(ipError)}");
                    return GetUserCountryByIpFree(ip);
                }
                else
                {
                    LogEntry($"GetUserCountryByIp() => {JsonConvert.SerializeObject(ipError)}");
                    return GetUserCountryByIpFree(ip);
                }
            }
            catch (Exception ex)
            {
                string exp = ex.ToString();
                LogEntry($"GetUserCountryByIp  Exception: => {exp}");
                return GetUserCountryByIp(ip);
            }

            return ipInfo;
        }

        public void LogEntry(string text)
        {
            if (this.isLogCreate)
            {
                try
                {
                    string LogFilePath = "/Log_CountrySession";
                    var fullLogFilePath = HttpContext.Current.Server.MapPath(LogFilePath);

                    if (!Directory.Exists(fullLogFilePath))
                    {
                        Directory.CreateDirectory(fullLogFilePath);
                    }

                    File.AppendAllText($"{fullLogFilePath}/LogFile {DateTime.Today.ToString("dd-MM-yyyy")}.txt", string.Format("{0}{1}", text, Environment.NewLine));
                }
                catch (Exception)
                {
                    LogEntry(text);
                }
            }
        }
    }
}
