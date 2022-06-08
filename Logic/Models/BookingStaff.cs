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
    public class BookingStaff
    {
        public BookingStaff(int staffId, int bookingId, bool deleted)
        {
            StaffId = staffId;
            BookingId = bookingId;
            Deleted = deleted;
        }

        public int StaffId { get; set; }
        public int BookingId { get; set; }
        public bool Deleted { get; set; }


        //Get list by bookingId
        public static List<BookingStaff> GetBookingStaff(int id)
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

                    var cmd = new MySqlCommand($"SELECT * FROM BuchungPersonal WHERE BuchungId = {id}", db.connection);

                    var reader = cmd.ExecuteReader();
                    List<BookingStaff> list = new List<BookingStaff>();
                    while (reader.Read())
                    {
                        list.Add(new BookingStaff(reader.GetInt32(0), reader.GetInt32(1), reader.GetBoolean(2)));
                    }
                    return list;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein Personal zu dieser Buchung gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }
    }
}
