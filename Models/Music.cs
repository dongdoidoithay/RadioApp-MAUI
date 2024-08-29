using RadioApp.Enums;

namespace RadioApp.Models;

public class Music
{
    public string Id { get; set; } = null!;

    /// <summary>
    /// Platform
    /// </summary>
    public PlatformEnum Platform { get; set; }



    /// <summary>
    /// name of the song
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Singer name
    /// </summary>
    public string Artist { get; set; } = null!;
    /// <summary>
    /// The album name
    /// </summary>
    public string Album { get; set; } = null!;
    /// <summary>
    /// The map's address
    /// </summary>
    public string ImageUrl { get; set; } = null!;

    /// <summary>
    /// Song durability
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// The song is durable, the format is "minute: second", for example: 05: 44
    /// </summary>
    public string DurationText => $"{Duration.Minutes}:{Duration.Seconds:D2}";

    /// <summary>
    /// Platform unique data
    /// </summary>
    public string? ExtendDataJson { get; set; }
    /// <summary>
    /// Cost (free, VIP, etc.)
    /// </summary>
    public FeeEnum Fee { get; set; }
}