using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "LevelsBackgroundData", menuName = "")]
    public class LevelsBackgroundData : ScriptableObject
    {
        public List<Color> BackgroundColors;
    }
}
