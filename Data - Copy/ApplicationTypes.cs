using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionStr;

namespace DataApplicationTypes
{
    public class clsDataApplicationTypes
    {
        public static DataView GetAllApplicationTypes()
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from ApplicationTypes";
            SqlCommand command = new SqlCommand(query, connection);
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);

                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            DataView dv = new DataView(dt);

            return dv;
        }
        public static bool UpdateApplicationType(int ID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);

            string query = "Update ApplicationTypes set ApplicationTypeTitle = @ApplicationTypeTitle, ApplicationFees = @ApplicationFees where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            command.Parameters.AddWithValue("@ApplicationTypeID", ID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
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
                connection.Close();
            }


            return (rowsAffected > 0);
        }
    
        public static bool FindUserByID(int ID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from ApplicationTypes where ApplicationTypeID = @ApplicationTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ID);
            bool IsFound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationFees = (decimal)reader["ApplicationFees"];
                    IsFound = true;
                }
                else
                    IsFound = false;
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }
    }
}
