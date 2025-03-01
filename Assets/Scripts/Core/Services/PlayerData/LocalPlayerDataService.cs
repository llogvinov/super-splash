using System.Collections.Generic;
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
                playerData.CurrentLevelNumber = (uint)PlayerPrefs.GetInt("CurrentLevelNumber");
            }
            else
            {
                playerData.CurrentLevelNumber = 1;
            }

            if (PlayerPrefs.HasKey("OpenedLevels"))
            {
                var openedLevelString = PlayerPrefs.GetString("OpenedLevels");
                var openedLevelList = openedLevelString.Split(',');

                playerData.OpenedLevels = new List<uint>();
                foreach (var item in openedLevelList)
                {
                    playerData.OpenedLevels.Add(uint.Parse(item));
                }
            }
            else
            {
                playerData.OpenedLevels = new List<uint>() { 1 };
            }

            Save(playerData);
            return playerData;
        }

        public void Save(PlayerData playerData)
        {
            PlayerPrefs.SetInt("CurrentLevelNumber", (int)playerData.CurrentLevelNumber);

            var openedLevelList = string.Join(",", playerData.OpenedLevels);
            PlayerPrefs.SetString("OpenedLevels", openedLevelList);

            PlayerPrefs.Save();
        }
    }
}