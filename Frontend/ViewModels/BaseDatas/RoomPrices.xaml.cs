using Buchungs_und_Planungssystem.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaktionslogik für RoomPrices.xaml
    /// </summary>
    public partial class RoomPrices : UserControl
    {
        public RoomPrices()
        {
            InitializeComponent();
            UpdateDataGrid();
            UpdateRoomCombo();
            UpdateSeasonCombo();
            UpdateLocationCombo();
        }

        public string RoomName { get; set; }
        public string SeasonName { get; set; }
        public string LocationName { get; set; }

        //On selection changed method
        private void RoompricesDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selected item
            object item = RoompricesDatagrid.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Es ist kein gültiger Datensatz ausgewählt");
            }
            else
            {
                //Selected item | room
                string room = (RoompricesDatagrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | season
                string season = (RoompricesDatagrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | location
                string location = (RoompricesDatagrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | price
                string price = (RoompricesDatagrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock)?.Text;


                Room roomObj = Room.GetRoomByName(room);
                Season seasonObj = Season.GetSeasonByName(season);
                Location locationObj = Location.GetLocationByName(location);

                if(roomObj != null)
                {
                    RoomCombo.SelectedValue = roomObj.Id;
                }
                if(seasonObj != null)
                {
                    SeasonCombo.SelectedValue = seasonObj.Id;
                }
                if (locationObj != null)
                {
                    LocationCombo.SelectedValue = locationObj.Id;
                }
                if(price != "")
                {
                    PriceText.Text = price;
                }
            }
            
        }


        //Update data in elements
        private void UpdateDataGrid()
        {
            try
            {
                var dataset = RoomPrice.GetDataSet();
                RoompricesDatagrid.ItemsSource = dataset.Tables["ZimmerSaisonPreis"].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }

        private void UpdateRoomCombo()
        {
            try
            {
                var dataset = Room.GetDataSetRoom();
                RoomCombo.ItemsSource = dataset.Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }

        private void UpdateSeasonCombo()
        {
            try
            {
                var dataset = Season.GetDataSetSaison();
                SeasonCombo.ItemsSource = dataset.Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }

        private void UpdateLocationCombo()
        {
            try
            {
                var dataset = Location.GetDataSetLocation();
                LocationCombo.ItemsSource = dataset.Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }

        private void SaveRoomPrice(object sender, RoutedEventArgs e)
        {
            //Values
            var roomId = RoomCombo.SelectedValue.ToString();
            var seasonId = SeasonCombo.SelectedValue.ToString();
            var locationId = LocationCombo.SelectedValue.ToString();
            var price = PriceText.Text;

            RoomPrice.SaveRoomPrice(new RoomPrice(Int32.Parse(roomId), Int32.Parse(seasonId), Int32.Parse(locationId), double.Parse(price), false));
            
            RoompricesDatagrid.SelectedItem = null;
            UpdateDataGrid();
        }

        private void DeleteRoomPrice(object sender, RoutedEventArgs e)
        {
            //Values
            var roomId = RoomCombo.SelectedValue.ToString();
            var seasonId = SeasonCombo.SelectedValue.ToString();
            var locationId = LocationCombo.SelectedValue.ToString();
            var price = 0;

            RoomPrice.DeleteRoomPrice(new RoomPrice(Int32.Parse(roomId), Int32.Parse(seasonId), Int32.Parse(locationId), price, true));
            Thread.Sleep(2000);
            RoompricesDatagrid.SelectedItem = null;
            UpdateDataGrid();
        }
    }
}
