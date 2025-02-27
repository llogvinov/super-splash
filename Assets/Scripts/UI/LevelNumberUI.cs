using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelNumberUI : MonoBehaviour
    {
        [SerializeField] private Text _levelNumber;

        public void SetLevelNumber(int levelNumber)
        {
            _levelNumber.text = $"Level {levelNumber}";
        }
    }
}