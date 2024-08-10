using ConnectionStr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DataUsers
{
    public class clsUsersData
    {
       
        public static bool GetInfoByUsername(string UserName, ref int UserID, ref int PersonID, ref string Password, ref bool isActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from Users where UserName=@UserName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];
                    UserName = (string)reader["UserName"];
                    isFound = true;
                    
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

        public static bool GetUserInfoByPersonID(ref string UserName, ref int UserID, int? PersonID, ref string Password, ref bool isActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from Users where PersonID=@PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = (int)reader["UserID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];
                    UserName = (string)reader["UserName"];
                    isFound = true;

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

        public static bool GetUserInfoByUserID(ref string UserName, int? UserID, ref int PersonID, ref string Password, ref bool isActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select * from Users where UserID=@UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];
                    UserName = (string)reader["UserName"];
                    isFound = true;

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

        public static bool IsActive(string UserName)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select IsActive from Users where UserName = @UserName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            bool Result = false;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && bool.TryParse(result.ToString(), out bool IntResult))
                {
                    Result = IntResult;
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }

            return Result;
        }

        public static bool IsExist(string UserName)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found=1 from Users where UserName = @UserName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            int Result = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int IntResult))
                {
                    Result = IntResult;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.message);
            }
            finally
            {
                connection.Close();
            }

            return (Result == 1);
        }

        public static DataView GetAllUsers()
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select UserID, PersonID, UserName, IsActive from Users";
            SqlCommand command = new SqlCommand(query, connection);
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
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

        public static DataView GetAllUsers(string FilterKey, string FilterWord)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = $"select UserID, PersonID, UserName, IsActive from Users where {FilterKey} like @FilterWord";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FilterWord", $"{FilterWord}%");
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
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
                connection.Close();
            }

            DataView dv = new DataView(dt);
            return dv;
        }

        public static DataView GetAllUsers(bool Active)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "";
            if (Active)
                query = "select UserID, PersonID, UserName, IsActive from Users where IsActive = 1";
            else
                query = "select UserID, PersonID, UserName, IsActive from Users where IsActive = 0";


            SqlCommand command = new SqlCommand(query, connection);
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
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

        public static int InsertUsers(int? PersonID, string UserName, string Password, bool IsActive)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Insert into Users (PersonID, UserName, Password, IsActive) values (@PersonID, @UserName, @Password, @IsActive) select scope_identity();";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ResultInt))
                {
                    ID = ResultInt;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }



            return ID;
        }
   
        public static bool IsPersonUser(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "select found=1 from Users where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            int Result = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int IntResult))
                {
                    Result = IntResult;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.message);
            }
            finally
            {
                connection.Close();
            }

            return (Result == 1);
        }
    
        public static bool DeleteUser(int?  UserID)
        {
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Delete Users where UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
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
    
    
        public static bool UpdatePerson(int? PersonID, string UserName, string Password, bool IsActive)
        {
            int RowsAffeected = 0;
            SqlConnection connection = new SqlConnection(clsConnectionString.ConnectionString);
            string query = "Update Users set UserName = @UserName, Password = @Password, IsActive = @IsActive where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                RowsAffeected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }


            return (RowsAffeected > 0);
        }
    

        
        
    }
}
