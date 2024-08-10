using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DataLicenses;

namespace BuisnessLicenses
{
    public class clsLicenses
    {
        public enum enMode { AddNew, Update}
        public enMode _Mode;
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public int IssueReason { get; set; }
        public int CreatedByUserID { get; set; }

        public clsLicenses()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            PaidFees = -1;
            IsActive = false;
            IssueReason = -1;
            CreatedByUserID = -1;
            _Mode = enMode.AddNew;
        }

        private clsLicenses(int licenseID, int applicationID, int driverID, int licenseClass, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive, int issueReason, int createdByUserID)
        {
            LicenseID = licenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;
            _Mode = enMode.Update;
        }

        private bool IssueLicense()
        {
            this.LicenseID = clsDataLicenses.IssueLicense(this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, this.IssueReason, this.CreatedByUserID);
            return (this.LicenseID != -1);
        }

        public static clsLicenses Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClass = -1, IssueReason = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = -1;
            bool IsActive = false;

            if (clsDataLicenses.FindLicenseByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicenses(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public static int FindLocalDrivingLicenseApplicationID(int LicenseID)
        {
            return clsDataLicenses.FindLocalDrivingLicenseApplicationID(LicenseID);
        }

        public static bool IsLicenseActive(int LicenseID)
        {
            return clsDataLicenses.IsActive(LicenseID);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDataLicenses.IsLicenseDetained(LicenseID);
        }

        public static int GetIssueReason(int LicenseID)
        {
            return clsDataLicenses.GetIssueReason(LicenseID);
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            return clsDataLicenses.DeActiveLicense(LicenseID);
        }

        public static int FindDriverID(int LicenseID)
        {
            return clsDataLicenses.FindDriverID(LicenseID);
        }

        public static int FindLicenseID(string NationalNo)
        {
            return clsDataLicenses.FindLicenseID(NationalNo);
        }
        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (IssueLicense())
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
