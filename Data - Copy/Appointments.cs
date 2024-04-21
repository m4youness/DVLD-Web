using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ConnectionStr;

namespace DataAppointments
{
    public class clsDataAppointments
    {
        public static DataView LoadInAppointments(int LocalLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select TestAppointmentID as AppointmentID, AppointmentDate, PaidFees, IsLocked from TestAppointments where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalLicenseApplicationID);

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



            DataView dv = new DataView(dt);
            return dv;

        }
    
        public static bool DidPersonAlreadyPassTest(int LocalLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found = 1 from TestAppointments inner join Tests on Tests.TestAppointmentID = TestAppointments.TestAppointmentID inner join TestTypes on TestAppointments.TestTypeID = TestTypes.TestTypeID where TestTypes.TestTypeID = @TestTypeID and LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestResult = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            int Found = -1;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int InsertedValue))
                {
                    Found = InsertedValue;
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
        
        public static bool IsLocked(int TestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found = 1 from TestAppointments where IsLocked = 1 and TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestApplicationID);
            int found = -1;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    found = IntSender;
                }    
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        return (found == 1);
        }
        public static int AddAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked)
        {
            int TestAppointmentID = -1;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Insert into TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked) values (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked) select SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();

                if (sender != null && int.TryParse(sender.ToString(), out int AppointmentID))
                {
                    TestAppointmentID = AppointmentID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }


            return TestAppointmentID;
        }
        
        public static int GetNumOfPassedTestCounts(int LocalLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "SELECT (SELECT COUNT(dbo.TestAppointments.TestTypeID) AS PassedTestCount FROM Tests INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID WHERE (TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID) AND (Tests.TestResult = 1)) AS PassedTestCountFROM dbo.LocalDrivingLicenseApplications INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID INNER JOIN LicenseClasses ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID INNER JOIN People ON Applications.ApplicantPersonID = People.PersonID where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalLicenseApplicationID);
            int PasssedTestCount = -1;


            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int SenderInt))
                {
                    PasssedTestCount = SenderInt;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return PasssedTestCount;
        }
   
        public static bool GetAppointmentByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from TestAppointments where TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            bool Isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Isfound = true;
                    TestTypeID = (int)reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return Isfound;
        }
    
        public static bool UpdateAppointmentLockedStatus(int TestAppointmentID, bool IsLocked)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Update TestAppointments set IsLocked = @IsLocked where TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
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
    
        public static bool PersonAlreadyHasAppointment(int AppointmentID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found = 1 from TestAppointments where TestAppointmentID = @TestAppointmentID and IsLocked = 0";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", AppointmentID);
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
    
        public static bool DidPersonFail(int TestAppointmentID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select fail = 1 from Tests inner join TestAppointments on Tests.TestAppointmentID = TestAppointments.TestAppointmentID where TestResult = 0 and TestAppointments.TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
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
    
    }
}
