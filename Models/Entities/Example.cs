using Microsoft.EntityFrameworkCore;

namespace Packet_Sniffers.Models.Entities;

public class Trail
{
    // Primary Key
    public int TrailId {get;set;}

    // Trail Name
    public string Name {get; set;} = string.Empty;

    // Location
    public string Location {get; set;} = string.Empty;

    // Length
    public double Length {get; set;}

    // Difficulty
    public string Difficulty {get; set;} = string.Empty;

    // Rating
    public int Rating {get; set;}

    // Description
    public string Description {get; set;} = string.Empty;

    // Trail Image URL
    public string? ImageUrl {get; set;}

    /* In case we need the option to upload Images...
    // Trail Image Upload
    public FormFile? ImageUpload {get; set;}
    */
}