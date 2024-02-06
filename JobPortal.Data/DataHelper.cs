using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;

namespace JobPortal.Data
{
    public static class DataHelperExtension
    {
        public static IEnumerable<T> TakePercent<T>(this ICollection<T> source, double percent)
        {
            var count = (int)(source.Count * percent / 100);
            if (count == 0)
            {
                count = source.Count;
            }
            return source.Take(count);
        }

        public static IQueryable<T> In<T>(this IQueryable<T> source,
            IQueryable<T> checkAgainst)
        {
            IQueryable<T> list = null;
            try
            {
                list = from s in source
                       where checkAgainst.Contains(s)
                       select s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public static IQueryable<T> NotIn<T>(this IQueryable<T> source,
            IQueryable<T> checkAgainst)
        {
            IQueryable<T> list = null;
            try
            {
                list = from s in source
                       where !checkAgainst.Contains(s)
                       select s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        ///     Builds the filter criteria for query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static BinaryExpression Build<T>(Hashtable parameters, out ParameterExpression parameter)
            where T : class
        {
            var type = typeof(T);
            BinaryExpression criteria = null;
            parameter = Expression.Parameter(type, "p");

            try
            {
                var keys = parameters.Keys.GetEnumerator();
                while (keys.MoveNext())
                {
                    var key = keys.Current.ToString();
                    var property = type.GetProperty(key);
                    var left = Expression.PropertyOrField(parameter, property.Name);
                    var right = Expression.Constant(parameters[key], property.PropertyType);

                    if (criteria == null)
                    {
                        criteria = Expression.Equal(left, right);
                    }
                    else
                    {
                        var body = Expression.Equal(left, right);
                        criteria = Expression.And(criteria, body);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return criteria;
        }
    }

    public class DataHelper
    {
        private static string DELETE_FIELD_NAME = "IsDeleted";
        private static string ACTIVE_FIELD_NAME = "IsActive";
        private static string EXPIRED_FIELD_NAME = "IsExpired";
        private JobPortalEntities context;
        private static readonly object sync = new object();
        //private static readonly string DELETE_FIELD_NAME = "IsDeleted";
        //private static readonly string ACTIVE_FIELD_NAME = "IsActive";

        public DataHelper()
        {
            this.context = new JobPortalEntities();
        }

        public DataHelper(JobPortalEntities context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        ///     Builds the filter criteria for query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public BinaryExpression BuildEqualityAnd<T>(Hashtable parameters, out ParameterExpression parameter)
            where T : class
        {
            var type = typeof(T);
            BinaryExpression criteria = null;
            parameter = Expression.Parameter(type, "p");

            try
            {
                var keys = parameters.Keys.GetEnumerator();
                while (keys.MoveNext())
                {
                    var key = keys.Current.ToString();
                    var property = type.GetProperty(key);
                    var left = Expression.PropertyOrField(parameter, property.Name);
                    var right = Expression.Constant(parameters[key], property.PropertyType);

                    if (criteria == null)
                    {
                        criteria = Expression.Equal(left, right);
                    }
                    else
                    {
                        var body = Expression.Equal(left, right);
                        criteria = Expression.And(criteria, body);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return criteria;
        }

        /// <summary>
        /// Build lamda express with "AND" condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> BuildAnd<T>(List<Parameter> parameters)
           where T : class
        {
            var type = typeof(T);
            BinaryExpression criteria = null;
            ParameterExpression parameter = Expression.Parameter(type, "x");
            PropertyInfo property;
            MemberExpression left;
            ConstantExpression right;

            try
            {
                foreach (Parameter p in parameters)
                {
                    switch (p.Comparison)
                    {
                        case ParameterComparisonTypes.Equals:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.Equal(left, right);
                            }
                            else
                            {
                                var body = Expression.Equal(left, right);
                                criteria = Expression.And(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.NotEquals:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.NotEqual(left, right);
                            }
                            else
                            {
                                var body = Expression.NotEqual(left, right);
                                criteria = Expression.And(criteria, body);
                            }
                            break;

                        case ParameterComparisonTypes.GreaterThan:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.GreaterThan(left, right);
                            }
                            else
                            {
                                var body = Expression.GreaterThan(left, right);
                                criteria = Expression.And(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.GreaterThanEquals:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.GreaterThanOrEqual(left, right);
                            }
                            else
                            {
                                var body = Expression.GreaterThanOrEqual(left, right);
                                criteria = Expression.And(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.LessThan:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.LessThan(left, right);
                            }
                            else
                            {
                                var body = Expression.LessThan(left, right);
                                criteria = Expression.And(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.LessThanEqual:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.LessThanOrEqual(left, right);
                            }
                            else
                            {
                                var body = Expression.LessThanOrEqual(left, right);
                                criteria = Expression.And(criteria, body);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Expression.Lambda<Func<T, bool>>(criteria, parameter);
        }

        /// <summary>
        /// Build lamda express with "OR" condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> BuildOr<T>(List<Parameter> parameters)
           where T : class
        {
            var type = typeof(T);
            BinaryExpression criteria = null;
            ParameterExpression parameter = Expression.Parameter(type, "p");
            PropertyInfo property;
            MemberExpression left;
            ConstantExpression right;

            try
            {
                foreach (Parameter p in parameters)
                {
                    switch (p.Comparison)
                    {
                        case ParameterComparisonTypes.Equals:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.Equal(left, right);
                            }
                            else
                            {
                                var body = Expression.Equal(left, right);
                                criteria = Expression.Or(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.NotEquals:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.NotEqual(left, right);
                            }
                            else
                            {
                                var body = Expression.NotEqual(left, right);
                                criteria = Expression.Or(criteria, body);
                            }
                            break;

                        case ParameterComparisonTypes.GreaterThan:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.GreaterThan(left, right);
                            }
                            else
                            {
                                var body = Expression.GreaterThan(left, right);
                                criteria = Expression.Or(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.GreaterThanEquals:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.GreaterThanOrEqual(left, right);
                            }
                            else
                            {
                                var body = Expression.GreaterThanOrEqual(left, right);
                                criteria = Expression.Or(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.LessThan:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.LessThan(left, right);
                            }
                            else
                            {
                                var body = Expression.LessThan(left, right);
                                criteria = Expression.Or(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.LessThanEqual:

                            property = type.GetProperty(p.Name);
                            left = Expression.PropertyOrField(parameter, property.Name);
                            right = Expression.Constant(p.Value, property.PropertyType);

                            if (criteria == null)
                            {
                                criteria = Expression.LessThanOrEqual(left, right);
                            }
                            else
                            {
                                var body = Expression.LessThanOrEqual(left, right);
                                criteria = Expression.Or(criteria, body);
                            }
                            break;
                        case ParameterComparisonTypes.Contains:

                            var p1 = Expression.Parameter(typeof(T), "x");
                            left = Expression.Property(parameter, p.Name);
                            right = Expression.Constant(p.Value);
                            var ptype = right.Type;
                            var containsmethod = ptype.GetMethod("Contains", new[] { typeof(string) });
                            var call = Expression.Call(left, containsmethod, right);

                            var lambda = Expression.Lambda<Func<T, bool>>(call, parameter);

                            if (criteria == null)
                            {
                                criteria = Expression.MakeBinary(ExpressionType.Call, left, right, true, containsmethod);
                            }
                            else
                            {
                                var body = Expression.MakeBinary(ExpressionType.Call, left, right, true, containsmethod);
                                criteria = Expression.Or(criteria, body);
                            }

                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Expression.Lambda<Func<T, bool>>(criteria, parameter); ;
        }

        public IQueryable<T> GetQueryResult<T>(string query)
        {
            IQueryable<T> list = context.Database.SqlQuery<T>(query).AsQueryable();
            return list;

        }

        public Expression<Func<T, bool>> BuildExpression<T>(List<Parameter> parameters)
        {
            //Create the predicate and initialize it
            Expression<Func<T, bool>> predicate = x => false;
            //Define the type
            ParameterExpression parameterExp = Expression.Parameter(typeof(T), "type");

            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //Iterate over each kvp
            foreach (var p in parameters)
            {
                var body = predicate.Body;
                //set the property or field we are checking against
                var memberExpr = Expression.PropertyOrField(parameterExp, p.Name);
                var constExpr = Expression.Constant(p.Value, typeof(string));
                var containsMethodExpr = Expression.Call(memberExpr, method, constExpr);

                body = Expression.OrElse(body, containsMethodExpr);

                predicate = Expression.Lambda<Func<T, bool>>(body, parameterExp);
            }
            return predicate;
        }

        public Expression<Func<T, object>> BuildExpression<T>(string identity)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var left = Expression.PropertyOrField(parameter, identity);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(left, typeof(object)), parameter);
            return lambda;
        }

        public BinaryExpression Build<T>(Parameter parameter)
        {

            var type = typeof(T);
            BinaryExpression criteria = null;
            ParameterExpression parameterExpression = Expression.Parameter(type, "p");
            PropertyInfo property;
            MemberExpression left;
            ConstantExpression right;

            property = type.GetProperty(parameter.Name);
            left = Expression.PropertyOrField(parameterExpression, property.Name);
            right = Expression.Constant(parameter.Value, property.PropertyType);

            switch (parameter.Comparison)
            {
                case ParameterComparisonTypes.Equals:
                    criteria = Expression.Equal(left, right);
                    break;
                case ParameterComparisonTypes.NotEquals:
                    criteria = Expression.NotEqual(left, right);
                    break;
                case ParameterComparisonTypes.GreaterThan:
                    criteria = Expression.GreaterThan(left, right);
                    break;
                case ParameterComparisonTypes.GreaterThanEquals:
                    criteria = Expression.GreaterThanOrEqual(left, right);
                    break;
                case ParameterComparisonTypes.LessThan:
                    criteria = Expression.LessThan(left, right);
                    break;
                case ParameterComparisonTypes.LessThanEqual:
                    criteria = Expression.LessThanOrEqual(left, right);
                    break;
            }

            return criteria;
        }

        /// <summary>
        /// Gets the maximun identity number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity">As name of property</param>
        /// <returns></returns>
        public object GetMaxID<T>(string identity = "")
            where T : class
        {
            var id = new object();
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    var parameter = Expression.Parameter(typeof(T), "p");
                    if (!string.IsNullOrEmpty(identity))
                    {
                        var left = Expression.PropertyOrField(parameter, identity);
                        var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(left, typeof(object)), parameter);
                        id = context.Set<T>().Max(lambda.Compile());
                    }
                    else
                    {
                        var left = Expression.PropertyOrField(parameter, "Id");
                        var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(left, typeof(object)), parameter);
                        id = context.Set<T>().Max(lambda.Compile());
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        /// <summary>
        /// Gets the maximun identity number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identity">As name of property</param>
        /// <returns></returns>
        public object GetMaxID<T>(Hashtable parameters)
            where T : class
        {
            var id = new object();
            ParameterExpression expression;
            BinaryExpression criteria = BuildEqualityAnd<T>(parameters, out expression);
            try
            {
                ParameterExpression parameter = null;
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
           
                id = context.Set<T>().Max(whereClause);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        /// <summary>
        ///     Checks the record is exist or not.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True/False</returns>
        public bool IsExist<T>(int identityValue, string identityName = "")
            where T : class
        {
            var exist = false;
            BinaryExpression criteria = null;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    var parameters = new Hashtable();
                    if (!string.IsNullOrEmpty(identityName))
                    {
                        parameters.Add(identityName, identityValue);
                    }
                    else
                    {
                        parameters.Add("Id", identityValue);
                    }

                    ParameterExpression parameter = null;
                    criteria = BuildEqualityAnd<T>(parameters, out parameter);

                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                    var query = context.Set<T>().Where(whereClause);
                    exist = query.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exist;
        }

        /// <summary>
        ///     Checks the record is exist or not.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True/False</returns>
        public bool IsExist<T>(long identityValue, string identityName = "")
            where T : class
        {
            var exist = false;
            BinaryExpression criteria = null;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    var parameters = new Hashtable();
                    if (!string.IsNullOrEmpty(identityName))
                    {
                        parameters.Add(identityName, identityValue);
                    }
                    else
                    {
                        parameters.Add("Id", identityValue);
                    }

                    ParameterExpression parameter = null;
                    criteria = BuildEqualityAnd<T>(parameters, out parameter);

                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                    var query = context.Set<T>().Where(whereClause);
                    exist = query.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exist;
        }

        /// <summary>
        ///     Checks the record is exist or not.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True/False</returns>
        public bool IsExist<T>(Guid identityValue, string identityName = "")
            where T : class
        {
            var exist = false;
            BinaryExpression criteria = null;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    var parameters = new Hashtable();
                    if (!string.IsNullOrEmpty(identityName))
                    {
                        parameters.Add(identityName, identityValue);
                    }
                    else
                    {
                        parameters.Add("Id", identityValue);
                    }

                    ParameterExpression parameter = null;
                    criteria = BuildEqualityAnd<T>(parameters, out parameter);

                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                    var query = context.Set<T>().Where(whereClause);
                    exist = query.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exist;
        }

        /// <summary>
        ///     Checks the record is exist or not.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>True/False</returns>
        public bool IsExist<T>(Hashtable parameters)
            where T : class
        {
            var exist = false;
            BinaryExpression criteria = null;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    ParameterExpression parameter = null;
                    criteria = BuildEqualityAnd<T>(parameters, out parameter);

                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                    var query = context.Set<T>().Where(whereClause);
                    exist = query.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return exist;
        }

        /// <summary>
        ///     Adds new record into the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object Add<T>(T entity, string Username = null, string KeyField = "Id")
            where T : class
        {
            var id = new object();
            var type = typeof(T);
            try
            {
                var createdBy = type.GetProperty("CreatedBy");
                if (createdBy != null)
                {
                    createdBy.SetValue(entity, Username, null);
                }

                var dateCreated = type.GetProperty("DateCreated");
                if (dateCreated != null)
                {
                    dateCreated.SetValue(entity, DateTime.Now, null);
                }

                var isDeleted = type.GetProperty("IsDeleted");
                if (isDeleted != null)
                {
                    isDeleted.SetValue(entity, false, null);
                }

                var isActive = type.GetProperty("IsActive");
                if (isActive != null)
                {
                    isActive.SetValue(entity, true, null);
                }

                var isExpired = type.GetProperty("IsExpired");
                if (isExpired != null)
                {
                    isExpired.SetValue(entity, false, null);
                }

                var isPublished = type.GetProperty("IsPublished");
                if (isPublished != null)
                {
                    isPublished.SetValue(entity, false, null);
                }

                context.Set<T>().Add(entity);
                context.SaveChanges();

                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, KeyField);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(left, typeof(object)), parameter);
                id = context.Set<T>().Max(lambda.Compile());
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return id;
        }

        /// <summary>
        /// Adds new record into the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void AddEntity<T>(T entity, string Username = null)
            where T : class
        {
            var type = typeof(T);
            try
            {
                var createdBy = type.GetProperty("CreatedBy");
                if (createdBy != null)
                {
                    createdBy.SetValue(entity, Username, null);
                }

                var dateCreated = type.GetProperty("DateCreated");
                if (dateCreated != null)
                {
                    dateCreated.SetValue(entity, DateTime.Now, null);
                }

                var isDeleted = type.GetProperty("IsDeleted");
                if (isDeleted != null)
                {
                    isDeleted.SetValue(entity, false, null);
                }

                var isActive = type.GetProperty("IsActive");
                if (isActive != null)
                {
                    isActive.SetValue(entity, true, null);
                }

                var isExpired = type.GetProperty("IsExpired");
                if (isExpired != null)
                {
                    isExpired.SetValue(entity, false, null);
                }

                var isPublished = type.GetProperty("IsPublished");
                if (isPublished != null)
                {
                    isPublished.SetValue(entity, false, null);
                }

                context.Set<T>().Add(entity);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        /// <summary>
        ///     Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<T>(int Id, string Username = null)
            where T : class
        {
            var flag = false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var right = Expression.Constant(Id, Id.GetType());
                var body = Expression.Equal(left, right);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                var entity = GetSingle<T>(Id);


                var type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.Now, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Set<T>().Attach(entity);
                context.Entry<T>(entity).State = EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        ///     Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<T>(long Id, string Username = null)
            where T : class
        {
            var flag = false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var right = Expression.Constant(Id, Id.GetType());
                var body = Expression.Equal(left, right);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                var entity = GetSingle<T>(Id);


                var type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.Now, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Set<T>().Attach(entity);
                context.Entry<T>(entity).State = EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        ///     Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<T>(Guid Id, string Username = null)
            where T : class
        {
            var flag = false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var right = Expression.Constant(Id, Id.GetType());
                var body = Expression.Equal(left, right);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                var entity = GetSingle<T>(Id);


                var type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.Now, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Set<T>().Attach(entity);
                context.Entry<T>(entity).State = EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        ///     Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void DeleteUpdate<T>(T entity, string Username = null)
            where T : class
        {
            try
            {
                var type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.UtcNow, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Entry<T>(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<T>(T entity, string Username = null)
            where T : class
        {
            var flag = false;

            try
            {
                var type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.Now, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Set<T>().Attach(entity);
                context.Entry<T>(entity).State = EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public bool RemoveUnsaved<T>(T entity)
           where T : class
        {
            var flag = false;

            try
            {
                context.Entry(entity).State = EntityState.Deleted;
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public bool RemoveUnsaved<T>(IEnumerable<T> entities)
            where T : class
        {
            var flag = false;
            try
            {
                context.Set<T>().RemoveRange(entities);
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public bool Remove<T>(T entity)
            where T : class
        {
            var flag = false;

            try
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public bool Remove<T>(IEnumerable<T> entities)
            where T : class
        {
            var flag = false;
            try
            {
                context.Set<T>().RemoveRange(entities);
                context.SaveChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        ///     Updates the existing record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public bool Update<T>(T entity, string Username = null)
            where T : class
        {
            var flag = false;
            try
            {
                var type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    if (!string.IsNullOrEmpty(Username))
                    {
                        updated_by.SetValue(entity, Username, null);
                    }
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.Now, null);
                }

                context.Entry<T>(entity).State = EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return flag;
        }

        /// <summary>
        ///     Updates the existing record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public bool UpdateEntity<T>(T entity)
            where T : class
        {
            var flag = false;
            try
            {
                context.Entry<T>(entity).State = EntityState.Modified;

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }


        /// <summary>
        ///     Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingle<T>(int Id)
            where T : class
        {
            T entity = null;
            var id = new object();
            var type = typeof(T);
            var parameters = new Hashtable();
            BinaryExpression criteria = null;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    parameters.Add("Id", Id);
                    ParameterExpression parameter = null;
                    criteria = BuildEqualityAnd<T>(parameters, out parameter);

                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                    entity = context.Set<T>().SingleOrDefault(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        /// <summary>
        ///     Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingle<T>(long Id)
            where T : class
        {
            T entity = null;
            var id = new object();
            var type = typeof(T);
            var parameters = new Hashtable();
            BinaryExpression criteria = null;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    parameters.Add("Id", Id);
                    ParameterExpression parameter = null;
                    criteria = BuildEqualityAnd<T>(parameters, out parameter);

                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                    entity = context.Set<T>().SingleOrDefault(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        /// <summary>
        ///     Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingle<T>(string identity, object value)
            where T : class
        {
            T entity = null;
            var id = new object();
            var type = typeof(T);
            var parameters = new Hashtable();
            BinaryExpression criteria = null;
            try
            {
                parameters.Add(identity, value);

                var deleted = type.GetProperty(DELETE_FIELD_NAME);
                if (deleted != null)
                {
                    parameters.Add(DELETE_FIELD_NAME, false);
                }

                var expired = type.GetProperty(EXPIRED_FIELD_NAME);
                if (expired != null)
                {
                    parameters.Add(EXPIRED_FIELD_NAME, false);
                }

                var active = type.GetProperty(ACTIVE_FIELD_NAME);
                if (active != null)
                {
                    parameters.Add(ACTIVE_FIELD_NAME, true);
                }

                ParameterExpression parameter = null;
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                entity = context.Set<T>().FirstOrDefault(whereClause);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        /// <summary>
        ///     Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingle<T>(Guid Id)
            where T : class
        {
            T entity = null;
            var id = new object();
            var type = typeof(T);
            var parameters = new Hashtable();
            BinaryExpression criteria = null;
            try
            {
                parameters.Add("Id", Id);

                var deleted = type.GetProperty(DELETE_FIELD_NAME);
                if (deleted != null)
                {
                    parameters.Add(DELETE_FIELD_NAME, false);
                }

                var expired = type.GetProperty(EXPIRED_FIELD_NAME);
                if (expired != null)
                {
                    parameters.Add(EXPIRED_FIELD_NAME, false);
                }

                var active = type.GetProperty(ACTIVE_FIELD_NAME);
                if (active != null)
                {
                    parameters.Add(ACTIVE_FIELD_NAME, true);
                }

                ParameterExpression parameter = null;
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                entity = context.Set<T>().SingleOrDefault(whereClause);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        /// <summary>
        ///     Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingle<T>(Hashtable parameters)
            where T : class
        {
            T entity = null;
            BinaryExpression criteria = null;
            ParameterExpression parameter = null;

            if (parameters != null && parameters.Count > 0)
            {
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                entity = context.Set<T>().SingleOrDefault(whereClause);
            }

            return entity;
        }

        /// <summary>
        ///     Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingle<T>(List<Parameter> parameters)
            where T : class
        {
            T entity = null;
            if (parameters != null && parameters.Count > 0)
            {
                var criteria = BuildAnd<T>(parameters);
                entity = context.Set<T>().SingleOrDefault(criteria);
            }
            return entity;
        }

        /// <summary>
        ///     Gets the list of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of Type Parameter passed</returns>
        public IEnumerable<T> GetList<T>()
            where T : class
        {
            List<T> list = new List<T>();

            var id = new object();
            var type = typeof(T);
            var parameters = new Hashtable();
            BinaryExpression criteria = null;

            var deleted = type.GetProperty(DELETE_FIELD_NAME);
            if (deleted != null)
            {
                parameters.Add(DELETE_FIELD_NAME, false);
            }

            var active = type.GetProperty(ACTIVE_FIELD_NAME);
            if (active != null)
            {
                parameters.Add(ACTIVE_FIELD_NAME, true);
            }

            if (parameters.Count > 0)
            {
                ParameterExpression parameter = null;
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                list = context.Set<T>().Where(whereClause).ToList();
            }
            else
            {
                list = context.Set<T>().ToList();
            }

            return list;
        }

        /// <summary>
        ///     Gets the list of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of Type Parameter passed</returns>
        public IEnumerable<T> GetList<T>(List<SortBy> orderFields = null)
            where T : class
        {
            IEnumerable<T> list = null;

            var id = new object();
            var type = typeof(T);
            var parameters = new Hashtable();
            BinaryExpression criteria = null;

            var deleted = type.GetProperty(DELETE_FIELD_NAME);
            if (deleted != null)
            {
                parameters.Add(DELETE_FIELD_NAME, false);
            }

            var active = type.GetProperty(ACTIVE_FIELD_NAME);
            if (active != null)
            {
                parameters.Add(ACTIVE_FIELD_NAME, true);
            }

            //JobPortalEntities context = new JobPortalEntities();

            if (parameters.Count > 0)
            {
                ParameterExpression parameter = null;
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                var result = context.Set<T>().Where(whereClause);

                if (orderFields != null)
                {
                    foreach (SortBy order in orderFields)
                    {
                        if (order.Ascending)
                        {
                            Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                            result = result.OrderBy(field).AsQueryable();
                        }
                        else
                        {
                            Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                            result = result.OrderByDescending(field).AsQueryable();
                        }
                    }
                }
                list = result.AsEnumerable();
            }
            else
            {
                var result = context.Set<T>().AsQueryable();

                if (orderFields != null)
                {
                    foreach (SortBy order in orderFields)
                    {
                        if (order.Ascending)
                        {
                            Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                            result = result.OrderBy(field).AsQueryable();
                        }
                        else
                        {
                            Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                            result = result.OrderByDescending(field).AsQueryable();
                        }
                    }
                }

                list = result.AsEnumerable();
            }

            return list;
        }

        /// <summary>
        ///     Gets the list of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of Type Parameter passed</returns>
        public List<T> GetList<T>(List<SortBy> orderFields, int pageNumber = 0, int pageSize = 0)
            where T : class
        {
            List<T> list = null;

            var id = new object();
            var type = typeof(T);
            var parameters = new Hashtable();
            BinaryExpression criteria = null;

            var deleted = type.GetProperty(DELETE_FIELD_NAME);
            if (deleted != null)
            {
                parameters.Add(DELETE_FIELD_NAME, false);
            }

            var active = type.GetProperty(ACTIVE_FIELD_NAME);
            if (active != null)
            {
                parameters.Add(ACTIVE_FIELD_NAME, true);
            }

            if (parameters.Count > 0)
            {
                ParameterExpression parameter = null;
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                var result = context.Set<T>().Where(whereClause);

                if (orderFields != null)
                {
                    foreach (SortBy order in orderFields)
                    {
                        if (order.Ascending)
                        {
                            Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                            result = result.OrderBy(field).AsQueryable();
                        }
                        else
                        {
                            Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                            result = result.OrderByDescending(field).AsQueryable();
                        }
                    }
                }

                if (pageSize > 0)
                {
                    list = result.Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
                }
                else
                {
                    list = result.ToList();
                }
            }

            return list;
        }

        ///// <summary>
        /////     Gets the list of type based object (any data type) as Primary Key.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //public IEnumerable<T> GetList<T>(string columnName, object columnValue, int pageNumber = 0, int pageSize = 0)
        //    where T : class
        //{
        //    IEnumerable<T> list = null;
        //    var parameters = new Hashtable();
        //    BinaryExpression criteria = null;

        //    //JobPortalEntities context = new JobPortalEntities();
        //    ParameterExpression parameter = null;
        //    parameters.Add(columnName, columnValue);
        //    criteria = BuildEqualityAnd<T>(parameters, out parameter);

        //    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

        //    if (pageSize > 0)
        //    {
        //        list = context.Set<T>().Where(whereClause).Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
        //    }
        //    else
        //    {
        //        list = context.Set<T>().Where(whereClause).ToList();
        //    }

        //    return list;
        //}

        /// <summary>
        ///     Gets the list of type based integer as Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(string columnName, int columnValue, int pageNumber = 0, int pageSize = 0)
           where T : class
        {
            IEnumerable<T> list = null;
            var parameters = new Hashtable();
            BinaryExpression criteria = null;

            //JobPortalEntities context = new JobPortalEntities();
            ParameterExpression parameter = null;
            parameters.Add(columnName, columnValue);
            criteria = BuildEqualityAnd<T>(parameters, out parameter);

            var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

            if (pageSize > 0)
            {
                list = context.Set<T>().Where(whereClause).Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
            }
            else
            {
                list = context.Set<T>().Where(whereClause).ToList();
            }
            return list;
        }

        /// <summary>
        ///     Gets the list of type based long as Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(string columnName, long columnValue, int pageNumber = 0, int pageSize = 0)
          where T : class
        {
            IEnumerable<T> list = null;
            var parameters = new Hashtable();
            BinaryExpression criteria = null;

            //JobPortalEntities context = new JobPortalEntities();
            ParameterExpression parameter = null;
            parameters.Add(columnName, columnValue);
            criteria = BuildEqualityAnd<T>(parameters, out parameter);

            var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

            if (pageSize > 0)
            {
                list = context.Set<T>().Where(whereClause).Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
            }
            else
            {
                list = context.Set<T>().Where(whereClause).ToList();
            }

            return list;
        }
        /// <summary>
        ///     Gets the list of type based GUID as Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(string columnName, Guid columnValue, int pageNumber = 0, int pageSize = 0)
           where T : class
        {
            IEnumerable<T> list = null;
            var parameters = new Hashtable();
            BinaryExpression criteria = null;

            //JobPortalEntities context = new JobPortalEntities();
            ParameterExpression parameter = null;
            parameters.Add(columnName, columnValue);
            criteria = BuildEqualityAnd<T>(parameters, out parameter);

            var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

            if (pageSize > 0)
            {
                list = context.Set<T>().Where(whereClause).Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
            }
            else
            {
                list = context.Set<T>().Where(whereClause).ToList();
            }

            return list;
        }
        /// <summary>
        ///     Gets the list of type based on the parameters passed as criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns>List Type Parameter passed.</returns>
        public List<T> GetList<T>(out int rows, Expression<Func<T, bool>> criteria, List<SortBy> orderFields = null, int pageNumber = 0, int pageSize = 0)
            where T : class
        {
            List<T> list = null;
            rows = 0;
            try
            {
                if (pageSize > 0)
                {
                    var result = context.Set<T>().Where(criteria.Compile());

                    if (orderFields != null)
                    {
                        foreach (SortBy order in orderFields)
                        {
                            if (order.Ascending)
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderBy(field).AsQueryable();
                            }
                            else
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderByDescending(field).AsQueryable();
                            }
                        }
                    }
                    rows = result.Count();
                    list = result.Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
                }
                else
                {
                    var result = context.Set<T>().Where(criteria.Compile());
                    if (orderFields != null)
                    {
                        foreach (SortBy order in orderFields)
                        {
                            if (order.Ascending)
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderBy(field).AsQueryable();
                            }
                            else
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderByDescending(field).AsQueryable();
                            }
                        }
                    }
                    rows = result.Count();
                    list = result.ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        ///     Gets the list of type based on the parameters passed as criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns>List Type Parameter passed.</returns>
        public List<T> GetList<T>(Expression<Func<T, bool>> criteria, List<SortBy> orderFields = null, int pageNumber = 0, int pageSize = 0)
            where T : class
        {
            List<T> list = null;
            try
            {
                if (pageSize > 0)
                {
                    var result = context.Set<T>().Where(criteria.Compile());

                    if (orderFields != null)
                    {
                        foreach (SortBy order in orderFields)
                        {
                            if (order.Ascending)
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderBy(field).AsQueryable();
                            }
                            else
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderByDescending(field).AsQueryable();
                            }
                        }
                    }
                    list = result.Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
                }
                else
                {
                    var result = context.Set<T>().Where(criteria.Compile());
                    if (orderFields != null)
                    {
                        foreach (SortBy order in orderFields)
                        {
                            if (order.Ascending)
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderBy(field).AsQueryable();
                            }
                            else
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderByDescending(field).AsQueryable();
                            }
                        }
                    }

                    list = result.ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        ///     Gets the list of type based on the parameters passed as criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns>List Type Parameter passed.</returns>
        public IEnumerable<T> GetList<T>(Hashtable parameters, List<SortBy> orderFields = null, int pageNumber = 0, int pageSize = 0)
            where T : class
        {
            IEnumerable<T> list = null;
            BinaryExpression criteria = null;
            try
            {
                //JobPortalEntities context = new JobPortalEntities();
                ParameterExpression parameter = null;
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                if (pageSize > 0)
                {
                    var result = context.Set<T>().Where(whereClause);

                    if (orderFields != null)
                    {
                        foreach (SortBy order in orderFields)
                        {
                            if (order.Ascending)
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderBy(field).AsQueryable();
                            }
                            else
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderByDescending(field).AsQueryable();
                            }
                        }
                    }
                    list = result.Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).ToList();
                }
                else
                {
                    var result = context.Set<T>().Where(whereClause);
                    if (orderFields != null)
                    {
                        foreach (SortBy order in orderFields)
                        {
                            if (order.Ascending)
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderBy(field).AsQueryable();
                            }
                            else
                            {
                                Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                                result = result.OrderByDescending(field).AsQueryable();
                            }
                        }
                    }

                    list = result.ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public int GetCount<T>(Expression<Func<T, bool>> criteria)
           where T : class
        {
            int rows = 0;
            try
            {
                rows = context.Set<T>().Count(criteria.Compile());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rows;
        }

        public int GetCounts<T>(Hashtable parameters)
            where T : class
        {
            BinaryExpression criteria = null;
            int rows = 0;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    ParameterExpression parameter = null;
                    criteria = BuildEqualityAnd<T>(parameters, out parameter);
                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                    rows = context.Set<T>().Count(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rows;
        }

        /// <summary>
        ///     Gets the count of all records by including deleted one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int GetCounts<T>()
            where T : class
        {
            var counts = 0;
            try
            {
                //using (JobPortalEntities context = new JobPortalEntities())
                {
                    counts = context.Set<T>().Count();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return counts;
        }

        /// <summary>
        ///     Gets all the records including deleted one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(int pageNumber = 0, int pageSize = 0)
            where T : class
        {
            IEnumerable<T> list = null;
            try
            {
                if (pageSize > 0)
                {
                    list = context.Set<T>().Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize).AsEnumerable();
                }
                else
                {
                    list = context.Set<T>().AsEnumerable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IQueryable<T> Get<T>(int pageNumber = 0, int pageSize = 0)
            where T : class
        {
            IQueryable<T> list = null;
            try
            {
                if (pageSize > 0)
                {
                    list = context.Set<T>().Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize);
                }
                else
                {
                    list = context.Set<T>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IQueryable<T> Get<T>(string columnName, object columnValue)
            where T : class
        {
            IQueryable<T> list = null;
            try
            {
                var parameters = new Hashtable();
                BinaryExpression criteria = null;

                ParameterExpression parameter = null;
                parameters.Add(columnName, columnValue);
                criteria = BuildEqualityAnd<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
               
                list = context.Set<T>().AsNoTracking().Where(whereClause);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <param name="criteria"></param>
        /// <param name="orderFields"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IQueryable<T> Get<T>(out int rows, Expression<Func<T, bool>> criteria, List<SortBy> orderFields, int pageNumber, int pageSize)
            where T : class
        {
            IQueryable<T> list = null;
            try
            {
                var result = context.Set<T>().Where(criteria.Compile()).AsQueryable();

                foreach (SortBy order in orderFields)
                {
                    if (order.Ascending)
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderBy(field).AsQueryable();
                    }
                    else
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderByDescending(field).AsQueryable();
                    }
                }

                rows = result.Count();
                list = result.Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> criteria, List<SortBy> orderFields, int pageNumber, int pageSize)
           where T : class
        {
            IQueryable<T> list = null;
            try
            {
                var result = context.Set<T>().Where(criteria.Compile()).AsQueryable();

                foreach (SortBy order in orderFields)
                {
                    if (order.Ascending)
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderBy(field).AsQueryable();
                    }
                    else
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderByDescending(field).AsQueryable();
                    }
                }
                list = result.Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }



        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="orderFields"></param>
        /// <returns></returns>
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> criteria, List<SortBy> orderFields)
            where T : class
        {
            IQueryable<T> list = null;
            try
            {
                list = context.Set<T>().Where(criteria).AsQueryable();

                foreach (SortBy order in orderFields)
                {
                    if (order.Ascending)
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        list = list.OrderBy(field).AsQueryable();
                    }
                    else
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        list = list.OrderByDescending(field).AsQueryable();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> criteria)
            where T : class
        {
            IQueryable<T> result = null;
            try
            {
                if (criteria != null)
                {
                    result = context.Set<T>().Where(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IQueryable<T> Get<T>(List<Parameter> parameters)
            where T : class
        {
            IQueryable<T> result = null;

            try
            {
                var criteria = BuildAnd<T>(parameters);
                if (criteria != null)
                {
                    result = context.Set<T>().Where(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderFields"></param>
        /// <returns></returns>
        public IQueryable<T> Get<T>(List<SortBy> orderFields)
            where T : class
        {
            IQueryable<T> result = null;
            try
            {
                foreach (SortBy order in orderFields)
                {
                    if (order.Ascending)
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderBy(field).AsQueryable();
                    }
                    else
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderByDescending(field).AsQueryable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderFields"></param>
        /// <returns></returns>
        public IQueryable<T> Get<T>(List<SortBy> orderFields, int pageNumber, int pageSize)
            where T : class
        {
            IQueryable<T> result = null;
            try
            {
                foreach (SortBy order in orderFields)
                {
                    if (order.Ascending)
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderBy(field).AsQueryable();
                    }
                    else
                    {
                        Func<T, object> field = BuildExpression<T>(order.Field).Compile();
                        result = result.OrderByDescending(field).AsQueryable();
                    }
                }
                result = result.Skip((pageNumber > 0 ? ((pageNumber - 1) * pageSize) : pageNumber * pageSize)).Take(pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}