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
    public class ExperiencePackage
    {
        public ExperiencePackage(int id, int locationId,string designation, double price, double commission, bool deleted)
        {
            Id = id;
            LocationId = locationId;
            Designation = designation;
            Price = price;
            Commission = commission;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Designation { get; set; }
        public double Price { get; set; }
        public double Commission{ get; set; }
        public bool Deleted { get; set; }



        //Get Dataset from db
        public static DataSet GetDataSetExpPack()
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
                    var cmd = new MySqlDataAdapter("SELECT * FROM Erlebnispakete WHERE Entfernt = 0", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "Erlebnispakete");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Erlebnispakete abgerufen werden: " + e);
                }
            }
            return null;
        }


        //Get object by id
        public static ExperiencePackage GetPackById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Erlebnispakete WHERE Id = {id}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new ExperiencePackage(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetDouble(3), 
                            reader.GetDouble(4), reader.GetBoolean(5));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein Erlebnispaket mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }
    }
}
