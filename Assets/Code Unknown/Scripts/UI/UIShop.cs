using System;
using UnityEngine;


namespace UI
{
    public class UIShop : MonoBehaviour
    {
        public ShopOption[] ShopOptions;
        
        
        private void Start()
        {
            var balls = LubanTablesMgr.Instance.tables.TbBallParam;
            RefreshNewBalls();
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
    }
}

