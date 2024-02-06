using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobPortal.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.Add(new Route("photo/{id}",
            //    new ImageRouteHandler()));

            routes.MapRoute(
                name: "authenticate",
                url: "api/authenticate",
                defaults: new { controller = "Api", action = "Authenticate", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryRegister",
                url: "{country}/account/register",
                defaults: new { country = UrlParameter.Optional, controller = "Account", action = "Register", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Register",
                url: "account/register",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "RegisterNow1",
               url: "account/registernow1",
               defaults: new { controller = "Account", action = "Register1", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "CountryAdmin",
                url: "{country}/Admin/Index",
                defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Admin",
                url: "Admin/Index",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "Home",
             url: "{country}/Home",
             defaults: new { country = UrlParameter.Optional, controller = "job", action = "home", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "suriname",
             url: "suriname",
             defaults: new { controller = "job", action = "homesn", id = UrlParameter.Optional }
            );



            routes.MapRoute(
            name: "Thailand",
            url: "Thailand",
            defaults: new { controller = "job", action = "homeThai", country = "", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "Philippines",
           url: "Philippines",
           defaults: new { controller = "job", action = "homePhil", country = "", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "Japan",
           url: "Japan",
           defaults: new { controller = "job", action = "homejapan", country = "", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "india",
             url: "india",
             defaults: new { controller = "job", action = "homein", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "singapore",
             url: "singapore",
             defaults: new { controller = "job", action = "homesg", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "pakistan",
            url: "pakistan",
            defaults: new { controller = "job", action = "homepk", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "malaysia",
           url: "malaysia",
           defaults: new { controller = "job", action = "homeml", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "china",
           url: "china",
           defaults: new { controller = "job", action = "homech", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "indonesia",
           url: "indonesia",
           defaults: new { controller = "job", action = "homeid", id = UrlParameter.Optional }
          );
            routes.MapRoute(
         name: "Vietnam",
         url: "Vietnam",
         defaults: new { controller = "job", action = "Homevie", id = UrlParameter.Optional }
        );
            routes.MapRoute(
           name: "Afghanistan",
           url: "Afghanistan",
           defaults: new { controller = "job", action = "homeaf", id = UrlParameter.Optional }
          );



            routes.MapRoute(
                      name: "BRUNEI",
                      url: "BRUNEI",
                      defaults: new { controller = "job", action = "HomeBRUNEI", id = UrlParameter.Optional }
                     );



            routes.MapRoute(
          name: "CYPRUS",
          url: "CYPRUS",
          defaults: new { controller = "job", action = "HomeCYPRUS", id = UrlParameter.Optional }
         );

            routes.MapRoute(
          name: "GEORGIA",
          url: "GEORGIA",
          defaults: new { controller = "job", action = "HomeGEORGIA", id = UrlParameter.Optional }
         );

            routes.MapRoute(
         name: "IRAN",
         url: "IRAN",
         defaults: new { controller = "job", action = "HomeIRAN", id = UrlParameter.Optional }
        );

            routes.MapRoute(
         name: "IRAQ",
         url: "IRAQ",
         defaults: new { controller = "job", action = "HomeIRAQ", id = UrlParameter.Optional }
        );
            routes.MapRoute(
         name: "ISRAEL",
         url: "ISRAEL",
         defaults: new { controller = "job", action = "HomeISRAEL", id = UrlParameter.Optional }
        );



            routes.MapRoute(
        name: "JORDEN",
        url: "JORDEN",
        defaults: new { controller = "job", action = "HomeJORDEN", id = UrlParameter.Optional }
       );

            routes.MapRoute(
        name: "KAZAKHSTAN",
        url: "KAZAKHSTAN",
        defaults: new { controller = "job", action = "HomeKAZAKHSTAN", id = UrlParameter.Optional }
       );

            routes.MapRoute(
        name: "LAOS",
        url: "LAOS",
        defaults: new { controller = "job", action = "HomeLAOS", id = UrlParameter.Optional }
       );


            routes.MapRoute(
        name: "LEBANON",
        url: "LEBANON",
        defaults: new { controller = "job", action = "HomeLEBANON", id = UrlParameter.Optional }
       );




            routes.MapRoute(
        name: "MONGOLIA",
        url: "MONGOLIA",
        defaults: new { controller = "job", action = "HomeMONGOLIA", id = UrlParameter.Optional }
       );

            routes.MapRoute(
       name: "PALESTINE",
       url: "PALESTINE",
       defaults: new { controller = "job", action = "HomePALESTINE", id = UrlParameter.Optional }
      );



            routes.MapRoute(
           name: "Armenia",
           url: "Armenia",
           defaults: new { controller = "job", action = "homear", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "Azerbaijan",
           url: "Azerbaijan",
           defaults: new { controller = "job", action = "homeaz", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "Bahrain",
           url: "Bahrain",
           defaults: new { controller = "job", action = "homebr", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "Bangladesh",
           url: "Bangladesh",
           defaults: new { controller = "job", action = "homebd", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "Bhutan",
           url: "Bhutan",
           defaults: new { controller = "job", action = "homebh", id = UrlParameter.Optional }
          );
            routes.MapRoute(
     name: "maldives",
     url: "maldives",
     defaults: new { controller = "job", action = "homemd", id = UrlParameter.Optional }
    );
            routes.MapRoute(
         name: "nepal",
         url: "nepal",
         defaults: new { controller = "job", action = "homenp", id = UrlParameter.Optional }
        );
            routes.MapRoute(
         name: "myanmar",
         url: "myanmar",
         defaults: new { controller = "job", action = "homemn", id = UrlParameter.Optional }
        );
            routes.MapRoute(
            name: "srilanka",
            url: "srilanka",
            defaults: new { controller = "job", action = "homesl", id = UrlParameter.Optional }
           );
            routes.MapRoute(
         name: "cambodia",
         url: "cambodia",
         defaults: new { controller = "job", action = "homecmd", id = UrlParameter.Optional }
        );
            routes.MapRoute(
         name: "fiji",
         url: "fiji",
         defaults: new { controller = "job", action = "homefij", id = UrlParameter.Optional }
        );
            routes.MapRoute(
         name: "australia",
         url: "australia",
         defaults: new { controller = "job", action = "homeaus", id = UrlParameter.Optional }
        );
            routes.MapRoute(
         name: "ecuador",
         url: "ecuador",
         defaults: new { controller = "job", action = "homeedr", id = UrlParameter.Optional }
        );
        //    routes.MapRoute(
        //     name: "cambodia/jobs",
        //     url: "cambodia/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "Cambodia" }
        //    );
        //    routes.MapRoute(
        //     name: "fiji/jobs",
        //     url: "fiji/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "Fiji" }
        //    );
        //    routes.MapRoute(
        //     name: "australia/jobs",
        //     url: "australia/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "Australia" }
        //    );
        //    routes.MapRoute(
        //     name: "ecuador/jobs",
        //     url: "ecuador/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "Ecuador" }
        //    );
        //    routes.MapRoute(
        //     name: "maldives/jobs",
        //     url: "maldives/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "Maldives" }
        //    );
        //    routes.MapRoute(
        //     name: "nepal/jobs",
        //     url: "nepal/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "Nepal" }
        //    );
        //    routes.MapRoute(
        //     name: "myanmar/jobs",
        //     url: "myanmar/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "Myanmar" }
        //    );
        //    routes.MapRoute(
        //     name: "srilanka/jobs",
        //     url: "srilanka/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "SriLanka" }
        //    );
        //    routes.MapRoute(
        //     name: "suriname/jobs",
        //     url: "suriname/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "suriname" }
        //    );

        //    routes.MapRoute(
        //     name: "india/jobs",
        //     url: "india/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "india" }
        //    );
        //    routes.MapRoute(
        //     name: "singapore/jobs",
        //     url: "singapore/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "singapore" }
        //    );
        //    routes.MapRoute(
        //     name: "pakistan/jobs",
        //     url: "pakistan/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "pakistan" }
        //    );
        //    routes.MapRoute(
        //     name: "malaysia/jobs",
        //     url: "malaysia/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "malaysia" }
        //    );
        //    routes.MapRoute(
        //     name: "indonesia/jobs",
        //     url: "indonesia/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "indonesia" }
        //    );
        //    routes.MapRoute(
        //     name: "china/jobs",
        //     url: "china/jobs",
        //     defaults: new { controller = "job", action = "searchjobs", dileep = "china" }
        //    );
        //    routes.MapRoute(
        // name: "Afghanistan/jobs",
        // url: "Afghanistan/jobs",
        // defaults: new { controller = "job", action = "searchjobs", dileep = "Afghanistan" }
        //);
        //    routes.MapRoute(
        //    name: "Armenia/jobs",
        //    url: "Armenia/jobs",
        //    defaults: new { controller = "job", action = "searchjobs", dileep = "Armenia" }
        //   );
        //    routes.MapRoute(
        //    name: "Azerbaijan/jobs",
        //    url: "Azerbaijan/jobs",
        //    defaults: new { controller = "job", action = "searchjobs", dileep = "Azerbaijan" }
        //   );
        //    routes.MapRoute(
        //    name: "Bahrain/jobs",
        //    url: "Bahrain/jobs",
        //    defaults: new { controller = "job", action = "searchjobs", dileep = "Bahrain" }
        //   );
        //    routes.MapRoute(
        //    name: "Bangladesh/jobs",
        //    url: "Bangladesh/jobs",
        //    defaults: new { controller = "job", action = "searchjobs", dileep = "Bangladesh" }
        //   );
        //    routes.MapRoute(
        //    name: "Bhutan/jobs",
        //    url: "Bhutan/jobs",
        //    defaults: new { controller = "job", action = "searchjobs", dileep = "Bhutan" }
        //   );

            routes.MapRoute(
              name: "CountryCompanies",
              url: "{country}/companies",
              defaults: new { country = UrlParameter.Optional, controller = "Home", action = "Companies", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Companies",
              url: "companies",
              defaults: new { controller = "Home", action = "Companies", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "CountryIndividuals",
              url: "{country}/individuals",
              defaults: new { country = UrlParameter.Optional, controller = "Home", action = "Individuals", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Individuals",
              url: "individuals",
              defaults: new { controller = "Home", action = "Individuals", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "CountryShareProfile",
               url: "{country}/share-profile",
               defaults: new { country = UrlParameter.Optional, controller = "Home", action = "Share", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "ShareProfile",
               url: "share-profile",
               defaults: new { controller = "Home", action = "Share", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryMessages",
               url: "{country}/messages",
               defaults: new { country = UrlParameter.Optional, controller = "Message", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Messages",
               url: "messages",
               defaults: new { controller = "Message", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "CountryResume12",
              url: "{country}/Resume12",
              defaults: new { country = UrlParameter.Optional, controller = "JobSeeker", action = "Resume12", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Resume12",
              url: "Resume12",
              defaults: new { controller = "JobSeeker", action = "Resume12", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "CountryMessage",
             url: "{country}/message",
             defaults: new { country = UrlParameter.Optional, controller = "Message", action = "List", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "Message",
             url: "message",
             defaults: new { controller = "Message", action = "List", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "CountryConnectByPhone",
              url: "{country}/connectbyphone",
              defaults: new { country = UrlParameter.Optional, controller = "Network", action = "ConnectByPhone", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "ConnectByPhone",
              url: "connectbyphone",
              defaults: new { controller = "Network", action = "ConnectByPhone", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "CountryConnect",
               url: "{country}/connect",
               defaults: new { country = UrlParameter.Optional, controller = "Network", action = "Connect", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Connect",
               url: "connect",
               defaults: new { controller = "Network", action = "Connect", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryDisconnect",
                url: "{country}/disconnect",
                defaults: new { country = UrlParameter.Optional, controller = "Network", action = "Disconnect", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Disconnect",
                url: "disconnect",
                defaults: new { controller = "Network", action = "Disconnect", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryConnections",
               url: "{country}/connections",
               defaults: new { country = UrlParameter.Optional, controller = "Network", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Connections",
               url: "connections",
               defaults: new { controller = "Network", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryBlockedList",
                url: "{country}/blocked-list",
                defaults: new { country = UrlParameter.Optional, controller = "Network", action = "BlockedList", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BlockedList",
                url: "blocked-list",
                defaults: new { controller = "Network", action = "BlockedList", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryListBulkJobs",
                url: "{country}/list-bulk-jobs",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "UploadJobs", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ListBulkJobs",
                url: "list-bulk-jobs",
                defaults: new { controller = "Employer", action = "UploadJobs", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryListedJobs",
                url: "{country}/listed-jobs",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ListedJobs",
                url: "listed-jobs",
                defaults: new { controller = "Employer", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryListedJobs1",
                url: "{country}/listed-jobs1",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "Index_Copy", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ListedJobs1",
                url: "listed-jobs1",
                defaults: new { controller = "Employer", action = "Index_Copy", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "CountryPostedJobs",
             url: "{country}/Posted-jobs",
             defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "Index1", id = UrlParameter.Optional }
         );

            routes.MapRoute(
             name: "PostedJobs",
             url: "Posted-jobs",
             defaults: new { controller = "Admin", action = "Index1", id = UrlParameter.Optional }
         );

            routes.MapRoute(
                name: "CountryPostedJobs1",
                url: "{country}/Posted-jobs1",
                defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "Index_Copy1", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "PostedJobs1",
                url: "Posted-jobs1",
                defaults: new { controller = "Admin", action = "Index_Copy1", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryBookmarks",
                url: "{country}/bookmarks",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "Bookmarks", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Bookmarks",
                url: "bookmarks",
                defaults: new { controller = "Employer", action = "Bookmarks", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryInterviews",
                url: "{country}/interviews",
                defaults: new { country = UrlParameter.Optional, controller = "Interview", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Interviews",
                url: "interviews",
                defaults: new { controller = "Interview", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryProfileAndResume",
                url: "{country}/profile-resume",
                defaults: new { country = UrlParameter.Optional, controller = "JobSeeker", action = "UpdateProfile1", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ProfileAndResume",
                url: "profile-resume",
                defaults: new { controller = "JobSeeker", action = "UpdateProfile1", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryDownloads",
                url: "{country}/downloads",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "DownloadHistory", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Downloads",
                url: "downloads",
                defaults: new { controller = "Employer", action = "DownloadHistory", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryCheckOut",
               url: "{country}/checkout",
               defaults: new { country = UrlParameter.Optional, controller = "Payment", action = "Checkout", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "CheckOut",
               url: "checkout",
               defaults: new { controller = "Payment", action = "Checkout", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "CountryBilling",
                url: "{country}/billing",
                defaults: new { country = UrlParameter.Optional, controller = "Payment", action = "Billing", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Billing",
                url: "billing",
                defaults: new { controller = "Payment", action = "Billing", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryPurchaseProfiles",
                url: "{country}/purchasedprofiles",
                defaults: new { country = UrlParameter.Optional, controller = "Payment", action = "PaidProfiles", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "PurchaseProfiles",
                url: "purchasedprofiles",
                defaults: new { controller = "Payment", action = "PaidProfiles", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryEmployerProfile",
                url: "{country}/company/profile",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "UpdateProfile", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EmployerProfile",
                url: "company/profile",
                defaults: new { controller = "Employer", action = "UpdateProfile", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryChangeEmailAddress",
                url: "{country}/change-email-address",
                defaults: new { country = UrlParameter.Optional, controller = "Account", action = "ChangeEmailAddress", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChangeEmailAddress",
                url: "change-email-address",
                defaults: new { controller = "Account", action = "ChangeEmailAddress", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryLoginHistory",
               url: "{country}/login-history",
               defaults: new { country = UrlParameter.Optional, controller = "Account", action = "LoginHistory", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "LoginHistory",
               url: "login-history",
               defaults: new { controller = "Account", action = "LoginHistory", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryAnnouncements",
                url: "{country}/announcements",
                defaults: new { country = UrlParameter.Optional, controller = "Announcement", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Announcements",
                url: "announcements",
                defaults: new { controller = "Announcement", action = "Index", id = UrlParameter.Optional }
            );

            // Check
            routes.MapRoute(
                name: "CountryCampaign",
                url: "{country}/campaign",
                defaults: new { country = UrlParameter.Optional, controller = "Campaign", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Campaign",
                url: "campaign",
                defaults: new { controller = "Campaign", action = "Index", id = UrlParameter.Optional }
            );
            //

            routes.MapRoute(
               name: "CountryWebsites",
               url: "{country}/websites",
               defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "Websites", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Websites",
               url: "websites",
               defaults: new { controller = "Admin", action = "Websites", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "CountrySMJobs",
               url: "{country}/SMJobs",
               defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "SMJobs", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "SMJobs",
               url: "SMJobs",
               defaults: new { controller = "Admin", action = "SMJobs", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "CountryEmployerDashboard",
                url: "{country}/employer",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EmployerDashboard",
                url: "employer",
                defaults: new { controller = "Employer", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryJobseekerDashboard",
                url: "{country}/jobseeker",
                defaults: new { country = UrlParameter.Optional, controller = "Jobseeker", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "JobseekerDashboard",
                url: "jobseeker",
                defaults: new { controller = "Jobseeker", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryJobApplications",
                url: "{country}/applications",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "Applications", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "JobApplications",
                url: "applications",
                defaults: new { controller = "Employer", action = "Applications", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "CountryForgotPassword",
                "{country}/forgot-password",
                defaults: new { country = UrlParameter.Optional, controller = "Home", action = "ForgotPassword", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "ForgotPassword",
                "forgot-password",
                defaults: new { controller = "Home", action = "ForgotPassword", Id = UrlParameter.Optional } // Parameter defaults
            );

            // Check
            routes.MapRoute(
                "CountryJobs/",
                "{country}/Jobs",
                defaults: new { country = UrlParameter.Optional, controller = "Job", action = "SearchJobs" } // Parameter defaults
            );

            routes.MapRoute(
                "Jobs/",
                "Jobs",
                defaults: new { controller = "Job", action = "SearchJobs" } // Parameter defaults
            );
            //

            routes.MapRoute(
            name: "Countryaboutus",
            url: "{country}/home/aboutus",
            defaults: new { country = UrlParameter.Optional, controller = "home", action = "aboutus", id = UrlParameter.Optional }
           );

            routes.MapRoute(
            name: "aboutus",
            url: "home/aboutus",
            defaults: new { controller = "home", action = "aboutus", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                "CountryCompany",
                "{country}/company",
                new { country = UrlParameter.Optional, controller = "Job", action = "SearchCompany", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Company",
                "company",
                new { controller = "Job", action = "SearchCompany", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "CountryStudent",
                "{country}/student",
                defaults: new { country = UrlParameter.Optional, controller = "Job", action = "SearchStudent", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Student",
                "student",
                defaults: new { controller = "Job", action = "SearchStudent", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "CountrySearchResumes",
                url: "{country}/search-resumes",
                defaults: new { country = UrlParameter.Optional, controller = "Home", action = "SearchResumes", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SearchResumes",
                url: "search-resumes",
                defaults: new { controller = "Home", action = "SearchResumes", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountrySearchJobseeker",
                url: "{country}/search-jobseekers",
                defaults: new { country = UrlParameter.Optional, controller = "Jobseeker", action = "Search", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SearchJobseeker",
                url: "search-jobseekers",
                defaults: new { controller = "Jobseeker", action = "Search", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryBasedSearchPeople",
                url: "{country}/search-people",
                defaults: new { country = UrlParameter.Optional, controller = "Home", action = "SearchPeople", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SearchPeople",
                url: "search-people",
                defaults: new { controller = "Home", action = "SearchPeople", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryBlock",
               url: "{country}/block",
               defaults: new { country = UrlParameter.Optional, controller = "Home", action = "Block", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Block",
               url: "block",
               defaults: new { controller = "Home", action = "Block", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "CountryUnblock",
               url: "{country}/unblock",
               defaults: new { country = UrlParameter.Optional, controller = "Home", action = "Unblock", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Unblock",
               url: "unblock",
               defaults: new { controller = "Home", action = "Unblock", id = UrlParameter.Optional }
           );

            // Check
            routes.MapRoute(
                name: "CountryAutoMatchJobs",
                url: "{country}/auto-match-jobs",
                defaults: new { country = UrlParameter.Optional, controller = "JobSeeker", action = "AutoMatch", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AutoMatchJobs",
                url: "auto-match-jobs",
                defaults: new { controller = "JobSeeker", action = "AutoMatch", id = UrlParameter.Optional }
            );
            //

            routes.MapRoute(
                name: "CountryBookmarkedJobs",
                url: "{country}/bookmarked-jobs",
                defaults: new { country = UrlParameter.Optional, controller = "JobSeeker", action = "BookMarkedJobs", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BookmarkedJobs",
                url: "bookmarked-jobs",
                defaults: new { controller = "JobSeeker", action = "BookMarkedJobs", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "CountryJobApply",
                "{country}/apply",
                new { country = UrlParameter.Optional, controller = "Job", action = "Apply", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "JobApply",
                "apply",
                new { controller = "Job", action = "Apply", Id = UrlParameter.Optional } // Parameter defaults
            );

            // Check
            routes.MapRoute(
                "CountryAllJobsOfcompany",
                "{country}/alljobs-of-company-{EmployerId}",
                new { country = UrlParameter.Optional, controller = "Job", action = "viewalljobofcompany", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "AllJobsOfcompany",
                "alljobs-of-company-{EmployerId}",
                new { controller = "Job", action = "viewalljobofcompany", Id = UrlParameter.Optional } // Parameter defaults
            );
            //

            routes.MapRoute(
                "CountryJobSummary",
                "{country}/job/{title}-{Id}",
                new { country = UrlParameter.Optional, controller = "job", action = "jobview", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "JobSummary",
                "job/{title}-{Id}",
                new { controller = "job", action = "jobview", Id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "Countryterms",
                url: "{country}/terms",
                defaults: new { country = UrlParameter.Optional, controller = "Help", action = "Terms", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "terms",
                url: "terms",
                defaults: new { controller = "Help", action = "Terms", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Countryprivacy",
                url: "{country}/privacy",
                defaults: new { country = UrlParameter.Optional, controller = "Help", action = "Privacy", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "privacy",
                url: "privacy",
                defaults: new { controller = "Help", action = "Privacy", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Countrycopyright",
                url: "{country}/copyright",
                defaults: new { country = UrlParameter.Optional, controller = "Help", action = "Copyright", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "copyright",
                url: "copyright",
                defaults: new { controller = "Help", action = "Copyright", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Countrycontact",
                url: "{country}/contact",
                defaults: new { country = UrlParameter.Optional, controller = "Home", action = "Contact", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryListJob",
                url: "{country}/listjob",
                defaults: new { country = UrlParameter.Optional, controller = "Employer", action = "ListJob", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ListJob",
                url: "listjob",
                defaults: new { controller = "Employer", action = "ListJob", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryListJob1",
               url: "{country}/listjob1",
               defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "ListJob1", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "ListJob1",
               url: "listjob1",
               defaults: new { controller = "Admin", action = "ListJob1", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "CountryAdminProfile",
               url: "{country}/administrator/profile",
               defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "MyProfile", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "AdminProfile",
               url: "administrator/profile",
               defaults: new { controller = "Admin", action = "MyProfile", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryAdmin/ProfilesLists",
               url: "{country}/Admin/ProfilesLists/{skil}/{ig}",
               defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "Profileslists", skil = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Admin/ProfilesLists",
               url: "Admin/ProfilesLists/{skil}/{ig}",
               defaults: new { controller = "Admin", action = "Profileslists", skil = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CountryAdmin/Monsterlist",
               url: "{country}/Admin/Monsterlist/{skil}/{ig}",
               defaults: new { country = UrlParameter.Optional, controller = "Admin", action = "Monsterlist", skil = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Admin/Monsterlist",
               url: "Admin/Monsterlist/{skil}/{ig}",
               defaults: new { controller = "Admin", action = "Monsterlist", skil = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryConfirmAccount",
                url: "{country}/confirm",
                defaults: new { country = UrlParameter.Optional, controller = "Account", action = "Confirm", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConfirmAccount",
                url: "confirm",
                defaults: new { controller = "Account", action = "Confirm", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CountryConfirmEmail",
                url: "{country}/verify",
                defaults: new { country = UrlParameter.Optional, controller = "Account", action = "ConfirmEmail", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConfirmEmail",
                url: "verify",
                defaults: new { controller = "Account", action = "ConfirmEmail", id = UrlParameter.Optional }
            );

            // Check
            routes.MapRoute(
               name: "ChangePass",
               url: "account/changepass",
               defaults: new { controller = "Account", action = "ChangePass", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "ChangePassword",
                url: "account/changepassword",
                defaults: new { controller = "Account", action = "Manage", id = UrlParameter.Optional }
            );
            //
            // Done

            // Check
            routes.MapRoute(
                "",
                "{address}",
                new { controller = "Job", action = "Home" } // Parameter defaults
            );
            //

            routes.MapRoute(
                "Countryprofile",
                "{country}/profile/{address}",
                new { country = UrlParameter.Optional, controller = "Home", action = "MemberProfile" } // Parameter defaults
            );

            routes.MapRoute(
                "profile",
                "profile/{address}",
                new { controller = "Home", action = "MemberProfile" } // Parameter defaults
            );

            routes.MapRoute(
                name: "Countrylogin",
                url: "{country}/account/login",
                defaults: new { country = UrlParameter.Optional, controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "login",
                url: "account/login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Countrylogout",
                url: "{country}/account/logout",
                defaults: new { country = UrlParameter.Optional, controller = "Account", action = "LogOff", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "logout",
                url: "account/logout",
                defaults: new { controller = "Account", action = "LogOff", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Countrywebinar",
                url: "{country}/webinar",
                defaults: new { country = UrlParameter.Optional, controller = "Webinar", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "webinar",
                url: "webinar",
                defaults: new { controller = "Webinar", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SingaporeDefault",
                url: "singapore/{controller}/{action}/{id}",
                defaults: new { country = "singapore", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MalaysiaDefault",
                url: "malaysia/{controller}/{action}/{id}",
                defaults: new { country = "malaysia", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "IndiaDefault",
                url: "india/{controller}/{action}/{id}",
                defaults: new { country = "india", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "ChinaDefault",
                url: "china/{controller}/{action}/{id}",
                defaults: new { country = "china", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "VietnamDefault",
                url: "vietnam/{controller}/{action}/{id}",
                defaults: new { country = "vietnam", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "USDefault",
                url: "us/{controller}/{action}/{id}",
                defaults: new { country = "us", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CanadaDefault",
                url: "canada/{controller}/{action}/{id}",
                defaults: new { country = "canada", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "SurinameDefault",
                url: "suriname/{controller}/{action}/{id}",
                defaults: new { country = "suriname", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "ThailandDefault",
                url: "thailand/{controller}/{action}/{id}",
                defaults: new { country = "thailand", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "PhilippinesDefault",
                url: "philippines/{controller}/{action}/{id}",
                defaults: new { country = "philippines", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "JapanDefault",
                url: "japan/{controller}/{action}/{id}",
                defaults: new { country = "japan", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "IndonesiaDefault",
                url: "indonesia/{controller}/{action}/{id}",
                defaults: new { country = "indonesia", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "AfghanistanDefault",
                url: "afghanistan/{controller}/{action}/{id}",
                defaults: new { country = "afghanistan", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "BruneiDefault",
                url: "brunei/{controller}/{action}/{id}",
                defaults: new { country = "brunei", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "CyprusDefault",
                url: "cyprus/{controller}/{action}/{id}",
                defaults: new { country = "cyprus", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "GeorgiaDefault",
                url: "georgia/{controller}/{action}/{id}",
                defaults: new { country = "georgia", controller = "Default", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { action = "home",id="" }
            //);
        }
    }
}