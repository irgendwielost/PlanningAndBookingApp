using System;
using System.Data;
using System.Windows;
using MySqlConnector;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class RegionLocation
    {
        public RegionLocation(int locationId, int regionId)
        {
            LocationId = locationId;
            RegionId = regionId;
        }
        
        public int LocationId { get; set; }
        public int RegionId { get; set; }
        
        
        //Functions
        public static RegionLocation GetRegionLocationByLocation(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM RegionStandort WHERE RegionId = {id}", 
                        db.connection);
                    
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new RegionLocation(reader.GetInt32(0), id);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu dem Regions-Standort abgerufen werden: \n" + e);
                }
            }
            return null;
        }
    }
}