using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionStr;

namespace DataLicenses
{
    public class clsDataLicenses
    {
        public static int IssueLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, int IssueReason, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Insert Into Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID) Values (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID) select SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            int LicenseID = -1;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    LicenseID = IntSender;
                }    
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return LicenseID;
        }
    
        public static bool FindLicenseByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, ref bool IsActive, ref int IssueReason, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from Licenses where LicenseID = @LicenseID";
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
                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    Notes = (string)reader["Notes"];
                    PaidFees = (decimal)reader["PaidFees"];
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (int)reader["IssueReason"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
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
    
        public static int FindLocalDrivingLicenseApplicationID(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID from Licenses inner join Applications on Licenses.ApplicationID = Applications.ApplicationID inner join LocalDrivingLicenseApplications on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID where LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            int LocalDrivingLicenseApplicationID = -1;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    LocalDrivingLicenseApplicationID = IntSender;
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

            return LocalDrivingLicenseApplicationID;
        }
   
        public static bool IsActive(int LicenseID)
        {
            int Found = -1;

            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found = 1 from Licenses where LicenseID = @LicenseID and IsActive = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    Found = IntSender;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return (Found == 1);

        }
    
        public static bool IsLicenseDetained(int LicenseID)
        {
            int Found = -1;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 Found = 1 from DetainedLicenses where LicenseID = @LicenseID order by DetainID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    Found = IntSender;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return (Found == 1);
        }
    
        public static int GetIssueReason(int LicenseID)
        {
            int IssueReason = -1;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select IssueReason from Licenses where LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    IssueReason = IntSender;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }


            return IssueReason;
        }
   
        public static bool DeActiveLicense(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Update Licenses set IsActive = 0 where LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            int RowsAffected = 0;

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

        public static int FindDriverID(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select DriverID from Licenses where LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
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

            }
            finally
            {
                connection.Close();
            }

            return DriverID;
        }

        public static int FindLicenseID(string NationalNo)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 Licenses.LicenseID from People inner join Drivers on Drivers.PersonID = People.PersonID inner join Licenses on Licenses.DriverID = Drivers.DriverID where People.NationalNo = @NationalNo order by LicenseID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            int LicenseID = -1;


            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    LicenseID = IntSender;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return LicenseID;
        }
    }
}
