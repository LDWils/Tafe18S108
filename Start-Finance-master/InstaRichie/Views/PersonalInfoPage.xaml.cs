using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalInfoPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public PersonalInfoPage()
        {
            this.InitializeComponent();
            savebtn.Visibility = Visibility.Collapsed;
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }


        public void Results()
        {
            // Creating table
            conn.CreateTable<PersonalInfo>();

            /// Refresh Data
            var query = conn.Table<PersonalInfo>();

           PersonalInfoView.ItemsSource = query.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

       

        public string FinalDate()
        {
            string CDay = dobTxt.Date.Value.Day.ToString();
            string CMonth = dobTxt.Date.Value.Month.ToString();
            string CYear = dobTxt.Date.Value.Year.ToString();
            string EndDate = "" + CDay+ "/" + CMonth + "/" + CYear;

            return EndDate;
        }
        private async void AddInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (fNameTxt.Text.ToString() == "" || lNameTxt.Text.ToString() == "" || emailTxt.Text.ToString() == "" ||
                    phoneTxt.Text.ToString() == "" || dobTxt.Date.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("All fields must be entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new PersonalInfo()
                    {
                        FirstName = fNameTxt.Text.ToString(),
                        LastName = lNameTxt.Text.ToString(),
                        Email = emailTxt.Text.ToString(),
                        Phone = phoneTxt.Text.ToString(),
                        DOB = FinalDate(),
                        Gender = genderTxt.Text.ToString()
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog(" Entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("A Similar person  already exists, Try a different td", "Oops..!");
                    await dialog.ShowAsync();
                }
            }


        }


        private async void DeleteInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string listSeleccted = ((PersonalInfo)PersonalInfoView.SelectedItem).FirstName;
                int idSelected = ((PersonalInfo)PersonalInfoView.SelectedItem).PersonalID;
                if (listSeleccted == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the List", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<PersonalInfo>();
                    var query1 = conn.Table<PersonalInfo>();
                    var query3 = conn.Query<PersonalInfo>("DELETE FROM PersonalInfo WHERE PersonalID ='" + idSelected + "'");
                    PersonalInfoView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog(" Info Not selected from list", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        int selected;

        private  void AcceptChange_Click(object sender, RoutedEventArgs e)
        {
            int id = ((PersonalInfo)PersonalInfoView.SelectedItem).PersonalID;
            string first = fNameTxt.Text;
            string last = lNameTxt.Text;
            string mail = emailTxt.Text;
            string num = phoneTxt.Text;
            string bday = FinalDate();
            string gen = genderTxt.Text;


            var queryedit = conn.Query<PersonalInfo>("UPDATE PersonalInfo SET FirstName = '" + first +
                          "', LastName = '" + last + "', Email = '" + mail + "', Phone = '" + num +
                          "', Gender = '" + gen + "', DOB = '" + bday + "' WHERE PersonalID = '" + id + "'");
            Results();
            editbtn.Visibility = Visibility.Visible;
            savebtn.Visibility = Visibility.Collapsed;



        }

        private async void EditInfo_Click(object sender, RoutedEventArgs e)
        {
            
            if (PersonalInfoView.SelectedIndex == -1)
            {
                MessageDialog dialog = new MessageDialog("No selected event", "Oops..!");
                await dialog.ShowAsync();
            }
            else
            {
                selected = ((PersonalInfo)PersonalInfoView.SelectedItem).PersonalID; //Store ID to be able to update data.
                fNameTxt.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).FirstName;
                lNameTxt.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).LastName;
                emailTxt.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).Email;
                phoneTxt.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).Phone;
                genderTxt.Text = ((PersonalInfo)PersonalInfoView.SelectedItem).Gender;
                //  dobTxt.Text = FinalDate;
               // string finDate = FinalDate();
              //  dobTxt. = Convert.ToInt32(finDate.Substring(3, 4);
              

                editbtn.Visibility = Visibility.Collapsed;
                savebtn.Visibility = Visibility.Visible;
            }
        }


    }
}
