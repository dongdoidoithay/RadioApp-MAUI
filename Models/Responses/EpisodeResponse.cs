namespace RadioApp.Models.Responses;

public class EpisodeResponse
{
    public string episodeId { get; set; }
    public string? albumId { get; set; }
    public string? cateId { get; set; }
    public string? subtitle { get; set; }
    public string? type { get; set; }
    public string? image { get; set; }
    public string? auth { get; set; }
    public string? desc { get; set; }
    public string? language { get; set; }
    public string? year { get; set; }
    public string? lyrics { get; set; }
    public string? downloadUrl { get; set; }
    public string? songUrl { get; set; }
    public DateTime date { get; set; }
    public bool isActive { get; set; }
    public string Duration { get; set; }

    public Uri Url { get; set; }
}
public class RootEpisodeResponse
{
    public bool success { get; set; }
    public string? error { get; set; }
    public string? message { get; set; }
    public string? code { get; set; }
    public List<EpisodeResponse> data { get; set; }
    public int totalRecode { get; set; }
    public int currentPage { get; set; }
    public int totalPage { get; set; }
}