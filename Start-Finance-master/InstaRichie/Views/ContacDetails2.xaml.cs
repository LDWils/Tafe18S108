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
using StartFinance.ViewModels;
using Windows.UI.Popups;
using SQLite.Net;
using StartFinance.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContacDetails2 : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        public ContacDetails2()
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
            // Creating table
            conn.CreateTable<ContactDteails2>();
            var query = conn.Table<ContactDteails2>();
            ContacDetailList.ItemsSource = query.ToList();
            // TransactionList.ItemsSource = query.ToList();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (txtContactID.Text == "")
                {
                    MessageDialog myDialo = new MessageDialog("Provide your ID");
                }
                // checks if account name is null
                else if (txtFirstName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Your firstName is not Entered :Oops..!");
                    await dialog.ShowAsync();
                }
                if (txtLasttName.Text.ToString() == "")
                {
                    MessageDialog variableerror = new MessageDialog("Enter your LastName", "Oops..!");
                }
                if (txtCompanyName.Text == "")
                {
                    MessageDialog variableerror = new MessageDialog("Enter your LastName", "Oops..!");
                }
                else
                {   // Inserts the data
                    conn.CreateTable<ContactDteails2>();
                    conn.Insert(new ContactDteails2
                    {
                        //ContactID = int.Parse(txtContactID.Text.ToString()),
                        FirstName = txtFirstName.Text.ToString(),
                        LastName = txtLasttName.Text.ToString(),
                        CompanyName = txtCompanyName.Text.ToString(),
                        MobilePhone = txtMobilePhone.Text.ToString(),
                    });
                    Results();

                    txtFirstName.Text = "";

                    txtLasttName.Text = "";

                    txtCompanyName.Text = "";

                    txtMobilePhone.Text = "";


                }
            }
            catch (Exception ex)
            {   // Exception to display when amount is invalid or not numbers
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }   // Exception handling when SQLite contraints are violated
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Account Name already exist, Try Different Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }

            }

        }


        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            // checks if data is null else inserts
            try
            {
                int selection = ((ContactDteails2)ContacDetailList.SelectedItem).ContactID;
                if (selection < 0)
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<ContactDteails2>();
                    var query1 = conn.Table<ContactDteails2>();
                    var query3 = conn.Query<ContactDteails2>("DELETE FROM ContactDteails2 WHERE ContactID = " + selection);
                    ContacDetailList.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog ClearDialog = new MessageDialog("Please select the item to Delete", "Oops..!");
                await ClearDialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void AppBarEditButton_Click(object sender, RoutedEventArgs e)
        {
           // string Acountselection = "";

            try
            {

              string   Acountselection = ((ContactDteails2)ContacDetailList.SelectedItem).ContactID.ToString() ;
                ContactDteails2 myContact = ContacDetailList.SelectedItem as ContactDteails2;
                    txtFirstName.Text = myContact.FirstName;
                txtLasttName.Text = myContact.LastName;
                txtCompanyName.Text = myContact.CompanyName;
                txtMobilePhone.Text = myContact.MobilePhone;
               
                if (Acountselection == "")
                {
                    var message = new MessageDialog("Select Item to edit");
                    await message.ShowAsync();
                    return;

                }
                else
                {
                    conn.CreateTable<ContactDteails2>();
                    var quiry1 = conn.Table<ContactDteails2>();
                    //  var quiry2 = conn.Query<ContactDteails2>("EDIT FROM ContactDitails2 WHERE ContactID=" + Acountselection);
                    conn.Update(new ContactDteails2
                    {
                        FirstName = txtFirstName.Text.ToString(),
                        LastName = txtLasttName.Text.ToString(),
                        CompanyName = txtCompanyName.Text.ToString(),
                        MobilePhone = txtMobilePhone.Text.ToString(),
                        

                    });
                    

                    
                }


                }

               catch (Exception)
               {
                var Messaage = new MessageDialog("Select Item");
                await Messaage.ShowAsync();
                return;
               }
         
          
                
  
            }

    





        //    if (ContacDetailList.SelectedIndex > -1)

        //    {

        //        //btnUpdate.Visibility = Visibility.Visible;


        //       // int ContactID = ((ContacDetails2)ContacDetailList.SelectedItem).t;

        //        //string firstName = ((ContacDetails2)ContacDetailList.SelectedItem).FirstName;

        //        //string lastName = ((ContacDetails2)ContacDetailList.SelectedItem).LastName;

        //        //string companyName = ((ContacDetails2)ContacDetailList.SelectedItem).CompanyName;

        //        //string phone = ((ContacDetails2)ContacDetailList.SelectedItem).Phone;






        //        txtFirstName.Text = "";

        //       txtLasttName.Text = "";

        //        txtCompanyName.Text = "";

        //        txtMobilePhone.Text = "";

        //    }

        //    else

        //    {

        //        MessageDialog dialog = new MessageDialog("Not selected");

        //        await dialog.ShowAsync();

        //    }

        //}
    }
    }
    

