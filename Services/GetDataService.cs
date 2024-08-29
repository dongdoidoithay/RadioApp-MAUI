using System.Net.Http.Json;
using System.Text.Json;
using RadioApp.Models;
using RadioApp.Models.Responses;

namespace RadioApp.Services;

public class GetDataService
{
    private readonly HttpClient httpClient;
    private bool firstLoad = true;

    public GetDataService()
    {
        this.httpClient = new HttpClient() { BaseAddress = new Uri(Config.APIUrl) };
        httpClient.DefaultRequestHeaders.Add("api-version", "1.0");
    
    }

    public async Task<List<Category>> GetRootCate()
    {
        var rootCateResponse = await TryGetAsync<RootCategoryResponse>($"api/blog/get-root-cate");
        return rootCateResponse?.data?.Select(response => new Category(response)).ToList();
    }
    public async Task<List<Category>> GetCateByParent(string parentCateId)
    {
        var rootCateResponse = await TryGetAsync<RootCategoryResponse>($"api/blog/get-cate-by-parent/{parentCateId}");
        return rootCateResponse?.data?.Select(response => new Category(response)).ToList();
    }
    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        var categoryResponse = await TryGetAsync<RootCategoryResponse>($"api/blog/get-all-cate");
        return categoryResponse?.data?.Select(response => new Category(response));
    }

    public async Task<CateEpisodes?> GetEpisodeByCate(string cateId, int page = 0, int count = 22)
    {
        var categoryResponse = await TryGetAsync<RootCateEpisodeResponse>($"api/blog/get-episode-by-cate/{cateId}/{count}/{page}");
        return categoryResponse?.data;
    }



    private Task<T> TryGetAsync<T>(string path)
    {
        if (firstLoad)
        {
            firstLoad = false;

            // On first load, it takes a significant amount of time to initialize
            // the GetDataService. For example, Connectivity.NetworkAccess, Barrel.Current.Get,
            // and HttpClient all take time to initialize.
            //
            // Don't block the UI thread while doing this initialization, so the app starts faster.
            // Instead, run the first TryGet in a background thread to unblock the UI during startup.
            return Task.Run(() => TryGetImplementationAsync<T>(path));
        }

        return TryGetImplementationAsync<T>(path);
    }

    private async Task<T> TryGetImplementationAsync<T>(string path)
    {
        var json = string.Empty;

        T responseData = default;
        try
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                var response = await httpClient.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadFromJsonAsync<T>();
                }
            }
            else
            {
                responseData = JsonSerializer.Deserialize<T>(json);
            }

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return responseData;
    }
}
