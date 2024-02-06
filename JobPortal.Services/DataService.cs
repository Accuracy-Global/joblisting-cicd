using JobPortal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public abstract class DataService : IDataService
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
            try
            {
                for (int i = 0; i < record.FieldCount; i++)
                {
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        if (propertyInfo.Name == record.GetName(i))
                        {
                            if (record.GetValue(i) != DBNull.Value)
                            {
                                try
                                {
                                    propertyInfo.SetValue(entity, Convert.ChangeType(record.GetValue(i), record.GetFieldType(i)), null);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        public async Task<List<Output>> ReadAsync<Output>(string queryString)
        {
            List<Output> data = new List<Output>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = queryString;
                        cmd.CommandTimeout = 180;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Output entity = MapRecord<Output>(reader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public List<Output> Read<Output>(string queryString)
        {
            List<Output> data = new List<Output>();
            try
            {
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
                                Output entity = MapRecord<Output>(reader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");

            if (commandParameters != null)
            {
                string param = string.Empty;
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                        param = param + p.Value.ToString() + ",";
                    }
                }
            }
        }
        public async Task<List<Output>> Read1Async<Output>(string procedure, SqlParameter[] parameters)
        {
            List<Output> data = new List<Output>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(procedure))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedure;
                        cmd.CommandTimeout = 216000;
                        SqlCommandBuilder.DeriveParameters(cmd);
                        cmd.Parameters.Clear();
                        AttachParameters(cmd, parameters);
                        var reader = await cmd.ExecuteReaderAsync();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Output entity = MapRecord<Output>(reader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<List<Output>> ReadAsync<Output>(string procedure, List<Parameter> parameters)
        {
            List<Output> data = new List<Output>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(procedure))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.CommandText = procedure;
                        cmd.CommandTimeout = 3600;
                        SqlCommandBuilder.DeriveParameters(cmd);

                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            Parameter param = parameters.Where(x => x.Name.ToString().Trim() == p.ParameterName).FirstOrDefault();
                            if (param != null)
                            {
                                p.Value = param.Value;
                            }
                        }

                        //for (int i = 0; i < cmd.Parameters.Count; i++)
                        //{
                        //    for (int j = 0; j < parameters.Count; j++)
                        //    {

                        //        if (parameters[j].Name.Replace("@", "") == cmd.Parameters[i].ParameterName.Replace("@", ""))
                        //        {
                        //            cmd.Parameters[i].Value = parameters[j].Value;
                        //            break;
                        //        }

                        //    }
                        //}

                        //foreach (SqlParameter param in cmd.Parameters)
                        //{
                        //    foreach (Parameter parameter in parameters)
                        //    {
                        //        if (parameter.Name.Trim().ToLower().Replace("@", "") == param.ParameterName.Trim().ToLower().Replace("@", ""))
                        //        {
                        //            param.Value = parameter.Value;
                        //            break;
                        //        }
                        //    }
                        //}

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Output entity = MapRecord<Output>(reader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public List<Output> Read<Output>(string procedure, List<Parameter> parameters)
        {
            List<Output> data = new List<Output>();
            try
            {
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
                                    break;
                                }
                            }
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Output entity = MapRecord<Output>(reader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<List<Output>> ReadAsync<Output, Input>(string procedure, Input model)
        {
            List<Output> data = new List<Output>();
            try
            {
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

                        PropertyInfo[] properties = model.GetType().GetProperties();

                        foreach (PropertyInfo property in properties)
                        {
                            for (int i = 0; i < cmd.Parameters.Count; i++)
                            {
                                if (property.Name == cmd.Parameters[i].ParameterName.Replace("@", ""))
                                {
                                    cmd.Parameters[i].Value = property.GetValue(model);
                                    break;
                                }
                            }
                        }

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Output entity = MapRecord<Output>(reader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public List<Output> Read<Output, Input>(string procedure, Input model)
        {
            List<Output> data = new List<Output>();
            try
            {
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

                        PropertyInfo[] properties = model.GetType().GetProperties();

                        foreach (PropertyInfo property in properties)
                        {
                            for (int i = 0; i < cmd.Parameters.Count; i++)
                            {
                                if (property.Name == cmd.Parameters[i].ParameterName.Replace("@", ""))
                                {
                                    cmd.Parameters[i].Value = property.GetValue(model);
                                    break;
                                }
                            }
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Output entity = MapRecord<Output>(reader);
                                data.Add(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public Output Single<Output>(string queryString)
        {
            Output data = Activator.CreateInstance<Output>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(queryString))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = queryString;
                        cmd.CommandTimeout = 180;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = MapRecord<Output>(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<Output> SingleAsync<Output>(string procedure, List<Parameter> parameters)
        {
            Output data = default(Output);
            try
            {
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

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                data = MapRecord<Output>(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public Output Single<Output>(string procedure, List<Parameter> parameters)
        {
            Output data = default(Output);
            try
            {
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
                                data = MapRecord<Output>(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public Output Single<Output, Input>(string procedure, Input criteria)
        {
            Output data = Activator.CreateInstance<Output>();
            try
            {
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
                                data = MapRecord<Output>(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<Output> ScalerAsync<Output>(string queryString)
        {
            object data = null;
            Output value = default(Output);
            value = Activator.CreateInstance<Output>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = queryString;
                        cmd.CommandTimeout = 180;
                        data = await cmd.ExecuteScalarAsync();
                    }
                }
                if (data != null)
                {
                    value = (Output)data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public Output Scaler<Output>(string queryString)
        {
            object data = null;
            Output value = default(Output);
            value = Activator.CreateInstance<Output>();
            try
            {
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
                    value = (Output)data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public Output Scaler<Output>(string procedure, List<Parameter> parameters)
        {
            Output data = default(Output);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedure;
                        cmd.CommandTimeout = 180;
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                        }
                        data = (Output)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public object Scaler(string procedure, List<Parameter> parameters)
        {
            object data = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedure;
                        cmd.CommandTimeout = 180;
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                        }
                        data = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public Output Scaler<Output, Input>(string procedure, Input model)
        {
            object data = null;
            Output value = default(Output);
            value = Activator.CreateInstance<Output>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedure;
                        cmd.CommandTimeout = 180;
                        SqlCommandBuilder.DeriveParameters(cmd);

                        PropertyInfo[] properties = model.GetType().GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            for (int i = 0; i < cmd.Parameters.Count; i++)
                            {
                                if (property.Name == cmd.Parameters[i].ParameterName.Replace("@", ""))
                                {
                                    cmd.Parameters[i].Value = property.GetValue(model);
                                    break;
                                }
                            }
                        }

                        data = cmd.ExecuteScalar();
                    }
                }
                if (data != null)
                {
                    value = (Output)data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        /// <summary>
        /// Manage data using DML Statements
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public int HandleData(string queryString)
        {
            int stat = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = queryString;
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        stat = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
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
        public async Task<int> HandleDataAsync(string storedProcedure, List<Parameter> parameters)
        {
            int stat = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = storedProcedure;
                        command.CommandType = CommandType.StoredProcedure;

                        foreach (var item in parameters)
                        {
                            command.Parameters.Add(new SqlParameter(item.Name, item.Value));
                        }

                        connection.Open();
                        stat = await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
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
        public int HandleData(string storedProcedure, List<Parameter> parameters)
        {
            int stat = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = storedProcedure;
                        command.CommandType = CommandType.StoredProcedure;

                        foreach (var item in parameters)
                        {
                            command.Parameters.Add(new SqlParameter(item.Name, item.Value));
                        }

                        connection.Open();
                        stat = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
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
        public int HandleData<Input>(string storedProcedure, Input model)
        {
            int stat = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = storedProcedure;
                        command.CommandType = CommandType.StoredProcedure;

                        SqlCommandBuilder.DeriveParameters(command);

                        PropertyInfo[] properties = model.GetType().GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            for (int i = 0; i < command.Parameters.Count; i++)
                            {
                                if (property.Name == command.Parameters[i].ParameterName.Replace("@", ""))
                                {
                                    command.Parameters[i].Value = property.GetValue(model);
                                    break;
                                }
                            }
                        }

                        connection.Open();
                        stat = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        throw ex;
                    }
                }
            }
            return stat;
        }


        public async Task<Output> ScalerAsync<Output>(string procedure, List<Parameter> parameters)
        {
            Output data = default(Output);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedure;
                        cmd.CommandTimeout = 180;
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                        }
                        data = (Output)(await cmd.ExecuteScalarAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<Output> ScalerAsync<Output, Input>(string procedure, Input model)
        {
            object data = null;
            Output value = default(Output);
            value = Activator.CreateInstance<Output>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedure;
                        cmd.CommandTimeout = 180;
                        SqlCommandBuilder.DeriveParameters(cmd);

                        PropertyInfo[] properties = model.GetType().GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            for (int i = 0; i < cmd.Parameters.Count; i++)
                            {
                                if (property.Name == cmd.Parameters[i].ParameterName.Replace("@", ""))
                                {
                                    cmd.Parameters[i].Value = property.GetValue(model);
                                    break;
                                }
                            }
                        }

                        data = await cmd.ExecuteScalarAsync();
                    }
                }
                if (data != null)
                {
                    value = (Output)data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
    }
}
