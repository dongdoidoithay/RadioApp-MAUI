using CommunityToolkit.Mvvm.ComponentModel;
using RadioApp.Models.Responses;

namespace RadioApp.Models;

public partial class Category : ObservableObject
{
    public Category(CategoryResponse response)
    {
        CateId = response.cateId;
        CateParentId = response.cateParentId;
        Name = response.name;
        Description = response.description;
        Type = response.description;
        Image = response.image;
        IsActive = response.isActive;
    }

    [ObservableProperty]
    private string cateId;
    [ObservableProperty]
    private string? cateParentId;
    [ObservableProperty]
    private string? name;
    [ObservableProperty]
    private string? description;
    [ObservableProperty]
    private string? type;
    [ObservableProperty]
    private string? image;
    [ObservableProperty]
    private bool isActive;
    [ObservableProperty]
    private bool isSelected;


    //public string ImageUrl { get {

    //        if (!string.IsNullOrEmpty(Image) &&  !Image.Contains("http"))
    //        {
    //            return Config.APIUrl+Image;
    //        }
    //        else
    //        {
    //            return Image;
    //        }

    //    } }
}
