using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class Staff
    {
        public Staff(int id, string name, int positionId, double wage, string password, int locationId, bool deleted)
        {
            Id = id;
            Name = name;
            PositionId = positionId;
            Wage = wage;
            Password = password;
            LocationId = locationId;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public double Wage { get; set; }
        public string Password { get; set; }
        public int LocationId { get; set; }
        public bool Deleted { get; set; }


        //Get object by id
        public static Staff StaffLogIn(string name, string password)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Personal " +
                                               $"WHERE PersonalName = '{name}' AND Passwort = '{password}'", 
                        db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Staff(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDouble(3),
                            reader.GetString(4), reader.GetInt32(5), reader.GetBoolean(6));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Bitte Benutzername und Kennwort überprüfen\n" + e);
                    return null;
                }
            }
            return null;
        }

        //Get object by id
        public static Staff GetStaffById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Personal WHERE Id = {id}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Staff(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDouble(3), 
                            reader.GetString(4), reader.GetInt32(5), reader.GetBoolean(6));
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
