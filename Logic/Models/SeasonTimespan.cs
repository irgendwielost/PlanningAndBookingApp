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
    public class SeasonTimespan
    {
        public SeasonTimespan(int seasonId, DateTime start, DateTime end, bool deleted)
        {
            SeasonId = seasonId;
            Start = start;
            End = end;
            Deleted = deleted;
        }
        
        public int SeasonId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Deleted { get; set; }



        //Get object by timespan
        public static SeasonTimespan GetSeasonByDay(DateTime? date)
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

                    var cmd = new MySqlCommand($"SELECT * FROM SaisonZeitraum WHERE Beginn >= @date", db.connection);
                    cmd.Parameters.AddWithValue("@date", date.ToString().Replace('.', '/'));
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new SeasonTimespan(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2), reader.GetBoolean(3));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Saison mit diesem Datum gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        //Get object by timespan
        public static SeasonTimespan GetSeasonByTimeSpan(DateTime? start, DateTime end)
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

                    var cmd = new MySqlCommand($"SELECT * FROM SaisonZeitraum WHERE Beginn >= @start", db.connection);
                    cmd.Parameters.AddWithValue("@start", start.ToString().Replace('.', '/'));
                    cmd.Parameters.AddWithValue("@end", end.ToString().Replace('.', '/'));
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new SeasonTimespan(2, reader.GetDateTime(1), reader.GetDateTime(2), reader.GetBoolean(3));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Saison mit diesem Datum gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }
    }
}
