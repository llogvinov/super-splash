using System;
using UnityEngine;

namespace UI
{
    public class BaseUI : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;

        public virtual void ShowUI()
        {
            _canvas.gameObject.SetActive(true);
        }

        public virtual void HideUI()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}