using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class StaffPosition
    {
        public StaffPosition(int id, string designation, bool deleted)
        {
            Id = id;
            Designation = designation;
            Deleted = deleted;
        }

        public int Id { get; set; }
        public string Designation { get; set; }
        public bool Deleted { get; set; }
    }
}
