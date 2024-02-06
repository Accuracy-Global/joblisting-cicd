using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Data
{
    public abstract partial class DataService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Convert IDataRecord into specified class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        /// <param name="myClass"></param>
        private T MapRecord<T>(IDataRecord record)
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            T entity = default(T);
            entity = Activator.CreateInstance<T>();
            for (int i = 0; i < record.FieldCount; i++)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.Name == record.GetName(i))
                    {
                        if (record.GetValue(i) != DBNull.Value)
                        {
                            propertyInfo.SetValue(entity, null);
                            propertyInfo.SetValue(entity, Convert.ChangeType(record.GetValue(i), record.GetFieldType(i)), null);
                           // propertyInfo.SetValue(entity, record.GetValue(i), record.GetFieldType(i), null);


                        }
                        break;
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// Reads single field data from database using SQL Query
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected object ReadDataField(string queryString)
        {
            object data = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = queryString;
                    cmd.CommandTimeout = 180;
                    data = cmd.ExecuteScalar();
                }
            }
            return data;
        }

        /// <summary>
        /// Reads single field data from database using SQL Query
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected T ReadDataField<T>(string queryString)
        {
            object data = null;
            T value = default(T);
            value = Activator.CreateInstance<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = queryString;
                    cmd.CommandTimeout = 180;
                    data = cmd.ExecuteScalar();
                }
            }
            if (data != null)
            {
                value = (T)data;
            }
            return value;
        }

        protected T ReadDataField<T>(string procedure, List<DbParameter> parameters)
        {
            object data = null;
            T value = default(T);
            value = Activator.CreateInstance<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    data = cmd.ExecuteScalar();
                }
            }
            if (data != null)
            {
                value = (T)data;
            }
            return value;
        }

        /// <summary>
        /// Reads single field data from database using procedure
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object ReadDataField(string procedure, List<DbParameter> parameters)
        {
            object data = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    data = cmd.ExecuteScalar();
                }
            }
            return data;
        }

        /// <summary>
        /// Reads single record data from database using SQL Query
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected T ReadSingleData<T>(string queryString)
        {
            T data = Activator.CreateInstance<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = queryString;
                    cmd.CommandTimeout = 180;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = MapRecord<T>(reader);
                        }
                    }
                }
            }
            return data;
        }


        protected DataTable ReadSingleData1(string queryString)
        {
            //SqlDataReader data = Activator.CreateInstance<SqlDataReader>();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = queryString;
                    cmd.CommandTimeout = 180;
                    SqlDataReader dr = cmd.ExecuteReader();
                   
                    dt.Load(dr);
                     
                }
            }
            return dt;
        }





        /// <summary>
        /// Reads single record data from database using procedure
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected T ReadSingleData<T>(string procedure, List<DbParameter> parameters)
        {
            T data = Activator.CreateInstance<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = MapRecord<T>(reader);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Reads single record from database based on parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected T ReadSingleData<T>(string procedure, List<Parameter> parameters)
        {
            T data = Activator.CreateInstance<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    foreach (SqlParameter param in cmd.Parameters)
                    {
                        foreach (Parameter parameter in parameters)
                        {
                            if (parameter.Name.Equals(param.ParameterName))
                            {
                                param.Value = parameter.Value;
                            }
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = MapRecord<T>(reader);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Reads single record from database based on specified criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="procedure"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected T ReadSingleData<T, T1>(string procedure, T1 criteria)
        {
            T data = Activator.CreateInstance<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    PropertyInfo[] properties = criteria.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        for (int i = 0; i < cmd.Parameters.Count; i++)
                        {
                            if (property.Name == cmd.Parameters[i].ParameterName.Replace("@", ""))
                            {
                                cmd.Parameters[i].Value = property.GetValue(criteria);
                                break;
                            }
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = MapRecord<T>(reader);
                            
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Reads data from database using SQL Query
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected List<T> ReadData<T>(string queryString)
        {
            List<T> data = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = queryString;
                    cmd.CommandTimeout = 180;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T entity = MapRecord<T>(reader);
                            data.Add(entity);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Reads data from database using procedure
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected List<T> ReadData<T>(string procedure, List<DbParameter> parameters)
        {
            List<T> data = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T entity = MapRecord<T>(reader);
                            data.Add(entity);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Reads data from database using procedure
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected List<T> ReadDataList<T>(string procedure, List<DbParameter> parameters)
        {
            List<T> data = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                           
                            data.Add((T)reader[0]);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Reads data from database based on the paraemters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected List<T> ReadData<T>(string procedure, List<Parameter> parameters)
        {
            List<T> data = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    foreach (SqlParameter param in cmd.Parameters)
                    {
                        foreach (Parameter parameter in parameters)
                        {
                            if (parameter.Name.Equals(param.ParameterName))
                            {
                                param.Value = parameter.Value;
                            }
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T entity = MapRecord<T>(reader);
                            data.Add(entity);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Reads data from database based on the filter criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="procedure"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected List<T> ReadData<T, T1>(string procedure, T1 criteria)
        {
            List<T> data = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedure))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedure;
                    cmd.CommandTimeout = 180;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    PropertyInfo[] properties = criteria.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        for (int i = 0; i < cmd.Parameters.Count; i++)
                        {
                            if (property.Name == cmd.Parameters[i].ParameterName.Replace("@", ""))
                            {
                                cmd.Parameters[i].Value = property.GetValue(criteria);
                                break;
                            }
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T entity = MapRecord<T>(reader);
                            data.Add(entity);
                        }
                    }
                }
            }
            return data;
        }


        /// <summary>
        /// Manage data using DML Statements
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected int HandleData(string queryString)
        {
            int stat = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = queryString;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    stat = command.ExecuteNonQuery();
                }
            }
            return stat;
        }

        /// <summary>
        /// Manage data using store procedures
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected int HandleData(string storedProcedure, List<DbParameter> parameters)
        {
            int stat = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedure;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());
                    connection.Open();
                    stat = command.ExecuteNonQuery();
                }
            }
            return stat;
        }
    }
}
