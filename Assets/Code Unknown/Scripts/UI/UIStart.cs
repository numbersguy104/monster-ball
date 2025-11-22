using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace UI
{
    public class UIStart : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public GameObject UIPrepare;
        private GameObject _UIPrepareObj;

        public GameObject UIStory;
        private GameObject _UIStoryObj;

        public GameObject UIGuide;
        private GameObject _UIGuideObj;
        
        public void OnStartGameBtnClick()
        {
            if (_UIPrepareObj == null)
            {
                Canvas canvas = FindAnyObjectByType<Canvas>();
                _UIPrepareObj = Instantiate(UIPrepare, canvas.transform, false);
            }
            _UIPrepareObj.SetActive(true);
            gameObject.SetActive(false);

            RectTransform rect = _UIPrepareObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, Screen.height);
            
            rect.DOAnchorPos(Vector2.zero, 0.6f)
                .SetEase(Ease.OutBack); 
        }

        private void Start()
        {
            /*
            var seq = DOTween.Sequence();

            seq.AppendCallback(() => title.gameObject.SetActive(true));
            seq.AppendInterval(1f);
            seq.AppendCallback(() => title.gameObject.SetActive(false));
            seq.AppendInterval(1f);

            seq.SetLoops(-1);
            */
        }

        public void OnCollectionBtnClick()
        {
            if (_UIStoryObj == null)
            {
                Canvas canvas = FindAnyObjectByType<Canvas>();
                _UIStoryObj = Instantiate(UIStory, canvas.transform, false);
            }
            _UIStoryObj.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnGuideBtnClick()
        {
            if (_UIGuideObj == null)
            {
                Canvas canvas = FindAnyObjectByType<Canvas>();
                _UIGuideObj = Instantiate(UIGuide, canvas.transform, false);
            }
            _UIGuideObj.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnExitBtnClick()
        {
            Application.Quit();
        }
    }
}