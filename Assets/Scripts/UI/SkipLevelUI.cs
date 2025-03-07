using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class SkipLevelUI : MonoBehaviour
    {
        public Action AdShown;
        
        [SerializeField] private Button _skipLevelButton;
        [SerializeField] private Text _hintText;
        [SerializeField] private ConfirmationPanel _confirmationPanel;
        
        private const string HINT_RU = "ПРОПУСТИТЬ?";
        private const string HINT_EN = "SKIP LEVEL?";

        private void Awake()
        {
            SwitchLanguage(YG2.lang);
        }

        private void Start()
        {
            YG2.onSwitchLang += SwitchLanguage;

            _skipLevelButton.onClick.AddListener(OpenConfirmationPanel);
            _confirmationPanel.AdShown += OnAdShown;
        }

        private void OnDestroy()
        {
            YG2.onSwitchLang -= SwitchLanguage;

            _skipLevelButton.onClick.RemoveListener(OpenConfirmationPanel);
            _confirmationPanel.AdShown -= OnAdShown;
        }

        private void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    _hintText.text = HINT_RU;
                    break;
                default:
                    _hintText.text = HINT_EN;
                    break;
            }
        }

        private void OpenConfirmationPanel()
        {
            _confirmationPanel.ShowUI();
        }

        private void OnAdShown()
        {
            AdShown?.Invoke();
        }
    }
}