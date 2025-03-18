using System;
using System.Collections.Generic;
using DG.Tweening;
using Main;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class LevelCompletedUI : BaseUI
    {
        public Action NextLevelButtonClicked;

        [SerializeField] private Transform _panel;
        [SerializeField] private Text _titleText;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Text _nextText;
        [Space]
        [SerializeField] private List<GameObject> _objectsToHideOnOpen;
        [Space]
        [SerializeField] private ParticleSystem _confetti;

        private const string TITLE_RU = "УРОВЕНЬ ПРОЙДЕН!";
        private const string TITLE_EN = "LEVEL COMPLETED!";

        private const string NEXT_RU = "СЛЕДУЮЩИЙ УРОВЕНЬ";
        private const string NEXT_EN = "NEXT LEVEL";

        private string _title;
        private string _next;

        private Vector3 _initialScale;
        private Vector3 _startAnimationScale;

        private void Awake()
        {
            SwitchLanguage(YG2.lang);
            _initialScale = _panel.localScale;
            _startAnimationScale = _initialScale / 2f;
        }

        private void Start()
        {
            YG2.onSwitchLang += SwitchLanguage;
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }

        private void OnDestroy()
        {
            YG2.onSwitchLang -= SwitchLanguage;
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        }

        private void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    _title = TITLE_RU;
                    _next = NEXT_RU;
                    break;
                default:
                    _title = TITLE_EN;
                    _next = NEXT_EN;
                    break;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            _titleText.text = _title;
            _nextText.text = _next;
        }

        public override void ShowUI()
        {
            BallMoveController.IsInputAllowed = false;
            ToggleHidable(false);

            _confetti.Play();

            ToggleNextButton(false);
            _panel.transform.localScale = _startAnimationScale;
            base.ShowUI();

            _panel.transform.DOScale(_initialScale, 1f)
                .SetEase(Ease.InOutBack)
                .OnComplete(() => ToggleNextButton(true));
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
            _nextLevelButton.interactable = enable;

        private void OnNextLevelButtonClicked()
        {
            NextLevelButtonClicked?.Invoke();
        }
    }
}