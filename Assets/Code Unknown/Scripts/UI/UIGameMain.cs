using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace UI
{
    public class UIGameMain : MonoBehaviour
    {
        public GameObject UIShopPanel;
        public TextMeshProUGUI reqPointsText;
        
        private List<int> _thresholdPoint = new List<int>();
        private int _level = 0;
        private GameObject _UIShopPanel;
        
        private void Start()
        {
            _thresholdPoint.Clear();
            var milestones = LubanTablesMgr.Instance.tables.TbMilestoneParam;
            foreach (var item in milestones.DataList)
            {
                _thresholdPoint.Add(item.MilestoneReq);
            }

            reqPointsText.text = _thresholdPoint[_level].ToString();
        }

        private void Update()
        {
            if (_level < _thresholdPoint.Count && _thresholdPoint[_level] <= GameStatsManager.Instance.score)
            {
                _level++;
                reqPointsText.text = _thresholdPoint[_level].ToString();
                OpenShopPanel();
            }
        }

        private void OpenShopPanel()
        {
            if (_UIShopPanel == null)
            {
                GameObject canvas = GameObject.FindWithTag("canvas");
                _UIShopPanel = Instantiate(UIShopPanel, canvas.transform, false);
            }
            else
            {
                _UIShopPanel.SetActive(true);
            }
            Time.timeScale = 0f;
        }
    }
}

