using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace JobPortal.Data
{
    public static class DataProvider
    {
        private static string provider = ConfigurationManager.ConnectionStrings["DefaultConnection"].ProviderName;
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static DbProviderFactory factory;
        private static DbConnection connection = null;
        private static DbDataAdapter dataAdapter = null;

        static DataProvider()
        {
            factory = DbProviderFactories.GetFactory(provider);
        }

        public static object GetSingleValue(string query)
        {
            object status = null;
            DbCommand command = null;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = query;
                    status = command.ExecuteScalar();
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public static int Execute(string query)
        {
            int status = 0;
            DbCommand command = null;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = query;
                    status = command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public static int Execute(string query, List<DbParameter> parameters)
        {
            int status = 0;
            DbCommand command = null;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = query;
                    foreach (DbParameter parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    status = command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public static int Execute(string query, CommandType type, List<DbParameter> parameters)
        {
            int status = 0;
            DbCommand command = null;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandType = type;
                    command.CommandText = query;
                    foreach (DbParameter parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                    status = command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public static DataTable GetDataTable(string query)
        {
            DataTable oDataTable = new DataTable();
            try
            {
                using (connection = factory.CreateConnection())
                {
                    
                    connection.ConnectionString = connectionString;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = connection.CreateCommand();
                    dataAdapter.SelectCommand.CommandText = query;
                    dataAdapter.Fill(oDataTable);

                    dataAdapter.SelectCommand.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oDataTable;
        }
        public static void GetDataTable(string query, CommandType type, ref DataTable dataTable)
        {
            DataTable oDataTable = new DataTable();
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = connection.CreateCommand();
                    dataAdapter.SelectCommand.CommandType = type;
                    dataAdapter.SelectCommand.CommandText = query;
                    dataAdapter.Fill(dataTable);

                    dataAdapter.SelectCommand.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GetDataTable(string query, ref DataTable dataTable)
        {
            DataTable oDataTable = new DataTable();
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = connection.CreateCommand();
                    dataAdapter.SelectCommand.CommandText = query;
                    dataAdapter.Fill(dataTable);

                    dataAdapter.SelectCommand.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetDataTable(string query, List<DbParameter> parameters)
        {
            DataTable oDataTable = new DataTable();
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = connection.CreateCommand();
                    dataAdapter.SelectCommand.CommandText = query;
                    dataAdapter.Fill(oDataTable);
                    foreach (DbParameter parameter in parameters)
                    {
                        dataAdapter.SelectCommand.Parameters.Add(parameter);
                    }
                    dataAdapter.SelectCommand.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oDataTable;
        }

        public static DataTable GetDataTable(string query, CommandType type, List<DbParameter> parameters)
        {
            DataTable oDataTable = new DataTable();
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = connection.CreateCommand();
                    dataAdapter.SelectCommand.CommandType = type;
                    dataAdapter.SelectCommand.CommandText = query;
                    dataAdapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                    dataAdapter.Fill(oDataTable);
                    dataAdapter.SelectCommand.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oDataTable;
        }

        public static DataTable GetDataTable(string query, CommandType type, List<DbParameter> parameters, out int rows)
        {
            DataTable oDataTable = new DataTable();
            rows = 0;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = connection.CreateCommand();
                    dataAdapter.SelectCommand.CommandTimeout = 200;
                    dataAdapter.SelectCommand.CommandType = type;
                    dataAdapter.SelectCommand.CommandText = query;
                    dataAdapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                    dataAdapter.Fill(oDataTable);
                    foreach (DbParameter parameter in parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Output)
                        {
                            string value = Convert.ToString(parameter.Value);
                            if (!string.IsNullOrEmpty(value))
                            {
                                rows = Convert.ToInt32(parameter.Value);
                                break;
                            }
                        }
                    }
                    dataAdapter.SelectCommand.Parameters.Clear();
                    dataAdapter.SelectCommand.Dispose();
                    dataAdapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oDataTable;
        }

        public static Dictionary<int, List<object>> GetRecords(string query)
        {
            Dictionary<int, List<object>> records = new Dictionary<int, List<object>>();
            DbCommand command = null;
            DbDataReader dataReader = null;
            int index = 0;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = query;
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int f = 0; f < dataReader.FieldCount; f++)
                        {
                            row.Add(dataReader[f]);
                        }
                        records.Add(index, row);
                        index++;
                    }
                    command.Dispose();
                    dataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return records;
        }

        public static Dictionary<int, List<object>> GetRecords(string query, CommandType type)
        {
            Dictionary<int, List<object>> records = new Dictionary<int, List<object>>();
            DbCommand command = null;
            DbDataReader dataReader = null;
            int index = 0;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandType = type;
                    command.CommandText = query;
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int f = 0; f < dataReader.FieldCount; f++)
                        {
                            row.Add(dataReader[f]);
                        }
                        records.Add(index, row);
                        index++;
                    }
                    command.Dispose();
                    dataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return records;
        }
        public static Dictionary<int, List<object>> GetRecords(string query, CommandType type, List<DbParameter> parameters)
        {
            Dictionary<int, List<object>> records = new Dictionary<int, List<object>>();
            DbCommand command = null;
            DbDataReader dataReader = null;
            int index = 0;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandType = type;
                    command.CommandText = query;
                    command.Parameters.AddRange(parameters.ToArray());
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int f = 0; f < dataReader.FieldCount; f++)
                        {
                            row.Add(dataReader[f]);
                        }
                        records.Add(index, row);
                        index++;
                    }
                    command.Dispose();
                    dataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return records;
        }
        public static Dictionary<int, List<object>> GetRecords(string query, CommandType type, List<DbParameter> parameters, out int rows)
        {
            Dictionary<int, List<object>> records = new Dictionary<int, List<object>>();
            DbCommand command = null;
            DbDataReader dataReader = null;
            int index = 0;
            rows = 0;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandType = type;
                    command.CommandText = query;
                    command.Parameters.AddRange(parameters.ToArray());
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int f = 0; f < dataReader.FieldCount; f++)
                        {
                            row.Add(dataReader[f]);
                        }
                        records.Add(index, row);
                        index++;
                    }

                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            rows = Convert.ToInt32(dataReader["RECORDS"]);
                        }
                    }
                    command.Parameters.Clear();
                    command.Dispose();
                    dataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return records;
        }
        public static int GetCounts(string query)
        {
            DataTable oDataTable = new DataTable();
            DbCommand command = null;
            object value = null;
            int counts = 0;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = query;

                    value = command.ExecuteScalar();
                    if (value != null)
                    {
                        counts = Convert.ToInt32(value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return counts;
        }

        public static int GetCounts(string query, List<DbParameter> parameters)
        {
            DataTable oDataTable = new DataTable();
            DbCommand command = null;
            object value = null;
            int counts = 0;
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = query;
                    command.Parameters.AddRange(parameters.ToArray());

                    value = command.ExecuteScalar();
                    if (value != null)
                    {
                        counts = Convert.ToInt32(value);
                    }
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return counts;
        }


        public static void GetDataTable(string query, List<DbParameter> parameters, ref DataTable dataTable)
        {
            DataTable oDataTable = new DataTable();
            try
            {
                using (connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    dataAdapter.SelectCommand = connection.CreateCommand();
                    dataAdapter.SelectCommand.CommandType = CommandType.Text;
                    dataAdapter.SelectCommand.CommandText = query;
                    foreach (DbParameter parameter in parameters)
                    {
                        dataAdapter.SelectCommand.Parameters.Add(parameter);
                    }
                    dataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}