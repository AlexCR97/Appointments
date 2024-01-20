namespace Appointments.Core.Infrastructure.Services.Geo;

public class GeoSearchResponse
{
    public int place_id { get; set; }
    public string licence { get; set; }
    public string osm_type { get; set; }
    public int osm_id { get; set; }
    public string[] boundingbox { get; set; }
    public string lat { get; set; }
    public string lon { get; set; }
    public string display_name { get; set; }
    public int place_rank { get; set; }
    public string category { get; set; }
    public string type { get; set; }
    public float importance { get; set; }
}
