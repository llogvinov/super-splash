using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButtonUI : MonoBehaviour
    {
        public static event Action<uint> LoadLevelEvent;

        [SerializeField] private Text _levelNumberText;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _lock;

        public uint LevelNumber { get; private set; }

        public void Initialize(LevelData levelData, bool isLevelOpened)
        {
            LevelNumber = levelData.LevelNumber;
            _levelNumberText.text = LevelNumber.ToString();
            UpdateLock(isLevelOpened);
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void UpdateLock(bool isLevelOpened) =>
            _lock.SetActive(!isLevelOpened);

        private void OnButtonClicked()
        {
            // if level is open
            LoadLevel();
        }

        private void LoadLevel()
        {
            LoadLevelEvent?.Invoke(LevelNumber);
        }
    }
}