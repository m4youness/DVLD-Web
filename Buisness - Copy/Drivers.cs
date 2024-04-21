using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDrivers;


namespace BuisnessDrivers
{
    public class clsDrivers
    {
        public enum enMode { AddNew, Update}
        public enMode _Mode;
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDrivers()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
            _Mode = enMode.AddNew;
        }

        private bool _AddDrivers()
        {
            this.DriverID = clsDataDrivers.AddDrivers(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            return (this.DriverID != -1);
        }

        public static DataView GetAllDrivers()
        {
            return clsDataDrivers.GetAllDrivers();
        }
        public static DataView GetAllDrivers(string FilterKey, string FilterValue)
        {
            return clsDataDrivers.GetAllDrivers(FilterKey, FilterValue);
        }

        public static int GetPersonIDFromDrivers(int DriverID)
        {
            return clsDataDrivers.GetPersonIDFromDriver(DriverID);
        }

        public static int GetDriverIDWithPersonID(int PersonID)
        {
            return clsDataDrivers.GetDriverIDWithPersonID(PersonID);
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (_AddDrivers())
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
