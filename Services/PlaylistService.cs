using RadioApp.Models;
using RadioApp.Models.Entities;
using RadioApp.Services.Interface;
using RadioApp.Storages;
namespace RadioApp.Services;


public class PlaylistService : IPlaylistService
{
    public async Task<bool> AddOrUpdateAsync(Episode playlist)
    {
        int count;
        var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().FirstOrDefaultAsync(x => x.MusicId == playlist.episodeId);
        if (playlists != null)
        {
            if (playlists.ImageUrl.IsEmpty())
            {
                playlists.ImageUrl = playlist.ImageUrl;
                if (await DatabaseProvide.DatabaseAsync.UpdateAsync(playlists) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        playlists = new PlaylistEntity()
        {
            MusicId = playlist.episodeId,
            Artist = playlist.cateId,
            Name = playlist.subtitle,
            Album = playlist.albumId,
            ImageUrl = playlist.ImageUrl,
            DurationMillisecond = 0,//(int)playlist.Duration.,
            ExtendDataJson = playlist.songUrl,
            EditTime = DateTime.Now
        };
        count = await DatabaseProvide.DatabaseAsync.InsertAsync(playlists);

        if (count == 0)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> AddOrUpdateAsync(List<Episode> playlists)
    {
        await DatabaseProvide.DatabaseAsync.RunInTransactionAsync(tran =>
        {
            foreach (var playlist in playlists)
            {
                var playlistEntity = new PlaylistEntity()
                {
                    MusicId = playlist.songUrl,
                    Artist = playlist.cateId,
                    Name = playlist.subtitle,
                    Album = playlist.albumId,
                    ImageUrl = playlist.ImageUrl,
                    DurationMillisecond = 0,//(int)playlist.Duration.,
                    ExtendDataJson = playlist.songUrl,
                    EditTime = DateTime.Now
                };
                tran.InsertOrReplace(playlistEntity);
            }
            tran.Commit();
        });
        return true;
    }
    public async Task<Episode?> GetOneAsync(string songId)
    {
        var playlistEntity = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().FirstOrDefaultAsync(x => x.MusicId == songId);
        if (playlistEntity == null)
        {
            return default;
        }
        return new Episode()
        {
            episodeId = songId,
            subtitle = playlistEntity.Name,
            albumId = playlistEntity.Album,
            cateId = playlistEntity.Artist,
            image = playlistEntity.ImageUrl,
            songUrl =playlistEntity.ExtendDataJson
        };
    }
    public async Task<List<Episode>> GetAllAsync()
    {
        var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().ToListAsync();
        return playlists.Select(x => new Episode()
        {
            episodeId = x.MusicId,
            subtitle = x.Name,
            albumId = x.Album,
            cateId = x.Artist,
            image = x.ImageUrl,
            songUrl = x.ExtendDataJson
        }).ToList();
    }

    public async Task<bool> RemoveAsync(string musicId)
    {
        await DatabaseProvide.DatabaseAsync.DeleteAsync<PlaylistEntity>(musicId);
        return true;
    }

    public async Task<bool> RemoveAllAsync()
    {
        await DatabaseProvide.DatabaseAsync.DeleteAllAsync<PlaylistEntity>();
        return true;
    }
}