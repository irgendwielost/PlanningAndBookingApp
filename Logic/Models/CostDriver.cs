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
    public class CostDriver
    {
        public CostDriver(int id, string designation, double costs, bool fix, bool variable, bool deleted)
        {
            Id = id;
            Designation = designation;
            Costs = costs;
            Fix = fix;
            Variable = variable;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public string Designation { get; set; }
        public double Costs { get; set; }
        public bool Fix { get; set; }
        public bool Variable { get; set; }
        public bool Deleted { get; set; }



        //Get object by id
        public static CostDriver GetCostDriverById(int id)
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
                    var cmd = new MySqlCommand($"SELECT * FROM Kostenstellen WHERE Id = {id}", db.connection);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new CostDriver(id, reader.GetString(1), reader.GetDouble(2), reader.GetBoolean(3),
                            reader.GetBoolean(4), reader.GetBoolean(5));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Es konnte keine Kostenstellle mit dieser Id gefunden werden: \n" + e);
                    return null;
                }
            }
            return null;
        }
    }
}
