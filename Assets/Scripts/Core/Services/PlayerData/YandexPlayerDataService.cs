using YG;

namespace Core.Services.PlayerData
{
    public class YandexPlayerDataService : IPlayerDataService
    {
        public PlayerData Load()
        {
            var playerData = new PlayerData();
            playerData.CurrentLevelNumber = YG2.saves.CurrentLevelNumber;
            playerData.OpenedLevels = YG2.saves.OpenedLevels;
            return playerData;
        }

        public void Save(PlayerData playerData)
        {
            YG2.saves.CurrentLevelNumber = playerData.CurrentLevelNumber;
            YG2.saves.OpenedLevels = playerData.OpenedLevels;
            YG2.SaveProgress();
        }
    }
}