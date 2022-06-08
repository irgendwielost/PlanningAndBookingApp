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

namespace Buchungs_und_Planungssystem
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow(int location)
        {
            InitializeComponent();
            LocationId = location;
        }

        int LocationId = 0;
        //Contentcontrol
        BaseDataTab basedata = new BaseDataTab();
        Statistics statistics = new Statistics();
        BookingTab bookingTab = new BookingTab();
        HomeTab home = new HomeTab(1);

        public void SwitchTab(int index)
        {
            tab.SelectedIndex = index;
        }

        private void SwitchToBasedata(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = basedata;
            SwitchTab(3);

        }
        private void SwitchToHome(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = home;
            SwitchTab(0);

        }
        private void SwitchToStatistics(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = statistics;
            SwitchTab(1);
        }

        private void SwitchToBooking(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = bookingTab;
            SwitchTab(2);
        }


    }
}
