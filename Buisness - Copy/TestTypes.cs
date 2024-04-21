using DataTestTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessTestTypes
{
    public class clsTestTypes
    {
        public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public decimal TestTypeFees { get; set; }
        public enum enMode { UpdateMode };
        private enMode _Mode;

        private clsTestTypes(int testTypeID, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            TestTypeID = testTypeID;
            TestTypeTitle = testTypeTitle;
            TestTypeDescription = testTypeDescription;
            TestTypeFees = testTypeFees;
            _Mode = enMode.UpdateMode;
        }

        public static DataView GetAllTestTypes()
        {
            return clsDataTestTypes.GetAllTestTypes();
        }

        public static clsTestTypes Find(int ID)
        {
            string TestTypeTitle = "", TestTypeDescription = "";
            decimal TestTypeFees = -1;

            if (clsDataTestTypes.FindTestTypeByID(ID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
            {
                return new clsTestTypes(ID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            else
                return null;
        }

        public bool UpdateTestType()
        {
            return clsDataTestTypes.UpdateTestType(this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }

        public bool Save()
        {
            if (_Mode == enMode.UpdateMode)
                return UpdateTestType();
            return false;
        }
    }
}
