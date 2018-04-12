
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AppointmentPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            Results();
        }


        public void Results()
        {
            conn.CreateTable<Appointment>();
            var query = conn.Table<Appointment>();
            AppoingmentListView.ItemsSource = query.ToList();
        }


        private async void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CDay = DPEDate.Date.Day.ToString();
                string CMonth = DPEDate.Date.Month.ToString();
                string CYear = DPEDate.Date.Year.ToString();
                string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;


                string CTime = TPTime.Time.ToString();

                if (TBEName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Event Name");
                    await dialog.ShowAsync();
                }
                else if (TBLocation.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Location");
                    await dialog.ShowAsync(); ;
                }
                else if (DPEDate == null)
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Choose The Date");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new Appointment()
                    {
                        EventName = TBEName.Text,
                        Location = TBLocation.Text,
                        EventDate = FinalDate,
                        EndTime = CTime
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid Amount", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Wish Name already exist, Try Different Name", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AccSelection = ((Appointment)AppoingmentListView.SelectedItem).AppointmentID;
                if (AccSelection == 0)
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Appointment>();
                    var query1 = conn.Table<Appointment>();
                    string _eventName = TBEName.Text;
                    string _Location = TBLocation.Text;
                    string CDay = DPEDate.Date.Day.ToString();
                    string CMonth = DPEDate.Date.Month.ToString();
                    string CYear = DPEDate.Date.Year.ToString();
                    string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;
                    string CTime = TPTime.Time.ToString();
                    var query3 = conn.Query<Appointment>("UPDATE Appointment SET EventName ='" + TBEName.Text + "'" + ", Location ='" +
                                                                    TBLocation.Text + "'" + ", EventDate ='" +
                                                                    FinalDate + "'" + ", EndTime ='" +
                                                                    CTime + "'" + " WHERE AppointmentID ='" +
                                                                    AccSelection + "'");
                    AppoingmentListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private async void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            //  var query3 = conn.Query<Appointment>("DROP TABLE Appointment");
            try
            {
                string AccSelection = ((Appointment)AppoingmentListView.SelectedItem).EventName;

                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Appointment>();
                    var query1 = conn.Table<Appointment>();
                    var query3 = conn.Query<Appointment>("DELETE FROM Appointment WHERE EventName ='" + AccSelection + "'");
                    AppoingmentListView.ItemsSource = query1.ToList();
                }
              
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }
    }
}
