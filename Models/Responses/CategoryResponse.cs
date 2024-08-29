namespace RadioApp.Models.Responses;

public class CategoryResponse
{
    public string cateId { get; set; }
    public string? cateParentId { get; set; }
    public string name { get; set; }
    public string? description { get; set; }
    public string? type { get; set; }
    public string? image { get; set; }
    public bool isActive { get; set; }=true;
}

public class RootCategoryResponse
{
    public bool success { get; set; }
    public string? error { get; set; }
    public string? message { get; set; }
    public string? code { get; set; }
    public List<CategoryResponse> data { get; set; }
    public int totalRecode { get; set; }
    public int currentPage { get; set; }
    public int totalPage { get; set; }
}
