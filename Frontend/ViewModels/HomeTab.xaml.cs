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
    /// Interaktionslogik für HomeTab.xaml
    /// </summary>
    public partial class HomeTab : UserControl
    {
        public HomeTab(int location)
        {
            InitializeComponent();
            UpdateDataGrid();
            LocationId = location;
        }

        public DateTime Date { get; set; }
        public DateTime Today { get; set; }
        int Planned = 0;
        int Booked = 0;
        int IsAmount = 0;
        int BookingId = 0;
        int LocationId = 0;
        //Update data in elements
        private void UpdateDataGrid()
        {
            try
            {
                var dataset = Booking.GetDataSetBooking();
                BookingsDataGrid.ItemsSource = dataset.Tables["Buchung"].DefaultView;
            }
            catch(Exception e)
            {
                MessageBox.Show("Es konnten keine Daten zu den Buchungen abgerufen werden" + e);
            }
        }


        public void SaveBooking(object sender, RoutedEventArgs e)
        {
            if(PlannedText.Text != "")
            {
                IsAmount = Int32.Parse(isAmountText.Text);
                Booking.CreateBooking(new Booking(BookingId, LocationId, Date, Planned, Booked, IsAmount, 0, false));
            }
            else
            {
                MessageBox.Show("Überprüfen Sie Ihre Angaben");
            }
            
        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = (DateTime)DatePick.SelectedDate;
            Today = DateTime.Today;

            if(Date > Today)
            {
                isAmountLabel.IsEnabled = false;
                isAmountText.IsEnabled = false;
            }
            else
            {
                isAmountLabel.IsEnabled = true;
                isAmountText.IsEnabled = true;
            }
        }


        private void BookingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selected item
            object item = BookingsDataGrid.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Es ist kein gültiger Datensatz ausgewählt");
            }
            else
            {
                //Selected item | id
                string id = (BookingsDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | planned
                string planned= (BookingsDataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | booked
                string booked = (BookingsDataGrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | isAmount
                string isAmount = (BookingsDataGrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock)?.Text;

                
                
                if(id != "")
                {
                    if(planned != "" && isAmount != "" && booked != "")
                    {
                        PlannedText.Text = planned;
                        isAmountText.Text = isAmount;
                        Planned = Int32.Parse(planned);
                        Booked = Int32.Parse(booked);
                        IsAmount = Int32.Parse(isAmount);
                    }
                    
                    BookingId = Int32.Parse(id);
                    Booking booking = Booking.GetBookingById(Int32.Parse(id));
                    if (booking != null)
                    {
                        DatePick.SelectedDate = booking.Date;
                    }

                    
                }
            }
        }
    }
}
