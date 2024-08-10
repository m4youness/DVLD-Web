using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDetainedLicenses;

namespace BuisnessDetainLicense
{
    public class clsDetainLicense
    {
        public enum enMode { AddNew, Update }
        public enMode _Mode;
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        
        public clsDetainLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            CreatedByUserID = -1;
            DetainDate = DateTime.Now;
            FineFees = -1;
            IsReleased = false;
            _Mode = enMode.AddNew;
        }

        private clsDetainLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID, bool isReleased)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
           
            _Mode = enMode.Update;
        }

        private bool _AddDetainedLicense()
        {
            this.DetainID = clsDataDetainedLicenses.AddDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID, this.IsReleased);
            return (this.DetainID != -1);
        }

        public static clsDetainLicense Find(int LicenseID)
        {
            int DetainID = -1, CreatedByUserID = -1;
            DateTime DetainDate = DateTime.Now;
            decimal FineFees = -1;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.Now;
           

            if (clsDataDetainedLicenses.FindDetainedLicenseByID(ref DetainID, LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased))
            {
                return new clsDetainLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased);
            }
            else
                return null;
        }

        public static bool ReleaseLicense(int LicenseID)
        {
            return clsDataDetainedLicenses.ReleaseLicense(LicenseID);
        }
        public static bool IsLicenseReleased(int DetainID)
        {
            return clsDataDetainedLicenses.IsReleased(DetainID);
        }

        public static bool IsLicenseReleasedWithLicenseID(int LicenseID)
        {
            return clsDataDetainedLicenses.IsReleasedWithLicenseID(LicenseID);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDataDetainedLicenses.IsDetained(LicenseID);
        }

        public static DataView GetAllDetainedLicense()
        {
            return clsDataDetainedLicenses.GetAllDetainedLicenses();
        }

        public static DataView GetAllDetainedLicense(string Filter, string Key)
        {
            return clsDataDetainedLicenses.GetAllDetainedLicenses(Filter, Key);
        }

        public static DataView GetAllDetainedLicense(bool IsReleased)
        {
            return clsDataDetainedLicenses.GetAllDetainedLicenses(IsReleased);
        }

        public static bool _UpdateDetainedLicense(int DetainID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDataDetainedLicenses.UpdateDetainedLicenses(DetainID, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
        }
        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (_AddDetainedLicense())
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
