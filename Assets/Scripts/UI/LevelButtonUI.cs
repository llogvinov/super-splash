using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButtonUI : MonoBehaviour
    {
        public static event Action<LevelButtonUI> LevelButtonClicked;
        public static event Action<uint> LoadLevelEvent;

        [SerializeField] private Text _levelNumberText;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _outline;
        [SerializeField] private GameObject _lock;

        public uint LevelNumber { get; private set; }

        public void Initialize(LevelData levelData, bool isLevelOpened)
        {
            LevelNumber = levelData.LevelNumber;
            _levelNumberText.text = LevelNumber.ToString();
            UpdateLock(isLevelOpened);
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void UpdateLock(bool isLevelOpened)
        {
            _lock.SetActive(!isLevelOpened);
            _button.interactable = isLevelOpened;
        }

        private async void OnButtonClicked()
        {
            LevelButtonClicked?.Invoke(this);
            // if level is open
            await Task.Delay(500);
            LoadLevel();
        }

        private void LoadLevel()
        {
            LoadLevelEvent?.Invoke(LevelNumber);
        }

        public void ToggleOutline(bool enable) => 
            _outline.SetActive(enable);
    }
}