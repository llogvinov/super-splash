using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Main;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelCompletedUI : BaseUI
    {
        public Action NextLevelButtonClicked;
        
        [SerializeField] private Button _nextLevelButton;
        [Space]
        [SerializeField] private List<GameObject> _objectsToHideOnOpen;

        private void Start()
        {
            
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }

        private void OnDestroy()
        {
            
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        }

        public override void ShowUI()
        {
            BallMoveController.IsInputAllowed = false;
            base.ShowUI();
            ToggleHidable(false);
        }

        public override void HideUI()
        {
            BallMoveController.IsInputAllowed = true;
            base.HideUI();
            ToggleHidable(true);
        }

        private void ToggleHidable(bool enable)
        {
            foreach (var obj in _objectsToHideOnOpen)
            {
                obj.SetActive(enable);
            }
        }

        public void ToggleNextButton(bool enable) =>
            _nextLevelButton.gameObject.SetActive(enable);

        private async void OnNextLevelButtonClicked()
        {
            await Task.Delay(500);
            NextLevelButtonClicked?.Invoke();
        }
    }
}