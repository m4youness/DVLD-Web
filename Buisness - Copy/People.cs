using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DataBaseLayer;

namespace BuisnessLayer
{
    public class clsPeople
    {
        public enum enMode { AddNew = 0, UpdateNew = 1};
        public enMode Mode;
        public int? ID { get; set; }
        public string NationalNo { get; set;}
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string Gendor { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }


        private clsPeople(int? ID, string NationalNo, string firstName, string secondName, string thirdName, string lastName, string gendor, DateTime dateOfBirth, string address, string countryName, string phone, string email, int NationalityCountryID)
        {
            this.ID = ID;
            this.NationalNo = NationalNo;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.Gendor = gendor;
            this.DateOfBirth = dateOfBirth;
            this.Address = address;
            this.CountryName = countryName;
            this.Phone = phone;
            this.Email = email;
            this.NationalityCountryID = NationalityCountryID;
            
            Mode = enMode.UpdateNew;
        }

        public clsPeople()
        {
            this.ID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.Gendor = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.NationalityCountryID = -1;
            this.Phone = "";
            this.Email = "";
            Mode = enMode.AddNew;
        }

        public static DataView GetAllInfo()
        {
            return clsPeopleData.GetAllInfo();
        }
        public static DataView GetAllInfo(string FilterWord, string FilterKeyWord)
        {
            return clsPeopleData.GetAllInfo(FilterWord, FilterKeyWord);
        }
    
        public static clsPeople Find(int? ID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Gendor = "", Email = "", Address = "", Phone = "", CountryName = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            
            if (clsPeopleData.GetInfoByID(ID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref Gendor, ref DateOfBirth, ref Address, ref CountryName, ref Phone, ref Email, ref NationalityCountryID))
            {
                return new clsPeople(ID, NationalNo,FirstName, SecondName, ThirdName, LastName, Gendor, DateOfBirth, Address, CountryName, Phone, Email, NationalityCountryID);
            }
            else
                return null;
        }

        public static clsPeople Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Gendor = "", Email = "", Address = "", Phone = "", CountryName = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1, ID = -1;

            if (clsPeopleData.GetInfoByNationalNo(ref ID, NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref Gendor, ref DateOfBirth, ref Address, ref CountryName, ref Phone, ref Email, ref NationalityCountryID))
            {
                return new clsPeople(ID, NationalNo, FirstName, SecondName, ThirdName, LastName, Gendor, DateOfBirth, Address, CountryName, Phone, Email, NationalityCountryID);
            }
            else
                return null;
        }

        public static bool IsNationalIdExist(string NationalNo)
        {
            return clsPeopleData.IsNationalNoExists(NationalNo);
        }
        

        public static bool DeleteByID(int? ID)
        {
            return clsPeopleData.DeleteInfoByID(ID);
        }

        private bool _AddPerson()
        {
            this.ID = clsPeopleData.InsetPeople(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.Gendor, this.DateOfBirth, this.Address, this.Phone, this.Email, this.NationalityCountryID);
            return (ID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPeopleData.UpdateInfoByID(this.ID, this.NationalNo,this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.Gendor, this.DateOfBirth, this.Address, this.Phone, this.Email, this.NationalityCountryID);
        }

        public static int GetLocalDrivingLicenseID(string NationalNo)
        {
            return clsPeopleData.GetLocalDrivingApplicationID(NationalNo);
        }

        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                if (_AddPerson())
                {
                    Mode = enMode.UpdateNew;
                    return true;
                }
                else
                    return false;
            }
            else if (Mode == enMode.UpdateNew)
            {
                return _UpdatePerson();
            }


            return false;
        }


    
    }
}
