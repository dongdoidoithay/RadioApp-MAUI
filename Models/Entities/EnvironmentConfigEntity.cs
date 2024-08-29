using SQLite;


namespace RadioApp.Models.Entities;
[Table("EnvironmentConfig")]
internal class EnvironmentConfigEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } = 0;
    public string PlayerSettingJson { get; set; }
    public string GeneralSettingJson { get; set; }
    public string SearchSettingJson { get; set; }
    public string PlaySettingJson { get; set; }
}
