﻿#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services.Contracts
{
    public interface IManageService
    {
#pragma warning disable CS0246 // The type or namespace name 'Category' could not be found (are you missing a using directive or an assembly reference?)
        Category GetCategory(int id);
#pragma warning restore CS0246 // The type or namespace name 'Category' could not be found (are you missing a using directive or an assembly reference?)
    }
}
