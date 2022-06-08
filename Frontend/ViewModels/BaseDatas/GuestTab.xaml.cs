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
    /// Interaktionslogik für GuestTab.xaml
    /// </summary>
    public partial class GuestTab : UserControl
    {
        public GuestTab()
        {
            InitializeComponent();
            UpdateDataGrid();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string CreditCard { get; set; }

        //On selection changed method
        private void GuestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selected item
            object item = GuestDataGrid.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Es ist kein gültiger Datensatz ausgewählt");
            }
            else
            {
                //Selected item | room
                var id = (GuestDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | season
                var name= (GuestDataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | location
                var birthday = (GuestDataGrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock)?.Text;

                //Selected item | price
                var creditcard = (GuestDataGrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock)?.Text;

                Id = Int32.Parse(id);
                Name = name;
                CreditCard = creditcard;
                BirthdayText.Text = birthday;
                NameText.Text = Name;
                CreditcardText.Text = CreditCard;
                Birthday = DateTime.Today;
            }

        }

        private void SaveGuest(object sender, RoutedEventArgs e)
        {
            Guest.SaveGuest(new Guest(Id, Name, Birthday, CreditCard, false));
        }

        private void DeleteGuest(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete");
        }
        private void UpdateDataGrid()
        {
            try
            {
                var dataset = Guest.GetDataSetGuest();
                GuestDataGrid.ItemsSource = dataset.Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Fehler");
            }
        }
    }
}
