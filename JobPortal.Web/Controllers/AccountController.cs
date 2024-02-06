using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using Microsoft.Owin.Security;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using System.Web.Script.Serialization;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Text.RegularExpressions;
using Twilio;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Configuration;
using TweetSharp;
using GoogleAuthentication.Services;
using JobPortal.Web.App_Start;

namespace JobPortal.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Cons
        private const string XsrfKey = "XsrfId";


        public AccountController(IUserService service) : base(service)
        {

        }
        #endregion
        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult Confirm(long? id, string token, string returnUrl, string type = null)
        {
            UserInfoEntity uinfo = null;
            if (id != null)
            {
                uinfo = _service.Get(id.Value);
                if (string.IsNullOrEmpty(type))
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        if (uinfo != null)
                        {
                            if (uinfo.IsConfirmed)
                            {
                                TempData["UpdateData"] = "You have already verified your account!";
                                if (Request.IsAuthenticated)
                                {
                                    return Redirect(returnUrl);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    if (uinfo.ConfirmationToken.Equals(token))
                                    {
                                        _service.Confirm(id.Value);
                                        if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("listjob"))
                                        {
                                            TempData["UpdateData"] = string.Format("Account confirmed successfully!");
                                        }
                                        if (returnUrl.ToLower().Contains("indeed"))
                                        {
                                            if (uinfo.IsJobseeker)
                                            {
                                                return Redirect(returnUrl);
                                            }
                                            else
                                            {
                                                if (uinfo.Role != SecurityRoles.Employers)
                                                {
                                                    return Redirect(string.Format("/job/preview?returnUrl={0}", returnUrl));
                                                }
                                            }
                                        }
                                        return Redirect(returnUrl);
                                    }
                                    else
                                    {
                                        TempData["Error"] = "The code you are using has already expired!";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (uinfo.IsConfirmed)
                        {
                            TempData["UpdateData"] = "You have already verified your account!";
                            if (Request.IsAuthenticated)
                            {
                                return Redirect(string.Format("/{0}", uinfo.PermaLink));
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(token))
                            {
                                if (uinfo.ConfirmationToken.Equals(token))
                                {
                                    _service.Confirm(id.Value);
                                    if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("listjob"))
                                    {
                                        TempData["UpdateData"] = string.Format("Account confirmed successfully!");
                                    }

                                    return RedirectToUrl(uinfo);
                                }
                                else
                                {
                                    TempData["Error"] = "The code you are using has already expired!";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(type) && type.Equals("ecr"))
                    {
                        if (uinfo.ConfirmationToken.Equals(token))
                        {
                            _service.Confirm(id.Value, type);
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                UserInfoEntity nuinfo = _service.Get(id.Value);
                                token = UIHelper.Get6DigitCode();
                                _service.GenerateToken(id.Value, token);

                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                                var body = reader.ReadToEnd();
                                if (uinfo.IsConfirmed)
                                {
                                    body = body.Replace("@@receiver", nuinfo.FullName);
                                }
                                else
                                {
                                    body = body.Replace("@@receiver", "Dear User");
                                }
                                body = body.Replace("@@code", token);
                                var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                    string.Format("/Confirm?id={0}&token={1}", id, token);

                                body = body.Replace("@@url", hosturl);

                                string[] receipent = { nuinfo.Username };
                                var subject = "Confirm Your Email Address";

                                AlertService.Instance.SendMail(subject, receipent, body);
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                if (User != null)
                                {
                                    UserInfoEntity nuinfo = _service.Get(id.Value);
                                    token = UIHelper.Get6DigitCode();
                                    _service.GenerateToken(id.Value, token);

                                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                                    var body = reader.ReadToEnd();
                                    if (uinfo.IsConfirmed)
                                    {
                                        body = body.Replace("@@receiver", nuinfo.FullName);
                                    }
                                    else
                                    {
                                        body = body.Replace("@@receiver", "Dear User");
                                    }
                                    body = body.Replace("@@code", token);
                                    var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                        string.Format("/Confirm?id={0}&token={1}", id, token);

                                    body = body.Replace("@@url", hosturl);

                                    string[] receipent = { nuinfo.Username };
                                    var subject = "Confirm Your Email Address";

                                    AlertService.Instance.SendMail(subject, receipent, body);

                                    return RedirectToUrl(uinfo);
                                }
                                else
                                {
                                    return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
                                }
                            }
                        }
                        else
                        {
                            TempData["Error"] = "The code you are using has already expired!";
                        }
                    }
                }
            }
            ViewBag.Id = id;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Email = uinfo != null ? uinfo.Username : "";

            return View();
        }

        #region External Login
        #region ExistUser for External Login
        //If user is exist then it'll logined in
        //or if not then it'll redirect to registor page.
        public ActionResult IsUserExist(RegisterModel registerModel)
        {
            UserInfoEntity uinfo = _service.Get(registerModel.Username);

            if (uinfo != null)
            {
                LoginModel model = new LoginModel
                {
                    LoginUsername = uinfo.Username,
                    LoginPassword = "temp123",
                    RememberMe = false,
                    number = null,
                    otp = null,
                    IsexternalLogin = true
                };
                return Login(model);
            }
            else
            {
                return RedirectToAction("Register", registerModel);
            }


        }
        #endregion

        #region Twitter Auth
        public class TwitterModel
        {
            public string ProfileImageUrl { get; set; }
            public string Name { get; set; }
            public string Id { get; set; }
            public string ScreenName { get; set; }
            public string Description { get; set; }
            public string StatusesCount { get; set; }
            public string FollowersCount { get; set; }
            public string FriendsCount { get; set; }
            public string FavouritesCount { get; set; }
            public string Location { get; set; }
            public string Email { get; set; }
        }
        public ActionResult TwitterAuth()
        {
            string Key = "mBKFrmuAqVesjnCYClf7FscOQ";
            string Secret = "4aG7S3YhY9L5V3bhqbc6M7Ad90BdyBSIxSLgZrZeT9gvyCd9ks";
            TwitterService service = new TwitterService(Key, Secret);
            //Obtaining a request token  
            OAuthRequestToken requestToken = service.GetRequestToken("https://www.joblisting.com/account/TwitterCallback");
            Uri uri = service.GetAuthenticationUrl(requestToken);
            //Redirecting the user to Twitter Page  
            return Redirect(uri.ToString());
        }

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier)
        {
            var requestToken = new OAuthRequestToken
            {
                Token = oauth_token
            };
            string Key = "mBKFrmuAqVesjnCYClf7FscOQ";
            string Secret = "4aG7S3YhY9L5V3bhqbc6M7Ad90BdyBSIxSLgZrZeT9gvyCd9ks";
            try
            {
                OAuthHelper oauthhelper = new OAuthHelper();
                oauthhelper.GetUserTwAccessToken(oauth_token, oauth_verifier);
                oauthhelper.GetUser(oauth_token, oauth_verifier);
                TwitterService service = new TwitterService(Key, Secret);
                //Get Access Tokens
                //OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);
                service.AuthenticateWith(oauthhelper.oauth_access_token, oauthhelper.oauth_access_token_secret);
                VerifyCredentialsOptions option = new VerifyCredentialsOptions { IncludeEntities = true, SkipStatus = true };
                //According to Access Tokens get user profile details  
                TwitterUser user = service.VerifyCredentials(option);

                return View();
            }
            catch
            {
                throw;
            }
        }

        #endregion
        #region LinkedIn Auth
        //Authenticate by linked Auth API user and get user basic data
        public ActionResult LinkedINAuth(string code, string state)
        {
            try
            {
                //Get Accedd Token
                var client = new RestClient("https://www.linkedin.com/oauth/v2/accessToken");
                var request = new RestRequest(Method.POST);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", Request.QueryString["code"].ToString());
                request.AddParameter("redirect_uri", "https://www.joblisting.com/account/linkedINAuth");
                request.AddParameter("client_id", WebConfigurationManager.AppSettings["LinkedIn_client_id"]);
                request.AddParameter("client_secret", WebConfigurationManager.AppSettings["LinkedIn_client_secret"]);
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                //Fetch AccessToken
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                LinkedINVM linkedINVM = jsonSerializer.Deserialize<LinkedINVM>(content);

                //Get Profile Details
                var client2 = new RestClient("https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))&oauth2_access_token=" + linkedINVM.access_token);
                client = new RestClient("https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))&oauth2_access_token=" + linkedINVM.access_token);
                request = new RestRequest(Method.GET);
                var request2 = new RestRequest(Method.GET);
                response = client.Execute(request);
                var response2 = client2.Execute(request2);
                content = response.Content;
                var content2 = response2.Content;
                var useremail = (JObject)JsonConvert.DeserializeObject(content2);
                dynamic data = JObject.Parse(content2);
                var profileEmail = useremail["elements"][0]["handle~"]["emailAddress"].ToString();
                var user = (JObject)JsonConvert.DeserializeObject(content);
                var firstname = user["firstName"]["localized"]["en_US"].ToString();
                var lastname = user["lastName"]["localized"]["en_US"].ToString();
                RegisterModel registerModel = new RegisterModel { FirstName = firstname, LastName = lastname, Username = profileEmail };
                return RedirectToAction("IsUserExist", registerModel);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Google Auth
        //Genrate URL for google authentication
        private string GoogleAuthURL()
        {
            string url = "";
            try
            {

                string ClientID = WebConfigurationManager.AppSettings["Google_ClientID"];
                string callbackurl = "https://www.joblisting.com/Account/GoogleLoginCallback";
                var response = GoogleAuth.GetAuthUrl(ClientID, callbackurl);
                url = response.ToString();
            }
            catch (Exception ex)
            { }
            return url;
        }
        //Authenticate by Google plus API user and get user basic data
        public async Task<ActionResult> GoogleLoginCallback(string code)
        {
            RegisterModel registerModel = new RegisterModel();

            try
            {
                string ClientID = WebConfigurationManager.AppSettings["Google_ClientID"];
                string ClientsecreteId = WebConfigurationManager.AppSettings["Google_ClientsecreteId"];
                string callbackurl = "https://www.joblisting.com/Account/GoogleLoginCallback";
                var token = await GoogleAuth.GetAuthAccessToken(code, ClientID, ClientsecreteId, callbackurl);
                var userprofile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
                var googleuser = JsonConvert.DeserializeObject<GoogleProfile>(userprofile);
                registerModel = new RegisterModel { FirstName = googleuser.Name, Username = googleuser.Email, Mobile = googleuser.MobilePhone };

            }
            catch (Exception ex)
            { }
            return RedirectToAction("IsUserExist", registerModel);
        }
        #endregion

        #region facebook
        //URL builder for facebook user authentication
        private Uri RediredtUri
        {
            get

            {
                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }

        }
        [AllowAnonymous]

        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = WebConfigurationManager.AppSettings["Facebook_Clientid"],
                client_secret = WebConfigurationManager.AppSettings["Facebook_clientsecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        //after authentication of user, Get access_token for access user data.
        public ActionResult FacebookCallback(string code)
        {
            RegisterModel registerModel = new RegisterModel();
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = WebConfigurationManager.AppSettings["Facebook_Clientid"],
                client_secret = WebConfigurationManager.AppSettings["Facebook_clientsecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            //Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            registerModel.Username = me.email;
            registerModel.FirstName = me.first_name;
            registerModel.LastName = me.last_name;
            //TempData["picture"] = me.picture.data.url;
            //FormsAuthentication.SetAuthCookie(email, false);

            return RedirectToAction("IsUserExist", registerModel);
        }
        #endregion
        #endregion External Login
        [Authorize]
        public ActionResult ConfirmRegistration()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SendOTP()
        {

            ViewBag.error = TempData["Error"];
            ViewBag.ss = TempData["valid"];
            ViewBag.er = TempData["invalid"];
            return View();
        }
        [HttpPost]
        public ActionResult SendOTP(string country, string mobile1)
        {

            Regex reg = null;
            String mobile = Convert.ToString(Request["number"]);
            TempData["number"] = mobile;
            reg = new Regex("^[1-9][0-9]*$");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {

                SqlCommand sql = new SqlCommand("SELECT * FROM UserProfiles WHERE Mobile = @mobile", conn);
                SqlDataAdapter sda = new SqlDataAdapter(sql);
                sql.Parameters.AddWithValue("@mobile", mobile.Trim());
                DataSet ds = new DataSet();
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string message = string.Empty;
                    if (!string.IsNullOrEmpty(mobile) && reg.IsMatch(mobile))
                    {
                        Random random = new Random();
                        int value = random.Next(1001, 9999);
                        string AccountSid = ConfigService.Instance.GetConfigValue("TwilioSID");
                        string AuthToken = ConfigService.Instance.GetConfigValue("TwilioToken");
                        string from = ConfigService.Instance.GetConfigValue("TwilioNumber");
                        var twilio = new TwilioRestClient(AccountSid, AuthToken);

                        StringBuilder sbSMS = new StringBuilder();
                        sbSMS.AppendFormat("Yur Joblisting account verification OTP is " + value + " ", value);
                        ViewBag.SaveData = "OTP sent successfully!";
                        ViewBag.SaveData1 = mobile;
                        Guid token = Guid.NewGuid();
                        Session["otp"] = value;

                        string to = string.Format("{0}{1}", country, mobile);
                        using (JobPortalEntities context = new JobPortalEntities())
                        {

                            var sms = twilio.SendMessage(from, to, sbSMS.ToString());

                            if (sms.RestException == null)
                            {
                                Session["SaveData"] = "OTP sent successfully!!";
                                //Session["number"] = mobile;
                                var msg = twilio.GetMessage(sms.Sid);
                                if (msg.Status.Equals("delivered") || msg.Status.Equals("sent"))
                                {

                                    //Session["SaveData"] = "OTP sent successfully!";
                                }
                            }
                            else
                            {
                                if (sms.RestException.Code.Equals("14101") || sms.RestException.Code.Equals("21211"))
                                {
                                    TempData["Error"] = "Please provide valid mobile number!";
                                }
                            }

                        }
                    }
                    else
                    {
                        ViewBag.Error1 = "Please provide valid mobile number!";
                    }
                }
                else
                {

                    TempData["error"] = mobile + " is not register with Joblisting";
                    return RedirectToAction("SendOTP", "account");
                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult Verify(LoginModel model, string returnUrl)
        {
            String otp = Convert.ToString(Request["otp"]);
            string enterotp = Convert.ToString(Session["otp"]);
            //if (Request.Browser.Cookies == false)
            //{
            //    TempData["Error"] = "<b>Cookies are Disabled</b><br/>Kindly enable cookies in your browser and refresh.";

            //    return View();
            //}

            //if (ModelState.IsValid)
            //{
            //    string username = model.LoginUsername.Trim().ToLower();
            //    string passwrod = model.LoginPassword.Trim();

            //    var user = Membership.GetUser(username);
            //    UserInfoEntity uinfo = _service.Get(username);
            //    if (user != null && (uinfo != null && uinfo.IsActive))
            //    {
            //        if (!user.IsLockedOut)
            //        {
            //            if (WebSecurity.Login(username, passwrod, model.RememberMe))
            //            {
            //                if (!string.IsNullOrEmpty(uinfo.Provider))
            //                {
            //                    OAuthWebSecurity.Login(uinfo.Provider, uinfo.ProviderId, model.RememberMe);
            //                }

            //                JavaScriptSerializer serializer = new JavaScriptSerializer();
            //                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
            //                         1,
            //                         uinfo.Id.ToString(),
            //                         DateTime.UtcNow,
            //                         DateTime.UtcNow.AddHours(10),
            //                         model.RememberMe,
            //                         "");

            //                string encTicket = FormsAuthentication.Encrypt(authTicket);
            //                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            //                if (authTicket.IsPersistent)
            //                {
            //                    faCookie.Expires = authTicket.Expiration;
            //                }
            //                Response.Cookies.Add(faCookie);

            //                var bytes = System.Text.Encoding.UTF8.GetBytes(model.LoginPassword);
            //                _service.TrackLoginHistory(username, Request.UserHostAddress, Request.Browser.Browser, Convert.ToBase64String(bytes));
            //                _service.TrackUserStatus(uinfo.Id, (int)OnlineStatus.Online, DateTime.Now);

            //                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.ToLower().Contains("indeed"))
            //                {
            //                    if (uinfo.IsJobseeker)
            //                    {
            //                        return Redirect(returnUrl);
            //                    }
            //                    else
            //                    {
            //                        if (uinfo.Role != SecurityRoles.Employers)
            //                        {
            //                            return Redirect(string.Format("/job/preview?returnUrl={0}", returnUrl));
            //                        }
            //                    }
            //                }

            //                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("/Jobseeker/Download?id"))
            //                {
            //                    if ((SecurityRoles)uinfo.Type != SecurityRoles.Employers)
            //                    {
            //                        TempData["Error"] = "Only Employers can download resume!";
            //                    }
            //                    else
            //                    {
            //                        TempData["DownloadUrl"] = returnUrl;
            //                    }
            //                    return RedirectToUrl(uinfo);
            //                }
            //                else
            //                {
            //                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
            //                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            //                    {
            //                        return Redirect(returnUrl);
            //                    }
            //                    else
            //                    {
            //                        TempData["LoginStatus"] = "LoggedIn";
            //                        if (uinfo.Type == 4 || uinfo.Type == 13)
            //                        {
            //                            TempData["UpdateData"] = string.Format("Welcome1&nbsp;{0}!<br />Check&nbsp;<a href=\"/jobseeker/careertips\" style=\"color:#34ba08\">Career&nbsp;Tips</a>!", uinfo.FirstName);
            //                        }
            //                        else
            //                        {
            //                            TempData["UpdateData"] = string.Format("Welcome&nbsp;{0}!", uinfo.FirstName);
            //                        }
            //                        return RedirectToUrl(uinfo);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                ViewBag.ReturnUrl = returnUrl;
            //                ViewBag.LoginError = "<div class=\"message-error\">Invalid Username or Password.</div>";
            //            }
            //        }
            //        else if (user.IsLockedOut)
            //        {
            //            ViewBag.ReturnUrl = returnUrl;
            //            ViewBag.LoginError =
            //                "<div class=\"message-error\">Your account has been locked. Please contact administrator for further details..</div>";
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.ReturnUrl = returnUrl;
            //        ViewBag.LoginError = "<div class=\"message-error\">You have not registered account with us!</div>";
            //    }
            //}
            if (otp.Equals(enterotp))
            {
                TempData["valid"] = "OTP verified..!";
                //string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                //using (SqlConnection conn = new SqlConnection(constr))
                //{
                //    string username = "";
                //    string password = "";
                //    string mobile = (string)TempData["number"];
                //    SqlCommand sql = new SqlCommand("SELECT username FROM UserProfiles WHERE Mobile = @mobile", conn);
                //    SqlDataAdapter sda = new SqlDataAdapter(sql);
                //    sql.Parameters.AddWithValue("@mobile", mobile.Trim());
                //    sql.Parameters.AddWithValue("@username", username.Trim());
                //    //sql.Parameters.AddWithValue("@password", password.Trim());
                //    DataSet ds = new DataSet();
                //    sda.Fill(ds);
                //    var ar = ds.ToString();
                //    Session["returnurl"] = ar;
                //    ViewBag.ReturnUrl = Session["returnurl"];
                //    //TempData["username"] = username;
                //    //TempData["password"] = password;
                //    return RedirectToAction("login", "account");
                //}
                return RedirectToAction("searchjobs", "job");
            }


            else
            {
                TempData["invalid"] = "You Entered Wrong OTP try again!";
                return RedirectToAction("SendOTP", "account");
            }
        }

        [AllowAnonymous]
        public ActionResult Resend(long? id, string returnUrl)
        {
            if (User != null)
            {
                UserInfoEntity uinfo = _service.Get(User.Id);
                string token = UIHelper.Get6DigitCode();
                _service.GenerateToken(User.Id, token);

                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                var body = reader.ReadToEnd();
                body = body.Replace("@@receiver", uinfo.FullName);

                body = body.Replace("@@code", token);
                var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                    string.Format("/Confirm?id={0}&token={1}", User.Id, token);

                body = body.Replace("@@url", hosturl);

                string[] receipent = { User.Username };
                var subject = "Confirm Your Email Address";

                AlertService.Instance.SendMail(subject, receipent, body);

                //TempData["UpdateData"] = String.Format("We have sent confirmation link to your email <b>{0}</b>!<br/>Kindly check your email address.", UserInfo.Username);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    //return Redirect(returnUrl);                   
                    return RedirectToAction("Confirm", new { id = uinfo.Id, returnUrl = returnUrl });
                }
                else
                {
                    return RedirectToAction("Confirm", new { id = uinfo.Id });
                }
            }
            else
            {
                if (id != null)
                {
                    UserInfoEntity uinfo = _service.Get(id.Value);
                    string token = UIHelper.Get6DigitCode();
                    _service.GenerateToken(uinfo.Id, token);

                    var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                    var body = reader.ReadToEnd();
                    body = body.Replace("@@receiver", uinfo.FullName);
                    body = body.Replace("@@code", token);
                    var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                        string.Format("/Confirm?id={0}&token={1}", uinfo.Id, token);

                    body = body.Replace("@@url", hosturl);

                    string[] receipent = { uinfo.Username };
                    var subject = "Confirm Your Email Address";

                    AlertService.Instance.SendMail(subject, receipent, body);

                    //TempData["UpdateData"] = String.Format("We have sent confirmation link to your email <b>{0}</b>!<br/>Kindly check your email address.", UserInfo.Username);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Confirm", new { id = uinfo.Id, returnUrl = returnUrl });
                    }
                    else
                    {
                        return RedirectToAction("Confirm", new { id = uinfo.Id, returnUrl = returnUrl });
                    }
                }
            }
            return View();
        }


        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult ChangeEmailAddress(string email, string returnUrl)
        {
            var model = new ChangeEmailAddressModel();
            string username = User != null ? User.Username : email;
            if (!string.IsNullOrEmpty(username))
            {
                model.OldEmailAddress = username;
                model.ReturnUrl = returnUrl;
            }
            return View(model);
        }

        [Authorize]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeEmailAddress(ChangeEmailAddressModel model)
        {
            //if (model.NewEmailAddress.ToLower().Contains("joblisting.com"))
            //{
            //    TempData["Error"] = "joblisting.com not allowed!";
            //    return RedirectToAction("ChangeEmailAddress");
            //}

            if (ModelState.IsValid)
            {
                string existing_email = model.OldEmailAddress.Trim().ToLower();
                LoginHistoryEntity history = _service.GetLoginHistory(existing_email);
                string new_email = model.NewEmailAddress.Trim().ToLower();
                bool exist = MemberService.Instance.IsExist(new_email);
                if (exist)
                {
                    TempData["Error"] = string.Format("This <b>{0}</b> is already in use and cannot be used!", new_email);
                    return RedirectToAction("ChangeEmailAddress");
                }
                else
                {
                    string token = UIHelper.Get6DigitCode();
                    int stat = _service.ChangeEmail(existing_email, new_email, token);
                    if (stat > 0)
                    {
                        if (UserInfo != null && UserInfo.IsConfirmed == true)
                        {
                            //send email to old address  to take confirmation;
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/email_change.html"));
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", UserInfo.FullName);
                            body = body.Replace("@@oldemail", existing_email);
                            body = body.Replace("@@newemail", new_email);
                            var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                 string.Format("/confirm?id={0}&token={1}&type=ecr", UserInfo.Id, token);

                            body = body.Replace("@@url", hosturl);

                            string[] receipent = { existing_email };
                            var subject = "Confirm your email";

                            AlertService.Instance.SendMail(subject, receipent, body);
                            string msgtext = string.Format("We have sent confirmation link to your existing email: <b>{0}</b><br/>Before new email: <b>{1}</b> take place, confirmation is required.", existing_email, new_email);
                            TempData["UpdateData"] = msgtext;
                        }
                        else if ((UserInfo != null && UserInfo.IsConfirmed == false))
                        {
                            //send email to new address to send verification code and link;                            
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", UserInfo.FullName);
                            body = body.Replace("@@code", token);
                            var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                string.Format("/Confirm?id={0}&token={1}", UserInfo.Id, token);

                            body = body.Replace("@@url", hosturl);

                            string[] receipent = { new_email };
                            var subject = "Confirm Your Email Address";

                            AlertService.Instance.SendMail(subject, receipent, body);

                            //string msgtext = string.Format("<p>Kindly check your email <b>{0}</b> for a link to confirm email.", new_email);
                            //TempData["UpdateData"] = msgtext;
                        }
                    }

                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return RedirectToAction("ChangeEmailAddress");
                    }
                    else
                    {
                        return Redirect(model.ReturnUrl);
                    }
                }
            }
            return View();
        }

        [Authorize]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailAddressModel model)
        {
            string message = string.Empty;
            ResponseContext res = new ResponseContext();
            //if (model.NewEmailAddress.ToLower().Contains("joblisting.com"))
            //{
            //    res.Type = "Error";
            //    res.Id = -1;
            //    res.Message = "joblisting.com not allowed!";
            //    return Json(res, JsonRequestBehavior.AllowGet);
            //}

            if (ModelState.IsValid)
            {
                string existing_email = model.OldEmailAddress.Trim().ToLower();
                LoginHistoryEntity history = _service.GetLoginHistory(existing_email);
                string new_email = model.NewEmailAddress.Trim().ToLower();
                bool exist = MemberService.Instance.IsExist(new_email);
                if (exist)
                {
                    res.Type = "Error";
                    res.Id = -1;
                    res.Message = string.Format("This <b>{0}</b> is already in use and cannot be used!", new_email);
                }
                else
                {
                    string token = UIHelper.Get6DigitCode();
                    int stat = _service.ChangeEmail(existing_email, new_email, token);
                    if (stat > 0)
                    {
                        if (UserInfo != null && UserInfo.IsConfirmed == true)
                        {
                            //send email to old address  to take confirmation;
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/email_change.html"));
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", UserInfo.FullName);
                            body = body.Replace("@@oldemail", existing_email);
                            body = body.Replace("@@newemail", new_email);
                            var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                 string.Format("/confirm?id={0}&token={1}&type=ecr", UserInfo.Id, token);

                            body = body.Replace("@@url", hosturl);

                            string[] receipent = { existing_email };
                            var subject = "Confirm your email";

                            AlertService.Instance.SendMail(subject, receipent, body);
                            string msgtext = string.Format("We have sent confirmation link to your existing email: <b>{0}</b><br/>Before new email: <b>{1}</b> take place, confirmation is required.", existing_email, new_email);

                            res.Type = "Error";
                            res.Id = -1;
                            res.Message = msgtext;
                        }
                        else if ((UserInfo != null && UserInfo.IsConfirmed == false))
                        {
                            //send email to new address to send verification code and link;                            
                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", UserInfo.FullName);
                            body = body.Replace("@@code", token);
                            var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                string.Format("/Confirm?id={0}&token={1}", UserInfo.Id, token);

                            body = body.Replace("@@url", hosturl);

                            string[] receipent = { new_email };
                            var subject = "Confirm Your Email Address";

                            AlertService.Instance.SendMail(subject, receipent, body);
                            string msgtext = string.Format("<p>Kindly check your email <b>{0}</b> for a link to confirm email.", new_email);

                            res.Type = "Success";
                            res.Id = 1;
                            res.Message = msgtext;
                        }
                    }
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult RegisterOrLogin(string returnUrl)
        {
            if (returnUrl == null)
            {
                if (Request.IsAuthenticated)
                {
                    if (User.IsInRole("Jobseeker"))
                    {
                        return RedirectToAction("Index", "Network");
                    }
                    if (User.IsInRole("RecruitmentAgency"))
                    {
                        return RedirectToAction("Index", "Network");
                    }
                    if (User.IsInRole("Employers"))
                    {
                        return RedirectToAction("Index", "Network");
                    }
                    if (User.IsInRole("Institute"))
                    {
                        return RedirectToAction("Index", "Network");
                    }
                    if (User.IsInRole("Student"))
                    {
                        return RedirectToAction("Index", "Network");
                    }
                    if (User.IsInRole("Interns"))
                    {
                        return RedirectToAction("Index", "Network");
                    }
                    if (User.IsInRole("SuperUser") || User.IsInRole("Administrator"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
            }

            return View();
        }

        [Authorize]
        public ActionResult ConfirmAccount(string id, string token, string returnUrl)
        {
            var confirmationToken = Request["token"];
            if (!String.IsNullOrEmpty(confirmationToken))
            {
                UserInfoEntity uinfo = _service.Get(Convert.ToInt64(id));
                if (uinfo != null)
                {
                    if (uinfo.ConfirmationToken.Equals(new Guid(token)) && uinfo.Username.Equals(User.Username))
                    {
                        int stat = _service.Confirm(uinfo.Id);
                        if (stat > 0)
                        {
                            TempData["UpdateData"] = string.Format("Account confirmed successfully!");
                        }
                        else
                        {
                            TempData["Error"] = string.Format("Unable to confirmed your account, please try again!");
                        }
                    }
                    else
                    {
                        TempData["Error"] = string.Format("Confirmation link is expired!");
                    }
                    return RedirectToUrl(uinfo);
                }
            }
            else
            {
                TempData["Error"] = string.Format("Unable to confirmed your account, please try again!");
            }
            return RedirectToAction("Index", "Network");
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult ConfirmEmail(string id, string token)
        {
            UserProfile profile = new UserProfile();
            UserInfoEntity uinfo = MemberService.Instance.GetUserInfo(Convert.ToInt64(id));
            var confirmationToken = Request["token"];
            if (!String.IsNullOrEmpty(confirmationToken))
            {
                profile = MemberService.Instance.Get(Convert.ToInt64(id));

                if (profile.ConfirmationToken.Equals(token))
                {
                    profile.Username = profile.Username;
                    profile.IsConfirmed = true;
                    profile.IsValidUsername = true;
                    profile.ConfirmationToken = null;
                    MemberService.Instance.Update(profile);

                    TempData["UpdateData"] = string.Format("Your email address <b>{0}</b> is confirmed now!", profile.NewUsername);
                }
                else
                {
                    TempData["Error"] = "Confirmation link is expired!";
                }
            }
            else
            {
                TempData["Error"] = "Unable to confirmed your email, please try again!";
            }
            return RedirectToUrl(uinfo);
        }

        [AllowAnonymous]
        [UrlPrivilegeFilter]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.error = TempData["error"];
            ViewBag.SaveData = Session["SaveData"];
            ViewBag.SaveData1 = TempData["number"];
            ViewBag.invalid = TempData["invalid"];
            MetaData metaData = SharedService.Instance.GetMetaData("home");
            ViewBag.Title = metaData.Title;
            ViewBag.Description = metaData.Description;
            ViewBag.Keywords = metaData.Keywords;

            ViewBag.ReturnUrl = returnUrl;
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (!returnUrl.ToLower().Contains("indeed"))
                {
                    ViewBag.ReturnUrl = returnUrl.Replace(string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), "");
                }
                else
                {
                    if (User != null)
                    {
                        UserInfoEntity uinfo = _service.Get(User.Id);
                        if (uinfo != null)
                        {
                            if (uinfo.IsJobseeker)
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                if (uinfo.Role != SecurityRoles.Employers)
                                {
                                    return Redirect(string.Format("/job/preview?returnUrl={0}", returnUrl));
                                }
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                if (Request.IsAuthenticated)
                {
                    if (User != null && User.Info != null)
                    {
                        if (User.Info.Role == SecurityRoles.Jobseeker || User.Info.Role == SecurityRoles.Employers || User.Info.Role == SecurityRoles.RecruitmentAgency || User.Info.Role == SecurityRoles.Institute || User.Info.Role == SecurityRoles.Student || User.Info.Role == SecurityRoles.Interns)
                        {
                            return RedirectToUrl(_service.Get(User.Id));
                        }
                        if (User.IsInRole("SuperUser") || User.IsInRole("Administrator"))
                        {
                            //return RedirectToAction("Index", "Admin");
                            return Redirect("/Admin/JobsByCompanyAll");
                        }
                    }
                }
            }
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        // [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl = null)
        {
            if (Request.Browser.Cookies == false)
            {
                TempData["Error"] = "<b>Cookies are Disabled</b><br/>Kindly enable cookies in your browser and refresh.";

                return View();
            }

            //if (ModelState.IsValid)
            if (model.LoginUsername != null && model.LoginPassword != null)
            {
                string username = model.LoginUsername.Trim().ToLower();
                string passwrod = model.LoginPassword.Trim();

                var user = Membership.GetUser(username);
                UserInfoEntity uinfo = _service.Get(username);
                if (user != null && (uinfo != null && uinfo.IsActive))
                {
                    if (!user.IsLockedOut)
                    {
                        bool ValidUser = false;
                        if (model.IsexternalLogin)
                            ValidUser = true;
                        else
                            ValidUser = WebSecurity.Login(username, passwrod, model.RememberMe);
                        if (ValidUser)
                        {
                            if (!string.IsNullOrEmpty(uinfo.Provider))
                            {
                                OAuthWebSecurity.Login(uinfo.Provider.ToString(), uinfo.ProviderId, model.RememberMe);
                            }

                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                     1,
                                     uinfo.Id.ToString(),
                                     DateTime.UtcNow,
                                     DateTime.UtcNow.AddHours(10),
                                     model.RememberMe,
                                     "");

                            string encTicket = FormsAuthentication.Encrypt(authTicket);
                            System.Web.HttpCookie faCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                            if (authTicket.IsPersistent)
                            {
                                faCookie.Expires = authTicket.Expiration;
                            }
                            Response.Cookies.Add(faCookie);

                            var bytes = System.Text.Encoding.UTF8.GetBytes(model.LoginPassword);
                            _service.TrackLoginHistory(username, Request.UserHostAddress, Request.Browser.Browser, Convert.ToBase64String(bytes));
                            _service.TrackUserStatus(uinfo.Id, (int)OnlineStatus.Online, DateTime.Now);

                            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.ToLower().Contains("indeed"))
                            {
                                if (uinfo.IsJobseeker)
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    if (uinfo.Role != SecurityRoles.Employers)
                                    {
                                        return Redirect(string.Format("/job/preview?returnUrl={0}", returnUrl));
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("/Jobseeker/Download?id"))
                            {
                                if ((SecurityRoles)uinfo.Type != SecurityRoles.Employers)
                                {
                                    TempData["Error"] = "Only Employers can download resume!";
                                }
                                else
                                {
                                    TempData["DownloadUrl"] = returnUrl;
                                }
                                return RedirectToUrl(uinfo);
                            }
                            else
                            {
                                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    TempData["LoginStatus"] = "LoggedIn";
                                    if (uinfo.Type == 4 || uinfo.Type == 13)
                                    {
                                        TempData["UpdateData"] = string.Format("Welcome1&nbsp;{0}!<br />Check&nbsp;<a href=\"/jobseeker/careertips\" style=\"color:#34ba08\">Career&nbsp;Tips</a>!", uinfo.FirstName);
                                    }
                                    else
                                    {
                                        TempData["UpdateData"] = string.Format("Welcome&nbsp;{0}!", uinfo.FirstName);
                                    }
                                   
                                 
                                    return RedirectToUrl(uinfo);
                                }
                            }
                        }
                        else
                        {
                            ViewBag.ReturnUrl = returnUrl;
                            ViewBag.LoginError = "<div class=\"message-error\">Invalid Username or Password.</div>";
                        }
                    }
                    else if (user.IsLockedOut)
                    {
                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginError =
                            "<div class=\"message-error\">Your account has been locked. Please contact administrator for further details..</div>";
                    }
                }
                else
                {
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginError = "<div class=\"message-error\">You have not registered account with us!</div>";
                }
            }

            
            var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
          
            return View();
           
        }

        [UrlPrivilegeFilter]
        public ActionResult LogOff(string ReturnUrl)
        {
            if (User != null)
            {
                UserInfoEntity profile = _service.Get(User.Id);
                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    if (profile != null)
                    {
                        var onlineUser = dataHelper.GetSingle<OnlineUser>("Id", profile.Id);
                        if (onlineUser != null)
                        {
                            onlineUser.Status = (int)OnlineStatus.Offline;
                            dataHelper.Update<OnlineUser>(onlineUser);
                        }
                    }
                }
            }
            WebSecurity.Logout();
            FormsAuthentication.SignOut();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return RedirectToAction("Login", "Account", new { ReturnUrl = ReturnUrl });
        }

        [AllowAnonymous]
        [UrlPrivilegeFilter]
        public ActionResult Register(string returnUrl = "", RegisterModel model = null)
        {

            ViewBag.FirstName = model.FirstName;
            ViewBag.LastName = model.LastName;
            ViewBag.Username = model.Username;
            ViewBag.Mobile = model.Mobile;

            //string G_url = GoogleAuthURL();
            //if (G_url != "")
            //    ViewBag.GoogleUrl = G_url;

            MetaData metaData = SharedService.Instance.GetMetaData("home");
            ViewBag.Title = metaData.Title;
            ViewBag.Description = metaData.Description;
            ViewBag.Keywords = metaData.Keywords;
            ViewBag.ReturnUrl = returnUrl;
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl.Replace(string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), "");
            }
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        
        //[ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string returnUrl = null)
        {
            SecurityRoles account = (SecurityRoles)model.AccountType;
            UserInfoEntity uinfo = null;
            RegisterEntity regentity = null;
            if (model != null)
            {
                //if (model.Username.ToLower().Contains("joblisting.com"))
                //{
                //    TempData["Error"] = "joblisting.com not allowed!";
                //    return RedirectToAction("Register", new { returnUrl = returnUrl });
                //}


                if (account == SecurityRoles.Jobseeker)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(model.Title))
                        {
                            TempData["Error"] = "Provide Current role!";
                            return RedirectToAction("Register", new { returnUrl = returnUrl });
                        }


                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        TempData["Error"] = "Provide Current role!";
                    }

                }

                bool exist = MemberService.Instance.IsExist(model.Username.Trim().ToLower());
                if (exist == false)
                {
                    WebSecurity.CreateUserAndAccount(model.Username.Trim().ToLower(), model.Password.Trim());
                    WebSecurity.Login(model.Username.Trim().ToLower(), model.Password.Trim());
                    if (account == SecurityRoles.Jobseeker)
                    {
                        regentity = new RegisterEntity()
                        {
                            Username = model.Username.Trim().ToLower(),
                            Type = model.AccountType,
                            FirstName = model.FirstName.ToString().Trim().TitleCase(),
                            LastName = model.LastName.ToString().Trim().TitleCase(),
                            Mobile = model.Mobile.ToString().Trim().TitleCase(),
                            //Language = model.Language.ToString().Trim().TitleCase(),
                            //Gender = model.Gender.ToString().Trim().TitleCase(),
                            Title = (!string.IsNullOrEmpty(model.Title) ? model.Title.ToString().Trim().TitleCase() : null),
                            Category = model.Category,
                            CountryId = model.CountryId,
                            ConfirmationToken = UIHelper.Get6DigitCode()
                        };
                    }
                    else if (account == SecurityRoles.Employers)
                    {
                        regentity = new RegisterEntity()
                        {
                            Username = model.Username.Trim().ToLower(),
                            Type = model.AccountType,
                            FirstName = model.FirstName.ToString().Trim().TitleCase(),
                            LastName = model.LastName.ToString().Trim().TitleCase(),

                            Company = (!string.IsNullOrEmpty(model.Company) ? model.Company.ToString().Trim().TitleCase() : null),
                            Category = model.Category,
                            CountryId = model.CountryId,
                            ConfirmationToken = UIHelper.Get6DigitCode()
                        };
                    }
                    else if (account == SecurityRoles.Institute)
                    {
                        regentity = new RegisterEntity()
                        {
                            Username = model.Username.Trim().ToLower(),
                            Type = model.AccountType,
                            FirstName = model.FirstName.ToString().Trim().TitleCase(),
                            LastName = model.LastName.ToString().Trim().TitleCase(),
                            Company = (!string.IsNullOrEmpty(model.University) ? model.University.ToString().Trim().TitleCase() : null),

                            University = (!string.IsNullOrEmpty(model.University) ? model.University.ToString().Trim().TitleCase() : null),
                            Category = model.Category,
                            CountryId = model.CountryId,
                            ConfirmationToken = UIHelper.Get6DigitCode()
                        };
                    }
                    else if (account == SecurityRoles.Student)
                    {
                        regentity = new RegisterEntity()
                        {
                            Username = model.Username.Trim().ToLower(),
                            Type = model.AccountType,
                            FirstName = model.FirstName.ToString().Trim().TitleCase(),
                            LastName = model.LastName.ToString().Trim().TitleCase(),

                            CurrentEmployerToYear = (!string.IsNullOrEmpty(model.CurrentEmployerToYear) ? model.CurrentEmployerToYear.ToString().Trim().TitleCase() : null),
                            Category = model.Category,

                            Mobile = model.Mobile,
                            University = model.Universityname,
                            CountryId = model.CountryId,
                            ConfirmationToken = UIHelper.Get6DigitCode()
                        };
                    }
                    else if (account == SecurityRoles.Interns)
                    {
                        regentity = new RegisterEntity()
                        {
                            Username = model.Username.Trim().ToLower(),
                            Type = model.AccountType,
                            FirstName = model.FirstName.ToString().Trim().TitleCase(),
                            LastName = model.LastName.ToString().Trim().TitleCase(),
                            Mobile = model.Mobile.ToString().Trim().TitleCase(),
                            CurrentEmployerToYear = (!string.IsNullOrEmpty(model.CurrentEmployerToYear) ? model.CurrentEmployerToYear.ToString().Trim().TitleCase() : null),

                            University = model.Universityname,
                            CountryId = model.CountryId,
                            Category = model.Category,
                            ConfirmationToken = UIHelper.Get6DigitCode()
                        };
                    }
                    else if (account == SecurityRoles.RecruitmentAgency)
                    {
                        regentity = new RegisterEntity()
                        {
                            Username = model.Username.Trim().ToLower(),
                            Type = model.AccountType,
                            FirstName = model.FirstName.ToString().Trim().TitleCase(),
                            LastName = model.LastName.ToString().Trim().TitleCase(),
                            //CurrentEmployerToYear = (!string.IsNullOrEmpty(model.CurrentEmployerToYear) ? model.CurrentEmployerToYear.ToString().Trim().TitleCase() : null),

                            Company = model.CompanyName,
                            University = model.RecruiterName,
                            Category = model.Category,
                            CountryId = model.CountryId,
                            ConfirmationToken = UIHelper.Get6DigitCode()
                        };
                    }
                    uinfo = MemberService.Instance.Register(regentity);
                    Roles.AddUserToRoles(model.Username, new[] { account.ToString() });

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string userData = "";

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             uinfo.Id.ToString(),
                             DateTime.UtcNow,
                             DateTime.UtcNow.AddHours(10),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    System.Web.HttpCookie faCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    var bytes = System.Text.Encoding.UTF8.GetBytes(model.Password);
                    DomainService.Instance.ManageLoginHistory(regentity.Username, Request.UserHostAddress, Request.Browser.Browser, Convert.ToBase64String(bytes));

                    if (!string.IsNullOrEmpty(regentity.ConfirmationToken))
                    {
                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/registration.html"));
                        var body = reader.ReadToEnd();
                        body = body.Replace("@@user", string.Format("{0} {1}", regentity.FirstName, regentity.LastName));
                        body = body.Replace("@@code", regentity.ConfirmationToken);

                        var hosturl = Request.Url.GetLeftPart(UriPartial.Authority) +
                            string.Format("/Confirm?id={0}&token={1}", uinfo.Id, regentity.ConfirmationToken);

                        body = body.Replace("@@url", hosturl);
                        if (account == SecurityRoles.Jobseeker)
                        {
                            string tiplink = string.Format("You may also check <a href=\"{0}\">Career Tips</a>!<br />", string.Format("{0}/jobseeker/careertips", Request.Url.GetLeftPart(UriPartial.Authority)));
                            body = body.Replace("@@tiplink", tiplink);
                        }
                        else
                        {
                            body = body.Replace("@@tiplink", "");
                        }
                        string[] receipent = { model.Username.Trim().ToLower() };
                        var subject = "Confirm Your Email Address";

                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToUrl(uinfo, returnUrl);
                    }
                    else
                    {
                        return RedirectToUrl(uinfo);
                    }
                }
                else
                {
                    if (WebSecurity.Login(model.Username.Trim().ToLower(), model.Password.Trim()))
                    {
                        uinfo = _service.Get(model.Username);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string userData = "";

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                 1,
                                 uinfo.Id.ToString(),
                                 DateTime.UtcNow,
                                 DateTime.UtcNow.AddHours(10),
                                 false,
                                 userData);

                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        System.Web.HttpCookie faCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);

                        var bytes = System.Text.Encoding.UTF8.GetBytes(model.Password);
                        _service.TrackLoginHistory(model.Username.ToString().Trim().ToLower(), Request.UserHostAddress, Request.Browser.Browser, Convert.ToBase64String(bytes));
                        _service.TrackUserStatus(uinfo.Id, (int)OnlineStatus.Online, DateTime.Now);

                        if (uinfo != null && uinfo.IsConfirmed == false)
                        {
                            string token = UIHelper.Get6DigitCode();
                            _service.GenerateToken(uinfo.Id, token);

                            var reader = new StreamReader(Server.MapPath("~/Templates/Mail/resend_confirmation.html"));
                            var body = reader.ReadToEnd();
                            body = body.Replace("@@receiver", uinfo.FullName);
                            body = body.Replace("@@code", token);
                            var hosturl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                string.Format("/Confirm?id={0}&token={1}", uinfo.Id, token);

                            body = body.Replace("@@url", hosturl);

                            string[] receipent = { uinfo.Username };
                            var subject = "Confirm Your Email Address";

                            AlertService.Instance.SendMail(subject, receipent, body);

                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return RedirectToUrl(uinfo, returnUrl);
                            }
                            else
                            {
                                return RedirectToUrl(uinfo);
                            }
                        }
                        else if (uinfo != null && uinfo.IsConfirmed == true)
                        {
                            if (returnUrl.ToLower().Contains("indeed"))
                            {
                                if (uinfo.IsJobseeker)
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    if (uinfo.Role != SecurityRoles.Employers)
                                    {
                                        return Redirect(string.Format("/job/preview?returnUrl={0}", returnUrl));
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("/Jobseeker/Download?id"))
                            {
                                if ((SecurityRoles)uinfo.Type != SecurityRoles.Employers)
                                {
                                    TempData["Error"] = "Only Employers can download resume!";
                                }
                                else
                                {
                                    TempData["DownloadUrl"] = returnUrl;
                                }
                                return RedirectToUrl(uinfo);
                            }
                            else
                            {
                                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    TempData["LoginStatus"] = "LoggedIn";
                                    if (uinfo.Type == 4)
                                    {
                                        TempData["UpdateData"] = string.Format("Welcome&nbsp;{0}!<br />Check&nbsp;<a href=\"/jobseeker/careertips\" style=\"color:#34ba08\">Career&nbsp;Tips</a>!", uinfo.FirstName);
                                    }
                                    else
                                    {
                                        TempData["UpdateData"] = string.Format("Welcome&nbsp;{0}!", uinfo.FirstName);
                                    }
                                    return RedirectToUrl(uinfo);
                                }
                            }
                        }
                        else
                        {
                            return RedirectToAction("Failed", new { email = model.Username, returnUrl = returnUrl });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Failed", new { email = model.Username, returnUrl = returnUrl });
                    }
                }
            }

            else
            {
                return RedirectToAction("Register", new { returnUrl = returnUrl });
            }
        }

        [AllowAnonymous]
        public ActionResult Failed(string email, string returnUrl)
        {
            ViewBag.Email = email;
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
        //
        // POST: /Account/Disassociate

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerId)
        {
            var ownerAccount = OAuthWebSecurity.GetUserName(provider, providerId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Username)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (
                    var scope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Username));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Username).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        [Authorize]
        public ActionResult Manage(ManageMessageId? message)
        {
            TempData["UpdateData"] =
                message == ManageMessageId.ChangePasswordSuccess
                    ? "Your password has been changed."
                    : message == ManageMessageId.SetPasswordSuccess
                        ? "Your password has been set."
                        : message == ManageMessageId.RemoveLoginSuccess
                            ? "The external login was removed."
                            : null;
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Username));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePass(LocalPasswordModel model)
        {
            ResponseContext context = new ResponseContext();

            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$");

            if (model.NewPassword.Equals(model.ConfirmPassword))
            {
                if (regex.IsMatch(model.NewPassword))
                {
                    try
                    {
                        bool changed = WebSecurity.ChangePassword(User.Username, model.OldPassword,
                                   model.NewPassword);
                        if (changed)
                        {
                            context = new ResponseContext()
                            {
                                Id = 1,
                                Type = "Success",
                                Message = "Password changed successfully!"
                            };
                        }
                        else
                        {
                            context = new ResponseContext()
                            {
                                Id = 0,
                                Type = "Failed",
                                Message = "Unable to change password!"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        context = new ResponseContext()
                        {
                            Id = 0,
                            Type = "Failed",
                            Message = "Old password and New Password does not match!"
                        };
                    }
                }
                else
                {
                    context = new ResponseContext()
                    {
                        Id = 0,
                        Type = "Failed",
                        Message = "Password Min. 9 numbers & letters mixed!"
                    };
                }
            }
            else
            {
                context = new ResponseContext()
                {
                    Id = 0,
                    Type = "Failed",
                    Message = "New Password and Confirm Password does not match!"
                };
            }
            return Json(context, JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Account/Manage
        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Username));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Username, model.OldPassword,
                            model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                var state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Username, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("",
                            string.Format(
                                "Unable to create local account. An account with the name \"{0}\" may already exist.",
                                User.Username));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin


        //[HttpPost]
        //[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    //ControllerContext.HttpContext.Session.RemoveAll();
        //    //return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //
        // GET: /Account/ExternalLoginCallback

        //[AllowAnonymous]
        //public ActionResult ExternalLoginCallback(string returnUrl)
        //{
        //    var provider = string.Empty;
        //    var model = new RegisterExternalLoginModel();
        //    var loginInfo = AuthenticationManager.GetExternalLoginInfoAsync();

        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    if (loginInfo.Result != null)
        //    {
        //        provider = loginInfo.Result.Login.LoginProvider.ToLower();

        //        ViewBag.ProviderDisplayName = provider.TitleCase();

        //        if (provider.Equals("facebook"))
        //        {
        //            var claimsIdentity =
        //                AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
        //            if (claimsIdentity != null)
        //            {
        //                var facebookAccessToken = claimsIdentity.Result.FindFirstValue("FacebookAccessToken");
        //                var fb = new FacebookClient(facebookAccessToken);
        //                var profileData = (IDictionary<string, object>)fb.Get("/me?fields=id,first_name,last_name,email");

        //                model = new RegisterExternalLoginModel();
        //                object email = null;
        //                object firstName = null;
        //                object lastName = null;
        //                object facebookId = null;

        //                model.ProviderAccessToken = facebookAccessToken;
        //                model.Provider = loginInfo.Result.Login.LoginProvider;

        //                if (profileData.TryGetValue("email", out email))
        //                {
        //                    if (email != null)
        //                    {
        //                        model.UserName = Convert.ToString(email);
        //                        model.ProviderUsername = Convert.ToString(email);
        //                    }
        //                }

        //                if (profileData.TryGetValue("first_name", out firstName))
        //                {
        //                    if (firstName != null)
        //                    {
        //                        model.FirstName = Convert.ToString(firstName);
        //                    }
        //                }

        //                if (profileData.TryGetValue("last_name", out lastName))
        //                {
        //                    if (lastName != null)
        //                    {
        //                        model.LastName = Convert.ToString(lastName);
        //                    }
        //                }
        //                if (profileData.TryGetValue("id", out facebookId))
        //                {
        //                    if (facebookId != null)
        //                    {
        //                        model.ProviderId = Convert.ToString(facebookId);
        //                    }
        //                }
        //            }
        //        }
        //        else if (provider.Equals("linkedin"))
        //        {
        //            var claimsIdentity =
        //                AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
        //            if (claimsIdentity != null)
        //            {
        //                var accessToken = claimsIdentity.Result.FindAll("urn:linkedin:accesstoken").First();
        //                var client = new LinkedIn.Api.Client.Owin.LinkedInApiClient(HttpContext.GetOwinContext().Request,
        //                    accessToken.Value);

        //                var profileApi = new LinkedInProfileApi(client);
        //                var userProfile = profileApi.GetFullProfileAsync();

        //                model = new RegisterExternalLoginModel
        //                {
        //                    UserName = loginInfo.Result.Email,
        //                    FirstName = userProfile.Result.FirstName,
        //                    LastName = userProfile.Result.LastName,
        //                    Provider = loginInfo.Result.Login.LoginProvider,
        //                    ProviderId = loginInfo.Result.Login.ProviderKey,
        //                    ProviderUsername = loginInfo.Result.Email,
        //                    ProviderAccessToken = accessToken.Value
        //                };
        //            }
        //        }
        //        else if (provider.Equals("twitter"))
        //        {
        //            var claimsIdentity =
        //                AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

        //            model = new RegisterExternalLoginModel
        //            {
        //                UserName = loginInfo.Result.DefaultUserName,
        //                Provider = loginInfo.Result.Login.LoginProvider,
        //                ProviderId = loginInfo.Result.Login.ProviderKey
        //            };
        //        }
        //        else if (provider.Equals("googleplus"))
        //        {
        //            var claimsIdentity =
        //                AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
        //            model = new RegisterExternalLoginModel
        //            {
        //                UserName = loginInfo.Result.Email,
        //                Provider = loginInfo.Result.Login.LoginProvider,
        //                ProviderId = loginInfo.Result.Login.ProviderKey
        //            };
        //        }
        //        else if (provider.Equals("google"))
        //        {


        //            var claimsIdentity = AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie).GetAwaiter().GetResult();
        //            var emailClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email);
        //            var lastNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Surname);
        //            var givenNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.GivenName);

        //            model = new RegisterExternalLoginModel
        //            {
        //                UserName = emailClaim.Value,
        //                FirstName = givenNameClaim.Value,
        //                LastName = lastNameClaim.Value,
        //                Provider = loginInfo.Result.Login.LoginProvider,
        //                ProviderId = loginInfo.Result.Login.ProviderKey
        //            };
        //        }
        //        else
        //        {
        //            model = new RegisterExternalLoginModel
        //            {
        //                UserName = loginInfo.Result.Email,
        //                Provider = loginInfo.Result.Login.LoginProvider,
        //                ProviderId = loginInfo.Result.Login.ProviderKey
        //            };
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(model.Provider) && !string.IsNullOrEmpty(model.ProviderId))
        //    {
        //        UserProfile profile = null;
        //        if (!string.IsNullOrEmpty(model.UserName))
        //        {
        //            profile = MemberService.Instance.Get(model.UserName);
        //        }
        //        else
        //        {
        //            using (JobPortalEntities context = new JobPortalEntities())
        //            {
        //                DataHelper dataHelper = new DataHelper(context);
        //                Hashtable parameters = new Hashtable();

        //                parameters.Add("Provider", model.Provider);
        //                parameters.Add("ProviderId", model.ProviderId);
        //                profile = dataHelper.GetSingle<UserProfile>(parameters);
        //            }
        //        }

        //        if (profile != null && !string.IsNullOrEmpty(profile.Provider) && !string.IsNullOrEmpty(profile.ProviderId))
        //        {
        //            OAuthWebSecurity.Login(profile.Provider, profile.ProviderId, true);
        //            UserInfoEntity uinfo = _service.Get(model.UserName.Trim().ToLower());

        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            string userData = "";

        //            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
        //                     1,
        //                     profile.UserId.ToString(),
        //                     DateTime.UtcNow,
        //                     DateTime.UtcNow.AddHours(10),
        //                     true,
        //                     userData);

        //            string encTicket = FormsAuthentication.Encrypt(authTicket);
        //            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //            if (authTicket.IsPersistent)
        //            {
        //                faCookie.Expires = authTicket.Expiration;
        //            }
        //            Response.Cookies.Add(faCookie);
        //            return RedirectToUrl((SecurityRoles)profile.Type);
        //        }

        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    TempData["model"] = model;
        //    TempData["returnUrl"] = returnUrl;
        //    return View("ExternalLoginConfirmation",
        //       model);


        //}

        // POST: /Account/ExternalLoginConfirmation

        //[HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation()
        {
            RegisterExternalLoginModel model = (RegisterExternalLoginModel)TempData["model"];
            string returnUrl = (string)TempData["returnUrl"];
            ActionResult result = null;
            UserInfoEntity uinfo = null;

            var account = (SecurityRoles)model.AccountType;
            int age = 0;
            //if (model.UserName.ToLower().Contains("joblisting.com"))
            //{
            //    TempData["Error"] = "joblisting.com not allowed!";
            //    return View(model);
            //}

            if (account == SecurityRoles.Jobseeker)
            {
                age = DateTime.Now.Year - Convert.ToInt32(model.BYear);
                int limit = 13;
                if (age <= limit)
                {
                    TempData["Error"] = "You must be 13 years of age!";
                    ViewBag.ProviderDisplayName = model.Provider;
                    ViewBag.ReturnUrl = returnUrl;
                    return View(model);
                }
            }

            if (account == SecurityRoles.Employers)
            {
                if (string.IsNullOrEmpty(model.Company))
                {
                    TempData["Error"] = "Company Name should not be empty!";
                    return RedirectToAction("Register", new { returnUrl = returnUrl });
                }
            }
            else if (account == SecurityRoles.Jobseeker)
            {
                if (string.IsNullOrEmpty(model.Gender))
                {
                    TempData["Error"] = "Provide your gender!";
                    return RedirectToAction("Register", new { returnUrl = returnUrl });
                }

                if (string.IsNullOrEmpty(model.BDay) && string.IsNullOrEmpty(model.BMonth) && string.IsNullOrEmpty(model.BYear))
                {
                    TempData["Error"] = "Provide your birth date!";
                    return RedirectToAction("Register", new { returnUrl = returnUrl });
                }
            }
            string username = model.UserName.Trim().ToLower();
            uinfo = _service.Get(username);
            // Check if user already exists
            if (uinfo == null)
            {
                WebSecurity.CreateUserAndAccount(username, model.NewPassword);
            }

            RegisterEntity regentity = new RegisterEntity();
            regentity.Type = model.AccountType;
            regentity.Username = model.UserName.ToLower();
            regentity.FirstName = model.FirstName.Trim().TitleCase();
            regentity.LastName = model.LastName.Trim().TitleCase();
            regentity.CountryId = model.CountryId;
            regentity.Confirmed = true;
            if (account == SecurityRoles.Employers)
            {
                regentity.Company = model.Company;
            }

            if (account == SecurityRoles.Jobseeker)
            {
                regentity.Gender = model.Gender;
                regentity.DateOfBirth = model.BYear;
                regentity.Age = (age > 0 ? (byte?)age : null);
            }

            regentity.Provider = model.Provider;
            regentity.ProviderId = model.ProviderId;
            regentity.ProviderUsername = model.ProviderUsername;
            regentity.ProviderAccessToken = model.ProviderAccessToken;

            uinfo = MemberService.Instance.Register(regentity);
            try
            {
                Roles.AddUserToRoles(model.UserName, new[] { account.ToString() });
            }
            catch (Exception)
            {

            }
            MemberService.Instance.UpdateConnections(uinfo.Username);

            if (!string.IsNullOrEmpty(model.Provider) && !string.IsNullOrEmpty(model.Provider))
            {
                OAuthWebSecurity.CreateOrUpdateAccount(model.Provider, model.ProviderId, model.UserName.ToLower());
                OAuthWebSecurity.Login(model.Provider.ToLower(), model.ProviderId, true);
            }
            WebSecurity.Login(model.UserName.ToLower(), model.NewPassword, true);
            uinfo = _service.Get(username);
            result = RedirectToUrl(uinfo);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string userData = "";
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     uinfo.Id.ToString(),
                     DateTime.UtcNow,
                     DateTime.UtcNow.AddHours(10),
                     true,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            System.Web.HttpCookie faCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            if (authTicket.IsPersistent)
            {
                faCookie.Expires = authTicket.Expiration;
            }
            Response.Cookies.Add(faCookie);

            if (result != null)
            {
                return result;
            }
            else
            {
                ViewBag.ProviderDisplayName = model.Provider;
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial");
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            var accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Username);
            var externalLogins = new List<ExternalLogin>();
            foreach (var account in accounts)
            {
                var clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderId = account.ProviderUserId
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 ||
                                       OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Username));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            string controller = "Login", action = "Account";
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                var user = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                if (user != null)
                {
                    var type = (SecurityRoles)user.Type;
                    switch (type)
                    {
                        case SecurityRoles.Jobseeker:
                            controller = "Network";
                            action = "Index";
                            break;
                        case SecurityRoles.Employers:
                            controller = "Network";
                            action = "Index";
                            break;
                        case SecurityRoles.Institute:
                            controller = "Network";
                            action = "Index";
                            break;
                        case SecurityRoles.Student:
                            controller = "Network";
                            action = "Index";
                            break;
                        case SecurityRoles.RecruitmentAgency:
                            controller = "Network";
                            action = "Index";
                            break;
                        case SecurityRoles.Interns:
                            controller = "Network";
                            action = "Index";
                            break;
                        case SecurityRoles.Partner:
                            controller = "Partner";
                            action = "Index";
                            break;
                        case SecurityRoles.WebScraper:
                            controller = "Websites1";
                            action = "Employer";
                            break;
                        case SecurityRoles.Administrator:
                            controller = "Admin";
                            action = "Index";
                            break;
                        case SecurityRoles.UnverifiedUser:
                            controller = "Login";
                            action = "Account";
                            break;
                        default:
                            controller = "Login";
                            action = "Account";
                            break;
                    }
                }
            }
            return RedirectToAction(controller, action);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess
        }

        internal class ExternalLoginResult : ActionResult
        {
            private const string XsrfKey = "XsrfId";

            public ExternalLoginResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ExternalLoginResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        private RedirectToRouteResult RedirectToUrl(UserInfoEntity profile, string returnUrl = null)
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {

            RedirectToRouteResult result;
            SecurityRoles UserRole = (SecurityRoles)profile.Type;

            switch (UserRole)
            {
                case SecurityRoles.SuperUser:
                    result = RedirectToAction("JobsByCompanyAll", "Admin");
                    break;
                case SecurityRoles.WebScraper:
                    result = RedirectToAction("Websites1", "Employer");
                    break;
                case SecurityRoles.Administrator:
                    result = RedirectToAction("JobsByCompanyAll", "Admin");
                    break;
                case SecurityRoles.Employers:
                    if (returnUrl != null)
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    else
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    break;
                case SecurityRoles.Institute:
                    if (returnUrl != null)
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    else
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    break;
                case SecurityRoles.Jobseeker:
                    if (returnUrl != null && returnUrl.ToLower().Contains("indeed"))
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    else
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    break;
                case SecurityRoles.Student:
                    if (returnUrl != null && returnUrl.ToLower().Contains("indeed"))
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    else
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    break;
                case SecurityRoles.RecruitmentAgency:
                    if (returnUrl != null && returnUrl.ToLower().Contains("indeed"))
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    else
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    break;
                case SecurityRoles.Interns:
                    if (returnUrl != null && returnUrl.ToLower().Contains("indeed"))
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    else
                    {
                        result = RedirectToAction("MemberProfile", "Home", new { address = profile.PermaLink, returnUrl = returnUrl });
                    }
                    break;
                case SecurityRoles.Partner:
                    result = RedirectToAction("Index", "Partner");
                    break;
                default:
                    result = RedirectToAction("Login", "Account");
                    break;
            }
            return result;
        }

#pragma warning disable CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        private RedirectToRouteResult RedirectToUrl(SecurityRoles UserRole)
#pragma warning restore CS0246 // The type or namespace name 'SecurityRoles' could not be found (are you missing a using directive or an assembly reference?)
        {
            RedirectToRouteResult result;
            switch (UserRole)
            {
                case SecurityRoles.SuperUser:
                    result = RedirectToAction("JobsByCompanyAll", "Admin");
                    break;
                case SecurityRoles.WebScraper:
                    result = RedirectToAction("Websites1", "Employer");
                    break;
                case SecurityRoles.Administrator:
                    result = RedirectToAction("JobsByCompanyAll", "Admin");
                    break;
                case SecurityRoles.Employers:
                    result = RedirectToAction("Index", "Network");
                    break;
                case SecurityRoles.Institute:
                    result = RedirectToAction("Index", "Network");
                    break;
                case SecurityRoles.Jobseeker:
                    result = RedirectToAction("Index", "Network");
                    break;
                case SecurityRoles.Student:
                    result = RedirectToAction("Index", "Network");
                    break;
                case SecurityRoles.RecruitmentAgency:
                    result = RedirectToAction("Index", "Network");
                    break;
                case SecurityRoles.Interns:
                    result = RedirectToAction("Index", "Network");
                    break;
                case SecurityRoles.Partner:
                    result = RedirectToAction("Index", "Partner");
                    break;
                default:
                    result = RedirectToAction("Login", "Account");
                    break;
            }
            return result;
        }

        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult LoginHistory()
        {
            List<LoginHistoryEntity> loginHistoryList = DomainService.Instance.GetLoginHistory(User.Username);
            ViewBag.Model = loginHistoryList;
            return View();
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion

        [Authorize]
        public ActionResult Alerts()
        {
            IEnumerable<Alert> alerts = MemberService.Instance.GetAlerts(User.Username);

            return View(alerts);
        }

        [Authorize]
        public ActionResult UpdateAlert(int Id)
        {
            UserProfile profile = MemberService.Instance.Get(User.Username);

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Alert alert = dataHelper.GetSingle<Alert>(Id);
                AlertSetting setting = dataHelper.Get<AlertSetting>().SingleOrDefault(x => x.AlertId == Id && x.UserId == profile.UserId);
                if (setting == null)
                {
                    setting = new AlertSetting()
                    {
                        AlertId = Id,
                        Enabled = false,
                        UserId = profile.UserId,
                        DateUpdated = DateTime.Now
                    };
                    dataHelper.Add<AlertSetting>(setting);
                    TempData["UpdateData"] = string.Format("<b>{0}</b> alert disabled successfully!", alert.Description);
                }
                else
                {
                    if (setting.Enabled)
                    {
                        setting.Enabled = false;
                        TempData["UpdateData"] = string.Format("<b>{0}</b> alert disabled successfully!", alert.Description);
                    }
                    else
                    {
                        setting.Enabled = true;
                        TempData["UpdateData"] = string.Format("<b>{0}</b> alert enabled successfully!", alert.Description);
                    }
                    dataHelper.Update<AlertSetting>(setting);
                }
            }
            return RedirectToAction("Alerts");
        }
    }
}