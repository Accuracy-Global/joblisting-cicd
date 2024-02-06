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
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class UserInfo
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Avtar { get; set; }
        public string Area { get; set; }
      
        public string Gender { get; set; }

        public int? Age { get; set; }
        public string Content { get; set; }
        public Nullable<int> RelationshipStatus { get; set; }
        public string ConnectUrl { get; set; }
        public string ProfileUrl { get; set; }
        public string MessageUrl { get; set; }
        public string ShareUrl { get; set; }
        public string DownloadUrl { get; set; }
        public string BlockLink { get; set; }

        public string BlockIconUrl { get; set; }
        public string UnblockUrl { get; set; }
        public string Relationship { get; set; }

        public long Views { get; set; }

        public bool IsBlockedByMe { get; set; }
        public bool IsConnected { get; set; }
        public UserInfo(long id)
        {
            UserInfoEntity profile = MemberService.Instance.GetUserInfo(id);
            string bname = profile.FullName;

            IsConnected = false;
            IsBlockedByMe = false;
            UserInfoEntity member = null;
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                member = MemberService.Instance.GetUserInfo(Convert.ToInt64(HttpContext.Current.User.Identity.Name));
                if (member.Username != profile.Username)
                {
                    IsConnected = SharedService.Instance.IsConnected(member.Username, profile.Id);
                    if (member != null)
                    {
                        BlockedPeople blocked = SharedService.Instance.GetBlockedEntity(profile.Id, member.Id);
                        if (blocked != null)
                        {
                            IsBlockedByMe = (blocked.CreatedBy.Equals(member.Username));
                        }
                    }
                }
            }

            Id = profile.Id;
            Username = profile.Username;
            CategoryId = profile.CategoryId;
            SpecializationId = profile.SpecializationId;
            Content = profile.Content;
            Type = profile.Type;
            Name = profile.FullName;
            Avtar = "/Image/Avtar?Id=" + profile.Id;
            Area = profile.CountryName;
            Gender = profile.Gender;
            Age = (int?)profile.Age;
            RelationshipStatus = profile.RelationshipStatus;
            Views = MemberService.Instance.GetProfileViews(profile.Username);
            Relationship = profile.RelationshipStatus != null ? ((Relationships)profile.RelationshipStatus).GetDescription() : null;
            ProfileUrl = profile.PermaLink;
            ConnectUrl = string.Format("/Network/Connect?EmailAddress={0}", profile.Username);
            MessageUrl = string.Format("/Message/List?SenderId={0}", profile.Id);
            DownloadUrl = ((!string.IsNullOrEmpty(profile.Content) && profile.Type == 4) ? string.Format("/Jobseeker/Download?id={0}&redirect={1}", profile.Id, "/Employer/DownloadHistory") : null);
            ShareUrl = string.Format("/share-profile?url={0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, profile.PermaLink);
            if (member != null && member.Id != profile.Id)
            {
                BlockLink = " | " + string.Format("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"aBlock\" data-role=\"{2}\" data-connected=\"{3}\">Block</a>", profile.Username, bname, ((SecurityRoles)profile.Type).GetDescription(), IsConnected);
            }
            else
            {
                BlockLink = "";
            }
            if (member != null && profile.Id == member.Id)
            {
                var blk = "<svg class=\"lnr lnr-block-disabled\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\" /><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zM4 12c0-4.42 3.58-8 8-8 1.85 0 3.55.63 4.9 1.69L5.69 16.9C4.63 15.55 4 13.85 4 12zm8 8c-1.85 0-3.55-.63-4.9-1.69L18.31 7.1C19.37 8.45 20 10.15 20 12c0 4.42-3.58 8-8 8z\" /></svg>";
                BlockIconUrl = string.Format("<a href=\"javascript:void(0);\" data-toggle=\"tooltip\" title=\"Cannot block yourself\" >{0}</a>", blk);
            }
            else
            {
                var blk = "<svg class=\"lnr lnr-block\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\" /><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zM4 12c0-4.42 3.58-8 8-8 1.85 0 3.55.63 4.9 1.69L5.69 16.9C4.63 15.55 4 13.85 4 12zm8 8c-1.85 0-3.55-.63-4.9-1.69L18.31 7.1C19.37 8.45 20 10.15 20 12c0 4.42-3.58 8-8 8z\" /></svg>";
                BlockIconUrl = string.Format("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"aBlock\" data-role=\"{2}\"  data-connected=\"{3}\">{4}</a>", profile.Username, bname, ((SecurityRoles)profile.Type).GetDescription(), IsConnected, blk);
            }
            UnblockUrl = string.Format("/Home/Unblock?id={0}&redirect=/{1}", profile.Id, profile.PermaLink);

        }

        public UserInfo(string username)
        {
            UserInfoEntity profile = MemberService.Instance.GetUserInfo(username);
            string bname = profile.FullName;
            IsConnected = false;
            IsBlockedByMe = false;
            UserInfoEntity member = null;
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                member = MemberService.Instance.GetUserInfo(Convert.ToInt64(HttpContext.Current.User.Identity.Name));
                if (member.Username != profile.Username)
                {
                    IsConnected = SharedService.Instance.IsConnected(member.Username, profile.Id);
                    if (member != null)
                    {
                        BlockedPeople blocked = SharedService.Instance.GetBlockedEntity(profile.Id, member.Id);
                        if (blocked != null)
                        {
                            IsBlockedByMe = (blocked.CreatedBy.Equals(member.Username));
                        }
                    }
                }
            }

            Id = profile.Id;
            Username = profile.Username;
            CategoryId = profile.CategoryId;
            SpecializationId = profile.SpecializationId;
            Content = profile.Content;
            Type = profile.Type;
            Name = profile.FullName;
            Avtar = "/Image/Avtar?Id=" + profile.Id;
            Area = profile.CountryName;
            Gender = profile.Gender;
            Age = (int?)profile.Age;
            RelationshipStatus = profile.RelationshipStatus;
            Views = MemberService.Instance.GetProfileViews(profile.Username);
            Relationship = profile.RelationshipStatus != null ? ((Relationships)profile.RelationshipStatus).GetDescription() : null;
            ProfileUrl = profile.PermaLink;
            ConnectUrl = string.Format("/Network/Connect?EmailAddress={0}", profile.Username);
            MessageUrl = string.Format("/Message/List?SenderId={0}", profile.Id);
            DownloadUrl = ((!string.IsNullOrEmpty(profile.Content) && profile.Type == 4) ? string.Format("/Jobseeker/Download?id={0}&redirect={1}", profile.Id, "/Employer/DownloadHistory") : null);
            ShareUrl = string.Format("/share-profile?url={0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, profile.PermaLink);

            if (member != null && member.Id != profile.Id)
            {
                BlockLink = " | " + string.Format("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"aBlock\" data-role=\"{2}\" data-connected=\"{3}\">Block</a>", profile.Username, bname, ((SecurityRoles)profile.Type).GetDescription(), IsConnected);
            }
            else
            {
                BlockLink = "";
            }

            if (member != null && profile.Id == member.Id)
            {
                var blk = "<svg class=\"lnr lnr-block-disabled\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\" /><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zM4 12c0-4.42 3.58-8 8-8 1.85 0 3.55.63 4.9 1.69L5.69 16.9C4.63 15.55 4 13.85 4 12zm8 8c-1.85 0-3.55-.63-4.9-1.69L18.31 7.1C19.37 8.45 20 10.15 20 12c0 4.42-3.58 8-8 8z\" /></svg>";
                BlockIconUrl = string.Format("<a href=\"javascript:void(0);\" title=\"Cannot block yourself\" >{0}</a>", blk);
            }
            else
            {
                var blk = "<svg class=\"lnr lnr-block\" viewBox=\"0 0 24 24\"><path d=\"M0 0h24v24H0z\" fill=\"none\" /><path d=\"M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zM4 12c0-4.42 3.58-8 8-8 1.85 0 3.55.63 4.9 1.69L5.69 16.9C4.63 15.55 4 13.85 4 12zm8 8c-1.85 0-3.55-.63-4.9-1.69L18.31 7.1C19.37 8.45 20 10.15 20 12c0 4.42-3.58 8-8 8z\" /></svg>";
                BlockIconUrl = string.Format("<a href=\"#\" data-href=\"/Home/Block?email={0}\" data-name=\"{1}\" role=\"button\" data-toggle=\"modal\" data-target=\"#cDialog\" class=\"aBlock\" data-role=\"{2}\"  data-connected=\"{3}\">{4}</a>", profile.Username, bname, ((SecurityRoles)profile.Type).GetDescription(), IsConnected, blk);
            }
            UnblockUrl = string.Format("/Home/Unblock?id={0}&redirect=/{1}", profile.Id, profile.PermaLink);
        }
    }    
}