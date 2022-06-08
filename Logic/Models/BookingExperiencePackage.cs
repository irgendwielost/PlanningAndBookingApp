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
    public class BookingExperiencePackage
    {
        public BookingExperiencePackage(int bookingId, int expPackageId, double commission, bool deleted)
        {
            BookingId = bookingId;
            ExpPackageId = expPackageId;
            Commission = commission;
            Deleted = deleted;
        }

        public int BookingId { get; set; }
        public int ExpPackageId { get; set; }
        public double Commission { get; set; }
        public bool Deleted { get; set; }


        //Get object by bookingId
        public static List<BookingExperiencePackage> GetBookingExpPack(int id)
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

                    var cmd = new MySqlCommand($"SELECT * FROM BuchungErlebnispakete WHERE BuchungId = {id}", db.connection);

                    var reader = cmd.ExecuteReader();
                    List<BookingExperiencePackage> list = new List<BookingExperiencePackage>();
                    while (reader.Read())
                    {
                        list.Add(new BookingExperiencePackage(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2),reader.GetBoolean(3)));
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

        //Save experiencePackage
        public static void SaveExpPackage(BookingExperiencePackage expPack)
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
                    var cmd = new MySqlCommand($"INSERT INTO BuchungErlebnisPakete (BuchungId, ErlebnisPaketId, Provision, Entfernt) " +
                        $"VALUES (@booking, @expPack, @commission, @deleted)", db.connection);
                    cmd.Parameters.AddWithValue("@booking", expPack.BookingId);
                    cmd.Parameters.AddWithValue("@expPack", expPack.ExpPackageId);
                    cmd.Parameters.AddWithValue("@commission", expPack.Commission);
                    cmd.Parameters.AddWithValue("@deleted", 0);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte kein ErlebnisPaket der Buchung hinzugefügt werden");
                }


            }
        }
    }
}
