using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUpgrade : MonoBehaviour
    {
        // text
        public float SpeedUp = 0.1f;
        public float AtkUp = 0.1f;
        public float SizeUp = 0.1f;
        public float CritDMGUp = 0.1f;
        public float CritChanceUp = 0.1f;

        // slides
        public Image SpeedSlide;
        public Image AtkSlide;
        public Image SizeSlide;
        public Image CritDMGSlide;
        public Image CritChanceSlide;

        public GameObject ShopPanel;

        public GameObject NextBtn;

        private float _speedMulti;
        private float _atkMulti;
        private float _sizeMulti;
        private float _critDMGMulti;
        private float _critChanceMulti;

        private GameObject _UIShopPanel;
        private bool _canUpgrade;

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            NextBtn.SetActive(false);
            _canUpgrade = true;
        }

        public void OnClickSpeedUpgrade()
        {
            if (!_canUpgrade || _speedMulti >= 0.5f)
            {
                return;
            }
            _speedMulti += SpeedUp;
            SpeedSlide.fillAmount = _speedMulti / 0.5f;
            NextBtn.SetActive(true);
            _canUpgrade = false;
        }
        
        public void OnClickAtkUpgrade()
        {
            if (!_canUpgrade || _atkMulti >= 0.5f)
            {
                return;
            }
            _atkMulti += AtkUp;
            AtkSlide.fillAmount = _atkMulti / 0.5f;
            NextBtn.SetActive(true);
            _canUpgrade = false;
        }
        
        public void OnClickSizeUpgrade()
        {
            if (!_canUpgrade || _sizeMulti >= 0.5f)
            {
                return;
            }
            _sizeMulti += SizeUp;
            SizeSlide.fillAmount = _sizeMulti / 0.5f;
            NextBtn.SetActive(true);
            _canUpgrade = false;
        }
        
        public void OnClickCritDMGUpgrade()
        {
            if (!_canUpgrade || _critDMGMulti >= 0.5f)
            {
                return;
            }
            _critDMGMulti += CritDMGUp;
            CritDMGSlide.fillAmount = _critDMGMulti / 0.5f;
            NextBtn.SetActive(true);
            _canUpgrade = false;
        }
        
        public void OnClickCritChanceUpgrade()
        {
            if (!_canUpgrade || _critChanceMulti >= 0.5f)
            {
                return;
            }
            _critChanceMulti += CritChanceUp;
            CritChanceSlide.fillAmount = _critChanceMulti / 0.5f;
            NextBtn.SetActive(true);
            _canUpgrade = false;
        }

        public void OnClickNextBtn()
        {
            OpenShopPanel();
            gameObject.SetActive(false);
        }
        
        private void OpenShopPanel()
        {
            if (_UIShopPanel == null)
            {
                GameObject canvas = GameObject.FindWithTag("canvas");
                _UIShopPanel = Instantiate(ShopPanel, canvas.transform, false);
            }
            else
            {
                _UIShopPanel.SetActive(true);
            }
            Time.timeScale = 0f;
        }
    }
}

