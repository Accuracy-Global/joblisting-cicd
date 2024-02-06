using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Web.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Hubs
{
    public class InboxHub : Hub
    {
        public void Update(string username)
        {
            List<Activity> activities = MemberService.Instance.GetActivityList(username);
            Clients.User(username).UpdateActivity(activities);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            //List<ActivityStreamModel> activities = MemberService.Instance.GetActivityList(Context.User.Identity.Name)
            //    .Select(x => new ActivityStreamModel() { Id = x.Id, DateUpdated = x.ActivityDate, PhotoId = x.ReferenceId }).ToList();
            //Clients.User(Context.User.Identity.Name).connected(activities);
            return base.OnConnected();
        }
    }
}