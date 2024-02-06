using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using PagedList;
using System.Collections;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Threading.Tasks;
using JobPortal.Web.App_Start;

namespace JobPortal.Web.Controllers
{
    public class MessageController : BaseController
    {

#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
        IJobService jobService;
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public MessageController(IUserService service, IJobService jobService)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'IJobService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {
            this.jobService = jobService;
        }
        // GET: Message
        [Authorize]
        [HttpGet]
        [UrlPrivilegeFilter]
        public ActionResult Index(long? UserId = null, int? Status = null, int pageNumber = 0)
        {
            int pageSize = 10;
            int rows = 0;

            var list = new List<long>();
            var userList = new List<UserProfile>();
            SelectList senderList = new SelectList(new List<string>() { "--SELECT--" });
            SelectList receiverList = new SelectList(new List<string>() { "--SELECT--" });

            if (User != null)
            {
                UserInfoEntity uinfo = User.Info;
                if (!uinfo.IsConfirmed)
                {
                    return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
                }

                using (JobPortalEntities context = new JobPortalEntities())
                {
                    DataHelper dataHelper = new DataHelper(context);
                    var bresult = dataHelper.Get<BlockedPeople>().Where(x => x.BlockedId == User.Id).Select(x => x.BlockerId).ToList();

                    var result = dataHelper.Get<Connection>().Where(x => x.UserId == User.Id);
                    result = result.Where(x => !x.EmailAddress.Equals(uinfo.Username));
                    var userresult = dataHelper.Get<UserProfile>().Where(x => x.IsDeleted == false && x.IsActive == true);
                    List<string> connections = new List<string>();
                    if (result.Count() > 0)
                    {
                        if (Status != null)
                        {
                            if (Status == 0)
                            {
                                connections = result.Where(x => x.IsConnected == false && x.IsAccepted == false && x.IsDeleted == false && x.UserId == User.Id).Select(x => x.EmailAddress).ToList();
                            }
                            else if (Status == 1)
                            {
                                connections = result.Where(x => x.IsConnected == true && x.IsAccepted == true && x.UserId == User.Id).Select(x => x.EmailAddress).ToList();
                            }
                            else if (Status == 2)
                            {
                                connections = result.Where(x => x.IsConnected == false && x.IsAccepted == false && x.IsDeleted == true && x.UserId == User.Id).Select(x => x.EmailAddress).ToList();
                            }
                            userList = userresult.Where(x => connections.Contains(x.Username) && !bresult.Contains(x.UserId)).ToList();
                        }
                        else
                        {
                            connections = result.Select(x => x.EmailAddress).ToList();
                            userList = userresult.Where(x => connections.Contains(x.Username) && !bresult.Contains(x.UserId)).ToList();
                        }
                    }

                    if (UserId != null)
                    {
                        userList = userList.Where(x => x.UserId == UserId.Value).ToList();
                    }

                    rows = userList.Count;
                    if (userList.Count > 0)
                    {
                        userList = userList.OrderBy(x => x.UserId).Skip((pageNumber > 0 ? (pageNumber - 1) * pageSize : pageNumber * pageSize)).ToList();
                    }
                    receiverList = new SelectList(userList.Select(x => new { Id = x.UserId, Name = x.FirstName + " " + x.LastName }).ToList(), "Id", "Name");
                    ViewBag.ReceiverList = receiverList;
                }
            }

            ViewBag.Model = new StaticPagedList<UserProfile>(userList, (pageNumber == 0 ? 1 : pageNumber), pageSize, rows);
            ViewBag.Rows = rows;

            return View();
        }

