using UnityEngine;

public class UIPrepare : MonoBehaviour
{
    public GameObject UIMain;
    public GameObject pinballMachine;

    private GameObject _UIMainObj;
    private CameraControl _cameraControl;

    public void OnBeginBtnClick()
    {
        if (_UIMainObj == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            _UIMainObj = Instantiate(UIMain, canvas.transform, false);
        }
        _UIMainObj.SetActive(true);
        gameObject.SetActive(false);


        if (pinballMachine != null)
        {
            var machine = Instantiate(pinballMachine);
        }

        // CameraControl.BlendTo(true);
        if (_cameraControl == null)
        {
            _cameraControl = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        }
        
        _cameraControl.BlendTo(true);
    }
}
