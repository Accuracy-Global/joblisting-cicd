﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobPortal.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class JobPortalEntities : DbContext
    {
        public JobPortalEntities()
            : base("name=JobPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ConfigParameter> ConfigParameters { get; set; }
        public virtual DbSet<FrequencyType> FrequencyTypes { get; set; }
        public virtual DbSet<Interview> Interviews { get; set; }
        public virtual DbSet<List> Lists { get; set; }
        public virtual DbSet<LoginHistory> LoginHistories { get; set; }
        public virtual DbSet<Newsletter> Newsletters { get; set; }
        public virtual DbSet<SitePage> SitePages { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<SubSpecialization> SubSpecializations { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }
        public virtual DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
        public virtual DbSet<OnlineUser> OnlineUsers { get; set; }
        public virtual DbSet<Resume> Resumes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Tracking> Trackings { get; set; }
        public virtual DbSet<TrackingDetail> TrackingDetails { get; set; }
        public virtual DbSet<FollowUp> FollowUps { get; set; }
        public virtual DbSet<AlertHistory> AlertHistories { get; set; }
        public virtual DbSet<Alert> Alerts { get; set; }
        public virtual DbSet<Unsubscription> Unsubscriptions { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<BlockedPeople> BlockedPeoples { get; set; }
        public virtual DbSet<Connection> Connections { get; set; }
        public virtual DbSet<EmailAddress> EmailAddresses { get; set; }
        public virtual DbSet<InterviewDiscussion> InterviewDiscussions { get; set; }
        public virtual DbSet<InterviewNote> InterviewNotes { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<UserAnnouncement> UserAnnouncements { get; set; }
        public virtual DbSet<Communication> Communications { get; set; }
        public virtual DbSet<Hit> Hits { get; set; }
        public virtual DbSet<Tip> Tips { get; set; }
        public virtual DbSet<Dictionary> Dictionaries { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<AlertSetting> AlertSettings { get; set; }
        public virtual DbSet<Invite> Invites { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<ProfileView> ProfileViews { get; set; }
        public virtual DbSet<PeopleMatch> PeopleMatches { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<User_Certificate> User_Certificate { get; set; }
        public virtual DbSet<User_Education> User_Education { get; set; }
        public virtual DbSet<User_Skills> User_Skills { get; set; }
        public virtual DbSet<User_Experience> User_Experience { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<UserCampaign> UserCampaigns { get; set; }
        public virtual DbSet<ListJob1C> ListJob1C { get; set; }
        public virtual DbSet<SMJobList> SMJobLists { get; set; }
        public virtual DbSet<WebsiteList> WebsiteLists { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int USP_UpdateEmailAddress(string oldEmail, string newEmail)
        {
            var oldEmailParameter = oldEmail != null ?
                new ObjectParameter("OldEmail", oldEmail) :
                new ObjectParameter("OldEmail", typeof(string));
    
            var newEmailParameter = newEmail != null ?
                new ObjectParameter("NewEmail", newEmail) :
                new ObjectParameter("NewEmail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USP_UpdateEmailAddress", oldEmailParameter, newEmailParameter);
        }
    
        public virtual int USP_ValidateEmails(string body, Nullable<bool> delete)
        {
            var bodyParameter = body != null ?
                new ObjectParameter("Body", body) :
                new ObjectParameter("Body", typeof(string));
    
            var deleteParameter = delete.HasValue ?
                new ObjectParameter("Delete", delete) :
                new ObjectParameter("Delete", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USP_ValidateEmails", bodyParameter, deleteParameter);
        }
    }
}