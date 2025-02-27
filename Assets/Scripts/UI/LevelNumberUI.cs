using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelNumberUI : MonoBehaviour
    {
        [SerializeField] private Text _levelNumber;

        public void SetLevelNumber(uint levelNumber)
        {
            _levelNumber.text = $"Level {levelNumber}";
        }
    }
}