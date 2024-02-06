using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using TweetSharp;
using System.Net;
using Facebook;
using System.IO;
using System.Collections.Specialized;
using System.Text;
using System.Dynamic;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using nQuant;
using System.Drawing;
using System.Drawing.Imaging;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
namespace JobPortal.Web.Controllers
{
    public class ImageController : BaseController
    {
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public ImageController(IUserService service)
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
            : base(service)
        {

        }
        public ActionResult Index()
        {
            return View();
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        // GET: Image
        public ActionResult Avtar(long Id, int? height, int? width)
        {
            byte[] buffer = MemberService.Instance.GetProfilePhoto(Id);           
            return File(buffer, "image/png");
        }
        public string Avtar1(long Id, int? height, int? width)
        {
            byte[] buffer = MemberService.Instance.GetProfilePhoto(Id);
            return Convert.ToBase64String(buffer);
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        public ActionResult Load(long? Id)
        {
            byte[] buffer = new byte[0];
            byte[] ibuffer = MemberService.Instance.GetAlbumPhoto(Id.Value);
            if (ibuffer != null || ibuffer.Length > 0)
            {
                buffer = ibuffer;
            }
            return File(buffer, "image/png");
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        public ActionResult LoadImage(long? Id)
        {
            byte[] buffer = new byte[1];
            string imageType = string.Empty;

            ImageEntity image = (DomainService.Instance.GetPhoto(Id));
            if (image != null)
            {
                buffer = image.Image;
                imageType = image.Type;
            }
            return new ContentResult() { Content = string.Format("{0},{1}", imageType, Convert.ToBase64String(buffer))};
        }

        public ActionResult LoadThumbnail(long? Id, int height, int width)
        {
            byte[] buffer = new byte[1];
            string defaultImage = string.Empty;
            ImageEntity photo = (DomainService.Instance.GetPhoto(Id));

            if (photo != null && photo.InEditMode == true)
            {
                if (User.Info.Role == SecurityRoles.SuperUser || User.Info.Role == SecurityRoles.Administrator)
                {
                    if (photo.IsApproved == false)
                    {
                        buffer = UIHelper.ResizeTo(photo.NewImage, width, height);
                    }
                    else
                    {
                        buffer = UIHelper.ResizeTo(photo.Image, width, height);
                    }
                }
                else
                {
                    buffer = UIHelper.ResizeTo(photo.Image, width, height);
                }
            }
            else
            {
                buffer = UIHelper.ResizeTo(photo.Image, width, height);
            }
            return File(buffer, "image/png");
        }
    }
}