using JobPortal.Data;
using JobPortal.Domain;
using JobPortal.Library.Enumerators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace JobPortal.Web
{
    public class ImageRouteHandler : IRouteHandler
    {
        public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ImageHandler(requestContext);
        }
    }

    public class ImageHandler : DataService, IHttpHandler
    {
        public bool IsReusable { get { return false; } }
        protected RequestContext RequestContext { get; set; }
        public ImageHandler() : base() { }

        public ImageHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }
        public void ProcessRequest(HttpContext context)
        {
            //if (context.Request.Url.ToString().Contains("photo"))
            //{
            //    byte[] buffer = new byte[1];
            //    object v = ReadDataField(string.Format("GetPhoto {0}", context.Request.Url.ToString().Substring(context.Request.Url.ToString().LastIndexOf('/') + 1, context.Request.Url.ToString().IndexOf("?"))));

            //    if (v is string)
            //    {
            //        buffer = Convert.FromBase64String(Convert.ToString(v));
            //    }
            //    else
            //    {
            //        buffer = (byte[])v;
            //    }
            //    context.Response.ContentType = "image/png";
            //    context.Response.BinaryWrite(buffer);
            //    context.Response.Flush();
            //}
        }
    }
}