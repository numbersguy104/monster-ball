using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


namespace UI
{
    public class UIFinish : MonoBehaviour
    {
        public TextMeshProUGUI terrainPointsText;
        public TextMeshProUGUI monsterSlainText;
        public TextMeshProUGUI goldEarnedText;
        public TextMeshProUGUI ballUsedText;
        public TextMeshProUGUI totalPointsText;
        public TextMeshProUGUI damageDealtText;

        public GameObject level1;
        public GameObject UIMain;

        private void Start()
        {
            ShowTexts();
        }

        public void OnReplayButtonClick()
        {
            var _level1 = GameObject.FindWithTag("level");
            Destroy(_level1);
            var machine = Instantiate(level1);

            var uiMain = FindObjectOfType<UIGameMain>();
            Destroy(uiMain.gameObject);
            var uiupgrade = FindObjectOfType<UIUpgrade>();
            if (uiupgrade != null)
            {
                Destroy(uiupgrade);
            }
            GameObject canvas = GameObject.FindWithTag("canvas");
            var _UIMainObj = Instantiate(UIMain, canvas.transform, false);
            Destroy(gameObject);
        }

        public void OnClickBackToTitle()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        public void ShowTexts()
        {
            var terrainPoint = FindFirstObjectByType<PointsTracker>().GetTerrainPoints();
            string terrainPointStr = "Terrain Points" + new string('.', 25 - 14 - terrainPoint.ToString().Length) + terrainPoint.ToString();
            ShowTextAnim(terrainPointsText, terrainPointStr, 3f);
            
            var monsterSlain = GameStatsManager.Instance.killCount;
            string monsterSlainStr = "Monster Slain" + new string('.', 25 - 13 - monsterSlain.ToString().Length) + monsterSlain.ToString();
            ShowTextAnim(monsterSlainText, monsterSlainStr, 3f);
            
            var ballUsed = FindFirstObjectByType<BallCounter>().countTotal;
            string ballUsedtStr = "Ball Used" + new string('.', 25 - 9 - ballUsed.ToString().Length) + ballUsed.ToString();
            ShowTextAnim(ballUsedText, ballUsedtStr, 3f);
            
            var totalPoint = GameStatsManager.Instance.score;
            string totalPointStr = "Total Points" + new string('.', 25 - 12 - totalPoint.ToString().Length) + totalPoint.ToString();
            ShowTextAnim(totalPointsText, totalPointStr, 3f);
        }

        void ShowTextAnim(TextMeshProUGUI textMesh, string content, float time)
        {
            textMesh.text = "";
            textMesh.DOText(content, time);
        }
    }
}

