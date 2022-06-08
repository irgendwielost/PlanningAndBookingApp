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

namespace Buchungs_und_Planungssystem.Frontend.ViewModels
{
    /// <summary>
    /// Interaktionslogik für BaseDataTab.xaml
    /// </summary>
    public partial class BaseDataTab : UserControl
    {
        public BaseDataTab()
        {
            InitializeComponent();
        }

        RoomPrices roomPrices = new RoomPrices();
        GuestTab guest = new GuestTab();
        private void SwitchToRoomPrices(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = roomPrices;
        }

        private void SwitchToGuest(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = guest;
        }
    }
}
