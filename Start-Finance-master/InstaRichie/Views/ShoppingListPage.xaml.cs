//Shopping List Page View for Start Finance
//Date last modified 06/04/2018
//@Author: Leon Wilson

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

namespace StartFinance.Views
{
    public sealed partial class ShoppingListPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ShoppingListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            ShopResults();
        }

        public void ShopResults()
        {
            conn.CreateTable<ShoppingList>();
            var query1 = conn.Table<ShoppingList>();
            ShoppingListView.ItemsSource = query1.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ShopResults();
        }

        private async void AddShopItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_ShopName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    double TempPriceQuote = Convert.ToDouble(PriceQuote.Text);
                    string CDay = _ShoppingDate.Date.Day.ToString();
                    string CMonth = _ShoppingDate.Date.Month.ToString();
                    string CYear = _ShoppingDate.Date.Year.ToString();
                    string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;
                    conn.CreateTable<ShoppingList>();
                    conn.Insert(new ShoppingList
                    {
                        ShopName = _ShopName.Text.ToString(),
                        NameOfItem = _NameOfItem.Text.ToString(),
                        ShoppingDate = FinalDate,
                        PriceQuoted = TempPriceQuote
                    });
                    // Creating table
                    ShopResults();
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
                else
                {
                    /// no idea
                }
            }
        }

        private async void DeleteShopItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((ShoppingList)ShoppingListView.SelectedItem).NameOfItem;
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("No  selected Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<ShoppingList>();
                    var query1 = conn.Table<ShoppingList>();
                    var query3 = conn.Query<ShoppingList>("DELETE FROM ShoppingList WHERE NameOfItem ='" + AccSelection + "'");
                    ShoppingListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No selected Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private async void EditShopItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelectionEdit = ((ShoppingList)ShoppingListView.SelectedItem).NameOfItem;
                if (AccSelectionEdit == "")
                {
                    MessageDialog dialog = new MessageDialog("No  selected Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    if (_NameOfItem.Text == "")
                    {
                        MessageDialog dialog = new MessageDialog("Name of item not entered", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        conn.CreateTable<ShoppingList>();
                        var query1 = conn.Table<ShoppingList>();
                        string SName = _ShopName.Text;
                        string IName = _NameOfItem.Text;
                        double PQuote = Convert.ToDouble(PriceQuote.Text);
                        string CDay = _ShoppingDate.Date.Day.ToString();
                        string CMonth = _ShoppingDate.Date.Month.ToString();
                        string CYear = _ShoppingDate.Date.Year.ToString();
                        string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;
                        var query3 = conn.Query<ShoppingList>("UPDATE ShoppingList SET ShopName ='" +
                                                                _ShopName.Text + "'" + ", NameOfItem ='" +
                                                                _NameOfItem.Text + "'" + ", PriceQuoted ='" +
                                                                PQuote + "'" + ", ShoppingDate ='" +
                                                                FinalDate + "'" + " WHERE NameOfItem ='" +
                                                                AccSelectionEdit + "'");
                        ShoppingListView.ItemsSource = query1.ToList();
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No selected Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }
    }
}
