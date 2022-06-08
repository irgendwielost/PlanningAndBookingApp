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
    public class BookingCostDriver
    {
        public BookingCostDriver(int bookingId, int costDriverId, bool deleted)
        {
            BookingId = bookingId;
            CostDriverId = costDriverId;
            Deleted = deleted;
        }

        public int BookingId { get; set; }
        public int CostDriverId { get; set; }
        public bool Deleted { get; set; }



        //Get list by bookingId
        public static List<BookingCostDriver> GetCostDrivers(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM BuchungKostenstellen WHERE BuchungId = {id}", db.connection);

                    var reader = cmd.ExecuteReader();
                    List<BookingCostDriver> list = new List<BookingCostDriver>();
                    while (reader.Read())
                    {
                        list.Add(new BookingCostDriver(reader.GetInt32(0), reader.GetInt32(1), reader.GetBoolean(2)));
                    }
                    return list;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Kostenstelle zu dieser Buchung gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        //Get list by bookingId and cost driver id
        public static List<BookingCostDriver> GetCostDriversById(int cdId)
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
                    var cmd = new MySqlCommand($"SELECT * FROM BuchungKostenstellen WHERE KostenStellenId = {cdId}", 
                        db.connection);

                    var reader = cmd.ExecuteReader();
                    List<BookingCostDriver> list = new List<BookingCostDriver>();
                    while (reader.Read())
                    {
                        list.Add(new BookingCostDriver(reader.GetInt32(0), reader.GetInt32(1), reader.GetBoolean(2)));
                    }
                    return list;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Kostenstelle zu dieser Buchung gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }
    }
}
