#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services.Contracts
{
    public interface IHelperService
    {
#pragma warning disable CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Country>> CountryList();
#pragma warning restore CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Category' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Category>> Categories();
#pragma warning restore CS0246 // The type or namespace name 'Category' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Specialization' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Specialization>> Specializations(int categoryId);
#pragma warning restore CS0246 // The type or namespace name 'Specialization' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'State' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<State>> StateList(long countryId);
#pragma warning restore CS0246 // The type or namespace name 'State' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Qualification' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Qualification>> Qualifications();
#pragma warning restore CS0246 // The type or namespace name 'Qualification' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IndeedEntity' could not be found (are you missing a using directive or an assembly reference?)
        List<IndeedEntity> IndeedList();
#pragma warning restore CS0246 // The type or namespace name 'IndeedEntity' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Package>> PackageList(long? countryId = null, string name = null, int? type=null, string ptype = null, int page = 1, int pageSize = 10);
#pragma warning restore CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Boost' could not be found (are you missing a using directive or an assembly reference?)
        Task<List<Boost>> BoostList(int id);
#pragma warning restore CS0246 // The type or namespace name 'Boost' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        Task<Package> GetPackageAsync(int id);
#pragma warning restore CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        Package GetPackage(int id);
#pragma warning restore CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PromoteItem' could not be found (are you missing a using directive or an assembly reference?)
        long ManagePromotion(PromoteItem model);
#pragma warning restore CS0246 // The type or namespace name 'PromoteItem' could not be found (are you missing a using directive or an assembly reference?)
        
#pragma warning disable CS0246 // The type or namespace name 'Promote' could not be found (are you missing a using directive or an assembly reference?)
        Promote PromotePrice(long id, string type);
#pragma warning restore CS0246 // The type or namespace name 'Promote' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
        BuyResume ResumeDonwloadPrice(long countryId);
#pragma warning restore CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
        BuyMessage MessagePrice(long countryId, int type);
#pragma warning restore CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BuyProfile' could not be found (are you missing a using directive or an assembly reference?)
        BuyProfile ProfilePrice(long countryId);
#pragma warning restore CS0246 // The type or namespace name 'BuyProfile' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BuyInterview' could not be found (are you missing a using directive or an assembly reference?)
        BuyInterview InterviewPrice(long countryId);

        Task<List<SkillsList>> SkillsList();

        Task<List<CategoryList1>> CategoryList1();
#pragma warning restore CS0246 // The type or namespace name 'BuyInterview' could not be found (are you missing a using directive or an assembly reference?)
    }
}
