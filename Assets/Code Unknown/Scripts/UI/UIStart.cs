using UnityEngine;

namespace UI
{
    public class UIStart : MonoBehaviour
    {

        public GameObject UIPrepare;

        private GameObject _UIPrepareObj;
        
        public void OnStartGameBtnClick()
        {
            if (_UIPrepareObj == null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                _UIPrepareObj = Instantiate(UIPrepare, canvas.transform, false);
                
            }
            _UIPrepareObj.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

