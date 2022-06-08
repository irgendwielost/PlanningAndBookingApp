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
    public class Stay
    {
        public Stay(int id, int guestId, int fellowTravs, int locationId, int roomId, DateTime arrival, DateTime departure, bool deleted)
        {
            Id = id;
            GuestId = guestId;
            FellowTravs = fellowTravs;
            LocationId = locationId;
            RoomId = roomId;
            Arrival = arrival;
            Departure = departure;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public int GuestId { get; set; }
        public int FellowTravs{ get; set; }
        public int LocationId{ get; set; }
        public int RoomId { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public bool Deleted { get; set; }

        //Save roomprice
        public static void SaveStay(Stay stay)
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
                    var cmd = new MySqlCommand($"INSERT INTO Aufenthalt (GastId, Mitreisende, StandortId, ZimmerId, Anreise, Abreise, Entfernt) " +
                                               $"VALUES (@guest, @fellowTravs, @location, @room, @arrival, @departure, @deleted)", db.connection);
                    cmd.Parameters.AddWithValue("@guest", stay.GuestId);
                    cmd.Parameters.AddWithValue("@fellowTravs", stay.FellowTravs);
                    cmd.Parameters.AddWithValue("@location", stay.LocationId);
                    cmd.Parameters.AddWithValue("@room", stay.RoomId);
                    cmd.Parameters.AddWithValue("@arrival", stay.Arrival);
                    cmd.Parameters.AddWithValue("@departure", stay.Departure);
                    cmd.Parameters.AddWithValue("@deleted", stay.Deleted);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Fehler" + e);
                }
                
            }
        }
    }
}
