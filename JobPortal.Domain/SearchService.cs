#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain
{
#pragma warning disable CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    public class SearchService : DataService
#pragma warning restore CS0246 // The type or namespace name 'DataService' could not be found (are you missing a using directive or an assembly reference?)
    {
        private static volatile SearchService instance;
        private static readonly object sync = new object();

        /// <summary>
        /// Default private constructor
        /// </summary>
        private SearchService()
        {
        }

        /// <summary>
        /// Gets the Single Instance of SearchService
        /// </summary>
        public static SearchService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new SearchService();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Search people as per the criteria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        public List<PeopleEntity> People(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<PeopleEntity> list = new List<PeopleEntity>();

            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@Name", model.Name));
            parameters.Add(new SqlParameter("@Where", model.Where));
            parameters.Add(new SqlParameter("@AgeMin", model.AgeMin));
            parameters.Add(new SqlParameter("@AgeMax", model.AgeMax));
            parameters.Add(new SqlParameter("@Gender", model.Gender));
            parameters.Add(new SqlParameter("@Relationship", model.Relationship));
            parameters.Add(new SqlParameter("@CountryId", model.CountryId));
            parameters.Add(new SqlParameter("@StateId", model.StateId));
            parameters.Add(new SqlParameter("@City", model.City));
            parameters.Add(new SqlParameter("@Username", model.Username));
            parameters.Add(new SqlParameter("@PageNumber", model.PageNumber));
            parameters.Add(new SqlParameter("@PageSize", model.PageSize));

            list = ReadData<PeopleEntity>("SearchPeople", parameters);
  
            return list;
        }

#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<PeopleEntity> SearchPeople(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<PeopleEntity, SearchResume>("SearchPeople", model);
        }

        /// <summary>
        /// Search jobseekers as per the criteria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        public List<PeopleEntity> Jobseekers(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<PeopleEntity> list = new List<PeopleEntity>();

            //List<DbParameter> parameters = new List<DbParameter>();

            //parameters.Add(new SqlParameter("@Title", model.Title));
            //parameters.Add(new SqlParameter("@Where", model.Where));
            //parameters.Add(new SqlParameter("@AgeMin", model.AgeMin));
            //parameters.Add(new SqlParameter("@AgeMax", model.AgeMax));
            //parameters.Add(new SqlParameter("@SalaryMin", model.MinSalary));
            //parameters.Add(new SqlParameter("@SalaryMax", model.MaxSalary));
            //parameters.Add(new SqlParameter("@CategoryId", model.CategoryId));
            //parameters.Add(new SqlParameter("@SpecializationId", model.SpecializationId));
            //parameters.Add(new SqlParameter("@CountryId", model.CountryId));
            //parameters.Add(new SqlParameter("@StateId", model.StateId));
            //parameters.Add(new SqlParameter("@City", model.City));
            //parameters.Add(new SqlParameter("@Experience", (byte?)model.Experience));
            //parameters.Add(new SqlParameter("@Qualification", model.QualificationId));
            //parameters.Add(new SqlParameter("@Username", model.Username));
            //parameters.Add(new SqlParameter("@PageNumber", model.PageNumber));
            //parameters.Add(new SqlParameter("@PageSize", model.PageSize));

            return ReadData<PeopleEntity, SearchResume>("SearchJobseekers", model);
        }

        /// <summary>
        /// Search jobseekers as per the criteria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<PeopleEntity> SearchJobseekers(SearchResume model)
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchResume' could not be found (are you missing a using directive or an assembly reference?)
        {
            return ReadData<PeopleEntity, SearchResume>("SearchJobseekers", model);
        }

        /// <summary>
        /// Search people as per the criteria
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
#pragma warning disable CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<SearchedJobEntity> Jobs(SearchJob model)
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'SearchJob' could not be found (are you missing a using directive or an assembly reference?)
        {
            List<SearchedJobEntity> list = new List<SearchedJobEntity>();

            List<DbParameter> parameters = new List<DbParameter>();

            parameters.Add(new SqlParameter("@Name", model.Title));
            parameters.Add(new SqlParameter("@Where", model.Where));
            parameters.Add(new SqlParameter("@CategoryId", model.CategoryId));
            parameters.Add(new SqlParameter("@SpecializationId", model.SpecializationId));
            parameters.Add(new SqlParameter("@CountryId", model.CountryId));
            parameters.Add(new SqlParameter("@StateOrCity", model.StateOrCity));
            parameters.Add(new SqlParameter("@JobType", model.EmploymentType));
            parameters.Add(new SqlParameter("@Qualification", model.QualificationId));
            parameters.Add(new SqlParameter("@Start", model.StartDate));
            parameters.Add(new SqlParameter("@End", model.EndDate));
            parameters.Add(new SqlParameter("@Username", model.Username));
            parameters.Add(new SqlParameter("@PageNumber", model.PageNumber));
            parameters.Add(new SqlParameter("@PageSize", model.PageSize));

            list = ReadData<SearchedJobEntity>("SearchJobs", parameters);

            return list;
        }
    }
}