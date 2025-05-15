using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Helpers
{
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = QuanLyKho; Integrated Security = True; ";

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static SqlCommand CreateCommand(string query, SqlConnection connection)
        {
            var command = new SqlCommand(query, connection);
            command.CommandType = CommandType.Text;
            return command;
        }

        // AddParameter method to simplify adding parameters to SqlCommand
        public static void AddParameter(this SqlCommand command, string parameterName, object value)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Ghi log lỗi
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw; // Ném lại lỗi để xử lý ở tầng cao hơn
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                Console.WriteLine($"General Error: {ex.Message}");
                throw; // Ném lại lỗi để xử lý ở tầng cao hơn
            }
        }

        public static SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            try
            {
                var connection = GetConnection();
                var command = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw;
            }
        }

    }
}

//Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = QuanLyKhoVietNam; Integrated Security = True; ";