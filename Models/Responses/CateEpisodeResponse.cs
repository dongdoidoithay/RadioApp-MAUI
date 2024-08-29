namespace RadioApp.Models.Responses;

public class RootCateEpisodeResponse
{
    public bool success { get; set; }
    public string? error { get; set; }
    public string? message { get; set; }
    public string? code { get; set; }
    public CateEpisodes data { get; set; }
    public int totalRecode { get; set; }
    public int currentPage { get; set; }
    public int totalPage { get; set; }
}

public class CateEpisodes
{
    public List<Episode> episodes { get; set; }
    public CategoryResponse cate { get; set; }
}
//public class Cate
//{
//    public string cateId { get; set; }
//    public object cateParentId { get; set; }
//    public string name { get; set; }
//    public object description { get; set; }
//    public object type { get; set; }
//    public string image { get; set; }
//    public bool isActive { get; set; }
//}
