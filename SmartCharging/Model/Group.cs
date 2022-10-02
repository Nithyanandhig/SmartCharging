namespace SmartCharging.Model
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float CapacityInAmps { get; set; }

        public List<ChargingStation> Stations { get; set; }
    }

    public class ChargingStation
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }

        public List<Connector> Connectors { get; set; }
     
    }

    public class Connector
    {
        public int Id { get; set; }

        public int StationId { get; set; }

        public int MaxCurrentInAmps { get; set; }
    }
}
