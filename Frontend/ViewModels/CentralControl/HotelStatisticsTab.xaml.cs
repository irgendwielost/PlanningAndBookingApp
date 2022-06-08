using System.Windows;
using System.Windows.Controls;
using Buchungs_und_Planungssystem.Logic.Models;

namespace Planning_and_Booking_System.Frontend.ViewModels.CentralControl
{
    public partial class HotelStatisticsTab : UserControl
    {
        public HotelStatisticsTab()
        {
            InitializeComponent();
            UpdateDataGrid();
            UpdateRegionCombo();
        }
        
        private void UpdateRegionCombo()
        {
            try
            {
                var dataset = Region.GetDataSetRegion();
                RegionCombo.ItemsSource = dataset.Tables["Region"].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }
        
        
        //Update data in elements
        private void UpdateDataGrid()
        {
            try
            {
                var dataset = LocationOps.GetDataSetLocationOps();
                HotelsDatagrid.ItemsSource = dataset.Tables["StandortBetrieb"].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }
        
        //Update data in elements
        private void UpdateDataGridWithLocation()
        {
            try
            {
                var dataset = LocationOps.GetDataSetLocationOps();
                HotelsDatagrid.ItemsSource = dataset.Tables["StandortBetrieb"].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }
    }
}