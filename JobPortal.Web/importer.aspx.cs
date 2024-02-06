using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
//using System.Net.Mail;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Configuration;

namespace JobPortal.Web
{
    public partial class importer : Page
    {
        /// <summary>
        ///     Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        /// <summary>
        ///     Gets All Imported Contacts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Loaded Contacts
        protected void btnimportcontacts_Click(object sender, ImageClickEventArgs e)
        {
            //string addressbookData = addBook.Value;
            var list_addressbook = new List<addressbook>();
            //list_addressbook = serializeAddressBook(addressbookData);
            //list_addressbook contains the list of imported contacts in list object
        }

        #endregion

        /// <summary>
        ///     Send Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Send Button Click
        protected void btnsend_Click(object sender, ImageClickEventArgs e)
        {
            //string selectedContactsData = selectedcontacts.Value;
            //string subjectstr = subject.Value;
            //string messagestr = message.Value;
            //var list_addressbook = new List<addressbook>();
            //list_addressbook = serializeAddressBook(selectedContactsData); //Gets the selected contacts
            //var len = list_addressbook.Count;
            //for (var i = 0; i < list_addressbook.Count; i++)
            //{
            //    //Enable the below line to send email(remember to set the configuration of your mail server on the the fire email function)
            //    //firemail(list_addressbook[i].email[0].ToString(), "your from email address here", subjectstr, messagestr);
            //}
            ScriptManager.RegisterStartupScript(this, typeof(importer), UniqueID, "<script>sendConfirmation();</script>",
                false); //call javascript to show success message
        }

        #endregion

        /// <summary>
        ///     Seralize string data to list object
        /// </summary>
        /// <param name="data">JSON string</param>
        /// <returns>List<addressbook></returns>

