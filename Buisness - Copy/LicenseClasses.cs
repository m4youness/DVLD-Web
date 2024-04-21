using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLicenseClasses;

namespace BuisnessLicenseClasses
{
    public class clsLicenseClasses
    {

        public enum enMode { AddNew, Update}
        public enMode _Mode;

        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public int MinimumAllowedAge { get; set; }
        public int DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        private clsLicenseClasses(int licenseClassID, string className, string classDescription, int minimumAllowedAge, int defaultValidityLength, decimal classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = className;
            ClassDescription = classDescription;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
            _Mode = enMode.Update;
        }


        public static DataView GetAllLicenseClasses()
        {
            return clsDataLicenseClasses.GetAllLicenseClasses();
        }

        public static clsLicenseClasses Find(string ClassName)
        {
            int LicenseClassID = -1, MinimumAllowedAge = -1, DefaultValidityLength = -1;
            decimal ClassFees = -1;
            string ClassDescription = "";

            if (clsDataLicenseClasses.GetLicenseByClassName(ref LicenseClassID, ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClasses(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }
        public static clsLicenseClasses Find(int LicenseClassID)
        {
            int MinimumAllowedAge = -1, DefaultValidityLength = -1;
            decimal ClassFees = -1;
            string ClassDescription = "";
            string ClassName = "";

            if (clsDataLicenseClasses.GetLicenseByID(LicenseClassID, ref ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClasses(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }

    }
}
