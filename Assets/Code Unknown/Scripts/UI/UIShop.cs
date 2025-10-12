using System;
using TMPro;
using UnityEngine;


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
                so.Refresh();
            }
        }

        public void OnBtnSkip()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        public void OnBtnReroll()
        {
            RefreshNewBalls();
            RefreshStatus();
        } 
    }
}

