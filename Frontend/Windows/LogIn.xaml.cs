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
using System.Windows.Shapes;

namespace Buchungs_und_Planungssystem.Frontend.Windows
{
    /// <summary>
    /// Interaktionslogik für LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        public string Username { get; set; }
        public string Password { get; set; }

        private void LogUserIn(object sender, RoutedEventArgs e)
        {
            Username = UsernameText.Text;
            Password = PasswordText.Password;

            Staff user = Staff.StaffLogIn(Username, Password);
            if(user != null)
            {
                MainWindow main = new MainWindow(user.LocationId);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte Benutzername und Password überprüfen");
            }
        }
    }
}
