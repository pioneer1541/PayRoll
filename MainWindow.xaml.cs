using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;

namespace Payroll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddToList_Button_Click(object sender, RoutedEventArgs e)
        {
            AddToList();
        }

        private void SaveFile_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveDataFile();
        }

        private void LoadFile_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenSavedFile();
        }

        private void Clean_Button_Click(object sender, RoutedEventArgs e)
        {
            Clean_List();
        }

        private void GorssPay_Button_Click(object sender, RoutedEventArgs e)
        {
            SearchforPayRate();
        }

        private void WagesAfterTax_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeTaxCode();
        }

        private void AddToList()
        {
            try
            {
                PersonData person = new PersonData();
                //Declare an object from PersonData, in terms of PersonData Class, it could be store what user enters into TextBox Control and offer some methods.

                person.setPersonDetails(getUI_PersonDetails());
                //Get what user enters from UI and store it into properties of Class.

                if (Check_TextBox(person.index, 3, "Employee ID") && Check_TextBox(person.name, 0, "Name") && Check_TextBox(person.payrate, 1, "Pay rate") && Check_TextBox(person.email, 2, "Email"))
                //Check context what user enters is vaild through Check_TextBox method.

                {
                    string[] person_desc = { person.index, person.name, person.payrate, person.email };
                    //Declare an array as a value for add or change data to List .

                    PersonList list = new PersonList();
                    //Declare an object from PersonList Class,this class offer some methods to control List in UI and store some global variables 

                    Waga_Tax Rate_Data = new Waga_Tax();
                    //Declare an object from Wage_Tax Class,this class offer some methods to set and get value in its properties and store some global variables.

                    if (!list.Check_Key(person.index))
                    //According to Employment ID to decide whether to add new data to list or only change an existing data 
                    {
                        list.Add_NewDataArrary(person.index, person_desc);
                        // Add new data to List array.
                        employee_list.ItemsSource = list.getArrayForList();
                        //Update data for the list through set the ItemSource of the list. 

                        //******Part 2 solution******
                        Rate_Data.AddRateToDictionary(person.index, person.payrate);
                        //Add new rate into Dictionary.
                    }
                    else
                    {
                        list.Change_Data(person.index, person_desc);
                        employee_list.ItemsSource = list.getArrayForList();
                        //******Part 2 solution******
                        Rate_Data.ChangeRateToDictionary(person.index, person.payrate);
                        //Change new rate into Dictionary.
                    }
                    MessageBox.Show("Success!","Great!");
                }
            }
            
            catch(Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}","Error");
            }
        }

        private void Click_List(object sender, MouseButtonEventArgs e)
        //When user double click(WPF doesn't offer singel click event) the list.
        {
            string select_data = employee_list.SelectedItem.ToString();
            //Get which item is user selected.
            
            if (Person_File_Table.IsSelected) //When the user in the scene of Personal File
            {
                setUI_PersonDetails(select_data, 0); //Fill data into Personal File UI.
            }
            else if (Wages_Table.IsSelected) ////When the user in the scene of Wages
            {
                setUI_PersonDetails(select_data, 1); //Fill data into Wage UI.
            }

        }

        
        public void SaveDataFile()
        {
            try
            {
                StreamReader ReadFile = new StreamReader("PersonnelFile.txt");
                // Declare an object for read text file.

                List<string> Index_List = new List<string>();
                List<string> Value_List = new List<string>();
                //Declare two lists to store data.
                //Index_List is used to get whether the data which user want to store has existed in the file.
                //Value_List is used to store the data of specific Index.

                PersonList list = new PersonList();
                while (!ReadFile.EndOfStream)//read every single line to get data and store them into Index_List and Value_List.
                {
                    string x = ReadFile.ReadLine();
                    Index_List.Add(list.getIndexFromString(x));
                    //list.getIndexFromString(x) is a method which PersonList offerd,it could analysis and return the Index according to a whole string data.
                    Value_List.Add(x);
                }
                ReadFile.Close();

                foreach (string s in list.getArrayForList())
                // Traverse existing data of the list.
                {
                    string indexList_String = list.getIndexFromString(s);
                    //get its index string of the single line

                    if (!Index_List.Contains(indexList_String)) //if this a new data,just add it to the file.
                    {
                        StreamWriter WriteFile = new StreamWriter("PersonnelFile.txt", true);
                        WriteFile.AutoFlush = true;
                        WriteFile.WriteLine(s);
                        WriteFile.Close();
                    }
                    else //if this an old data, we need to replace it by new data..
                    {
                        string story = File.ReadAllText("PersonnelFile.txt");
                        //Declare a variable("story") to store the whole existing data.

                        string OldStr = Value_List[Index_List.IndexOf(indexList_String)];
                        //pick Oldstr up the to prepare to replace it.

                        story = story.Replace(OldStr, s); //replace OldStr by s(new data)
                        System.IO.File.WriteAllText(@"PersonnelFile.txt", string.Empty);
                        //Empty the whole old data in the file.

                        StreamWriter WriteFile = new StreamWriter("PersonnelFile.txt", true);
                        WriteFile.AutoFlush = true;
                        WriteFile.Write(story);
                        //Write the whole new data in the file

                        WriteFile.Close();
                    }

                    Waga_Tax Rate_Data = new Waga_Tax();
                    //Store the new data to Waga_Tax class since it needs these data to Check exist data.
                    //The benefit is does not have to read Data File every time when we want to get every person rate.
                    Rate_Data.AddRateToDictionary(s, Rate_Data.GetRateFromString(indexList_String, s));
                }
                MessageBox.Show("Success!");
            }

            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
            }

        }

        private void OpenSavedFile()
        {
            try
            {
                StreamReader ReadFile = new StreamReader("PersonnelFile.txt");
                PersonList list_data = new PersonList();
                employee_list.ItemsSource = list_data.getEmptyArray();
                //empty the list for new data list

                List<string> NewDataList = new List<string>();
                //This List is used to store new data come from the data file.
                //Do not use Array because we do not know how many data it needs to store.

                while (!ReadFile.EndOfStream)
                {
                    string data_string = ReadFile.ReadLine();
                    NewDataList.Add(data_string);
                }

                string[] NewData = NewDataList.ToArray();
                //convert List to Array, my class method based on Array as a parameter.

                Waga_Tax Rate_Data = new Waga_Tax();
                //As usual,the rate of every person will be stored into a Dictionary of Waga_Tax Class.

                if (list_data.getCountFromDictionary() > 0)
                    //when the number of the list item is not 0, we need to keep the data which user has already added to list 
                    //and add other existing data which is in the data file to list
                {

                    foreach (string n in list_data.getArrayForList())
                    // Firstly,Traverse existing data of the list.
                    //
                    {
                        string Index_New = list_data.getIndexFromString(n);
                        foreach (string o in NewData)
                        //Secondly, compare existing data with new data which came from the data file.
                        {
                            string Index_Old = list_data.getIndexFromString(o);
                            //analyze and get  the index of existing data through its string.
                            if (Index_New != Index_Old)
                            //when it is not an existing data, add it to Person List.
                            //if it is an existing data,we do not do anything,just keep what user enters.
                            {
                                list_data.Add_NewData(Index_Old, o);
                                Rate_Data.AddRateToDictionary(Index_Old, Rate_Data.GetRateFromString(Index_Old, o));
                            }
                        }
                    }
                }
                else
                // when the list is empty,we do not have to check data,just add new data to the Dictionary
                {
                    foreach (string d in NewDataList)
                    {
                        string index = Rate_Data.getIndexFromString(d);
                        list_data.Add_NewData(list_data.getIndexFromString(d), d);
                        Rate_Data.AddRateToDictionary(index, Rate_Data.GetRateFromString(index, d));
                    }
                }


                employee_list.ItemsSource = list_data.getArrayForList(); //updata ItemSource.
                ReadFile.Close();
                MessageBox.Show("Success!");
            }

            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
            }
        }

        private void SearchforPayRate()
        {
            try
            {
                if (Check_TextBox(wages_Employement_Hours.Text, 1, "Hours") && Check_TextBox(wages_Employement_Index.Text, 3, "Employee ID")) //call Check_TextBox to check whether context which user enters is valid.
                {
                    Waga_Tax Rate_Data = new Waga_Tax();
                    SearchRateInFile(); //loading from Data File
                    string em_id = wages_Employement_Index.Text; //get what index user enters.
                    if (Rate_Data.CheckID(em_id)) //if the index has existed in the data file. Get the data from the file to finish it.
                    {
                        decimal rate_num = Convert.ToDecimal(Rate_Data.GetRateFromDictionary(em_id));
                        decimal hours_num = Convert.ToDecimal(wages_Employement_Hours.Text);
                        decimal waga_num = Convert.ToDecimal(wages_Employement_Hours.Text) * rate_num;
                        showResult_Text.Text = "";
                        showResult_Text.AppendText($"Employee {em_id} Wage Details:" + Environment.NewLine);
                        showResult_Text.AppendText($"Pay rate is ${rate_num}" + Environment.NewLine);
                        showResult_Text.AppendText($"Working hours is {hours_num}" + Environment.NewLine);
                        showResult_Text.AppendText($"Wage is ${waga_num}");
                    }
                    else
                    {
                        MessageBox.Show($"Sorry,Can not find the ID.");
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
            }
        }

        private void SearchRateInFile()//this method will get rate from the data file and store it to a specific global Dictionary.
        {
            try
            {
                StreamReader ReadFile = new StreamReader("PersonnelFile.txt");
                Waga_Tax Rate_Data = new Waga_Tax();
                while (!ReadFile.EndOfStream)
                {
                    string x = ReadFile.ReadLine();
                    string index = Rate_Data.getIndexFromString(x);
                    if (!Rate_Data.CheckID(index))
                    {
                        Rate_Data.AddRateToDictionary(index, Rate_Data.GetRateFromString(index, x));
                    }
                }
                ReadFile.Close();
            }

            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
            }
        }



        private void MakeTaxCode()
        //this method will decide which level of Tax Code is suite for user.
        {
            try
            {
                if (Check_TextBox(wages_Employement_AfterTax.Text, 1, "Wage Before Tax"))
                    //Check the context is valid.
                {
                    decimal wage = decimal.Parse(wages_Employement_AfterTax.Text);
                    //convert the string to decimal.
                    string tax_code = ""; //this variable is used to store final result.
                    if (wage < 384)
                    {
                        tax_code = "A";
                    }
                    else if (wage >= 384 && wage < 671)
                    {
                        tax_code = "B";
                    }
                    else if (wage >= 671 && wage < 863)
                    {
                        tax_code = "C";
                    }
                    else if (wage >= 863 && wage < 1151)
                    {
                        tax_code = "D";
                    }
                    else if (wage >= 1151 && wage < 1534)
                    {
                        tax_code = "E";
                    }
                    else
                    {
                        tax_code = "F";
                    }

                    string[] TaxRate = SearchforTaxRate(tax_code);
                    //call SearchforTaxRate to get an Array for specific Tax Code.
                    //Because the initial data is an Array,I think we do not need to Write two methods to get it.

                    string tax_rate = TaxRate[1];
                    string tax_deduction = TaxRate[2];
                    decimal wage_initial = decimal.Parse(wages_Employement_AfterTax.Text);
                    decimal weekly_tax = wage_initial * decimal.Parse(tax_rate) - decimal.Parse(tax_deduction);
                    decimal new_wage = wage_initial - weekly_tax;
                    showResult_Text.Text = "";
                    showResult_Text.AppendText($"For wage before tax : {wage_initial}" + Environment.NewLine);
                    showResult_Text.AppendText($"Tax Code is : {tax_code}" + Environment.NewLine);
                    showResult_Text.AppendText($"Tax Rate is {tax_rate}" + Environment.NewLine);
                    showResult_Text.AppendText($"Tax Deduction is ${tax_deduction}" + Environment.NewLine);
                    showResult_Text.AppendText($"Weekly Tax is ${weekly_tax}" + Environment.NewLine);
                    showResult_Text.AppendText($"New wage after Tax is ${new_wage}" + Environment.NewLine);
                }
            }

            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
            }
        }

        private string[] SearchforTaxRate(string tax_code)
        //This method will return an specific Array base on different level.
        //It includes Rate and Deduction of this level Tax Code.
        //method 6 and metod 7 have been done in this method.
        {
            try
            {
                StreamReader ReadFile = new StreamReader("Taxtable.txt");
                Dictionary<string, string[]> RateTable = new Dictionary<string, string[]>();
                //Declare a Dictionary RateTable,Key is a index and Value is a Array.
                while (!ReadFile.EndOfStream)
                {
                    string data_string = ReadFile.ReadLine();
                    string[] rate_level = data_string.Split(',');
                    string index = rate_level[0];
                    RateTable.Add(index, rate_level);
                    //Store Rate and Deduction of different level Tax Code into different Array.
                }
                ReadFile.Close();
                return RateTable[tax_code];
                //there are Mehtod 6 and Method 7,they will return an array that include tax_deduction and tax_rate.
            }

            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
                throw e;
            }
            
        }

        public Boolean Check_TextBox(string Box_Text, int Check_Type, string Control_Name)
        // this method will check the context which the user enters based on different Check Model and give tips message box back to the user while it is invalid. 
        {
            try
            {
                if (Box_Text == "") //empty can not be accepted.
                {
                    MessageBox.Show($"{Control_Name} can not be empty");
                    return false;
                }
                else
                {
                    switch (Check_Type)
                    {
                        case 0: //only check the enter of user is not empty;
                            return true;
                        case 1://check the type of user-entered is number.
                            decimal num;
                            if (!decimal.TryParse(Box_Text, out num))
                            {
                                MessageBox.Show($"Only accept {Control_Name} type as number");
                                return false;
                            }
                            //*******Part 3 solution//*******
                            if (num < 0)
                            {
                                MessageBox.Show($"{Control_Name} must be grater than 0.");
                                return false;
                            }
                            else
                            {
                                return true;
                            }

                        case 2://Check the form of the email address,the Regular expression come from https://codereview.stackexchange.com/questions/55009/regex-validation-for-email-address
                            string Search_Pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                            Regex Search = new Regex(Search_Pattern);
                            if (!Search.IsMatch(Box_Text))
                            {
                                MessageBox.Show("The email address must follow the form of the Email address.");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        case 3: //******Part 1 solution******employee ID must be Integer Number.
                            int int_num;
                            if (!int.TryParse(Box_Text,out int_num))
                            {
                                MessageBox.Show($"{Control_Name} must be an Integer Number");
                                return false;
                            }
                            //*******Part 3 solution//*******
                            if (int_num < 0)
                            {
                                MessageBox.Show($"{Control_Name} must be grater than 0.");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                                
                        default:
                            return false;
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
                return false;
            }
        }

        private void setUI_PersonDetails(string data, int type)
            //this method will set and update person data to UI and property of the Class
        {
            try
            {
                if (type == 0) //type 0 is for the UI scene of Person File
                {
                    PersonData x = new PersonData();
                    x.setPersonDetails(data);
                    persFile_Employement_Index.Text = x.index;
                    persFile_Employement_Name.Text = x.name;
                    persFile_Employement_Rate.Text = x.payrate;
                    persFile_Employement_Email.Text = x.email;
                }
                else if (type == 1) //type 1 is for the UI scene of the Wages
                {
                    PersonData x = new PersonData();
                    x.setPersonDetails(data);
                    wages_Employement_Index.Text = x.index;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show($"Sorry,seems something wrong:{Environment.NewLine}{e}", "Error");
            }
        }

        private string getUI_PersonDetails()
        //this method will get and return the data as a string type which the user has already entered
        {
            return persFile_Employement_Index.Text + "," + persFile_Employement_Name.Text + "," + persFile_Employement_Rate.Text + "," + persFile_Employement_Email.Text;
        }

        private void Clean_List()
            //this method will clean the whole list.
        {
            PersonList person = new PersonList();
            employee_list.ItemsSource = person.getEmptyArray();
            //because I used the ItemsSource method, I have to clean the list by set a new empty Array.
            PersonList.data_Dictionary.Clear();
            MessageBox.Show("Success!");
        }
    }
}

