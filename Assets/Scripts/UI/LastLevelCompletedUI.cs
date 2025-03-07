using System;
using System.Collections.Generic;
using Main;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class LastLevelCompletedUI : BaseUI
    {        
        [SerializeField] private Text _titleText;
        [SerializeField] private Text _waitText;
        [SerializeField] private Text _selectText;
        [SerializeField] private Button _selectLevelButton;
        [SerializeField] private LevelSelectUI _levelSelectUI;
        [Space]
        [SerializeField] private List<GameObject> _objectsToHideOnOpen;
        
        private const string TITLE_RU = "УРОВЕНЬ ПРОЙДЕН!";
        private const string TITLE_EN = "LEVEL COMPLETED!";
        
        private const string WAIT_RU = "ОЖИДАЙТЕ НОВЫХ ИНТЕРЕСНЫХ УРОВНЕЙ...";
        private const string WAIT_EN = "STAY TUNED FOR NEW EXCITING LEVELS...";
        
        private const string SELECT_RU = "ВЫБРАТЬ УРОВЕНЬ";
        private const string SELECT_EN = "SELECT LEVEL";
        
        private void Awake()
        {
            SwitchLanguage(YG2.lang);
        }

        private void Start()
        {
            YG2.onSwitchLang += SwitchLanguage;

            _selectLevelButton.onClick.AddListener(OpenSelectLevelUI);
        }

        private void OnDestroy()
        {            
            YG2.onSwitchLang -= SwitchLanguage;

            _selectLevelButton.onClick.RemoveListener(OpenSelectLevelUI);
        }

        private void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    _titleText.text = TITLE_RU;
                    _waitText.text = WAIT_RU;
                    _selectText.text = SELECT_RU;
                    break;
                default:
                    _titleText.text = TITLE_EN;
                    _waitText.text = WAIT_EN;
                    _selectText.text = SELECT_EN;
                    break;
            }
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

        private void OpenSelectLevelUI()
        {
            HideUI();
            _levelSelectUI.ShowUI();
        }
    }
}