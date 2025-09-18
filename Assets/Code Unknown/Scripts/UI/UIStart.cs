using UnityEngine;

namespace UI
{
    public class UIStart : MonoBehaviour
    {

        public GameObject UIMain;

        private GameObject _UIMainObj;
        
        public void OnStartGameBtnClick()
        {
            if (_UIMainObj == null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                _UIMainObj = Instantiate(UIMain, canvas.transform, false);
            }
            _UIMainObj.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

