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
    public class RoomPrice
    {
        public RoomPrice(int roomId, int seasonId, int locationId, double price, bool deleted)
        {
            RoomId = roomId;
            SeasonId = seasonId;
            LocationId = locationId;
            Price = price;
            Deleted = deleted;
        }

        public int RoomId { get; set; }
        public int SeasonId { get; set; }
        public int LocationId { get; set; }
        public double Price { get; set; }
        public bool Deleted { get; set; }


        //Functions

        public static DataSet GetDataSetRoomPrices()
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
                    var query = new MySqlDataAdapter($"SELECT ZimmerSaisonPreis.*, " +
                                                     $"z.Bezeichnung as RoomName FROM ZimmerSaisonPreis " +
                                                     $"join Zimmer z on ZimmerSaisonPreis.ZimmerId = z.Id " +
                                                     $"WHERE z.Entfernt = 0", db.connection);
                    DataSet dataset = new DataSet();
                    query.Fill(dataset, "ZimmerSaisonPreis");
                    return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu der Buchung abgerufen werden: \n" + e);
                }
            }
            return null;
        }

        //Save roomprice
        public static void SaveRoomPrice(RoomPrice roomprice)
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
                RoomPrice roomPriceObj = GetRoomPriceByIds(roomprice.RoomId, roomprice.SeasonId, roomprice.LocationId);
                if(roomPriceObj != null)
                {
                    MessageBox.Show("Preis: " + roomprice.Price);
                    try
                    {
                        var query = new MySqlCommand($"UPDATE ZimmerSaisonPreis SET ZimmerPreis = {roomprice.Price} " +
                                         $"WHERE ZimmerId = {roomPriceObj.RoomId} " +
                                         $"AND SaisonId = {roomPriceObj.SeasonId} " +
                                         $"AND StandortId = {roomPriceObj.LocationId}", db.connection);
                        query.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Fehler" + e );
                    }
                }
                else
                {
                    try
                    {
                        var cmd = new MySqlCommand($"INSERT INTO ZimmerSaisonPreis (ZimmerId, SaisonId, StandortId, ZimmerPreis, Entfernt) " +
                            $"VALUES (@room, @season, @location, @price, @deleted)", db.connection);
                        cmd.Parameters.AddWithValue("@room", roomprice.RoomId);
                        cmd.Parameters.AddWithValue("@season", roomprice.SeasonId);
                        cmd.Parameters.AddWithValue("@location", roomprice.LocationId);
                        cmd.Parameters.AddWithValue("@price", roomprice.Price);
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


        //logical delete roomprice
        public static void DeleteRoomPrice(RoomPrice roomPrice)
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
                    var cmd = new MySqlCommand($"UPDATE ZimmerSaisonPreis SET Entfernt = @deleted " +
                                               $"WHERE ZimmerId = {roomPrice.RoomId} AND SaisonId = {roomPrice.SeasonId} " +
                                               $"AND StandortId = {roomPrice.LocationId}", db.connection);
                    cmd.Parameters.AddWithValue("@room", 2);
                    cmd.Parameters.AddWithValue("@season", roomPrice.SeasonId);
                    cmd.Parameters.AddWithValue("@location", roomPrice.LocationId);
                    cmd.Parameters.AddWithValue("@price", roomPrice.Price);
                    cmd.Parameters.AddWithValue("@deleted", 1);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Der Zimmerpreis konnte nicht entfernt werden: \n" + e);
                }
            }
            
        }
        //Get object by ids
        public static RoomPrice GetRoomPriceByIds(int roomId, int seasonId, int locationId)
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
                    var cmd = new MySqlCommand($"SELECT * FROM ZimmerSaisonPreis WHERE ZimmerId = {roomId} " +
                                               $"AND SaisonId = {seasonId} AND StandortId = {locationId}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new RoomPrice(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDouble(3),reader.GetBoolean(4));
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return null;
        }

        public static DataSet GetDataSet()
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

                  var cmd = new MySqlDataAdapter($"SELECT ZimmerSaisonPreis.*, Zimmer.Bezeichnung " +
                                                 "AS RoomName, Saison.Bezeichnung AS SeasonName, " +
                                                 "Hotelstandorte.Bezeichnung AS LocationName FROM Zimmer " +
                                                 "INNER JOIN(Hotelstandorte " +
                                                 "INNER JOIN(Saison INNER JOIN ZimmerSaisonPreis " +
                                                 "ON Saison.ID = ZimmerSaisonPreis.SaisonId) ON " +
                                                 "Hotelstandorte.ID = ZimmerSaisonPreis.StandortId) " +
                                                 "ON Zimmer.Id = ZimmerSaisonPreis.ZimmerId WHERE " +
                                                 "ZimmerSaisonPreis.Entfernt= 0", db.connection);

                  DataSet dataset = new DataSet();
                  cmd.Fill(dataset, "ZimmerSaisonPreis");
                  return dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnten keine Daten zu der Buchung abgerufen werden: \n" + e);
                }
            }
            return null;
        }

        
    }
}
