using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class Season
    {
        public Season(int id, string designation, bool deleted)
        {
            Id = id;
            Designation = designation;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public string Designation { get; set; }
        public bool Deleted{ get; set; }


        //Functions

        //Get Dataset from db
        public static DataSet GetDataSetSaison()
        {
            using (var db = new Database())
            {
                try
                {
                    db.connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"table opening error{e}");
                    throw;
                }
                try
                {
                    var cmd = new MySqlDataAdapter("SELECT * FROM Saison", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "Saison");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu Saison abgerufen werden: " + e);
                }
            }
            return null;
        }


        //Get object by name
        public static Season GetSeasonByName(string name)
        {
            //Opening a connection to the database
            using (var db = new Database())
            {
                try
                {
                    db.connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"table opening error{e}");
                    throw;
                }
                try
                {
                    var cmd = new MySqlCommand($"SELECT * FROM Saison WHERE Bezeichnung = '{name}'", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Season(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Saison mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }



       
    }
}
