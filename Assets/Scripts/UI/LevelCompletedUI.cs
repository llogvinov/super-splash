using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelCompletedUI : MonoBehaviour
    {
        public Action NextLevelButtonClicked;

        [SerializeField] private Canvas _panel;
        [SerializeField] private Button _nextLevelButton;

        private void Start()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }

        private void OnDestroy()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        }

        private void OnNextLevelButtonClicked()
        {
            NextLevelButtonClicked?.Invoke();
        }

        public void Show()
        {
            _panel.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _panel.gameObject.SetActive(false);
        }
    }
}