using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionStr;

namespace DataDetainedLicenses
{
    public class clsDataDetainedLicenses
    {
        public static int AddDetainedLicense(int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID, bool IsReleased)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Insert Into DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased) values (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased) select SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsReleased", IsReleased);
            int DetainID = -1;


            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    DetainID = IntSender;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return DetainID;
        }

        public static bool FindDetainedLicenseByID(ref int DetainID, int LicenseID, ref DateTime DetainDate, ref decimal FineFees, ref int CreatedByUserID, ref bool IsReleased)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 * from DetainedLicenses where LicenseID = @LicenseID order by DetainID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            bool IsFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    DetainID = (int)reader["DetainID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = (decimal)reader["FineFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsReleased = (bool)reader["IsReleased"];
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
    
        public static bool ReleaseLicense(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Update DetainedLicenses set IsReleased = 1 where LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            int RowsAffected = -1;

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
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

            return (RowsAffected > 0);
        }
        public static bool IsReleased(int DetainID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select IsReleased from DetainedLicenses where DetainID = @DetainID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", DetainID);
            bool IsReleased = false;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && bool.TryParse(sender.ToString(), out bool BoolSender))
                {
                    IsReleased = BoolSender;
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
            return IsReleased;
        }

        public static bool IsReleasedWithLicenseID(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 IsReleased from DetainedLicenses where LicenseID = @LicenseID order by DetainID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            bool IsReleased = false;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && bool.TryParse(sender.ToString(), out bool BoolSender))
                {
                    IsReleased = BoolSender;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return IsReleased;
        }

        public static bool IsDetained(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 IsReleased from DetainedLicenses where LicenseID = @LicenseID order by DetainID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            bool Found = false;
            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if(sender != null && bool.TryParse(sender.ToString(), out bool BoolSender))
                {
                    Found = BoolSender;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return !Found;

        }
   
        public static DataView GetAllDetainedLicenses()
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select DetainID, DetainedLicenses.LicenseID, DetainDate, IsReleased, FineFees, ReleaseDate, NationalNo, FullName = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName, ReleaseApplicationID  from DetainedLicenses inner join Licenses on Licenses.LicenseID = DetainedLicenses.LicenseID inner join Drivers on Drivers.DriverID = Licenses.DriverID inner join People on Drivers.PersonID = People.PersonID";
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

        public static DataView GetAllDetainedLicenses(string Filter, string Key)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = $"select * from DetainedLicensesView where {Filter} like @Key";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Key", $"{Key}%");
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

        public static DataView GetAllDetainedLicenses(bool IsReleased)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = $"select * from DetainedLicensesView where IsReleased = @IsReleased";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IsReleased", IsReleased);
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

        public static bool UpdateDetainedLicenses(int DetainID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Update DetainedLicenses Set ReleaseDate = @ReleaseDate, ReleasedByUserID = @ReleasedByUserID, ReleaseApplicationID = @ReleaseApplicationID where DetainID = @DetainID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@DetainID", DetainID);
            int RowsAffected = -1;

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }
   
            
    }
}
