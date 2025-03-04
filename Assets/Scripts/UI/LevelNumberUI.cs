using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class LevelNumberUI : MonoBehaviour
    {
        [SerializeField] private Text _levelNumberText;

        private const string TITLE_RU = "УРОВЕНЬ";
        private const string TITLE_EN = "LEVEL";

        private string _title;
        private uint _levelNumber;

        private void Awake()
        {
            SwitchLanguage(YG2.lang);
        }

        private void Start()
        {
            YG2.onSwitchLang += SwitchLanguage;
        }

        private void OnDestroy()
        {
            YG2.onSwitchLang -= SwitchLanguage;
        }

        private void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    _title = TITLE_RU;
                    break;
                default:
                    _title = TITLE_EN;
                    break;
            }
            UpdateUI();
        }

        public void SetLevelNumber(uint levelNumber)
        {
            _levelNumber = levelNumber;
            UpdateUI();
        }

        public void UpdateUI()
        {
            _levelNumberText.text = $"{_title} {_levelNumber}";
        }
    }
}