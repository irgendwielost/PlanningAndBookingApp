using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Forms;
using Buchungs_und_Planungssystem.Logic.Models;
using DataGrid = System.Windows.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Planning_and_Booking_System.Frontend.ViewModels.CentralControl
{
    public partial class HotelStatisticsTab : UserControl
    {
        public HotelStatisticsTab()
        {
            InitializeComponent();
            UpdateDataGrid(0);
            UpdateRegionCombo();
        }

        private int Location = 0;
        private double totalCosts = 0;
        private double totalWinnings = 0;
        private double totalOpsResult = 0;
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
        
        private void RegionComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = RegionCombo.SelectedValue;
            if (id != null)
            {
                if (Int32.Parse(id.ToString()) == 1)
                {
                    id = 0;
                }
                UpdateDataGrid(Int32.Parse(id.ToString()));
                CalculateTotals();
            }
            
        }
        //Update data in elements
        private void UpdateDataGrid(int id)
        {
            if (id == 0)
            {
                try
                {
                    var dataset = LocationOps.GetDataSetLocationOps();
                    HotelsDatagrid.ItemsSource = dataset.Tables["StandortBetrieb"].DefaultView;
                }
                catch
                {
                    MessageBox.Show("Es konnten keine Daten zu Standorten gefunden werden");
                }
            }
            else
            {
                try
                {
                    var dataset = LocationOps.GetDataSetLocationOpsByLocation(id);
                    HotelsDatagrid.ItemsSource = dataset.Tables["StandortBetrieb"].DefaultView;
                }
                catch
                {
                    MessageBox.Show("Es konnten keine Daten zu diesen Standorten gefunden werden");
                }
            }
        }

        private void CalculateTotals()
        {
            MessageBox.Show("Calculate");
            
            foreach (DataGridRow r in GetDataGridRows(HotelsDatagrid))
            {
                MessageBox.Show("Rows");
                foreach (DataGridColumn column in HotelsDatagrid.Columns)
                {
                    if (column.GetCellContent(r) is TextBlock)
                    {
                        TextBlock cellContent = column.GetCellContent(r) as TextBlock;
                        if (column.Header.ToString() == "Kosten")
                        {
                            if (cellContent != null) totalCosts += Double.Parse(cellContent.Text);
                        }

                        if (column.Header.ToString() == "Gewinn")
                        {
                            if (cellContent != null) totalWinnings += Double.Parse(cellContent.Text);
                        }

                        if (column.Header.ToString() == "Betriebsergebnis")
                        {
                            if (cellContent != null) totalOpsResult += Double.Parse(cellContent.Text);
                        }
                        MessageBox.Show(cellContent.Text);
                    }
                }
            }

            TotalCosts.Text = totalCosts.ToString(CultureInfo.InvariantCulture);
            TotalWinnings.Text = totalWinnings.ToString(CultureInfo.InvariantCulture);
            TotalOpsResult.Text = totalOpsResult.ToString(CultureInfo.InvariantCulture);
        }
        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }
    }
}