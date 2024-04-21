using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInternationalLicenses;

namespace BuisnessInternationalLicenses
{
    public class clsInternationalLicenses
    {
        public enum enMode { AddNew, Update}
        public enMode _Mode;
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public clsInternationalLicenses()
        {
            InternationalLicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = false;
            CreatedByUserID = -1;
            _Mode = enMode.AddNew;
        }

        private clsInternationalLicenses(int internationalLicenseID, int applicationID, int driverID, int issuedUsingLocalLicenseID, DateTime issueDate, DateTime expirationDate, bool isActive, int createdByUserID)
        {
            InternationalLicenseID = internationalLicenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUserID = createdByUserID;
            _Mode = enMode.Update;
        }

        public static DataView GetInternationalLicensesWithID(int PersonID)
        {
            return clsDataInternationalLicenses.GetInternationalLicensesWithID(PersonID);
        }

        public static DataView GetInternationalLicenses()
        {
            return clsDataInternationalLicenses.GetInternationalLicenses();
        }

        public static DataView GetInternationalLicenses(string Filter, string Value)
        {
            return clsDataInternationalLicenses.GetInternationalLicenses(Filter, Value);
        }
        private bool _AddInternationalLicense()
        {
            this.InternationalLicenseID = clsDataInternationalLicenses.AddInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }

        public static clsInternationalLicenses Find(int InternationalLicenseID) 
        {
            int ApplicationID = -1,  DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;

            if (clsDataInternationalLicenses.FindInternationalLicenseWithID(InternationalLicenseID, ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicenses(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            }
            else
                return null;
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (_AddInternationalLicense())
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
