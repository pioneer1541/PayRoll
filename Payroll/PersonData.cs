using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll
{
    class PersonData
    {
        public string index;
        public string name;
        public string payrate;
        public string email;

        public PersonData(string Index, string Name, string Payrate, string Email)
        //Constructor 1. It will initialize the property of the class by 4 parameters.
        {
            this.index = Index;
            this.name = Name;
            this.payrate = Payrate;
            this.email = Email;
        }

        public PersonData()
        //Constructor 2. It is used to be declared an object of this class. 
        {

        }

        public string getString_PersonDetails()
         //This method will return a String which the person data.
        {
            string person_details = this.index + "," + this.name + "," + this.payrate + "," + this.email;
            return person_details;
        }

        public string[] getArray_PersonDetails()
        //This method will return an Array which the person data.
        {
            string[] person_details = { this.index, this.name, this.payrate, this.email };
            return person_details;
        }

        public void setPersonDetails(string data)
        //This method will Set or Update the data of person data.
        {
            string[] PersonDetails_Array = data.Split(',');
            this.index = PersonDetails_Array[0];
            this.name = PersonDetails_Array[1];
            this.payrate = PersonDetails_Array[2];
            this.email = PersonDetails_Array[3];
        }
    }
}
