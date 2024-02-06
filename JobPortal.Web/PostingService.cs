using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web
{
    public class PostingService
    {
        private IPostingService _service;
        public PostingService(IPostingService service)
        {
            _service = service;
        }
        public string Post(Job model)
        {
            return _service.Post(model);
        }

        public void Delete(string id)
        {
            _service.Delete(id);
        }

        public static void Post()
        {
            IUserService iUserService = new UserService();
            
        }      
    }
}