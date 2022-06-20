using Buchungs_und_Planungssystem.Frontend.ViewModels;
using Buchungs_und_Planungssystem.Logic.Models;
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
using Planning_and_Booking_System.Frontend.ViewModels.CentralControl;

namespace Buchungs_und_Planungssystem
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow(int location, string username)
        {
            InitializeComponent();
            LocationId = location;
            Username.Text = username;
            CheckIfCentral();
        }

        private bool isCentral = false;
        int LocationId = 0;
        public void CheckIfCentral()
        {
            Location loca = Location.GetLocationById(LocationId);
            if (loca.Designation == "Zentrale")
            {
                MessageBox.Show("Zentralen Account");
                isCentral = true;
                BookRoomItem.Visibility = Visibility.Collapsed;
            }
        }
        
        //Contentcontrol
        BaseDataTab basedata = new BaseDataTab();
        Statistics statistics = new Statistics();
        BookingTab bookingTab = new BookingTab();
        HomeTab home = new HomeTab(1);
        //Central
        HotelStatisticsTab hotelStatisticsTab = new HotelStatisticsTab();
       
        private void SwitchToHotelStatistics()
        {
            if (isCentral)
            {
                ContentControl.Content = hotelStatisticsTab;
                TabName.Text = "Statistik";
            }
            else
            {
                ContentControl.Content = statistics;
            }
        }
        
        private void SwitchToBasedata()
        {
            ContentControl.Content = basedata;
            TabName.Text = "Stammdaten";
        }
        private void SwitchToHome()
        {
            ContentControl.Content = home;
            TabName.Text = "Home";
        }
       

        private void SwitchToBooking()
        {
            ContentControl.Content = bookingTab;
            TabName.Text = "Zimmer buchen";
        }


        private void ButtonCloseMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonMenuOpen.Visibility = Visibility.Visible;
            ButtonMenuClose.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonMenuClose.Visibility = Visibility.Visible;
            ButtonMenuOpen.Visibility = Visibility.Collapsed;
        }

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedItem = ListView.SelectedIndex;
            switch (selectedItem)
            {
                case 0:
                    SwitchToHome();
                    break;
                case 1:
                    SwitchToHotelStatistics();
                    break;
                case 2:
                    SwitchToBooking();
                    break;
                case 3:
                    SwitchToBasedata();
                    break;
                default:
                    ContentControl.Content = null;
                    break;
            }
        }
    }
}
