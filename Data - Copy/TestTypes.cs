using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionStr;

namespace DataTestTypes
{
    public class clsDataTestTypes
    {
        public static DataView GetAllTestTypes()
        {
            SqlConnection connction = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from TestTypes";
            SqlCommand command = new SqlCommand(query, connction);

            DataTable dt = new DataTable();

            try
            {
                connction.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
            }
            catch (Exception ex)
            {
                string source_name = "DVLD Project";
                if (!EventLog.SourceExists(source_name))
                {
                    EventLog.CreateEventSource(source_name, "Application");
                }

                EventLog.WriteEntry(source_name, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connction.Close();
            }
            DataView dv = new DataView(dt);

            return dv;
        }

        public static bool UpdateTestType(int ID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Update TestTypes set TestTypeTitle = @TestTypeTitle, TestTypeDescription = @TestTypeDescription, TestTypeFees = @TestTypeFees where TestTypeID = @TestTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
            command.Parameters.AddWithValue("@TestTypeID", ID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);

        }


        public static bool FindTestTypeByID(int ID, ref string TestTypeTitle, ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = $"select * from TestTypes where TestTypeID = {ID}";
            SqlCommand command = new SqlCommand(query, connection);

           

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeDescription = (string)reader["TestTypeDescription"];
                    TestTypeFees = (decimal)reader["TestTypeFees"];
                }
                else
                    isFound = false;
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
    }
}
