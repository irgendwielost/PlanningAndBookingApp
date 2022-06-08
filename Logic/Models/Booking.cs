using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class Booking
    {
        public Booking(int id, int locationId, DateTime date, int planned, int booked, int isCount, double revenue, bool deleted)
        {
            Id = id;
            LocationId = locationId;
            Date = date;
            Planned = planned;
            Booked = booked;
            IsCount = isCount;
            Revenue = revenue;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public int Planned { get; set; }
        public int Booked { get; set; }
        public int IsCount { get; set; }
        public double Revenue { get; set; }
        public bool Deleted { get; set; }


        //Functions
        public static DataSet GetDataSetBooking()
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
                    var cmd = new MySqlDataAdapter("SELECT * FROM Buchung", db.connection);
                    DataSet dataset = new DataSet();
                    cmd.Fill(dataset, "Buchung");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu der Buchung abgerufen werden: \n" + e);
                }
            }
            return null;
        }

        public static Booking GetBookingByDate(DateTime date)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Buchung WHERE Datum = @date AND Entfernt = 0", 
                        db.connection);
                    cmd.Parameters.AddWithValue("@date", date);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Booking(reader.GetInt32(0), reader.GetInt32(1), date, 
                            reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), 
                            reader.GetDouble(6), reader.GetBoolean(7));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Buchung für diesen Tag gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        public static Booking GetBookingById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Buchung WHERE Id = @id AND Entfernt = 0", db.connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Booking(id, reader.GetInt32(1), reader.GetDateTime(2),
                            reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5),
                            reader.GetDouble(6), reader.GetBoolean(7));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Buchung für diese Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }

        //Create booking
        public static void CreateBooking(Booking booking)
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
                Booking bookingObj = GetBookingById(booking.Id);
                if (bookingObj != null)
                {
                    try
                    {
                        var cmd = new MySqlCommand($"UPDATE Buchung SET geplant = @planned, istAnzahl = @isAmount " +
                                                   $"WHERE Id = {bookingObj.Id}", db.connection);
                        cmd.Parameters.AddWithValue("@planned", booking.Planned);
                        cmd.Parameters.AddWithValue("@isAmount", booking.IsCount);

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
                        var cmd = new MySqlCommand($"INSERT INTO Buchung (StandortId, Datum, geplant, gebucht, istAnzahl, Umsatz, Entfernt) " +
                             $"VALUES (@location, @date, @planned, @booked, @isAmount, @revenue, @deleted)", db.connection);
                        cmd.Parameters.AddWithValue("@location", booking.LocationId);
                        cmd.Parameters.AddWithValue("@date", booking.Date);
                        cmd.Parameters.AddWithValue("@planned", booking.Planned);
                        cmd.Parameters.AddWithValue("@booked", booking.Booked);
                        cmd.Parameters.AddWithValue("@isAmount", booking.IsCount);
                        cmd.Parameters.AddWithValue("@revenue", booking.Revenue);
                        cmd.Parameters.AddWithValue("@deleted", booking.Deleted);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Fehler" + e);
                    }
                }


            }
        }

        //Create booking
        public static void SaveBooking(Booking booking)
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
                Booking bookingObj = GetBookingByDate(booking.Date);
                if(bookingObj != null)
                {
                    int booked = bookingObj.Booked + 1;
                    try
                    {
                        var cmd = new MySqlCommand($"UPDATE Buchung SET gebucht = @booked WHERE Id = {bookingObj.Id}", db.connection);
                        cmd.Parameters.AddWithValue("@booked", booked);

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
                        var cmd = new MySqlCommand($"INSERT INTO Buchung (StandortId, Datum, geplant, gebucht, istAnzahl, Umsatz, Entfernt) " +
                             $"VALUES (@location, @date, @planned, @booked, @isAmount, @revenue, @deleted)", db.connection);
                        cmd.Parameters.AddWithValue("@location", booking.LocationId);
                        cmd.Parameters.AddWithValue("@date", booking.Date);
                        cmd.Parameters.AddWithValue("@planned", booking.Planned);
                        cmd.Parameters.AddWithValue("@booked", booking.Booked);
                        cmd.Parameters.AddWithValue("@isAmount", booking.IsCount);
                        cmd.Parameters.AddWithValue("@revenue", booking.Revenue);
                        cmd.Parameters.AddWithValue("@deleted", booking.Deleted);
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

    
}
