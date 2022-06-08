using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class Database :  IDisposable
    {
        
        public MySqlConnection connection { get; }
        
        //Function to connect to the database
        public Database()
        {
            try
            {
                connection = new MySqlConnection(
                    new MySqlConnectionStringBuilder
                    {
                        Server = "sql11.your-server.de",
                        UserID = "facolo",
                        Password = "R51pLqUlS0rJlaY9",
                        Database = "hotelplanning"
                    }.ConnectionString);
                connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Es konnte keine Verbindung zum Internet hergestellt werden!");
                throw;
            }

            connection.Close();
        }

        //Function to execute a query
        public void ExecuteQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        //Function to return a DataTable
        public DataTable ReturnDataTable(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}

