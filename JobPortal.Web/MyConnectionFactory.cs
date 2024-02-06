using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web
{
    public class MyConnectionFactory : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.User != null && request.User.Identity != null)
            {
                return request.User.Identity.Name;
            }
            return null;
        }
    }
}