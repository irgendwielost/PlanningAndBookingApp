using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class LocationOperatingResult
    {
        public LocationOperatingResult(int id, int locationId, DateTime date, double operatingResult, bool deleted)
        {
            Id = id;
            LocationId = locationId;
            Date = date;
            OperatingResult = operatingResult;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public double OperatingResult { get; set; }
        public bool Deleted { get; set; }
    }
}
