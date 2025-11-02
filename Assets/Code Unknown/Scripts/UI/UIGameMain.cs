using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
    public class UIGameMain : MonoBehaviour
    {
        [Tooltip("Finish Panel when Game Over")]
        [SerializeField] GameObject UIFinish;

        public GameObject UIShopPanel;
        public TextMeshProUGUI reqPointsText;
        public TextMeshProUGUI gold;
        public TextMeshProUGUI killsReq;
        public TextMeshProUGUI killsLvl;
        public TextMeshProUGUI killsAll;
        public TextMeshProUGUI activeBallCount;
        public TextMeshProUGUI ballQueueCount;
        public Image[] Artifects;
        public Image[] BallIcons;
        
        //private List<int> _thresholdPoint = new List<int>();
        //private int _level = 0;
        private GameObject _UIShopPanel;
        private GameStatsManager gm;
        private BallCounter bc;
        private PointsTracker pt;
        
        private void Start()
        {
            gm = GameStatsManager.Instance;
            //Use the GameStatsManager hook for tracking level ups
            gm.OnLevelUp.AddListener(LevelUp);
            long threshold = gm.levelUpThreshold;
            reqPointsText.text = threshold.ToString();
            killsReq.text = gm.killReq.ToString();
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

            bc = FindAnyObjectByType<BallCounter>();
            pt = FindAnyObjectByType<PointsTracker>();
        }

        private void Update()
        {
            int count = bc.CountActive();
            float mult = pt.GetMultiballMult();
            activeBallCount.text = count.ToString() + " (x" + mult.ToString() + ")";
        }

        private void LevelUp()
        {
            long threshold = gm.levelUpThreshold;
            reqPointsText.text = threshold.ToString();
            killsReq.text = gm.killReq.ToString();
            killsLvl.text = gm.lvlKills.ToString();
            killsAll.text = gm.killCount.ToString();

            OpenShopPanel();
        }

        //Old code for levelup
        //Functionality has been moved to LevelUp() using the GameStatsManager hook
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

        public void OpenGameOverPanel()
        {
            GameObject canvas = GameObject.FindWithTag("canvas");
            var _UIFinishObj = Instantiate(UIFinish, canvas.transform, false);
        }

        public void Refresh()
        {
            var pointTracker = FindFirstObjectByType<PointsTracker>();
            pointTracker.RefreshPoint();
            gold.text = GameStatsManager.Instance.gold.ToString();
            killsLvl.text = gm.lvlKills.ToString();
            killsAll.text = gm.killCount.ToString();
        }
        
        public void OnClickBackToTitle()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        public void UpdateBallQueue(Queue<AbstractBall> ballQueue)
        {
            AbstractBall[] balls = ballQueue.ToArray();
            int count = balls.Length;
            int i = 0;
            foreach (Image ballImage in BallIcons)
            {
                if (i < count)
                {
                    AbstractBall ball = balls[i];
                    UICommonUtils.LoadBallIcon(ballImage, ball.name);
                    ballImage.enabled = true;
                }
                else
                {
                    ballImage.enabled = false;
                }
                i++;
            }
            if (count > 5)
            {
                ballQueueCount.enabled = true;
                ballQueueCount.text = "(+" + (count-5).ToString() + ")";
            } else
            {
                ballQueueCount.enabled = false;
            }
        }
    }
}

