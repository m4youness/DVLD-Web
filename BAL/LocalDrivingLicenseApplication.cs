using DataLocalDrivingLicenseApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLocalDrivingLicenseApplication
{
    public class clsLocalDrivingLicenseApplication
    {

        public enum enMode { AddNew, Update }
        public enMode _Mode;
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int PassedTestCount { get; set; }
        public string Status { get; set; }
        public int TestAppointmentID { get; set; }


        public clsLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
            _Mode = enMode.AddNew;
        }

        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            _Mode = enMode.Update;
        }

        

        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, string ClassName, string NationalNo, string FullName, DateTime ApplicationDate, int PassedTestCount, string Status)
        {
            this.LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            this.ClassName = ClassName;
            this.NationalNo = NationalNo;
            this.FullName = FullName;
            this.ApplicationDate = ApplicationDate;
            this.PassedTestCount = PassedTestCount;
            this.Status = Status;
            _Mode = enMode.Update;
        }

        private bool AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsDataLocalDrivingLicenseApplication.AddNewLocalDrivingLicenseApplication(this.ApplicationID, this.LicenseClassID);
            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        public static DataView GetAllLocalDrivingLicenseApplications()
        {
            return clsDataLocalDrivingLicenseApplication.GetAllDrivingLicenseApplications();
        }

        public static DataView GetAllLocalDrivingLicenseApplications(string Filter, string Key)
        {
            return clsDataLocalDrivingLicenseApplication.GetAllDrivingLicenseApplications(Filter, Key);
        }

        public static DataView GetAllDrivingApplicationWithID(int PersonID)
        {
            return clsDataLocalDrivingLicenseApplication.GetAllDrivingLicensesWithID(PersonID);
        }

        public static bool DoesPersonHaveDuplicateLicense(string NationalNo, string ClassName)
        {
            return clsDataLocalDrivingLicenseApplication.DoesPersonHaveDuplicateLicense(NationalNo, ClassName);
        }
        public static bool CancelLocalDrivingLicenseApplication(int PersonID)
        {
            return clsDataLocalDrivingLicenseApplication.CancelLocalDrivingLicenseApplication(PersonID);
        }

        public static clsLocalDrivingLicenseApplication Find(int LocalDrivingLicenseApplicationID)
        {
            string ClassName = "", NationalNo = "", FullName = "", Status = "";
            DateTime ApplicationDate = DateTime.Now;
            int PassedTestCount = -1;

            if (clsDataLocalDrivingLicenseApplication.GetLocalDrivingLicenseWithID(LocalDrivingLicenseApplicationID, ref ClassName, ref NationalNo, ref FullName, ref ApplicationDate, ref PassedTestCount, ref Status))
            {
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ClassName, NationalNo, FullName, ApplicationDate, PassedTestCount, Status);
            }
            else
                return null;
        }

        public static int FindAppointmentID(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsDataLocalDrivingLicenseApplication.GetMostRecentAppointment(LocalDrivingLicenseApplicationID, TestTypeID);
        }


        public static clsLocalDrivingLicenseApplication FindAppID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1;

            if (clsDataLocalDrivingLicenseApplication.GetApplicationID(LocalDrivingLicenseApplicationID, ref ApplicationID))
            {
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID);
            }
            else
                return null;
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return clsDataLocalDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }

        public static int FindLicenseID(int LocalDrivingLicenseApplicationID)
        {
            return clsDataLocalDrivingLicenseApplication.FindLicenseID(LocalDrivingLicenseApplicationID);
        }


        public static int FindLocalDrivingApplicationIDForRenewedLicenses(int DriverID)
        {
            return clsDataLocalDrivingLicenseApplication.FindLocalDrivingApplicationIDForRenewedLicenses(DriverID);
        }
        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (AddNewLocalDrivingLicenseApplication())
                {
                    _Mode = enMode.Update;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }



    }
}
