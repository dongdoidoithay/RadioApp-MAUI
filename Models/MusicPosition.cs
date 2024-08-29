namespace RadioApp.Models;

public class MusicPosition
{
    /// <summary>
    /// Current progress
    /// </summary>
    public TimeSpan position { get; set; }
    /// <summary>
    /// total progress
    /// </summary>
    public TimeSpan Duration { get; set; }
    public double PlayProgress { get; set; }
}