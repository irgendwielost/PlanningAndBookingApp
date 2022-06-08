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
    public class Location
    {
        public Location(int id, string designation, bool deleted)
        {
            Id = id;
            Designation = designation;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public string Designation { get; set; }
        public bool Deleted { get; set; }

        //Functions

        //Get Dataset from db
        public static DataSet GetDataSetLocation()
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
                    var cmd = new MySqlDataAdapter("SELECT * FROM Hotelstandorte", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "Hotelstandorte");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu den Hotelstandorten abgerufen werden: " + e);
                }
            }
            return null;
        }

        //Get object by name
        public static Location GetLocationByName(string name)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Hotelstandorte WHERE Bezeichnung = '{name}'", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Location(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein Hotelstandort mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        //Get object by id
        public static Location GetLocationById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Hotelstandorte WHERE ID = {id}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Location(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein Hotelstandort mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }
    }
}

