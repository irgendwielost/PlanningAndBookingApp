using System;
using System.Data;
using System.Windows;
using MySqlConnector;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class Region
    {
        public Region(int id, string designation, bool deleted)
        {
            Id = id;
            Designation = designation;
            Deleted = deleted;
        }
        
        public int Id { get; set; }
        public string Designation { get; set; }
        public bool Deleted { get; set; }
        
        
        public static DataSet GetDataSetRegion()
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
                    var cmd = new MySqlDataAdapter($"SELECT * FROM Region", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "Region");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu dem Regionen abgerufen werden: \n" + e);
                }
            }
            return null;
        }
    }
}