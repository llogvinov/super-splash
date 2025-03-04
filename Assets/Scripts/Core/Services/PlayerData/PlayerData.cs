using System.Collections.Generic;

namespace Core.Services.PlayerData
{
    public class PlayerData
    {
        public uint CurrentLevelNumber;
        public List<uint> OpenedLevels;
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public uint CurrentLevelNumber = 0;
        public List<uint> OpenedLevels = new List<uint>() { 1 };
    }
}