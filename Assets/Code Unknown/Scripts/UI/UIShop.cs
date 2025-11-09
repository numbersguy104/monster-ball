using System;
using TMPro;
using UnityEngine;
using DG.Tweening;


namespace UI
{
    public class UIShop : MonoBehaviour
    {
        public ShopOption[] ShopOptions;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI goldText;
        
        private void Start()
        {
            
            RefreshNewBalls();
            RefreshStatus();
        }

        void RefreshStatus()
        {
            var score = GameStatsManager.Instance.score;
            var gold = GameStatsManager.Instance.gold;
            scoreText.text = score.ToString();
            goldText.text = gold.ToString();
        }

        void RefreshNewBalls()
        {
            foreach (var sop in ShopOptions)
            {
                RefreshNewBall(sop);
            }
        }

        void RefreshNewBall(ShopOption so)
        {
            if (so != null)
            {
                so.Refresh(RefreshStatus);
            }
        }

        public void OnBtnSkip()
        {
            // Time.timeScale = 1f;
            // gameObject.SetActive(false);
            var mainUI = FindFirstObjectByType<UIGameMain>();
            mainUI.Refresh();
            
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            // rect.anchoredPosition = new Vector2(0, Screen.height);
            Time.timeScale = 1f;

            rect.DOAnchorPos(new Vector2(0, Screen.height), 0.6f)
                .SetEase(Ease.InBack).OnComplete(
                    () =>
                    {
                        gameObject.SetActive(false);
                    }
                );
        }

        public void OnBtnReroll()
        {
            RefreshNewBalls();
            RefreshStatus();
        } 
    }
}

