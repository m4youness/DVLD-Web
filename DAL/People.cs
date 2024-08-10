using ConnectionStr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public class clsPeopleData
    {
        static string ConnectionString = "Server=.;Database=DVLD;User Id=sa;Password=sa123456;";

        static public DataView GetAllInfo()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            
            string query = "select PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, Gendor, DateOfBirth, CountryName, Phone, Email from People inner join Countries on People.NationalityCountryID = Countries.CountryID";
            
            SqlCommand command = new SqlCommand(query, connection);


            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
            DataView dv = new DataView(dt);

            return dv;
        }

        static public DataView GetAllInfo(string FilterWord, string FilterKeyWord)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            
            string query = $"select PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, Gendor, DateOfBirth, CountryName, Phone, Email from People inner join Countries on People.NationalityCountryID = Countries.CountryID where {FilterWord} like @FilterKeyWord";

            
            SqlCommand command = new SqlCommand(query, connection);

            
            command.Parameters.AddWithValue("@FilterKeyWord", $"{FilterKeyWord}%");

            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
            DataView dv = new DataView(dt);

            return dv;
        }

        

        static public bool GetInfoByID(int? ID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref string Gendor, ref DateTime DateOfBirth, ref string Address, ref string CountryName, ref string Phone, ref string Email, ref int NationalityCountryID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = "select  NationalNo, FirstName, SecondName, ThirdName, LastName, Gendor, DateOfBirth, Address, CountryName, Phone, Email, NationalityCountryID from People inner join Countries on People.NationalityCountryID = Countries.CountryID where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", ID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    ThirdName = (string)reader["ThirdName"];
                    LastName = (string)reader["LastName"];
                    Gendor = (string)reader["Gendor"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Address = (string)reader["Address"];
                    CountryName = (string)reader["CountryName"];
                    Phone = (string)reader["Phone"];
                    Email = (string)reader["Email"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];
   
                    
                }
                else
                    isFound = false;
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool GetInfoByNationalNo(ref int ID, string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref string Gendor, ref DateTime DateOfBirth, ref string Address, ref string CountryName, ref string Phone, ref string Email, ref int NationalityCountryID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = "select PersonID, FirstName, SecondName, ThirdName, LastName, Gendor, DateOfBirth, Address, CountryName, Phone, Email, NationalityCountryID from People inner join Countries on People.NationalityCountryID = Countries.CountryID where NationalNo= @NationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    ID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    ThirdName = (string)reader["ThirdName"];
                    LastName = (string)reader["LastName"];
                    Gendor = (string)reader["Gendor"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Address = (string)reader["Address"];
                    CountryName = (string)reader["CountryName"];
                    Phone = (string)reader["Phone"];
                    Email = (string)reader["Email"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];


                }
                else
                    isFound = false;
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        static public bool DeleteInfoByID(int? ID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = @"Delete People where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", ID);

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return (RowsAffected > 0);
        }
    
        static public int InsetPeople(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, string Gendor, DateTime DateOfBirth, string Address, string Phone, string Email, int NationalityCountryID)
        {
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, Gendor, DateOfBirth, Address, Phone, Email, NationalityCountryID)
                             VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @Gendor, @DateOfBirth, @Address, @Phone, @Email, @NationalityCountryID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
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
    
        static public bool UpdateInfoByID(int? ID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, string Gendor, DateTime DateOfBirth, string Address, string Phone, string Email, int NationalityCountryID)
        {
            int RowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Update People set NationalNo = @NationalNo, FirstName = @FirstName, SecondName = @SecondName, ThirdName = @ThirdName, LastName = @LastName, Gendor = @Gendor, DateOfBirth = @DateOfBirth, Address = @Address, Phone = @Phone, Email = @Email, NationalityCountryID = @NationalityCountryID where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@PersonID", ID);


            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (RowsAffected > 0);
        }
    
        static public bool IsNationalNoExists(string NationalNo)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found =1 from People where NationalNo = @NationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            int FoundResult = -1;
            

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int Result))
                {
                    FoundResult = Result;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }


            return (FoundResult == 1);

        }

        public static int GetLocalDrivingApplicationID(string NationalNo)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select top 1 LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID from People inner join Applications on People.PersonID = Applications.ApplicantPersonID inner join LocalDrivingLicenseApplications on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID where People.NationalNo = @NationalNo order by LocalDrivingLicenseApplicationID desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            int LocalDrivingLicenseID = -1;

            try
            {
                connection.Open();
                object sender = command.ExecuteScalar();
                if (sender != null && int.TryParse(sender.ToString(), out int IntSender))
                {
                    LocalDrivingLicenseID = IntSender;
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

            return LocalDrivingLicenseID;

        }

    }
}
