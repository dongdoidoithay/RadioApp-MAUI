using RadioApp.Models;

namespace RadioApp.Services.Interface;

public interface IPlaylistService
{
    Task<bool> AddOrUpdateAsync(Episode playlist);
    Task<bool> AddOrUpdateAsync(List<Episode> playlists);
    Task<Episode?> GetOneAsync(string musicId);
    Task<List<Episode>> GetAllAsync();
    Task<bool> RemoveAsync(string musicId);
    Task<bool> RemoveAllAsync();
}