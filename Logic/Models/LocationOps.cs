using System;
using System.Data;
using System.Windows;
using MySqlConnector;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class LocationOps
    {
        public LocationOps(int locationId, double costs, double winnings, double opsResult)
        {
            LocationId = locationId;
            Costs = costs;
            Winnings = winnings;
            OpsResult = opsResult;
        }
        
        public int LocationId { get; set; }
        public double Costs { get; set; }
        public double Winnings { get; set; }
        public double OpsResult { get; set; }
        
        
        //Functions
        public static DataSet GetDataSetLocationOps()
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
                    var cmd = new MySqlDataAdapter($"SELECT StandortBetrieb.*, l.Bezeichnung as LocationName " +
                                                   "FROM StandortBetrieb join Hotelstandorte l on StandortBetrieb.StandortId = l.ID", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "StandortBetrieb");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu dem Standortbetrieb abgerufen werden: \n" + e);
                }
            }
            return null;
        }
        
        
        public static DataSet GetDataSetLocationOpsByLocation(int id)
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
                    var cmd = new MySqlDataAdapter($"SELECT StandortBetrieb.*, l.Bezeichnung as LocationName " +
                                                   $"FROM StandortBetrieb join Hotelstandorte l on StandortBetrieb.StandortId " +
                                                   $"= l.ID WHERE StandortId = {id}", 
                        db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "StandortBetrieb");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu dem Standortbetrieb abgerufen werden: \n" + e);
                }
            }
            return null;
        }
    }
}