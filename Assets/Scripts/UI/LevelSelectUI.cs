using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectUI : MonoBehaviour
    {
        public Action NextLevelButtonClicked;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private LevelsData _levelsData;
        [SerializeField] private Transform _container;
        [SerializeField] private LevelButtonUI _levelButtonUI;
        [SerializeField] private Button _nextLevelButton;

        private void Start()
        {
            CleanUp();
            Initialize();
            LevelButtonUI.LoadLevelEvent += HideUI;
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }

        private void OnDestroy()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        }

        public void ShowUI()
        {
            _canvas.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            _canvas.gameObject.SetActive(false);
        }

        private void OnNextLevelButtonClicked() => 
            NextLevelButtonClicked?.Invoke();

        private void HideUI(uint levelNumber)
        {
            HideUI();
        }

        private void CleanUp()
        {
            foreach (Transform child in _container.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void Initialize()
        {
            foreach (var levelData in _levelsData.LevelDataList)
            {
                var levelButton = Instantiate(_levelButtonUI, _container);
                levelButton.Initialize(levelData);
            }
        }
    }
}