using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataApplicationTypes;

namespace BuisnessApplicationTypes
{
    public class clsApplicationTypes
    {

        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get;set; }
        public enum enMode { UpdateMode };
        private enMode _Mode;


        private clsApplicationTypes(int applicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            ApplicationTypeID = applicationTypeID;
            ApplicationTypeTitle = applicationTypeTitle;
            ApplicationFees = applicationFees;
            _Mode = enMode.UpdateMode;
        }

        public static DataView GetAllApplicationTypes()
        {
            return clsDataApplicationTypes.GetAllApplicationTypes();
        }

        private bool _UpdateApplicationType()
        {
            return clsDataApplicationTypes.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees); 
        }

        public static clsApplicationTypes Find(int ApplicationID)
        {
            string ApplicationTypeTitle = "";
            decimal ApplicationFees = -1;

            if (clsDataApplicationTypes.FindUserByID(ApplicationID, ref ApplicationTypeTitle, ref ApplicationFees))
            {
                return new clsApplicationTypes(ApplicationID, ApplicationTypeTitle, ApplicationFees);
            }
            else
                return null;
        }

        public bool Save()
        {
            if (_Mode == enMode.UpdateMode)
                return _UpdateApplicationType();
            return false;
        }

    }
}
