using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCountry;

namespace BuisnessCountry
{
    public class clsCountry
    {

        public string CountryName { get; set; }
        public int NationalityCountryID { get; set; }

        public clsCountry(int NationalityCountryID, string CountryName)
        {
            this.CountryName = CountryName;
            this.NationalityCountryID = NationalityCountryID;
        }
        public static DataView GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        public static clsCountry Find(int ID)
        {
            string CountryName = "";
            if (clsCountryData.GetCountryByID(ID, ref CountryName))
                return new clsCountry(ID, CountryName);
            else
                return null;
        }

        public static clsCountry Find(string CountryName)
        {
            int NationalityCountryID = -1;
            if (clsCountryData.GetCountryByName(ref NationalityCountryID, CountryName))
                return new clsCountry(NationalityCountryID, CountryName);
            else
                return null;
        }

    }
}
