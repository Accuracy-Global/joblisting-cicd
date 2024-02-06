#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Helpers;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
//using JobPortal.Domain;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Web
{  
    public class UserPrincipal : IPrincipal
    {
#pragma warning disable CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        IUserService iUserService;
#pragma warning restore CS0246 // The type or namespace name 'IUserService' could not be found (are you missing a using directive or an assembly reference?)
        public UserPrincipal(string Username)
        {
            iUserService = new UserService();
            this.Identity = new GenericIdentity(Username);
            roles = new string[] { SecurityRoles.Administrator.GetDescription(), SecurityRoles.Employers.GetDescription(), SecurityRoles.Jobseeker.GetDescription(), SecurityRoles.Partner.GetDescription(), SecurityRoles.SearchEngineOptimizer.GetDescription(), SecurityRoles.SuperUser.GetDescription(), SecurityRoles.Support.GetDescription(), SecurityRoles.UnverifiedUser.GetDescription() };             
        }
        public IIdentity Identity { get; private set; }
#pragma warning disable CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        public UserInfoEntity Info
#pragma warning restore CS0246 // The type or namespace name 'UserInfoEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            get
            {
                if (!string.IsNullOrEmpty(Identity.Name))
                {
                    return iUserService.Get(Convert.ToInt64(Identity.Name));
                }
                else
                {
                    return null;
                }
            }
        }

        //public SecurityRoles Role
        //{
        //    get
        //    {
        //        return (SecurityRoles)Info.Type;
        //    }
        //}
        public long Id
        {
            get
            {
                return !string.IsNullOrEmpty(Identity.Name) ? Convert.ToInt64(Identity.Name) : 0;
            }
        }
        public string Username
        {
            get
            {
                return (Info != null ? Info.Username : null);
            }
        }

        //public string FullName { get; set; }
        //public string CountryName { get; set; }
        //public string StateName { get; set; }
        //public string City { get; set; }
        //public string PermaLink { get; set; }
        //public bool IsJobseeker { get; set; }
        //public bool IsConfirmed { get; set; }
        private string[] roles { get; set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //public class UserPrincipalSerializeModel
    //{
    //    public long Id { get; set; }
    //    public int Type { get; set; }
    //    public string Username { get; set; }
    //    public string FullName { get; set; }
    //    public SecurityRoles Role { get; set; }
    //    public string CountryName { get; set; }
    //    public string StateName { get; set; }
    //    public string City { get; set; }
    //    public string PermaLink { get; set; }
    //    public bool IsJobseeker { get; set; }
    //    public bool IsConfirmed { get; set; }
    //    public string[] roles { get; set; }
    //}

    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new UserPrincipal User
        {
            get
            {           
                return base.User as UserPrincipal;
            }
        }
        //public virtual new UserInfoEntity SignInUser
        //{
        //    get
        //    {
        //        UserService us = new UserService();
        //        UserInfoEntity entity = null;
        //        if (User != null)
        //        {
        //            entity = us.Get(User.Id);
        //        }
        //        return entity;
        //    }
        //}
    }
    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new UserPrincipal User
        {
            get
            {
                return base.User as UserPrincipal;
            }
        }
        //public virtual new UserInfoEntity SignInUser
        //{
        //    get
        //    {
        //        UserService us = new UserService();
        //        UserInfoEntity entity = null;
        //        if (User != null)
        //        {
        //            entity = us.Get(User.Id);
        //        }
        //        return entity;
        //    }
        //}
    }
}