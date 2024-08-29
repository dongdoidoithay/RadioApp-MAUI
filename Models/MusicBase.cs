using RadioApp.Enums;

namespace RadioApp.Models;

public class MusicBase
{
    /// <summary>
    /// platform dùng cho nhiều nguồn dữ liệu 
    /// </summary>
    public PlatformEnum Platform { get; set; }
    /// <summary>
    /// tên bài hát
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// tên tác giả
    /// </summary>
    public string Artist { get; set; } = null!;
    /// <summary>
    /// tên album
    /// </summary>
    public string Album { get; set; } = null!;
    /// <summary>
    /// link ảnh
    /// </summary>
    public string ImageUrl { get; set; } = null!;

    /// <summary>
    /// thời gian 
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    ///thời gian nhạc,định dạng hiển thị "minute: second", for example: 05: 44
    /// </summary>
    public string DurationText => $"{Duration.Minutes}:{Duration.Seconds:D2}";

    /// <summary>
    ///thông tin thêm dạng Json data
    /// </summary>
    public string? ExtendDataJson { get; set; }
    /// <summary>
    ///Cost (free, VIP, etc.)
    /// </summary>
    public FeeEnum Fee { get; set; }
}