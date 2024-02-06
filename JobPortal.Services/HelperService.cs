#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using Rabbit.Cache.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JobPortal.Services
{
    public class HelperService : DataService, IHelperService
    {
#pragma warning disable CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Country>> CountryList()
#pragma warning restore CS0246 // The type or namespace name 'Country' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Country> list = new List<Country>();
            InMemoryCache cache = new InMemoryCache();

            list = cache.Get<List<Country>>("country-list");
            if (list == null || list.Count == 0)
            {
                list = await ReadAsync<Country>("CountryList");
                cache.Set<List<Country>>("country-list", list);
            }
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'Category' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Category>> Categories()
#pragma warning restore CS0246 // The type or namespace name 'Category' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Category> list = new List<Category>();
            InMemoryCache cache = new InMemoryCache();

            list = cache.Get<List<Category>>("category-list");
            if (list == null || list.Count == 0)
            {
                list = await ReadAsync<Category>("CategoryList");
                cache.Set<List<Category>>("category-list", list);
            }
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'Specialization' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Specialization>> Specializations(int categoryId)
#pragma warning restore CS0246 // The type or namespace name 'Specialization' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CategoryId", categoryId)
            };

            return await ReadAsync<Specialization>("SpecializationList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'State' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<State>> StateList(long countryId)
#pragma warning restore CS0246 // The type or namespace name 'State' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId)
            };

            List<State> list = await ReadAsync<State>("StateList", parameters);
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'IndeedEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<IndeedEntity> IndeedList()
#pragma warning restore CS0246 // The type or namespace name 'IndeedEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<IndeedEntity> list = Read<IndeedEntity>("SELECT Lists.Id as CountryId, Value CountryCode,  Specializations.Id as CategoryId, Specializations.[Name] Category FROM Lists, Specializations WHERE Lists.Name='Country' AND VALUE IS NOT NULL");
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Package>> PackageList(long? countryId, string name = null, int? type = null, string ptype = null, int page = 1, int pageSize = 10)
#pragma warning restore CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId),
                new Parameter("Type", type),
                new Parameter("Name", name),
                new Parameter("PType", ptype),
                new Parameter("PageNumber", page),
                new Parameter("PageSize", pageSize)
            };
            return await ReadAsync<Package>("PackageList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'Boost' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Boost>> BoostList(int id)
#pragma warning restore CS0246 // The type or namespace name 'Boost' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("PackageId", id)
            };
            return await ReadAsync<Boost>("BoostList", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<Package> GetPackageAsync(int id)
#pragma warning restore CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("Id", id)
            };
            return await SingleAsync<Package>("PackageSingle", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        public Package GetPackage(int id)
#pragma warning restore CS0246 // The type or namespace name 'Package' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("Id", id)
            };
            return Single<Package>("PackageSingle", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'PromoteItem' could not be found (are you missing a using directive or an assembly reference?)
        public long ManagePromotion(PromoteItem model)
#pragma warning restore CS0246 // The type or namespace name 'PromoteItem' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("Type", model.Type),
                new Parameter("PackageId", model.PackageId),
                new Parameter("Id", model.Id),
                new Parameter("Days", model.Days),
                new Parameter("TransactionId", model.TransactionId),
                new Parameter("Method", model.Method)
            };
            return Scaler<long>("ManagePromotion", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
        public BuyResume ResumeDonwloadPrice(long countryId)
#pragma warning restore CS0246 // The type or namespace name 'BuyResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId),
                new Parameter("Type", "R")
            };
            return Single<BuyResume>("GetPrice", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
        public BuyMessage MessagePrice(long countryId, int type)
#pragma warning restore CS0246 // The type or namespace name 'BuyMessage' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId),
                new Parameter("Type", "M"),
                new Parameter("UserType", type)
            };
            return Single<BuyMessage>("GetPrice", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyProfile' could not be found (are you missing a using directive or an assembly reference?)
        public BuyProfile ProfilePrice(long countryId)
#pragma warning restore CS0246 // The type or namespace name 'BuyProfile' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId),
                new Parameter("Type", "P")
            };
            return Single<BuyProfile>("GetPrice", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'BuyInterview' could not be found (are you missing a using directive or an assembly reference?)
        public BuyInterview InterviewPrice(long countryId)
#pragma warning restore CS0246 // The type or namespace name 'BuyInterview' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("CountryId", countryId),
                new Parameter("Type", "I")
            };
            return Single<BuyInterview>("GetPrice", parameters);
        }

#pragma warning disable CS0246 // The type or namespace name 'Promote' could not be found (are you missing a using directive or an assembly reference?)
        public Promote PromotePrice(long id, string type)
#pragma warning restore CS0246 // The type or namespace name 'Promote' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Parameter> parameters = new List<Parameter>(){
                new Parameter("Id", id),
                new Parameter("Type", type)
            };
            return Single<Promote>("PromotePrice", parameters);
        }


#pragma warning disable CS0246 // The type or namespace name 'Qualification' could not be found (are you missing a using directive or an assembly reference?)
        public async Task<List<Qualification>> Qualifications()
#pragma warning restore CS0246 // The type or namespace name 'Qualification' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<Qualification> list = new List<Qualification>();
            InMemoryCache cache = new InMemoryCache();

            list = cache.Get<List<Qualification>>("qualification");
            if (list == null || list.Count == 0)
            {
                list = await ReadAsync<Qualification>("QualificationList");
                cache.Set<List<Qualification>>("qualification", list);
            }
            return list;
        }

        public async Task<List<SkillsList>> SkillsList()
        {
            List<SkillsList> list = new List<SkillsList>();
            InMemoryCache cache = new InMemoryCache();

            list = cache.Get<List<SkillsList>>("skills-list");
            if (list == null || list.Count == 0)
            {
                list = await ReadAsync<SkillsList>("SkillsList");
                cache.Set<List<SkillsList>>("skills-list", list);
            }
            return list;
        }


        public async Task<List<CategoryList1>> CategoryList1()
        {
            List<CategoryList1> list = new List<CategoryList1>();
            InMemoryCache cache = new InMemoryCache();

            list = cache.Get<List<CategoryList1>>("categry-list");
            if (list == null || list.Count == 0)
            {
                list = await ReadAsync<CategoryList1>("CategoryList1");
                cache.Set<List<CategoryList1>>("categry-list", list);
            }
            return list;
        }
    }
}
