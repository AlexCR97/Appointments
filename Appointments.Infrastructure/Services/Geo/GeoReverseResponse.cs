namespace Appointments.Core.Infrastructure.Services.Geo;

public class GeoReverseResponse
{
    public int place_id { get; set; }
    public string licence { get; set; }
    public string osm_type { get; set; }
    public long osm_id { get; set; }
    public string lat { get; set; }
    public string lon { get; set; }
    public int place_rank { get; set; }
    public string category { get; set; }
    public string type { get; set; }
    public float importance { get; set; }
    public string addresstype { get; set; }
    public string name { get; set; }
    public string display_name { get; set; }
    public Address address { get; set; }
    public string[] boundingbox { get; set; }
}

public class Address
{
    public string man_made { get; set; }
}
