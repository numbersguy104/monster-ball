using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


namespace UI
{
    public class ShopOption : MonoBehaviour
    {
        public TextMeshProUGUI BallName;
        public TextMeshProUGUI BallPrice;
        public Image BallIcon;
        public TextMeshProUGUI BallDesc;
        public Button purchase;
        
        private List<int> ballWeights = new List<int>();
        private int _price;
        private string _ballName;
        private Action _refresh;
        
        private void Awake()
        {
            ballWeights.Clear();
            var balls = LubanTablesMgr.Instance.tables.TbBallParam;
            foreach (var ball in balls.DataList)
            {
                ballWeights.Add(ball.BallShopChance);
            }
        }

        public void Refresh(Action refresh = null)
        {
            var index = GetRandomIndex(ballWeights);
            var ball = LubanTablesMgr.Instance.tables.TbBallParam.DataList[index];
            _ballName = ball.ID;
            _price = ball.BallPrice;
            BallName.text = _ballName;
            BallPrice.text = _price.ToString();

            string balldesc = ball.BallDesc;
            BallDesc.text = balldesc;

            string iconName = ball.BallIcon;
            UICommonUtils.LoadBallIcon(BallIcon, iconName);

            if (refresh != null)
            {
                _refresh = refresh;
            }
        }

        private static int GetRandomIndex(List<int> weights)
        {
            int total = weights.Sum();
            int rand = new Random().Next(0, total);
            int cumulative = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                cumulative += weights[i];
                if (rand < cumulative)
                    return i;
            }
            return weights.Count - 1;
        }

        public void OnClickPurchase()
        {
            if (GameStatsManager.Instance.gold < _price)
            {
                return;
            }

            GameStatsManager.Instance.SpendGold(_price);
            PinballQueue.Instance.AddBall(_ballName);
            Refresh();
        }
    }
}

