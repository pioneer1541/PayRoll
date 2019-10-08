using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll
{
    class PersonList
    {
        public static Dictionary<string,string[]> data_Dictionary= new Dictionary<string,string[]>();

        //This Dictionary will be stored the data of the List and it is a global variable.

        public bool Check_Key(string key)
        //This method will return whether the key has already be stored in the Dictionary.
        {
            return data_Dictionary.ContainsKey(key);
        }

        public void Add_NewData(string key, string New_Data)
        //This method will add new data in the Dictionary.
        //The Value type is String.
        {
            if (!data_Dictionary.ContainsKey(key))
            {
                data_Dictionary.Add(key, New_Data.Split(','));
            }
            
        }

        public void Add_NewDataArrary(string key, string[] New_Data)
        //This method will add new data in the Dictionary.
        //The Value type is Array.
        {
            data_Dictionary.Add(key, New_Data);
        }

        public void Change_Data(string key, string[] New_Data)
        //This method will update a Data in the Dictionary.
        //The Value type is Array.
        {
            data_Dictionary[key] = New_Data;
        }

        public void Delete_Item (string key)
        //This method will delete a specific item from the Dictionary.
        {
            data_Dictionary.Remove(key);
        }

        public string getIndexFromString(string data)
        //This method will analyze data from the parameter and return its index.
        {
            return data.Substring(0, data.IndexOf(','));
        }

        public string[] getArrayForList()
        //This method will return a specific item data as a String Array.
        {
            string[] result_data = new string[data_Dictionary.Count];
            int count = 0;
            foreach (KeyValuePair<string, string[]> x in data_Dictionary)
            {
                string a = string.Join(",", x.Value);
                result_data[count] = a;
                count++;
            }
            return result_data;
        }

        public int getCountFromDictionary()
        //This method will return how many items in the Dictionary.
        {
            return data_Dictionary.Count();
        }

        public string getStringForList(string index)
        //This method will return a specific item data as a String.
        {
            string[] a = new string[4];
            a = data_Dictionary[index];
            string result_data = string.Join(",", data_Dictionary[index]);
            return result_data;
        }

        public string[] getEmptyArray()
        //This method will return an empty Array.
        {
            string[] a = new string[1] ;
            return a;
        }
    }
}
