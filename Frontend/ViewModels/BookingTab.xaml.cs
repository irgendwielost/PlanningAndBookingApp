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

namespace Buchungs_und_Planungssystem.Frontend.ViewModels
{
    /// <summary>
    /// Interaktionslogik für BookingTab.xaml
    /// </summary>
    public partial class BookingTab : UserControl
    {
        public BookingTab()
        {
            InitializeComponent();
            FillGuestCombo();
            FillRoomCombo();
            FillExpPackCombo();
            locationId = 1;
        }

        public bool wantExpPackage { get; set; }
        public int locationId { get; set; }


        public void Book()
        {
            //Get all values from input
            var guest = GuestCombo.SelectedValue;
            var room = RoomCombo.SelectedValue;
            var fellowTravs = FellowTravellers.Text;
            var arrival = ArrivalDate.SelectedDate;
            var departure = DepartureDate.SelectedDate;
            var expPack = ExpPackageCombo.SelectedValue;

            
            if(guest != null && room != null && fellowTravs != "" && arrival != null && departure != null)
            {
                if (arrival < departure)
                {
                    Stay.SaveStay(new Stay(0, (int)guest, Int32.Parse(fellowTravs),
                        locationId, (int)room, DateTime.Parse(arrival.ToString()),
                        DateTime.Parse(departure.ToString()), false));

                  
                    IEnumerable<DateTime> daysBetween = GetDaysBetweenDateTimes(DateTime.Parse(arrival.ToString()), DateTime.Parse(departure.ToString()));
                    foreach (var day in daysBetween)
                    {
                        Booking.SaveBooking(new Booking(0, locationId, day, 0, 1, 0, 0, false));

                        Booking bookingObj = Booking.GetBookingByDate(day);
                        int bookingId = bookingObj.Id;

                        BookingRoom.SaveBookingRoom(new BookingRoom(bookingId, (int)room, false));

                        if(ExpPackageCombo.IsEnabled == true && expPack != null)
                        {
                            ExperiencePackage expPackage = ExperiencePackage.GetPackById((int)expPack);
                            double commissionValue = expPackage.Commission * expPackage.Price;
                            BookingExperiencePackage.SaveExpPackage(new BookingExperiencePackage(bookingId, (int)expPack, commissionValue, false));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Das Abfahrts-Datum muss nach dem Ankunfts-Datum sein!");
                }
                
            }
            else
            {
                MessageBox.Show("Bitte überprüfen sie Ihre Angaben");
            }

        }

        public IEnumerable<DateTime> GetDaysBetweenDateTimes(DateTime start, DateTime end)
        {
            var newEnd = end.AddDays(+1);
            var days = newEnd - start;
            for (int i = 0; i < days.TotalDays; i++)
            {
                yield return start.AddDays(i);
            }
        }

        public void FillGuestCombo()
        {
            try
            {
                var dataset = Guest.GetDataSetGuest();
                GuestCombo.ItemsSource = dataset.Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Es konnten keine Gäste gefunden werden");
            }
        }

        public void FillRoomCombo()
        {
            try
            {
                var dataset = Room.GetDataSetRoom();
                RoomCombo.ItemsSource = dataset.Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Es konnten keine Zimmer gefunden werden");
            }
        }

        public void FillExpPackCombo()
        {
            try
            {
                var dataset = ExperiencePackage.GetDataSetExpPack();
                ExpPackageCombo.ItemsSource = dataset.Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Es konnten keine Erlebnispakete gefunden werden");
            }
        }

        private void BookRoom(object sender, RoutedEventArgs e)
        {
            Book();
        }
    }
}
