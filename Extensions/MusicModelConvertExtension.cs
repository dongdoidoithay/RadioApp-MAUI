using RadioApp.Models;
using RadioApp.ViewModels;

namespace RadioApp.Extensions;

internal static class MusicModelConvertExtension
{
    public static List<Episode> ToLocalMusics(this List<MusicResultShowViewModel> musics)
    {
        return musics.Select(x => new Episode()
        {
            episodeId = x.Id,
            subtitle = x.Name,
            albumId = x.Album,
            cateId = x.Artist,
            songUrl = x.ExtendDataJson,
            image = x.ImageUrl
        }).ToList();
    }

    public static Episode ToLocalMusic(this MusicResultShowViewModel music)
    {
        return new Episode()
        {
            episodeId = music.Id,
            subtitle = music.Name,
            albumId = music.Album,
            cateId = music.Artist,
            songUrl = music.ExtendDataJson,
            image = music.ImageUrl
        };
    }
}
