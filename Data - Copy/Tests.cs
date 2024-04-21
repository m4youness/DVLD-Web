using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConnectionStr;

namespace DataTests
{
    public class clsDataTests
    {
        public static int AddTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Insert into Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID) values (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID) select scope_identity()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            int TestID = -1;


            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender!= null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    TestID = IntSender;
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

            return TestID;
        }

    }
}
