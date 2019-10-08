using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll
{
    class Waga_Tax
    {
        public static Dictionary<string, string> Rate = new Dictionary<string, string>();
        //This Dictionary will be stored the data of the every person's Rate and it is a global variable.

        public void AddRateToDictionary (string index, string rate)
        //This method will add new rate into the Dictionary.
        {
            if (!Rate.ContainsKey(index))
            {
                Rate.Add(index, rate);
            }
            else
            {
                ChangeRateToDictionary(index, rate);
            }
        }

        public void ChangeRateToDictionary(string index, string rate)
        //This method will change the rate into a specific key.
        {
            Rate[index] = rate;
        }

        public bool CheckID(string index)
        //This method will checks and return whether the id of the employee is valid.
        {
            return Rate.ContainsKey(index);
        }

        public void SetRateToDictionary(string index, string rate)
        //This method will set and update the rate of a specific person.
        {
            Rate[index] = rate;
        }

        public string GetRateFromDictionary(string index)
        //This method will get and return the rate of a specific person.
        {
            return Rate[index];
        }

        public string GetRateFromString(string index,string data)
        //This method will return the rate of a specific person by analyzing its data.
        {
            int rate_num_start = data.IndexOf(',', data.IndexOf(',', data.IndexOf(',')+1))+1;
            int rate_num_count = data.LastIndexOf(',') - rate_num_start;
            string rate = data.Substring(rate_num_start, rate_num_count);
            return rate;
        }

        public string getIndexFromString(string data)
        //This method will return the index of a specific person by analyzing its data.
        {
            return data.Substring(0, data.IndexOf(','));
        }

        public string getCountFromDictionary()
        //This method will return how many items in the Dictionary.
        {
            return Rate.Count.ToString();
        }
    }
}