        #region Seralize AddressBook
        public List<addressbook> serializeAddressBook(string data)
        {
            var list_addressbook = new List<addressbook>();
            var serializer = new JavaScriptSerializer();
            var obj_ad = serializer.Deserialize<deserializer>("{\"data\":" + data + "}");
            var len = ((object[])(obj_ad.data)).Length;
            if (len > 0)
            {
                var dics = new Dictionary<string, object>();
                for (var i = 0; i < len; i++)
                {
                    string firstname = "", lastname = "", day = "", month = "", year = "", imageurl = "", notes = "";
                    var email = new ArrayList();
                    var website = new ArrayList();
                    var phone = new ArrayList();
                    var address = new List<Address>();
                    var birthday = new Birthday();
                    var name = new Name();
                    try
                    {
                        dics = ((Dictionary<string, object>)((object[])(obj_ad.data))[i]);
                    }
                    catch (Exception)
                    {
                        email.Add((((object[])(obj_ad.data))[i]));
                        dics = new Dictionary<string, object>();
                    }
                    foreach (var contacts in dics)
                    {
                        if (contacts.Value != null)
                        {
                            if (contacts.Key == "name")
                            {
                                var dic_contacts = ((Dictionary<string, object>)(contacts.Value));
                                foreach (var nameobj in dic_contacts)
                                {
                                    if (nameobj.Key == "first_name")
                                    {
                                        if (nameobj.Value != null)
                                            firstname = Server.UrlDecode(nameobj.Value.ToString());
                                    }
                                    else if (nameobj.Key == "last_name")
                                    {
                                        if (nameobj.Value != null)
                                            lastname = Server.UrlDecode(nameobj.Value.ToString());
                                    }
                                }
                                name.first_name = firstname;
                                name.last_name = lastname;
                            }
                            else if (contacts.Key == "email")
                            {
                                var emailLen = ((object[])(contacts.Value)).Length;
                                for (var em = 0; em < emailLen; em++)
                                {
                                    email.Add(((object[])(contacts.Value))[em].ToString());
                                }
                            }
                            else if (contacts.Key == "imageurl")
                            {
                                if (contacts.Value != null)
                                    imageurl = Server.UrlDecode(contacts.Value.ToString());
                            }
                            else if (contacts.Key == "notes")
                            {
                                if (contacts.Value != null)
                                    notes = Server.UrlDecode(contacts.Value.ToString());
                            }
                            else if (contacts.Key == "birthday")
                            {
                                var dic_dob = ((Dictionary<string, object>)(contacts.Value));
                                foreach (var dobobj in dic_dob)
                                {
                                    if (dobobj.Value != null && dobobj.Key == "day")
                                        day = dobobj.Value.ToString();
                                    else if (dobobj.Value != null && dobobj.Key == "month")
                                        month = dobobj.Value.ToString();
                                    else if (dobobj.Value != null && dobobj.Key == "year")
                                        year = dobobj.Value.ToString();
                                }
                                birthday.day = day;
                                birthday.month = month;
                                birthday.year = year;
                            }
                            else if (contacts.Key == "website")
                            {
                                var websiteLen = ((object[])(contacts.Value)).Length;
                                for (var we = 0; we < websiteLen; we++)
                                {
                                    website.Add(((object[])(contacts.Value))[we].ToString());
                                }
                            }
                            else if (contacts.Key == "phone")
                            {
                                var phoneLen = ((object[])(contacts.Value)).Length;
                                for (var ph = 0; ph < phoneLen; ph++)
                                {
                                    phone.Add(((object[])(contacts.Value))[ph].ToString());
                                }
                            }
                            else if (contacts.Key == "address")
                            {
                                var addLen = ((object[])(contacts.Value)).Length;
                                for (var ad = 0; ad < addLen; ad++)
                                {
                                    var dic_address = ((Dictionary<string, object>)((object[])(contacts.Value))[ad]);
                                    string street = "", city = "", state = "", zip = "", country = "", formattedaddress = "";
                                    foreach (var address_obj in dic_address)
                                    {
                                        if (address_obj.Key == "street")
                                        {
                                            if (address_obj.Value != null)
                                                street = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "city")
                                        {
                                            if (address_obj.Value != null)
                                                city = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "state")
                                        {
                                            if (address_obj.Value != null)
                                                state = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "zip")
                                        {
                                            if (address_obj.Value != null)
                                                zip = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "country")
                                        {
                                            if (address_obj.Value != null)
                                                country = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                        else if (address_obj.Key == "formattedaddress")
                                        {
                                            if (address_obj.Value != null)
                                                formattedaddress = Server.UrlDecode(address_obj.Value.ToString());
                                        }
                                    }
                                    var addr = new Address();
                                    addr.city = city;
                                    addr.street = street;
                                    addr.state = state;
                                    addr.zip = zip;
                                    addr.country = country;
                                    addr.formattedaddress = formattedaddress;
                                    address.Add(addr);
                                }
                            }
                        }
                    }
                    if (email.Count > 0)
                    {
                        var gc = new addressbook();
                        gc.address = address;
                        gc.email = email;
                        gc.name = name;
                        gc.phone = phone;
                        gc.birthday = birthday;
                        gc.imageurl = imageurl.Replace("\"", "");
                        gc.website = website;
                        gc.notes = notes;
                        list_addressbook.Add(gc);
                    }
                }
            }
            return list_addressbook;
        }

        #endregion

        /// <summary>
        ///     Fire Email
        /// </summary>
        /// <param name="toaddr">To email address</param>
        /// <param name="fromaddr">From email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="bodytext">Email message</param>
        /// <returns></returns>

        #region Fire Email
        public int firemail(string toaddr, string fromaddr, string subject, string bodytext)
        {
            var status = 0;
            try
            {
                var objMailMessage = new MimeMessage();
                objMailMessage.From.Add(new MailboxAddress("",fromaddr));
                objMailMessage.To.Add(new MailboxAddress("", toaddr));
                objMailMessage.Subject = subject;
                objMailMessage.Body =new TextPart("html") { Text = bodytext };
                string postmail = ConfigurationManager.AppSettings["postmail"];
                string postpassword = ConfigurationManager.AppSettings["postpassword"];
                SmtpClient oSmtp = new SmtpClient();
                oSmtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                oSmtp.Connect("smtp.mailgun.org", 587, false);
                oSmtp.AuthenticationMechanisms.Remove("XOAUTH2");
                oSmtp.Authenticate(postmail, postpassword);

                oSmtp.Send(objMailMessage);
                oSmtp.Disconnect(true);
                
                status = 1;
            }
            catch (Exception et)
            {
                status = 0;
                var str_et = et.ToString();
            }
            return status;
        }

        #endregion
    }

    /// <summary>
    ///     Supportive class
    /// </summary>

    #region SUPPORTIVE CLASS
    public class deserializer
    {
        public object data;
    }

    public class addressbook
    {
        public List<Address> address;
        public Birthday birthday;
        public ArrayList email;
        public string imageurl;
        public Name name;
        public string notes;
        public ArrayList phone;
        public ArrayList website;
    }

    public class Name
    {
        public string first_name;
        public string last_name;
    }

    public class Birthday
    {
        public string day;
        public string month;
        public string year;
    }

    public class Address
    {
        public string city;
        public string country;
        public string formattedaddress;
        public string state;
        public string street;
        public string zip;
    }

    #endregion

}