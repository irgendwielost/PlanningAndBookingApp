using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class MarketingCosts
    {
        public MarketingCosts(int id, double costs, bool deleted)
        {
            Id = id;
            Costs = costs;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public double Costs { get; set; }
        public bool Deleted { get; set; }
    }
}
