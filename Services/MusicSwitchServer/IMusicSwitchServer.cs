
using RadioApp.Models;

namespace RadioApp.Services.MusicSwitchServer;

public interface IMusicSwitchServer
{
    public Task<Episode?> GetPreviousAsync(string currentMusicId);
    public Task<Episode?> GetNextAsync(string currentMusicId);
}