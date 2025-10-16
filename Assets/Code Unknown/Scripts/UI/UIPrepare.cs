using System;
using System.Collections.Generic;
using cfg;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIPrepare : MonoBehaviour
{
    public GameObject UIMain;
    public GameObject pinballMachine;
    public GameObject pinballDisplayItemObj;
    public Transform pinballDisplayRoot;
    public GameObject pinballPrepareItemObj;
    public Transform pinballPrepareRoot;
    public TextMeshProUGUI ballName;
    public TextMeshProUGUI ballDesc;
    public Sprite notReady;
    public Sprite ready;
    public Image readyIcon;

    private GameObject _UIMainObj;
    private CameraControl _cameraControl;
    private int _curID = 0;
    private GameObject _curGameObj;
    private List<int> _artifectList = new List<int>();

    public void OnBeginBtnClick()
    {
        if (_artifectList.Count <= 0)
        {
            return;
        }
        
        if (pinballMachine != null)
        {
            var machine = Instantiate(pinballMachine);
        }
        
        if (_UIMainObj == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            _UIMainObj = Instantiate(UIMain, canvas.transform, false);
        }
        _UIMainObj.SetActive(true);
        gameObject.SetActive(false);

        // CameraControl.BlendTo(true);
        if (_cameraControl == null)
        {
            _cameraControl = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        }
        
        _cameraControl.BlendTo(true);

        List<ArtifactParam> afList = new List<ArtifactParam>();
        var ap = LubanTablesMgr.Instance.tables.TbArtifactParam;
        foreach (var i in _artifectList)
        {
            var artifact = ap[i];
            afList.Add(artifact);
        }

        GameStatsManager.Instance.AddArtifacts(afList);
    }

    private void OnEnable()
    {
        SpawnDisplayItems();
        RefreshStatus();
    }

    void RefreshStatus()
    {
        readyIcon.sprite = _artifectList.Count > 0 ? ready : notReady;
        int count = pinballDisplayRoot.childCount;
        for (int i = 0; i < count; i++)
        {
            var ball = pinballDisplayRoot.GetChild(i).gameObject;
            if (ball.activeSelf)
            {
                var ballDisplay = ball.GetComponent<CommonIconDisplay>();
                // ballDisplay.OnClick();
                break;
            }

            if (i == count - 1)
            {
                ballName.text = String.Empty;
                ballDesc.text = String.Empty;
            }
        }
    }
    
    private void SpawnDisplayItems()
    {
        int count = pinballDisplayRoot.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            Destroy(pinballDisplayRoot.GetChild(i));
        }

        var balls = LubanTablesMgr.Instance.tables.TbArtifactParam;
        for (int i = 0; i < balls.DataList.Count; i++)
        {
            var ball = Instantiate(pinballDisplayItemObj, pinballDisplayRoot);
            var ballDisplay = ball.GetComponent<CommonIconDisplay>();
            string desc = string.Format(balls.DataList[i].ArtifactDes, balls.DataList[i].ArtifactStat1, balls.DataList[i].ArtifactStat2, balls.DataList[i].ArtifactStat3);
            ballDisplay.SetData(balls.DataList[i].ID, desc, balls.DataList[i].Id, ClickAdd, ShowDesc, balls.DataList[i].AritfactIcon);
            // if (i == 0)
            // {
            //     ballDisplay.OnClick();
            // }
        }
    }

    public void OnAddBtnClick()
    {
        AddItemToPrepare(_curID, _curGameObj);
        RefreshStatus();
    }
    
    private void AddItemToPrepare(int id, GameObject obj)
    {
        if (id < 0 || _artifectList.Contains(id))
        {
            return;
        }
        var balls = LubanTablesMgr.Instance.tables.TbArtifactParam;
        var ballCfg = balls[id];
        var ball = Instantiate(pinballPrepareItemObj, pinballPrepareRoot);
        var iconPrepare = ball.GetComponent<CommonIconPrepare>();
        iconPrepare.SetData(obj, id, ClickBack);
        // obj.SetActive(false);
        _artifectList.Add(id);
    }

    private void ShowDesc(string name, string desc)
    {
        ballName.text = name;
        ballDesc.text = desc;
    }

    private void ClickAdd(int id)
    {
        _curID = id;
        AddItemToPrepare(_curID, _curGameObj);
        RefreshStatus();
    }

    private void ClickBack(int id)
    {
        _artifectList.RemoveAll(i => i == id);
        RefreshStatus();
    }
}
