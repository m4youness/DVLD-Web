using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataApplications;
using DataLocalDrivingLicenseApplication;
using static System.Net.Mime.MediaTypeNames;

namespace BuisnessApplications
{
    public class clsApplications
    {
        public enum enMode { Update, AddNew }
        public enMode _Mode;
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }

        private clsApplications(int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID, int applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        { 
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            _Mode = enMode.Update;
        }

        public clsApplications()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = -1;
            ApplicationStatus = -1;
            LastStatusDate = DateTime.Now;
            PaidFees = -1;
            CreatedByUserID = -1;
            _Mode = enMode.AddNew;
        }


        private bool AddNewApplication()
        {
            this.ApplicationID = clsDataApplications.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID, this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID != -1);
        }

        public static clsApplications Find(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, ApplicationStatus = -1, CreatedByUserID = -1;
            decimal PaidFees = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;

            if (clsDataApplications.FindApplicationByID(ApplicationID, ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new clsApplications(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }
            else
                return null;
        }

        public static bool SetApplicationToComplete(int LocalDrivingLicenseApplicationID)
        {
            return clsDataApplications.SetLicenseStatusToComplete(LocalDrivingLicenseApplicationID);
        }

        public static int GetLocalDrivingApplicationID(int ApplicationID)
        {
            return clsDataApplications.FindLocalDrivingApplicationsID(ApplicationID);
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (AddNewApplication())
                {
                    _Mode = enMode.Update;
                    return true;
                }
                else
                    return false;
            }
            else if (_Mode == enMode.Update)
            {
                return false;
            }
            return false;
        }
    }
}
