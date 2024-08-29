using RadioApp.Models.Entities;
using SQLite;

namespace RadioApp.Storages;


public class DatabaseProvide
{
    private static string _dbPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RadioApp"), "RadioApp1.db");

    private DatabaseProvide()
    {

    }

    public static void Initialize()
    {
        Database.CreateTable<MusicEntity>();
        Database.CreateTable<PlaylistEntity>();
        Database.CreateTable<MyFavoriteEntity>();
        Database.CreateTable<MyFavoriteDetailEntity>();
        Database.CreateTable<EnvironmentConfigEntity>();
        Database.CreateTable<UserEntity>();
        Database.CreateTable<MusicCacheEntity>();
    }

    private static SQLiteConnection? _database;
    public static SQLiteConnection Database
    {
        get
        {
            if (_database == null)
            {
                if (_dbPath.IsEmpty())
                {
                    throw new Exception("Database path is not configured");
                }
                _database = new SQLiteConnection(_dbPath);
            }
            return _database;
        }
    }

    private static SQLiteAsyncConnection? _databaseAsync;
    public static SQLiteAsyncConnection DatabaseAsync
    {
        get
        {
            if (_databaseAsync == null)
            {
                if (_dbPath.IsEmpty())
                {
                    throw new Exception("Database path is not configured");
                }
                _databaseAsync = new SQLiteAsyncConnection(_dbPath);

            }
            return _databaseAsync;
        }
    }
}