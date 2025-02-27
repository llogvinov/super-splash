using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButtonUI : MonoBehaviour
    {
        public static event Action<uint> LoadLevelEvent;

        [SerializeField] private Text _levelNumberText;
        [SerializeField] private Button _button;

        private uint _levelNumber;
        
        public void Initialize(LevelData levelData)
        {
            _levelNumber = levelData.LevelNumber;
            _levelNumberText.text = _levelNumber.ToString();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            // if level is open
            LoadLevel();
        }

        private void LoadLevel()
        {
            LoadLevelEvent?.Invoke(_levelNumber);
        }
    }
}