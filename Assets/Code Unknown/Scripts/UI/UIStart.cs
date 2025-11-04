using UnityEngine;

namespace UI
{
    public class UIStart : MonoBehaviour
    {

        public GameObject UIPrepare;
        private GameObject _UIPrepareObj;

        public GameObject UIStory;
        private GameObject _UIStoryObj;
        
        public void OnStartGameBtnClick()
        {
            if (_UIPrepareObj == null)
            {
                Canvas canvas = FindAnyObjectByType<Canvas>();
                _UIPrepareObj = Instantiate(UIPrepare, canvas.transform, false);
            }
            _UIPrepareObj.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnCollectionBtnClick()
        {
            if (_UIStoryObj == null)
            {
                Canvas canvas = FindAnyObjectByType<Canvas>();
                _UIStoryObj = Instantiate(UIStory, canvas.transform, false);
            }
            _UIStoryObj.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

