
using RadioApp.Enums;

namespace RadioApp.Services.MusicSwitchServer;

public interface IMusicSwitchServerFactory
{
    public IMusicSwitchServer Create(PlayModeEnum playMode);
}
