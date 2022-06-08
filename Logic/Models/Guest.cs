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
    public class Guest
    {
        public Guest(int id, string name, DateTime birthday, string creditcard, bool deleted)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
            CreditCard = creditcard;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string CreditCard { get; set; }
        public bool Deleted { get; set; }


        //Functions
        public static DataSet GetDataSetGuest()
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
                    var cmd = new MySqlDataAdapter("SELECT * FROM Gast WHERE Entfernt = 0", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "Gast");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu Gästen abgerufen werden: \n" + e);
                }
            }
            return null;
        }

        //Get object by id
        public static Guest GetGuestById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Gast WHERE Id = {id}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Guest(id, reader.GetString(1), reader.GetDateTime(2), reader.GetString(3), reader.GetBoolean(4));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein Gast mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        //Create booking
        public static void SaveGuest(Guest guest)
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
                
                Guest guestObj = GetGuestById(guest.Id);
                if (guestObj != null)
                {
                    try
                    {
                        var cmd = new MySqlCommand($"UPDATE Gast SET GastName = @name, Geburtsdatum = @birthday," +
                                                   $" Kreditkarte = @creditcard WHERE Id = {guestObj.Id}", db.connection);
                        cmd.Parameters.AddWithValue("@name", guestObj.Name);
                        cmd.Parameters.AddWithValue("@birthday", guestObj.Birthday);
                        cmd.Parameters.AddWithValue("@creditcard", guestObj.CreditCard);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Fehler" + e);
                    }
                }
                else
                {
                    try
                    {
                        var cmd = new MySqlCommand($"INSERT INTO Gast (GastName, Geburtsdatum, Kreditkarte, Entfernt) " +
                             $"VALUES (@name, @birthday, @creditcard, @deleted)", db.connection);
                        cmd.Parameters.AddWithValue("@name", guest.Name);
                        cmd.Parameters.AddWithValue("@birthday", guest.Birthday);
                        cmd.Parameters.AddWithValue("@creditcard", guest.CreditCard);
                        cmd.Parameters.AddWithValue("@deleted", 0);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Fehler" + e);
                    }
                }


            }
        }

        //logical delete guest
        public static void DeleteGuest(Guest guest)
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
                    var cmd = new MySqlCommand($"UPDATE Gast SET Entfernt = 1 WHERE Id = {guest.Id}", db.connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Der Zimmerpreis konnte nicht entfernt werden: \n" + e);
                }
            }

        }
    }
}
