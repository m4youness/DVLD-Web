using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAppointments;

namespace BuisnessAppointments
{
    public class clsAppointments
    {

        public enum enMode { Update, AddNew }
        public enMode _Mode;
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }

        public clsAppointments()
        {
            int TestAppointmentID = -1;
            int TestTypeID = -1;
            int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = -1;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            _Mode = enMode.AddNew;
        }

        private clsAppointments(int testAppointmentID, int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, decimal paidFees, int createdByUserID, bool isLocked)
        {
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            _Mode = enMode.Update;
        }

        public static DataView GetAppointmentByID(int LocalLicenseApplicationID, int TestTypeID)
        {
            return clsDataAppointments.LoadInAppointments(LocalLicenseApplicationID, TestTypeID);
        }
        
        public static bool DidPersonPassTest(int LocalLicenseApplicationID, int TestTypeID)
        {
            return clsDataAppointments.DidPersonAlreadyPassTest(LocalLicenseApplicationID, TestTypeID);
        }

        private bool _AddNewAppointment()
        {
            this.TestAppointmentID = clsDataAppointments.AddAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked);
            return (this.TestAppointmentID != -1);
        }

        public static bool IsAppointmentLocked(int LocalLicenseApplicationID)
        {
            return clsDataAppointments.IsLocked(LocalLicenseApplicationID);
        }

        public static bool PersonAlreadyHasAppointment(int AppointmentID)
        {
            return clsDataAppointments.PersonAlreadyHasAppointment(AppointmentID);
        }

        public static int GetPassedTestCount(int LocalLicenseApplicationID)
        {
            return clsDataAppointments.GetNumOfPassedTestCounts(LocalLicenseApplicationID);
        }

        public static clsAppointments Find(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, CreatedByUserID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = -1;
            bool IsLocked = false;

            if (clsDataAppointments.GetAppointmentByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked))
            {
                return new clsAppointments(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked);
            }
            else
                return null;
        }

        private bool _UpdateTestAppointments()
        {
            return clsDataAppointments.UpdateAppointmentLockedStatus(this.TestAppointmentID, this.IsLocked);
        }

        public static bool DidPersonFail(int TestAppointmentID)
        {
            return clsDataAppointments.DidPersonFail(TestAppointmentID);
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (_AddNewAppointment())
                {
                    _Mode = enMode.Update;
                    return true;
                }
                else
                    return false;
            }
            else if (_Mode == enMode.Update)
            {
                return _UpdateTestAppointments();
            }
            else
                return false;
        }
    }
}
