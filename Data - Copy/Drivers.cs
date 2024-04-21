using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ConnectionStr;

namespace DataDrivers
{
    public class clsDataDrivers
    {
        public static int AddDrivers(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Insert Into Drivers (PersonID, CreatedByUserID, CreatedDate) Values (@PersonID, @CreatedByUserID, @CreatedDate) select SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);
            int DriversID = -1;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();

                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    DriversID = IntSender;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return DriversID;
        }
    
        public static DataView GetAllDrivers()
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from Drivers_View";
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

        public static DataView GetAllDrivers(string FilterKey, string FilterValue)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = $"select * from Drivers_View where {FilterKey} like @FilterValue";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FilterValue", $"{FilterValue}%");
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
    
        public static int GetPersonIDFromDriver(int DriverID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select PersonID from Drivers where DriverID = @DriverID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            int PersonID = -1;
            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    PersonID = IntSender;
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }

            return PersonID;
        }
    
        public static int GetDriverIDWithPersonID(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select DriverID from Drivers where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            int DriverID = -1;
            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    DriverID = IntSender;
                }
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
            return DriverID;
        }
    }
}
