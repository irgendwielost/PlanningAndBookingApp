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
    public class Room
    {
        public Room(int id, string designation, bool deleted)
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
        public static DataSet GetDataSetRoom()
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
                    var cmd = new MySqlDataAdapter("SELECT * FROM Zimmer", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "Zimmer");
                    return dataset;
                }
                catch(Exception e)
                {
                    MessageBox.Show("Es konnten keine Zimmer abgerufen werden: " + e);
                }
            }
            return null;
        }

        //Get object by id
        public static Room GetRoomById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Zimmer WHERE Id = {id}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Room(id, reader.GetString(1), reader.GetBoolean(2));
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Es konnte kein Zimmer mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        //Get object by id
        public static Room GetRoomByName(string roomName)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Zimmer WHERE Bezeichnung = '{roomName}'", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Room(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein Zimmer mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }
    }
}
