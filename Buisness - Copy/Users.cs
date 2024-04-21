using DataUsers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessUsers
{
    public class clsUsers
    {
        public enum enMode { AddMode = 0, UpdateMode = 1 }
        public  enMode Mode;
        public int? UserID { get; set; }
        public int? PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }

       

        private clsUsers(string UserName, int? UserID, int? PersonID, string Password, bool isActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.isActive = isActive;
            Mode = enMode.UpdateMode;

        }

        public clsUsers()
        {
            this.UserID = null;
            this.PersonID = null;
            this.UserName = "";
            this.Password = "";
            this.isActive = false;
            Mode = enMode.AddMode;
        }

        public static clsUsers Find(string UserName)
        {
            int UserID = -1, PersonID = -1;
            bool isActive = false;
            string Password = "";

            if (clsUsersData.GetInfoByUsername(UserName, ref UserID, ref PersonID, ref Password, ref isActive))
                return new clsUsers(UserName, UserID, PersonID, Password, isActive);
            else
                return null;
        }

        public static clsUsers Find(int? PersonID)
        {
            int UserID = -1;
            bool isActive = false;
            string Password = "", UserName = "";

            if (clsUsersData.GetUserInfoByPersonID(ref UserName, ref UserID, PersonID, ref Password, ref isActive))
                return new clsUsers(UserName, UserID, PersonID, Password, isActive);
            else
                return null;
        }

        public static clsUsers FindByUserID(int? UserID)
        {
            int PersonID = -1;
            bool isActive = false;
            string Password = "", UserName = "";

            if (clsUsersData.GetUserInfoByUserID(ref UserName, UserID, ref PersonID, ref Password, ref isActive))
                return new clsUsers(UserName, UserID, PersonID, Password, isActive);
            else
                return null;
        }

        public static bool IsActive(string UserName)
        {
            return clsUsersData.IsActive(UserName);
        }

        public static bool isExist(string UserName)
        {
            return clsUsersData.IsExist(UserName);
        }

        public static DataView GetUsers()
        {
            return clsUsersData.GetAllUsers();
        }
        public static DataView GetUsers(bool Active)
        {
            return clsUsersData.GetAllUsers(Active);
        }

        public static DataView GetUsers(string FilterKey, string FilterWord)
        {
            return clsUsersData.GetAllUsers(FilterKey, FilterWord);
        }

        public bool InsertUsers()
        {
            this.UserID = clsUsersData.InsertUsers(this.PersonID, this.UserName, this.Password, this.isActive);
            return (this.UserID != -1);
        }

        public static bool DeleteUser(int? UserID)
        {
            return clsUsersData.DeleteUser(UserID);
        }

        public static bool IsPersonUser(int PersonID)
        {
            return clsUsersData.IsPersonUser(PersonID);
        }
        public bool UpdateUser()
        {
            return clsUsersData.UpdatePerson(this.PersonID, this.UserName, this.Password, this.isActive);

        }


        public bool Save()
        {
            if (Mode == enMode.AddMode)
            {
                if (InsertUsers())
                {
                    Mode = enMode.UpdateMode;
                    return true;
                }
                else
                    return false;
            }
            else if (Mode == enMode.UpdateMode)
            {
                return UpdateUser();
            }

            return false;
                
        }


    }
}