        [Authorize]
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> ReceiverList(MessageFilter model)
#pragma warning restore CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list = new List<long>();
            model.UserId = User.Id;
            var userList = await _service.ReceiverList(model);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<ActionResult> Index(MessageFilter model)
#pragma warning restore CS0246 // The type or namespace name 'MessageFilter' could not be found (are you missing a using directive or an assembly reference?)
        {
            var list = new List<long>();
            var userList = new List<CommunicationEntity>();
            SelectList senderList = new SelectList(new List<string>() { "--SELECT--" });
            SelectList receiverList = new SelectList(new List<string>() { "--SELECT--" });

            if (User != null)
            {
                UserInfoEntity uinfo = User.Info;
                if (!uinfo.IsConfirmed)
                {
                    return RedirectToAction("Confirm", "Account", new { id = uinfo.Id, returnUrl = Request.Url.ToString() });
                }

                model.PageSize = 10;
                model.UserId = User.Id;
                userList = await _service.Messages(model);
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Delete all messages
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult DeleteAll(long id)
        {
            UserProfile sender = MemberService.Instance.Get(id);
            UserProfile profile = MemberService.Instance.Get(User.Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                var list = dataHelper.Get<Communication>().Where(x => x.UserId == profile.UserId && (x.ReceiverId == sender.UserId || x.SenderId == sender.UserId)).AsEnumerable();
                dataHelper.Remove<Communication>(list);
            }
            TempData["UpdateData"] = "Message(s) deleted successfully!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deletes single message
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(long Id, long SenderId)
        {
            Communication msg = MessageService.Instance.Get(Id);
            MessageService.Instance.Remove(Id);
            TempData["UpdateData"] = "Message deleted successfully!";
            return RedirectToAction("List", new { SenderId = SenderId });
        }

        /// <summary>
        /// Reply message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Reply(long id)
        {
            SendMessageModel model;
            UserProfile profile = MemberService.Instance.Get(User.Username);
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Communication entity = dataHelper.GetSingle<Communication>(id);

                if (entity.Receiver.Username.Equals(User.Username))
                {
                    var list = dataHelper.Get<Communication>().Where(x => x.ReceiverId == profile.UserId && x.SenderId == entity.SenderId);
                    foreach (Communication item in list)
                    {
                        item.Unread = false;
                        dataHelper.UpdateEntity<Communication>(item);
                    }
                }

                model = new SendMessageModel()
                {
                    MessageId = id,
                    ReceiverEmail = entity.Sender.Username,
                };
            }
            return View(model);
        }

        /// <summary>
        /// Sends message to the recipient.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Reply(SendMessageModel model)
        {
            var profile = MemberService.Instance.Get(User.Username);
            Communication msg = MessageService.Instance.Get(model.MessageId);

            var sender = MemberService.Instance.Get(msg.SenderId);
            var receiver = MemberService.Instance.Get(msg.ReceiverId);

            if (sender.Username.Equals(User.Username))
            {
                sender = MemberService.Instance.Get(msg.SenderId);
                receiver = MemberService.Instance.Get(msg.ReceiverId);
            }
            else
            {
                sender = MemberService.Instance.Get(msg.ReceiverId);
                receiver = MemberService.Instance.Get(msg.SenderId);
            }

            RedirectToRouteResult result = null;
            Communication entity = new Communication();
            if (sender != null && receiver != null)
            {
                string receiverName = (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : string.Format("{0} {1}", receiver.FirstName, receiver.LastName));
                string senderName = (!string.IsNullOrEmpty(sender.Company) ? sender.Company : string.Format("{0} {1}", sender.FirstName, sender.LastName));
                string subject = string.Empty;
                int msg_count = ConnectionHelper.MessageCounts(sender.UserId, receiver.UserId);

                MessageService.Instance.Send(model.Message, sender.UserId, receiver.UserId, User.Username, false, (msg_count > 0));

                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html"));
                var body = reader.ReadToEnd();
                body = body.Replace("@@sender", senderName);
                body = body.Replace("@@message", model.Message);
                string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, receiver.UserId);
                body = body.Replace("@@viewurl", viewurl);
                body = body.Replace("@@message", model.Message);
                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));
                body = body.Replace("@@receiver", receiverName);

                string[] receipent = { receiver.Username };
                if (msg_count > 0)
                {
                    subject = string.Format("Message Reply from {0}", senderName);
                }
                else
                {
                    subject = string.Format("Message from {0}", senderName);
                }

                AlertService.Instance.SendMail(subject, receipent, body);
                TempData["UpdateData"] = "Message sent successfully!";

                result = RedirectToAction("List", new { SenderId = receiver.UserId });
            }
            else
            {
                TempData["Error"] = "While sending message unable to retrive sender or receiver's info!";
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Send(string email, string redirect)
        {
            if (UserInfo != null && !UserInfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            var sender = MemberService.Instance.Get(User.Username);
            var receiver = MemberService.Instance.Get(email);
            bool byLoggedInUser = false;
            bool byEmailUser = false;

            BlockedPeople entity = ConnectionHelper.GetBlockedEntity(receiver.UserId, UserInfo.Id);
            if (entity != null && !entity.CreatedBy.Equals(UserInfo.Username))
            {
                TempData["Error"] = string.Format("{0} does not accept messages!", (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : string.Format("{0} {1}", receiver.FirstName, receiver.LastName)));
            }

            SendMessageModel model = new SendMessageModel()
            {
                ReceiverEmail = email
            };

            if (sender != null && receiver != null)
            {
                Connection connection = ConnectionHelper.Get(User.Username, receiver.Username);
                if (connection != null && connection.IsDeleted == false && connection.IsAccepted == false && connection.IsConnected == false)
                {
                    return RedirectToAction("List", new { SenderId = receiver.UserId });
                }
                else if (connection != null && connection.IsDeleted == false && connection.IsAccepted == true && connection.IsConnected == true)
                {
                    int counts = ConnectionHelper.GetMessageCounts(sender.UserId, receiver.UserId);
                    if (counts > 0)
                    {
                        return RedirectToAction("List", new { SenderId = receiver.UserId });
                    }
                }

                if (ConnectionHelper.IsConnected(email, sender.Username))
                {
                    byEmailUser = ConnectionHelper.IsBlocked(User.Username, receiver.UserId);
                    byLoggedInUser = ConnectionHelper.IsBlockedByMe(receiver.Username, sender.UserId);

                    if (byLoggedInUser)
                    {
                        if (sender != null)
                        {
                            TempData["Error"] = "You have blocked this person!<br/>Do you want to Unblock or Skip?<br/><a href=\"/Home/Unblock?id=" + receiver.UserId + "&redirect=/Network/Index\">Unblock</a>&nbsp;&nbsp;&nbsp;<a href=\"#\" onclick=\"window.location.reload();\">Skip</a>";
                        }
                    }
                    else if (byEmailUser)
                    {
                        TempData["Error"] = "This profile is in private mode!";
                    }
                }
            }

            ViewBag.Redirect = redirect;
            return View(model);
        }

        /// <summary>
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult PostSend(string email, string redirect)
        {
            if (UserInfo != null && !UserInfo.IsConfirmed)
            {
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            var sender = MemberService.Instance.Get(User.Username);
            var receiver = MemberService.Instance.Get(email);
            bool byLoggedInUser = false;
            bool byEmailUser = false;

            BlockedPeople entity = ConnectionHelper.GetBlockedEntity(receiver.UserId, UserInfo.Id);
            if (entity != null && !entity.CreatedBy.Equals(UserInfo.Username))
            {
                TempData["Error"] = string.Format("{0} does not accept messages!", (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : string.Format("{0} {1}", receiver.FirstName, receiver.LastName)));
            }

            SendMessageModel model = new SendMessageModel()
            {
                ReceiverEmail = email
            };

            if (sender != null && receiver != null)
            {
                Connection connection = ConnectionHelper.Get(User.Username, receiver.Username);
                if (connection != null && connection.IsDeleted == false && connection.IsAccepted == false && connection.IsConnected == false)
                {
                    return RedirectToAction("List", new { SenderId = receiver.UserId });
                }
                else if (connection != null && connection.IsDeleted == false && connection.IsAccepted == true && connection.IsConnected == true)
                {
                    int counts = ConnectionHelper.GetMessageCounts(sender.UserId, receiver.UserId);
                    if (counts > 0)
                    {
                        return RedirectToAction("List", new { SenderId = receiver.UserId });
                    }
                }

                if (ConnectionHelper.IsConnected(email, sender.Username))
                {
                    byEmailUser = ConnectionHelper.IsBlocked(User.Username, receiver.UserId);
                    byLoggedInUser = ConnectionHelper.IsBlockedByMe(receiver.Username, sender.UserId);

                    if (byLoggedInUser)
                    {
                        if (sender != null)
                        {
                            TempData["Error"] = "You have blocked this person!<br/>Do you want to Unblock or Skip?<br/><a href=\"/Home/Unblock?id=" + receiver.UserId + "&redirect=/Network/Index\">Unblock</a>&nbsp;&nbsp;&nbsp;<a href=\"#\" onclick=\"window.location.reload();\">Skip</a>";
                        }
                    }
                    else if (byEmailUser)
                    {
                        TempData["Error"] = "This profile is in private mode!";
                    }
                }
            }

            ViewBag.Redirect = redirect;
            return View(model);
        }

        private string ReplaceNumbers(string text)
        {
            string output = Regex.Replace(text, @"\d{5,}", string.Empty);
            return output;
        }

        private string ReplaceEmails(string text)
        {
            string output = Regex.Replace(text, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", string.Empty);
            return output;
        }

        private string ReplaceUrls(string text)
        {
            string output = Regex.Replace(text, @"http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?", string.Empty);
            return output;
        }

        /// <summary>
        /// Sends message to the recipient.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Send(SendMessageModel model, string redirect)
        {
            var sender = MemberService.Instance.Get(User.Username);
            var receiver = MemberService.Instance.Get(model.ReceiverEmail);
            var flag = DomainService.Instance.PaymentProcessEnabled();
            var quota = DomainService.Instance.HasMessageQuota(User.Id);
            int i = jobService.VerifiedTokenM(Convert.ToInt32(sender.UserId));

            if (flag)
            {
                if (quota == false)
                {
                    if (MessageService.Instance.Count(receiver.UserId, sender.UserId) == 0)
                    {
                        if (i == 0)
                        {
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            string userData = serializer.Serialize(model);

                            Guid id = Guid.NewGuid();
                            DomainService.Instance.StoreData(id, userData);
                            TempData["M"] = model.Message;

                            return RedirectToAction("MessagePriceList", "Package", new { id = id, returnurl = string.Format("/Message/List?SenderId={0}", (receiver != null ? receiver.UserId : 0)) });
                        }
                    }
                }
            }

            bool registered = false;
            bool connected = false;
            bool blocked = false;
            string[] receipent = new string[1];
            string subject;
            RedirectToRouteResult result = null;

            if (sender != null && receiver != null)
            {
                string receiverName = (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : string.Format("{0} {1}", receiver.FirstName, receiver.LastName));
                string senderName = (!string.IsNullOrEmpty(sender.Company) ? sender.Company : string.Format("{0} {1}", sender.FirstName, sender.LastName));

                var mrelation = ConnectionHelper.GetEntity(model.ReceiverEmail, sender.Username);
                var orelation = ConnectionHelper.GetEntity(sender.Username, model.ReceiverEmail);

                var blockedEntity = ConnectionHelper.GetBlockedEntity(receiver.UserId, sender.UserId);

                registered = (receiver != null);
                connected = ConnectionHelper.IsConnected(receiver.Username, sender.Username);
                blocked = ConnectionHelper.IsBlocked(sender.Username, receiver.UserId);

                Communication entity = new Communication();
                string msg = "";
                if (!string.IsNullOrEmpty(model.Message))
                {
                    msg = model.Message.RemoveEmails();
                    msg = msg.RemoveNumbers();
                    msg = msg.RemoveWebsites();
                }

                if (registered == true && connected == false && blocked == true)
                {
                    if (!blockedEntity.CreatedBy.Equals(User.Username))
                    {
                        TempData["Instruction"] = string.Format("{0} {1} do not want to receive messages from you!!", receiver.FileName, receiver.LastName);
                        result = RedirectToAction("Index");
                    }
                    else
                    {
                        DomainService.Instance.Unblock(receiver.UserId, sender.UserId);

                        using (JobPortalEntities context = new JobPortalEntities())
                        {
                            DataHelper dataHelper = new DataHelper(context);
                            if (mrelation == null && orelation == null)
                            {
                                mrelation = new Connection()
                                {
                                    UserId = sender.UserId,
                                    FirstName = !string.IsNullOrEmpty(receiver.FirstName) ? receiver.FirstName.TitleCase() : "",
                                    LastName = !string.IsNullOrEmpty(receiver.LastName) ? receiver.LastName.TitleCase() : "",
                                    EmailAddress = receiver.Username,
                                    IsAccepted = false,
                                    IsConnected = false,
                                    IsBlocked = false,
                                    IsDeleted = false,
                                    Initiated = true
                                };
                                dataHelper.Add<Connection>(mrelation, User.Username);

                                orelation = new Connection()
                                {
                                    UserId = receiver.UserId,
                                    FirstName = !string.IsNullOrEmpty(sender.FirstName) ? sender.FirstName.TitleCase() : "",
                                    LastName = !string.IsNullOrEmpty(sender.LastName) ? sender.LastName.TitleCase() : "",
                                    EmailAddress = sender.Username,
                                    IsBlocked = false,
                                    IsAccepted = false,
                                    IsConnected = false,
                                    IsDeleted = false,
                                    Initiated = true
                                };
                                dataHelper.Add<Connection>(orelation, User.Username);

                                MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                _service.ManageAccount(User.Id, receiver.UserId, null, 1);

                                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                {
                                    string body = reader.ReadToEnd();
                                    body = body.Replace("@@sender", string.Format("{0}", senderName));
                                    string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                    string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                    body = body.Replace("@@viewurl", viewurl);
                                    body = body.Replace("@@message", msg);
                                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                    body = body.Replace("@@receiver", receiverName);
                                    subject = string.Format("Message from {0}", senderName);

                                    receipent[0] = receiver.Username;
                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }
                            }
                            else if (mrelation != null && orelation != null)
                            {
                                if (mrelation.IsDeleted == true && orelation.IsDeleted == true)
                                {
                                    mrelation.IsAccepted = false;
                                    mrelation.IsConnected = false;
                                    mrelation.IsDeleted = false;
                                    mrelation.IsBlocked = false;
                                    mrelation.Initiated = true;
                                    mrelation.DateUpdated = DateTime.Now;
                                    mrelation.UpdatedBy = User.Username;
                                    mrelation.CreatedBy = User.Username;
                                    dataHelper.UpdateEntity<Connection>(mrelation);

                                    orelation.IsAccepted = false;
                                    orelation.IsConnected = false;
                                    orelation.IsDeleted = false;
                                    orelation.IsBlocked = false;
                                    orelation.Initiated = true;
                                    orelation.DateUpdated = DateTime.Now;
                                    orelation.UpdatedBy = User.Username;
                                    orelation.CreatedBy = User.Username;
                                    dataHelper.UpdateEntity<Connection>(orelation);

                                    dataHelper.Save();

                                    MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                    _service.ManageAccount(User.Id, receiver.UserId, null, 1);


                                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                    {
                                        string body = reader.ReadToEnd();

                                        body = body.Replace("@@sender", string.Format("{0}", senderName));
                                        string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                        string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                        body = body.Replace("@@viewurl", viewurl);
                                        body = body.Replace("@@message", msg);
                                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                        body = body.Replace("@@receiver", receiverName);
                                        subject = string.Format("Message from {0}", senderName);

                                        receipent[0] = receiver.Username;
                                        AlertService.Instance.SendMail(subject, receipent, body);
                                    }
                                    TempData["UpdateData"] = "Message sent successfully!";
                                }
                                else
                                {
                                    if (!mrelation.CreatedBy.Equals(User.Username) && !orelation.CreatedBy.Equals(User.Username))
                                    {
                                        mrelation.IsAccepted = true;
                                        mrelation.IsConnected = true;
                                        mrelation.IsDeleted = false;
                                        mrelation.IsBlocked = false;
                                        mrelation.DateUpdated = DateTime.Now;
                                        mrelation.UpdatedBy = User.Username;
                                        dataHelper.UpdateEntity<Connection>(mrelation);

                                        orelation.IsAccepted = true;
                                        orelation.IsConnected = true;
                                        orelation.IsDeleted = false;
                                        orelation.IsBlocked = false;
                                        orelation.DateUpdated = DateTime.Now;
                                        orelation.UpdatedBy = User.Username;
                                        dataHelper.UpdateEntity<Connection>(orelation);

                                        dataHelper.Save();

                                        MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                        //_service.ManageAccount(User.Id, receiver.UserId, null, 1);

                                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                                        {
                                            string body = reader.ReadToEnd();
                                            body = body.Replace("@@sender", senderName);
                                            body = body.Replace("@@message", msg);
                                            string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                            body = body.Replace("@@viewurl", viewurl);
                                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                            body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));

                                            body = body.Replace("@@receiver", receiverName);
                                            subject = string.Format("Message from {0}", senderName);

                                            receipent[0] = receiver.Username;
                                            AlertService.Instance.SendMail(subject, receipent, body);
                                        }

                                        using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html")))
                                        {
                                            string body = reader.ReadToEnd();
                                            body = body.Replace("@@receiver", receiverName);
                                            body = body.Replace("@@sender", senderName);
                                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                            string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                            body = body.Replace("@@viewurl", viewurl);

                                            receipent[0] = receiver.Username;
                                            subject = string.Format("{0} has Accepted Connection", senderName);
                                            AlertService.Instance.SendMail(subject, receipent, body);
                                        }
                                        TempData["UpdateData"] = "Message sent successfully!";
                                    }
                                    else
                                    {
                                        MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                        //if (MessageService.Instance.Count(receiver.UserId, sender.UserId) == 0)
                                        //{
                                        //    _service.ManageAccount(User.Id, receiver.UserId, null, 1);
                                        //}
                                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                        {
                                            string body = reader.ReadToEnd();

                                            body = body.Replace("@@sender", string.Format("{0}", senderName));
                                            string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                            string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                            body = body.Replace("@@viewurl", viewurl);
                                            body = body.Replace("@@message", msg);
                                            body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                            body = body.Replace("@@receiver", receiverName);
                                            subject = string.Format("Message from {0}", senderName);

                                            receipent[0] = receiver.Username;
                                            AlertService.Instance.SendMail(subject, receipent, body);
                                        }
                                        TempData["UpdateData"] = "Message sent successfully!";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (registered == true && connected == true && blocked == true)
                {
                    if (!blockedEntity.CreatedBy.Equals(User.Username))
                    {
                        TempData["Instruction"] = string.Format("{0} {1} do not want to receive messages from you!!", receiver.FileName, receiver.LastName);
                        result = RedirectToAction("Index");
                    }
                }
                else if (registered == true && connected == true && blocked == false)
                {
                    MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username);
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                    {
                        string body = reader.ReadToEnd();
                        body = body.Replace("@@sender", senderName);
                        body = body.Replace("@@message", msg);
                        string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                        body = body.Replace("@@viewurl", viewurl);
                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                        body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));

                        body = body.Replace("@@receiver", receiverName);
                        subject = string.Format("Message from {0}", senderName);

                        receipent[0] = receiver.Username;
                        AlertService.Instance.SendMail(subject, receipent, body);
                    }
                    TempData["UpdateData"] = "Message sent successfully!";
                }
                else if (registered == true && connected == false && blocked == false)
                {
                    using (JobPortalEntities context = new JobPortalEntities())
                    {
                        DataHelper dataHelper = new DataHelper(context);
                        if (mrelation == null && orelation == null)
                        {
                            mrelation = new Connection()
                            {
                                UserId = sender.UserId,
                                FirstName = !string.IsNullOrEmpty(receiver.FirstName) ? receiver.FirstName.TitleCase() : "",
                                LastName = !string.IsNullOrEmpty(receiver.LastName) ? receiver.LastName.TitleCase() : "",
                                EmailAddress = receiver.Username,
                                IsAccepted = false,
                                IsConnected = false,
                                IsBlocked = false,
                                IsDeleted = false,
                                Initiated = true
                            };
                            dataHelper.Add<Connection>(mrelation, User.Username);

                            orelation = new Connection()
                            {
                                UserId = receiver.UserId,
                                FirstName = !string.IsNullOrEmpty(sender.FirstName) ? sender.FirstName.TitleCase() : "",
                                LastName = !string.IsNullOrEmpty(sender.LastName) ? sender.LastName.TitleCase() : "",
                                EmailAddress = sender.Username,
                                IsBlocked = false,
                                IsAccepted = false,
                                IsConnected = false,
                                IsDeleted = false,
                                Initiated = true
                            };
                            dataHelper.Add<Connection>(orelation, User.Username);

                            MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                            _service.ManageAccount(User.Id, receiver.UserId, null, 1);
                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                            {
                                string body = reader.ReadToEnd();
                                body = body.Replace("@@sender", string.Format("{0}", senderName));
                                string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                body = body.Replace("@@viewurl", viewurl);
                                body = body.Replace("@@message", msg);
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                body = body.Replace("@@receiver", receiverName);
                                subject = string.Format("Message from {0}", senderName);

                                receipent[0] = receiver.Username;
                                AlertService.Instance.SendMail(subject, receipent, body);
                            }
                        }
                        else if (mrelation != null && orelation != null)
                        {
                            if (mrelation.IsDeleted == true && orelation.IsDeleted == true)
                            {
                                mrelation.IsAccepted = false;
                                mrelation.IsConnected = false;
                                mrelation.IsDeleted = false;
                                mrelation.IsBlocked = false;
                                mrelation.Initiated = true;
                                mrelation.DateUpdated = DateTime.Now;
                                mrelation.UpdatedBy = User.Username;
                                mrelation.CreatedBy = User.Username;
                                dataHelper.UpdateEntity<Connection>(mrelation);

                                orelation.IsAccepted = false;
                                orelation.IsConnected = false;
                                orelation.IsDeleted = false;
                                orelation.IsBlocked = false;
                                orelation.Initiated = true;
                                orelation.DateUpdated = DateTime.Now;
                                orelation.UpdatedBy = User.Username;
                                orelation.CreatedBy = User.Username;
                                dataHelper.UpdateEntity<Connection>(orelation);

                                dataHelper.Save();

                                MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                _service.ManageAccount(User.Id, receiver.UserId, null, 1);
                                using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                {
                                    string body = reader.ReadToEnd();

                                    body = body.Replace("@@sender", string.Format("{0}", senderName));
                                    string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                    string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                    body = body.Replace("@@viewurl", viewurl);
                                    body = body.Replace("@@message", msg);
                                    body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                    body = body.Replace("@@receiver", receiverName);
                                    subject = string.Format("Message from {0}", senderName);

                                    receipent[0] = receiver.Username;
                                    AlertService.Instance.SendMail(subject, receipent, body);
                                }
                                TempData["UpdateData"] = "Message sent successfully!";
                            }
                            else
                            {
                                if (!mrelation.CreatedBy.Equals(User.Username) && !orelation.CreatedBy.Equals(User.Username))
                                {
                                    mrelation.IsAccepted = true;
                                    mrelation.IsConnected = true;
                                    mrelation.IsDeleted = false;
                                    mrelation.IsBlocked = false;
                                    mrelation.DateUpdated = DateTime.Now;
                                    mrelation.UpdatedBy = User.Username;
                                    dataHelper.UpdateEntity<Connection>(mrelation);

                                    orelation.IsAccepted = true;
                                    orelation.IsConnected = true;
                                    orelation.IsDeleted = false;
                                    orelation.IsBlocked = false;
                                    orelation.DateUpdated = DateTime.Now;
                                    orelation.UpdatedBy = User.Username;
                                    dataHelper.UpdateEntity<Connection>(orelation);

                                    dataHelper.Save();

                                    MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                    //_service.ManageAccount(User.Id, receiver.UserId, null, 1);
                                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/message.html")))
                                    {
                                        string body = reader.ReadToEnd();
                                        body = body.Replace("@@sender", senderName);
                                        body = body.Replace("@@message", msg);
                                        string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                        body = body.Replace("@@viewurl", viewurl);
                                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                        body = body.Replace("@@navigateurl", string.Format("{0}://{1}/Message", Request.Url.Scheme, Request.Url.Authority));

                                        body = body.Replace("@@receiver", receiverName);
                                        subject = string.Format("Message from {0}", senderName);

                                        receipent[0] = receiver.Username;
                                        AlertService.Instance.SendMail(subject, receipent, body);
                                    }

                                    using (var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html")))
                                    {
                                        string body = reader.ReadToEnd();
                                        body = body.Replace("@@receiver", receiverName);
                                        body = body.Replace("@@sender", senderName);
                                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));
                                        string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, sender.UserId);
                                        body = body.Replace("@@viewurl", viewurl);

                                        receipent[0] = receiver.Username;
                                        subject = string.Format("{0} has Accepted Connection", senderName);
                                        AlertService.Instance.SendMail(subject, receipent, body);
                                    }
                                    TempData["UpdateData"] = "Message sent successfully!";
                                }
                                else
                                {
                                    MessageService.Instance.Send(msg, sender.UserId, receiver.UserId, User.Username, true);
                                    //_service.ManageAccount(User.Id, receiver.UserId, null, 1);

                                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitationmessage.html")))
                                    {
                                        string body = reader.ReadToEnd();

                                        body = body.Replace("@@sender", string.Format("{0}", senderName));
                                        string url = string.Format("{0}://{1}/Message/Index", Request.Url.Scheme, Request.Url.Authority);
                                        string viewurl = string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority, mrelation.Id);
                                        body = body.Replace("@@viewurl", viewurl);
                                        body = body.Replace("@@message", msg);
                                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, sender.PermaLink));

                                        body = body.Replace("@@receiver", receiverName);
                                        subject = string.Format("Message from {0}", senderName);

                                        receipent[0] = receiver.Username;
                                        AlertService.Instance.SendMail(subject, receipent, body);
                                    }
                                    TempData["UpdateData"] = "Message sent successfully!";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                TempData["UpdateData"] = "Message sending failed, unable to retrieve recipient details!";
            }
            return RedirectToAction("List", new { SenderId = receiver.UserId });
        }

        /// <summary>
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        [Authorize]
        [UrlPrivilegeFilter]
        public ActionResult List(long SenderId, string redirect = "")
        {
            if (Session["UpdateData"] != null)
            {
                TempData["UpdateData"] = Convert.ToString(Session["UpdateData"]);
                Session.Remove("UpdateData");
            }
            if (UserInfo != null && !UserInfo.IsConfirmed)
            {
                //return RedirectToAction("ConfirmRegistration", "Account");
                return RedirectToAction("Confirm", "Account", new { id = UserInfo.Id, returnUrl = Request.Url.ToString() });
            }

            Communication last = new Communication();
            List<Communication> msg_list_new = new List<Communication>();
            List<Communication> msg_list = new List<Communication>();
            UserProfile receiver = MemberService.Instance.Get(SenderId);

            bool connected = ConnectionHelper.IsConnected(receiver.Username, UserInfo.Username);
            bool blocked = ConnectionHelper.IsBlocked(UserInfo.Username, receiver.UserId);

            ViewBag.Connected = connected;
            ViewBag.Blocked = blocked;

            BlockedPeople entity = ConnectionHelper.GetBlockedEntity(receiver.UserId, UserInfo.Id);
            if (entity != null && !entity.CreatedBy.Equals(UserInfo.Username))
            {
                TempData["Error"] = string.Format("{0} does not accept messages!", (!string.IsNullOrEmpty(receiver.Company) ? receiver.Company : string.Format("{0} {1}", receiver.FirstName, receiver.LastName)));
            }


            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);

                // last = dataHelper.Get<Communication>().Where(x => x.UserId == profile.UserId && (x.SenderId == SenderId || x.ReceiverId == SenderId)).OrderByDescending(x => x.Id).FirstOrDefault();

                int counts = dataHelper.Get<Communication>().Count(x => x.UserId == UserInfo.Id && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == false);
                if (counts == 0)
                {
                    msg_list = dataHelper.Get<Communication>().Where(x => x.UserId == UserInfo.Id && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == true).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    msg_list = dataHelper.Get<Communication>().Where(x => x.UserId == UserInfo.Id && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == true).OrderBy(x => x.Id).ToList();
                    msg_list_new = dataHelper.Get<Communication>().Where(x => x.UserId == UserInfo.Id && (x.SenderId == SenderId || x.ReceiverId == SenderId) && x.IsDeleted == false).OrderBy(x => x.Id).ToList();
                }

                var list = dataHelper.Get<Communication>().Where(x => x.UserId == UserInfo.Id && x.SenderId == SenderId && x.Unread == true);
                foreach (Communication item in list)
                {
                    item.Unread = false;
                    dataHelper.UpdateEntity<Communication>(item);
                }
                dataHelper.Save();
            }
            ViewBag.ReceiverId = SenderId;
            ViewBag.MessageList = msg_list;
            ViewBag.MessageList_New = msg_list_new;

            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                Guid id = new Guid(Request.QueryString["sid"]);
                string jobData = DomainService.Instance.ReadData(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ViewBag.MsgText = (serializer.Deserialize<SendMessageModel>(jobData)).Message;
                DomainService.Instance.RemoveData(id);
            }
            return View(last);
        }


        [Authorize]
        public ActionResult Accept(long ConnectionId, string redirect = "")
        {
            Connection connection = new Connection();
            connection = ConnectionHelper.Get(ConnectionId);
            if (User.Username.Equals(connection.EmailAddress))
            {
                UserProfile profile = MemberService.Instance.Get(User.Username);
                if (!profile.IsConfirmed)
                {
                    profile.IsConfirmed = true;
                    profile.ConfirmationToken = null;
                    profile.IsValidUsername = true;
                    MemberService.Instance.Update(profile);
                }
            }

            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                Communication message = dataHelper.Get<Communication>().Where(x => (x.SenderId == profile.UserId || x.ReceiverId == profile.UserId)).OrderByDescending(x => x.Id).FirstOrDefault();

                connection = dataHelper.GetSingle<Connection>(ConnectionId);
                Hashtable parameters = new Hashtable();

                if (connection != null)
                {
                    if (connection.CreatedBy.Equals(User.Username))
                    {
                        profile = MemberService.Instance.Get(connection.EmailAddress);
                        TempData["Error"] = string.Format("You cannot accept this connection. {0} {1} has to accept it!", profile.FirstName, profile.LastName);
                    }
                    else
                    {
                        UserProfile loggedUser = MemberService.Instance.Get(User.Username);
                        UserProfile connected = dataHelper.GetSingle<UserProfile>("Username", connection.EmailAddress); // Contact
                        UserProfile requestor = dataHelper.GetSingle<UserProfile>("UserId", connection.UserId);

                        parameters = new Hashtable();
                        parameters.Add("UserId", connected.UserId);
                        parameters.Add("EmailAddress", requestor.Username);

                        Connection invitor = dataHelper.GetSingle<Connection>(parameters);

                        invitor.IsAccepted = true;
                        invitor.IsConnected = true;
                        invitor.Initiated = false;
                        dataHelper.Update<Connection>(invitor, User.Username);

                        connection.FirstName = connected.FirstName;
                        connection.LastName = connected.LastName;
                        connection.IsAccepted = true;
                        connection.IsConnected = true;
                        connection.Initiated = false;

                        dataHelper.Update<Connection>(connection, User.Username);

                        var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation_accepted.html"));
                        var body = reader.ReadToEnd();

                        var subject = string.Empty;
                        if (connected.Username.Equals(loggedUser.Username))
                        {
                            body = body.Replace("@@receiver", string.Format("{0}", !string.IsNullOrEmpty(requestor.Company) ? requestor.Company : string.Format("{0} {1}", requestor.FirstName, requestor.LastName)));
                        }
                        else
                        {
                            body = body.Replace("@@receiver", string.Format("{0}", !string.IsNullOrEmpty(connected.Company) ? connected.Company : string.Format("{0} {1}", connected.FirstName, connected.LastName)));
                        }
                        subject = string.Format("{0} has Accepted Connection", !string.IsNullOrEmpty(loggedUser.Company) ? loggedUser.Company : string.Format("{0} {1}", loggedUser.FirstName, loggedUser.LastName));


                        if (loggedUser.Type == (int)SecurityRoles.Jobseeker)
                        {
                            body = body.Replace("@@sender", string.Format("{0} {1}", loggedUser.FirstName, loggedUser.LastName));
                        }
                        else
                        {
                            body = body.Replace("@@sender", string.Format("{0}", !string.IsNullOrEmpty(loggedUser.Company) ? loggedUser.Company : string.Format("{0} {1}", loggedUser.FirstName, loggedUser.LastName)));
                        }
                        body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, loggedUser.PermaLink));

                        string viewurl = string.Format("{0}://{1}/Message/List?SenderId={2}", Request.Url.Scheme, Request.Url.Authority, loggedUser.UserId);
                        body = body.Replace("@@viewurl", viewurl);

                        string[] receipent = { "" };
                        if (connected.Username.Equals(loggedUser.Username))
                        {
                            receipent[0] = requestor.Username;
                        }
                        else
                        {
                            receipent[0] = connected.Username;
                        }

                        AlertService.Instance.SendMail(subject, receipent, body);

                        TempData["UpdateData"] = "Connection invitation accepted!";


                    }
                }
            }

            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }
            else
            {
                return RedirectToAction("List", new { SenderId = connection.UserId });
            }
        }

        public ActionResult Reject(long ConnectionId, string redirect = "")
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);

                Connection connection = dataHelper.GetSingle<Connection>(ConnectionId);
                if (connection != null)
                {
                    connection.IsAccepted = false;
                    connection.IsConnected = false;
                    connection.IsBlocked = false;
                    connection.IsDeleted = true;
                    connection.Initiated = false;
                    connection.DateDeleted = DateTime.Now;
                    connection.UpdatedBy = User.Username;
                    connection.DateUpdated = DateTime.Now;

                    dataHelper.UpdateEntity<Connection>(connection);

                    UserProfile connected = MemberService.Instance.Get(connection.EmailAddress);
                    if (connected != null)
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", connected.UserId);
                        parameters.Add("EmailAddress", profile.Username);
                        Connection connectedUser = dataHelper.GetSingle<Connection>(parameters);
                        if (connectedUser != null)
                        {
                            connectedUser.IsAccepted = false;
                            connectedUser.IsConnected = false;
                            connectedUser.IsBlocked = false;
                            connectedUser.IsDeleted = true;
                            connectedUser.Initiated = false;
                            connectedUser.UpdatedBy = User.Username;
                            connectedUser.DateDeleted = DateTime.Now;
                            connectedUser.DateUpdated = DateTime.Now;
                            dataHelper.UpdateEntity<Connection>(connectedUser);
                        }
                    }

                    dataHelper.Save();

                    Communication first = dataHelper.Get<Communication>()
                        .Where(x => x.IsInitial == true && x.IsDeleted == false && x.ReceiverId == profile.UserId)
                        .OrderBy(x => x.Id).FirstOrDefault();
                    if (first != null)
                    {
                        first.IsDeleted = true;
                        first.DateUpdated = DateTime.Now;
                        first.UpdatedBy = User.Username;

                        Communication second = dataHelper.Get<Communication>()
                           .Where(x => x.IsInitial == true && x.IsDeleted == false && x.ReceiverId == profile.UserId)
                           .OrderByDescending(x => x.Id).FirstOrDefault();
                        if (second != null)
                        {
                            dataHelper.UpdateEntity<Communication>(first);
                            dataHelper.Remove<Communication>(second);
                        }
                    }
                    dataHelper.Save();
                    TempData["UpdateData"] = "Rejected successfully!";
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Reconnect(string EmailAddress, string redirect)
        {
            UserProfile registeredProfile = new UserProfile();
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                List<Connection> contact_list = new List<Connection>();
                bool byLoggedInUser = false;
                bool byEmailUser = false;
                if (!string.IsNullOrEmpty(EmailAddress))
                {
                    UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);
                    registeredProfile = dataHelper.GetSingle<UserProfile>("Username", EmailAddress.ToLower());

                    if (profile != null && registeredProfile != null)
                    {
                        byEmailUser = ConnectionHelper.IsBlocked(User.Username, registeredProfile.UserId);
                        byLoggedInUser = ConnectionHelper.IsBlockedByMe(EmailAddress, profile.UserId);
                    }
                    if (byEmailUser == false && byLoggedInUser == false)
                    {
                        if (!profile.Username.ToLower().Equals(EmailAddress.ToLower()))
                        {
                            Hashtable parameters = new Hashtable();
                            parameters.Add("UserId", profile.UserId);
                            parameters.Add("EmailAddress", EmailAddress);
                            Connection contact = dataHelper.GetSingle<Connection>(parameters);
                            if (contact == null)
                            {
                                contact = new Connection()
                                {
                                    UserId = profile.UserId,
                                    FirstName = registeredProfile != null ? registeredProfile.FirstName.TitleCase() : "",
                                    LastName = registeredProfile != null ? registeredProfile.LastName.TitleCase() : "",
                                    EmailAddress = EmailAddress,
                                    IsAccepted = false,
                                    IsConnected = false,
                                    IsBlocked = false,
                                    Initiated = true
                                };

                                dataHelper.Add<Connection>(contact, User.Username);

                                if (registeredProfile != null)
                                {
                                    Connection networkContact =
                                        registeredProfile.Connections.SingleOrDefault(
                                            x => x.EmailAddress == profile.Username);
                                    if (networkContact == null)
                                    {
                                        networkContact = new Connection()
                                        {
                                            UserId = registeredProfile.UserId,
                                            FirstName = profile.FirstName.TitleCase(),
                                            LastName = profile.LastName.TitleCase(),
                                            EmailAddress = profile.Username,
                                            IsAccepted = false,
                                            IsConnected = false,
                                            IsBlocked = false,
                                            Initiated = true
                                        };

                                        dataHelper.Add<Connection>(networkContact, User.Username);
                                    }
                                }
                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                                var body = reader.ReadToEnd();
                                string name = contact.FirstName + " " + contact.LastName;
                                var subject = string.Empty;
                                if (registeredProfile != null)
                                {
                                    name = registeredProfile.FirstName + " " + registeredProfile.LastName;
                                }
                                else
                                {
                                    name = contact.FirstName + " " + contact.LastName;
                                    if (string.IsNullOrEmpty(name))
                                    {
                                        name = contact.EmailAddress;
                                    }
                                    else if (name.Trim() != " ")
                                    {
                                        name = contact.EmailAddress;
                                    }
                                }

                                body = body.Replace("@@receiver", name);
                                if (profile.Type == (int)SecurityRoles.Jobseeker)
                                {
                                    body = body.Replace("@@sender", string.Format("{0} {1}", profile.FirstName, profile.LastName));

                                    subject = string.Format("{0} Invites you to connect at Joblisting", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                                }
                                else
                                {
                                    body = body.Replace("@@sender", string.Format("{0}", !string.IsNullOrEmpty(profile.Company) ? profile.Company : string.Format("{0} {1}", profile.FirstName, profile.LastName)));
                                    subject = string.Format("{0} Invites you to connect at Joblisting", !string.IsNullOrEmpty(profile.Company) ? profile.Company : string.Format("{0} {1}", profile.FirstName, profile.LastName));
                                }
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, profile.PermaLink));

                                body = body.Replace("@@accepturl",
                                    string.Format("{0}://{1}/Message/Accept?ConnectionId={2}", Request.Url.Scheme, Request.Url.Authority,
                                        contact.Id));
                                body = body.Replace("@@button", "Accept");
                                parameters = new Hashtable()
                            {
                                {"Type", "Invitation"}
                            };
                                Alert alert = dataHelper.GetSingle<Alert>(parameters);
                                body = body.Replace("@@unsubscribeurl",
                                    string.Format("{0}://{1}/Network/Unsubscribe?alertId={2}&email={3}", Request.Url.Scheme,
                                        Request.Url.Authority, alert.Id, contact.EmailAddress));

                                string[] receipent = { contact.EmailAddress };

                                AlertService.Instance.SendMail(subject, receipent, body);

                                TempData["SaveData"] = "Connection invitation sent successfully!";
                            }
                            else
                            {
                                contact.IsAccepted = false;
                                contact.IsConnected = false;
                                contact.IsDeleted = false;
                                contact.Initiated = true;
                                contact.CreatedBy = User.Username;
                                contact.DateUpdated = DateTime.Now;
                                contact.UpdatedBy = User.Username;

                                if (registeredProfile != null)
                                {
                                    Connection networkContact =
                                        registeredProfile.Connections.SingleOrDefault(
                                            x => x.EmailAddress == profile.Username);
                                    if (networkContact != null)
                                    {
                                        networkContact.IsAccepted = false;
                                        networkContact.IsConnected = false;
                                        networkContact.IsDeleted = false;
                                        networkContact.Initiated = true;
                                        networkContact.CreatedBy = User.Username;
                                        networkContact.DateUpdated = DateTime.Now;
                                        networkContact.UpdatedBy = User.Username;

                                        dataHelper.UpdateEntity<Connection>(contact);
                                        dataHelper.UpdateEntity<Connection>(networkContact);
                                        dataHelper.Save();
                                    }
                                }

                                var reader = new StreamReader(Server.MapPath("~/Templates/Mail/invitation.html"));
                                var body = reader.ReadToEnd();
                                string name = contact.FirstName + " " + contact.LastName;
                                var subject = string.Empty;
                                if (registeredProfile != null)
                                {
                                    name = registeredProfile.FirstName + " " + registeredProfile.LastName;
                                }
                                else
                                {
                                    name = contact.FirstName + " " + contact.LastName;
                                    if (string.IsNullOrEmpty(name))
                                    {
                                        name = contact.EmailAddress;
                                    }
                                    else if (name.Trim() != " ")
                                    {
                                        name = contact.EmailAddress;
                                    }
                                }

                                body = body.Replace("@@receiver", name);
                                if (profile.Type == (int)SecurityRoles.Jobseeker)
                                {
                                    body = body.Replace("@@sender", string.Format("{0} {1}", profile.FirstName, profile.LastName));

                                    subject = string.Format("{0} Invites you to connect at Joblisting", string.Format("{0} {1}", profile.FirstName, profile.LastName));
                                }
                                else
                                {
                                    body = body.Replace("@@sender", string.Format("{0}", !string.IsNullOrEmpty(profile.Company) ? profile.Company : string.Format("{0} {1}", profile.FirstName, profile.LastName)));
                                    subject = string.Format("{0} Invites you to connect at Joblisting", !string.IsNullOrEmpty(profile.Company) ? profile.Company : string.Format("{0} {1}", profile.FirstName, profile.LastName));
                                }
                                body = body.Replace("@@profileurl", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, profile.PermaLink));

                                body = body.Replace("@@accepturl",
                                    string.Format("{0}://{1}/Network/Accept/{2}", Request.Url.Scheme, Request.Url.Authority,
                                        contact.Id));
                                body = body.Replace("@@button", "Accept");
                                parameters = new Hashtable()
                            {
                                {"Type", "Invitation"}
                            };
                                Alert alert = dataHelper.GetSingle<Alert>(parameters);
                                body = body.Replace("@@unsubscribeurl",
                                    string.Format("{0}://{1}/Network/Unsubscribe?alertId={2}&email={3}", Request.Url.Scheme,
                                        Request.Url.Authority, alert.Id, contact.EmailAddress));

                                string[] receipent = { contact.EmailAddress };

                                AlertService.Instance.SendMail(subject, receipent, body);

                                TempData["SaveData"] = "Connection invitation sent successfully!";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "You cannot connect with yourself!";
                        }
                    }
                    else
                    {
                        if (byLoggedInUser)
                        {
                            if (registeredProfile != null)
                            {
                                TempData["Error"] = "You have blocked this person!<br/>Do you want to Unblock or Skip?<br/><a href=\"/Home/Unblock?id=" + registeredProfile.UserId + "&redirect=/Network/Index\">Unblock</a>&nbsp;&nbsp;&nbsp;<a href=\"#\" onclick=\"window.location.reload();\">Skip</a>";
                            }
                        }
                        else if (byEmailUser)
                        {
                            if (profile != null)
                            {
                                TempData["Error"] = "This profile is in private mode!";
                            }
                        }
                    }
                    contact_list = dataHelper.GetList<Connection>().Where(x => x.UserId == profile.UserId).ToList();
                    ViewBag.Contacts = contact_list;
                }
                else
                {
                    TempData["Error"] = "Provide Email address!";
                }
            }

            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }
            else
            {
                return RedirectToAction("List", new { SenderId = registeredProfile.UserId });
            }
        }

        public ActionResult Disconnect(long Id, long SenderId)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Connection connection = dataHelper.GetSingle<Connection>(Id);
                UserProfile profile = dataHelper.GetSingle<UserProfile>("Username", User.Username);

                if (connection != null)
                {
                    connection.IsAccepted = false;
                    connection.IsConnected = false;
                    connection.IsDeleted = true;
                    connection.Initiated = false;
                    connection.DateDeleted = DateTime.Now;
                    connection.UpdatedBy = User.Username;
                    connection.DateUpdated = DateTime.Now;

                    dataHelper.UpdateEntity<Connection>(connection);

                    UserProfile connected = MemberService.Instance.Get(connection.EmailAddress);
                    if (connected != null)
                    {
                        Hashtable parameters = new Hashtable();
                        parameters.Add("UserId", connected.UserId);
                        parameters.Add("EmailAddress", profile.Username);
                        Connection connectedUser = dataHelper.GetSingle<Connection>(parameters);
                        if (connectedUser != null)
                        {
                            connectedUser.IsAccepted = false;
                            connectedUser.IsConnected = false;
                            connectedUser.IsDeleted = true;
                            connectedUser.Initiated = false;
                            connectedUser.UpdatedBy = User.Username;
                            connectedUser.DateDeleted = DateTime.Now;
                            connectedUser.DateUpdated = DateTime.Now;
                            dataHelper.UpdateEntity<Connection>(connectedUser);
                        }
                    }

                    //var result = dataHelper.Get<Communication>().Where(x => x.UserId == profile.UserId && (x.SenderId == SenderId || x.ReceiverId == SenderId));
                    var result = dataHelper.Get<Communication>().Where(x => (x.SenderId == profile.UserId || x.ReceiverId == profile.UserId) || (x.SenderId == connected.UserId || x.ReceiverId == connected.UserId));
                    foreach (var item in result)
                    {
                        item.IsDeleted = true;
                        item.DateUpdated = DateTime.Now;
                        item.UpdatedBy = User.Username;
                        dataHelper.UpdateEntity<Communication>(item);
                    }
                    dataHelper.Save();
                    TempData["UpdateData"] = "Disconnected successfully!";
                }
            }
            return RedirectToAction("Index");
        }
    }
}