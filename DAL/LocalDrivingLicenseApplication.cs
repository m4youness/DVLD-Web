using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ConnectionStr;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DataLocalDrivingLicenseApplication
{
    public class clsDataLocalDrivingLicenseApplication
    {
        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Insert into LocalDrivingLicenseApplications (ApplicationID, LicenseClassID) values (@ApplicationID, @LicenseClassID) select SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            int LocalDrivingLicenseApplications = -1;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();

                if (sender != null && int.TryParse(sender.ToString(), out int InsertedID))
                {
                    LocalDrivingLicenseApplications = InsertedID;
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

            return LocalDrivingLicenseApplications;
        }
   
        public static DataView GetAllDrivingLicenseApplications()
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from LocalDrivingLicenseApplications_View";
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

        public static DataView GetAllDrivingLicenseApplications(string Filter, string Key)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = $"select * from LocalDrivingLicenseApplications_View where {Filter} like @Key";
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

        public static bool DoesPersonHaveDuplicateLicense(string NationalNo, string ClassName)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found = 1 from LocalDrivingLicenseApplications_View where NationalNo = @NationalNo and ClassName = @ClassName and Status in ('Completed', 'New')";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@ClassName", ClassName);
            int Found = -1;

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
   
        public static bool CancelLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = $"Update Applications set ApplicationStatus = 2 from LocalDrivingLicenseApplications inner join Applications on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID inner join People on People.PersonID = Applications.ApplicantPersonID where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and ApplicationStatus = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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
   
        public static bool GetLocalDrivingLicenseWithID(int LocalDrivingLicenseID, ref string ClassName, ref string NationalNo, ref string FullName, ref DateTime ApplicationDate, ref int PassedTestCount, ref string Status)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from LocalDrivingLicenseApplications_View where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseID);
            bool IsFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ClassName = (string)reader["ClassName"];
                    NationalNo = (string)reader["NationalNo"];
                    FullName = (string)reader["FullName"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    PassedTestCount = (int)reader["PassedTestCount"];
                    Status = (string)reader["Status"];
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
        

        public static bool GetApplicationID(int LocalDrivingLicenseID, ref int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select ApplicationID from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseID);
            bool IsFound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)reader["ApplicationID"];
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
    
        public static int GetMostRecentAppointment(int LocalDrivingLicenseID, int TestTypeID)
        {
            string TestTypeTitle = "";

            switch (TestTypeID)
            {
                case 1:
                    TestTypeTitle = "Vision Test";
                    break;
                case 2:
                    TestTypeTitle = "Written (Theory) Test";
                    break;
                case 3:
                    TestTypeTitle = "Practical (Street) Test";
                    break;
            }

            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 TestAppointmentID from TestAppointments_View where TestTypeTitle = @TestTypeTitle and LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID order by TestAppointmentID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseID);
            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);


            int AppointmentID = -1;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                { 
                    AppointmentID = (int)reader["TestAppointmentID"];
                }
                
                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return AppointmentID;
        }
       
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Delete from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseID);
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

        public static DataView GetAllDrivingLicensesWithID(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select LicenseID, ApplicationID, ClassName, IssueDate, ExpirationDate, IsActive from Licenses inner join LicenseClasses on Licenses.LicenseClass = LicenseClasses.LicenseClassID inner join Drivers on Drivers.DriverID = Licenses.DriverID inner join People on People.PersonID = Drivers.PersonID where People.PersonID = @PersonID order by ExpirationDate desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
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
        public static int FindLocalDrivingApplicationIDForRenewedLicenses(int DriverID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 LocalDrivingLicenseApplicationID from Licenses inner join LicenseClasses on Licenses.LicenseClass = LicenseClasses.LicenseClassID \r\ninner join LocalDrivingLicenseApplications on LocalDrivingLicenseApplications.ApplicationID = Licenses.ApplicationID where DriverID = @DriverID order by ExpirationDate asc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", @DriverID);

            int LocalDrivingApplicationID = -1;


            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    LocalDrivingApplicationID = IntSender;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return LocalDrivingApplicationID;
        }

        public static int FindLicenseID(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 Licenses.LicenseID from LocalDrivingLicenseApplications " +
                "inner join Applications on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID " +
                "inner join People on Applications.ApplicantPersonID = People.PersonID inner join Drivers on Drivers.PersonID = People.PersonID " +
                "inner join Licenses on Licenses.DriverID = Drivers.DriverID where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID" +
                " order by Licenses.LicenseID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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
