namespace Buchungs_und_Planungssystem.Logic.Models
{
    public class RegionLocation
    {
        public RegionLocation(int locationId, int regionId)
        {
            LocationId = locationId;
            RegionId = regionId;
        }
        
        public int LocationId { get; set; }
        public int RegionId { get; set; }
        
        
        
    }
}