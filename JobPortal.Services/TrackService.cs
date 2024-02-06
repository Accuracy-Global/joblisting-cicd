#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public partial class TrackService : DataService, ITrackService
    {

        IUserService iUserService;
        public TrackService(IUserService iUserService)
        {
            this.iUserService = iUserService;
        }
        public string Bookmark(long Id, int type, long userId, out string message)
        {            
            List<Parameter> parameters = new List<Parameter>();
            string bookmarked = "";
            message = "";           
            switch (type)
            {
                case 0:
                    parameters.Add(new Parameter("JobId", Id));
                    parameters.Add(new Parameter("UserId", userId));
                    parameters.Add(new Parameter("Type", 11));                    
                    break;
                case 1:
                    parameters.Add(new Parameter("JobseekerId", Id));
                    parameters.Add(new Parameter("UserId", userId));
                    parameters.Add(new Parameter("Type", 11));                          
                    break;
            }

            bookmarked = Convert.ToString(Scaler("Bookmark", parameters));
            if (string.IsNullOrEmpty(bookmarked))
            {
                message = "Unable to bookmark!";
            }
            else if(bookmarked.Equals("1"))
            {
                message = "Already bookmarked!";
            }
            else
            {
                message = "Bookmarked successfully!";
            }
            return bookmarked;
        }
    }
}
