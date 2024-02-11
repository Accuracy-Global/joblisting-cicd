//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class UserProfile
    {
        public UserProfile()
        {
            this.OnlineUsers = new HashSet<OnlineUser>();
            this.Trackings = new HashSet<Tracking>();
            this.Interviews = new HashSet<Interview>();
            this.FollowUps = new HashSet<FollowUp>();
            this.BlockedPeoples = new HashSet<BlockedPeople>();
            this.Blockers = new HashSet<BlockedPeople>();
            this.Connections = new HashSet<Connection>();
            this.TrackedJobseekers = new HashSet<Tracking>();
            this.InterviewDiscussions = new HashSet<InterviewDiscussion>();
            this.InterviewNotes = new HashSet<InterviewNote>();
            this.Announcements = new HashSet<UserAnnouncement>();
            this.Receivers = new HashSet<Communication>();
            this.Senders = new HashSet<Communication>();
            this.Communications = new HashSet<Communication>();
            this.Activities = new HashSet<Activity>();
            this.AlertSettings = new HashSet<AlertSetting>();
            this.Photos = new HashSet<Photo>();
            this.ProfileViews = new HashSet<ProfileView>();
            this.Jobs = new HashSet<Job>();
            this.User_Education = new HashSet<User_Education>();
            this.User_Skills = new HashSet<User_Skills>();
            this.User_Experience = new HashSet<User_Experience>();
            this.UserCampaigns = new HashSet<UserCampaign>();
        }
    
        public long UserId { get; set; }
        public string Username { get; set; }
        public int Type { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Summary { get; set; }
        public string Address { get; set; }
        public Nullable<long> CountryId { get; set; }
        public Nullable<long> StateId { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public string Website { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public string PermaLink { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderUsername { get; set; }
        public string ProviderAccessToken { get; set; }
        public string Title { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public Nullable<byte> Age { get; set; }
        public string CurrentEmployer { get; set; }
        public Nullable<byte> Experience { get; set; }
        public string CurrentCurrency { get; set; }
        public Nullable<decimal> DrawingSalary { get; set; }
        public string ExpectedCurrency { get; set; }
        public Nullable<decimal> ExpectedSalary { get; set; }
        public string AreaOfExpertise { get; set; }
        public string ProfessionalExperience { get; set; }
        public string TechnicalSkills { get; set; }
        public string ManagementSkills { get; set; }
        public string Education { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string School { get; set; }
        public string University { get; set; }
        public Nullable<long> QualificationId { get; set; }
        public Nullable<long> EmploymentTypeId { get; set; }
        public string Certifications { get; set; }
        public string Affiliations { get; set; }
        public string PreviousEmployer { get; set; }
        public string Gender { get; set; }
        public Nullable<int> RelationshipStatus { get; set; }
        public string ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public string NewUsername { get; set; }
        public int EmailCorrectionTries { get; set; }
        public bool IsValidUsername { get; set; }
        public string Mobile { get; set; }
        public string PhoneCountryCode { get; set; }
        public string MobileCountryCode { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string GooglePlus { get; set; }
        public bool IsBuild { get; set; }
        public bool IsApproved { get; set; }
        public string CurrentEmployerFromMonth { get; set; }
        public string CurrentEmployerFromYear { get; set; }
        public string PreviousEmployerFromMonth { get; set; }
        public string PreviousEmployerFromYear { get; set; }
        public string CurrentEmployerToMonth { get; set; }
        public string CurrentEmployerToYear { get; set; }
        public string PreviousEmployerToMonth { get; set; }
        public string PreviousEmployerToYear { get; set; }
    
        public virtual List Country { get; set; }
        public virtual List State { get; set; }
        public virtual ICollection<OnlineUser> OnlineUsers { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
        public virtual ICollection<FollowUp> FollowUps { get; set; }
        public virtual ICollection<BlockedPeople> BlockedPeoples { get; set; }
        public virtual ICollection<BlockedPeople> Blockers { get; set; }
        public virtual Specialization Category { get; set; }
        public virtual SubSpecialization Specialization { get; set; }
        public virtual ICollection<Connection> Connections { get; set; }
        public virtual ICollection<Tracking> TrackedJobseekers { get; set; }
        public virtual ICollection<InterviewDiscussion> InterviewDiscussions { get; set; }
        public virtual ICollection<InterviewNote> InterviewNotes { get; set; }
        public virtual ICollection<UserAnnouncement> Announcements { get; set; }
        public virtual ICollection<Communication> Receivers { get; set; }
        public virtual ICollection<Communication> Senders { get; set; }
        public virtual ICollection<Communication> Communications { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<AlertSetting> AlertSettings { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<ProfileView> ProfileViews { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual User_Certificate User_Certificate { get; set; }
        public virtual ICollection<User_Education> User_Education { get; set; }
        public virtual ICollection<User_Skills> User_Skills { get; set; }
        public virtual ICollection<User_Experience> User_Experience { get; set; }
        public virtual ICollection<UserCampaign> UserCampaigns { get; set; }
    }
}