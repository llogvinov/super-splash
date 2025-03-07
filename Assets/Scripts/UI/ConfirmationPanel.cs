using System;
using System.Collections.Generic;
using Core.Services;
using Core.Services.Ad;
using Main;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class ConfirmationPanel : BaseUI
    {
        public Action AdShown;

        [SerializeField] private Text _titleText;
        [Space]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Text _yesText;
        [Space]
        [SerializeField] private Button _noButton;
        [SerializeField] private Text _noText;
        [Space]
        [SerializeField] private List<GameObject> _objectsToHideOnOpen;

        private IAdService _adService;
        private IAdService AdService => _adService ??= AllServices.Container.Single<IAdService>();

        private const string TITLE_RU = "ХОТИТЕ ПРОПУСТИТЬ УРОВЕНЬ И ПЕРЕЙТИ НА СЛЕДУЮЩИЙ?";
        private const string TITLE_EN = "DO YOU WANT TO SKIP LEVEL AND GO TO THE NEXT ONE?";

        private const string YES_RU = "ДА";
        private const string YES_EN = "YES";

        private const string NO_RU = "НЕТ";
        private const string NO_EN = "NO";

        private void Awake()
        {
            SwitchLanguage(YG2.lang);
        }

        private void Start()
        {
            YG2.onSwitchLang += SwitchLanguage;

            _yesButton.onClick.AddListener(ShowRewardedAd);
            _noButton.onClick.AddListener(HideUI);
        }

        private void OnDestroy()
        {
            YG2.onSwitchLang -= SwitchLanguage;

            _yesButton.onClick.RemoveListener(ShowRewardedAd);
            _noButton.onClick.RemoveListener(HideUI);
        }

        private void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    _titleText.text = TITLE_RU;
                    _yesText.text = YES_RU;
                    _noText.text = NO_RU;
                    break;
                default:
                    _titleText.text = TITLE_EN;
                    _yesText.text = YES_EN;
                    _noText.text = NO_EN;
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

        private void ShowRewardedAd()
        {
            AdService.ShowRewardedAd(OnAdShown);
            HideUI();
        }

        private void OnAdShown()
        {
            Debug.Log("open next level");
            AdShown?.Invoke();
        }
    }
}