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
        
        private List<int> ballWeights = new List<int>();
        
        private void Awake()
        {
            ballWeights.Clear();
            var balls = LubanTablesMgr.Instance.tables.TbBallParam;
            foreach (var ball in balls.DataList)
            {
                ballWeights.Add(ball.BallShopChance);
            }
        }

        public void Refresh()
        {
            var index = GetRandomIndex(ballWeights);
            var ball = LubanTablesMgr.Instance.tables.TbBallParam.DataList[index];
            string name = ball.ID;
            int price = ball.BallPrice;
            BallName.text = name;
            BallPrice.text = price.ToString();

            string balldesc = ball.BallDesc;
            BallDesc.text = balldesc;

            string iconName = ball.BallIcon;
            UICommonUtils.LoadBallIcon(BallIcon, iconName);
        }
        
        public static int GetRandomIndex(List<int> weights)
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
    }
}

