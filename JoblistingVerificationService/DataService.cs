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

namespace JoblistingVerificationService
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
                            propertyInfo.SetValue(entity, Convert.ChangeType(record.GetValue(i), record.GetFieldType(i)), null);
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
