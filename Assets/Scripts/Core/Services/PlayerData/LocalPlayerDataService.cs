using UnityEngine;

namespace Core.Services.PlayerData
{
    public class LocalPlayerDataService : IPlayerDataService
    {
        public PlayerData Load()
        {
            var playerData = new PlayerData();
            if (PlayerPrefs.HasKey("CurrentLevelNumber"))
            {
                playerData.CurrentLevelNumber = PlayerPrefs.GetInt("CurrentLevelNumber");
            }
            else
            {
                Save(playerData);
            }
            return playerData;
        }

        public void Save(PlayerData playerData)
        {
            PlayerPrefs.SetInt("CurrentLevelNumber", playerData.CurrentLevelNumber);
        }
    }
}