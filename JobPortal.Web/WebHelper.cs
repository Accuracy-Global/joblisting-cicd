using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace JobPortal.Web
{
    public static partial class WebHelper
    {
        public static SelectList GetEnumList(Type type)
        {
            Hashtable list = new Hashtable();
            foreach (int value in Enum.GetValues(type))
            {
                var name = Enum.GetName(type, value);
                list.Add(name, value);
            }
            return new SelectList(list, "value", "key");
        }
        public static bool IsRegistered(string email)
        {
            UserProfile profile = MemberService.Instance.Get(email);
            return (profile != null);
        }

        public static string TitleCase(this string input)
        {
            string statement = string.Empty;
            string[] words = input.ToLower().Split(' ');
            foreach (string word in words)
            {
                statement += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word) + " ";
            }
            return statement.Trim();
        }

        
    }
    public static class HtmlHelperExtensions
    {
        public static IHtmlString InlineScripts(this HtmlHelper htmlHelper, string bundleVirtualPath)
        {
            return htmlHelper.InlineBundle(bundleVirtualPath, htmlTagName: "script");
        }

        public static IHtmlString InlineStyles(this HtmlHelper htmlHelper, string bundleVirtualPath)
        {
            return htmlHelper.InlineBundle(bundleVirtualPath, htmlTagName: "style");
        }

        private static IHtmlString InlineBundle(this HtmlHelper htmlHelper, string bundleVirtualPath, string htmlTagName)
        {
            string bundleContent = LoadBundleContent(htmlHelper.ViewContext.HttpContext, bundleVirtualPath);
            string htmlTag = string.Format("<{0}>{1}</{0}>", htmlTagName, bundleContent);

            return new HtmlString(htmlTag);
        }

        private static string LoadBundleContent(HttpContextBase httpContext, string bundleVirtualPath)
        {
            var bundleContext = new BundleContext(httpContext, BundleTable.Bundles, bundleVirtualPath);
            var bundle = BundleTable.Bundles.Single(b => b.Path == bundleVirtualPath);
            var bundleResponse = bundle.GenerateBundleResponse(bundleContext);

            return bundleResponse.Content;
        }        
    }
}