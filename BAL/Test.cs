using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTests;

namespace BuisnessTest
{
    public class clsTest
    {
        public enum enMode { AddNew, Update}
        public enMode _Mode;
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }  
        public bool TestResult { get; set; }
        public string Notes { get; set; }   
        public int CreatedByUserID { get; set; }

       
        private bool AddTest()
        {
            this.TestID = clsDataTests.AddTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return (this.TestID != -1);
        }
        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (AddTest())
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
