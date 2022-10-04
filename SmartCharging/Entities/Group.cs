using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCharging.Model
{
    public class Group
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Capacity should be bigger than {1}")]
        public double CapacityInAmps { get; set; }

        public virtual List<ChargingStation> Stations { get; set; }
    }

    public class ChargingStation
    {
        [Key,Required]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
 
        public virtual List<Connector> Connectors { get; set; }
     
    }

    public class Connector
    {
        [Required]
        public int ConnectorId { get; set; }

        [Required]
        public int StationId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Max Current should bigger than {1}")]
        public double MaxCurrentInAmps { get; set; }
    }
}
