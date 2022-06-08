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
    public class BookingRoom
    {
        public BookingRoom(int bookingId, int roomId, bool deleted)
        {
            BookingId = bookingId;
            RoomId = roomId;
            Deleted = deleted;
        }

        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public bool Deleted { get; set; }


        //Get object by bookingId
        public static List<BookingRoom> GetBookingRoom(int id)
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
                    
                    var cmd = new MySqlCommand($"SELECT * FROM BuchungZimmer WHERE BuchungId = {id}", db.connection);

                    var reader = cmd.ExecuteReader();
                    List<BookingRoom> list = new List<BookingRoom>();
                    while (reader.Read())
                    {
                        list.Add(new BookingRoom(reader.GetInt32(0), reader.GetInt32(1), reader.GetBoolean(2)));
                    }
                    return list;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein gebuchtes Zimmer mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        //Save roomprice
        public static void SaveBookingRoom(BookingRoom bookingRoom)
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
                        var cmd = new MySqlCommand($"INSERT INTO BuchungZimmer (BuchungId, ZimmerId, Entfernt) " +
                                                   $"VALUES (@booking, @room, @deleted)", db.connection);
                        cmd.Parameters.AddWithValue("@booking", bookingRoom.BookingId);
                        cmd.Parameters.AddWithValue("@room", bookingRoom.RoomId);
                        cmd.Parameters.AddWithValue("@deleted", 0);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Es konnte kein Zimmer der Buchung hinzugefügt werden");
                    }
                

            }
        }
    }
}
