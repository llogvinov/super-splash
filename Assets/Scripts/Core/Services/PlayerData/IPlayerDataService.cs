using Core;

namespace Core.Services.PlayerData
{
    public interface IPlayerDataService : IService
    {
        PlayerData Load();
        void Save(PlayerData playerData);
    }
}