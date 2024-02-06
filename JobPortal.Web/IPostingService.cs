using JobPortal.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace JobPortal.Web
{
    public interface IPostingService
    {
        string Post(Job model);
        void Delete(string id);
    }
}