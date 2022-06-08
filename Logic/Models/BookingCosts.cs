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
    public class BookingCosts
    {
        public BookingCosts(int bookingId, double costDriverCosts, double staffCosts, double total, bool deleted)
        {
            BookingId = bookingId;
            CostDriverCosts = costDriverCosts;
            StaffCosts = staffCosts;
            Total = total;
            Deleted = deleted;
        }

        public int BookingId { get; set; }
        public double CostDriverCosts { get; set; }
        public double StaffCosts { get; set; }
        public double Total { get; set; }
        public bool Deleted { get; set; }


        //Save roomprice
        public static void SaveBookingCosts(BookingCosts bookingCosts)
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
                BookingCosts bookingCostsObj = GetBookingCostsById(bookingCosts.BookingId);
                if (bookingCostsObj != null)
                {
                    try
                    {
                        var cmd = new MySqlCommand($"UPDATE BuchungKosten SET KostenstellenKosten = @costDriver, Personalkosten = @staff, " +
                            $"Gesamt = @total, Entfernt = @deleted)", db.connection);

                        cmd.Parameters.AddWithValue("@costDriver", bookingCostsObj.CostDriverCosts);
                        cmd.Parameters.AddWithValue("@staff", bookingCostsObj.StaffCosts);
                        cmd.Parameters.AddWithValue("@total", bookingCostsObj.Total);
                        cmd.Parameters.AddWithValue("@deleted", 0);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Fehler" + e);
                    }
                }
                else
                {
                    try
                    {
                        var cmd = new MySqlCommand($"INSERT INTO BuchungKosten (KostenstellenKosten, Personalkosten, Gesamt, Entfernt) " +
                            $"VALUES (@costDriver, @staff, @total, @deleted)", db.connection);
                        cmd.Parameters.AddWithValue("@costDriver", bookingCosts.CostDriverCosts);
                        cmd.Parameters.AddWithValue("@staff", bookingCosts.StaffCosts);
                        cmd.Parameters.AddWithValue("@total", bookingCosts.Total);
                        cmd.Parameters.AddWithValue("@deleted", 0);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Fehler" + e);
                    }
                }

            }
        }


        //Get object by ids
        public static BookingCosts GetBookingCostsById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM BuchungKosten WHERE BuchungId = {id}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new BookingCosts(id, reader.GetDouble(1), reader.GetDouble(4), reader.GetDouble(5), reader.GetBoolean(6));
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return null;
        }
    }
}
