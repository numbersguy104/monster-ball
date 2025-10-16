//Commented out some stuff to move functionality to levelup hook (in GameStatsManager.cs)
//~Joseph

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UIGameMain : MonoBehaviour
    {
        public GameObject UIShopPanel;
        public TextMeshProUGUI reqPointsText;
        public Image[] Artifects;
        
        //private List<int> _thresholdPoint = new List<int>();
        //private int _level = 0;
        private GameObject _UIShopPanel;

        private GameStatsManager gm;
        
        private void Start()
        {
            gm = GameStatsManager.Instance;
            //Use the GameStatsManager hook for tracking level ups
            gm.OnLevelUp.AddListener(LevelUp);
            long threshold = gm.levelUpThreshold;
            reqPointsText.text = threshold.ToString();
            /*
            _thresholdPoint.Clear();
            var milestones = LubanTablesMgr.Instance.tables.TbMilestoneParam;
            foreach (var item in milestones.DataList)
            {
                _thresholdPoint.Add(item.MilestoneReq);
            }

            reqPointsText.text = _thresholdPoint[_level].ToString();
            */

            var afList = GameStatsManager.Instance.artifacts;
            var count = afList.Count;
            for (int i = 0; i < 3; i++)
            {
                if (i >= count)
                {
                    Artifects[i].gameObject.SetActive(false);
                }
                else
                {
                    Artifects[i].gameObject.SetActive(true);
                    var iconName = afList[i].AritfactIcon;
                    UICommonUtils.LoadArtifectIcon(Artifects[i], iconName);
                }
                
            }
        }

        private void LevelUp()
        {
            long threshold = gm.levelUpThreshold;
            reqPointsText.text = threshold.ToString();

            OpenShopPanel();
        }

        /*
        private void Update()
        {
            if (_level < _thresholdPoint.Count && _thresholdPoint[_level] <= GameStatsManager.Instance.score)
            {
                _level++;
                reqPointsText.text = _thresholdPoint[_level].ToString();
                OpenShopPanel();
            }
        }
        */

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

