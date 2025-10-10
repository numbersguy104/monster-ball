using System;
using UnityEngine;


namespace UI
{
    public class CommonIconPrepare : MonoBehaviour
    {

        private GameObject _originIconObj;
        private int _id;
        private Action<int> _ClickBack;

        public void SetData(GameObject originIcon, int id, Action<int> ClickBack)
        {
            _originIconObj = originIcon;
            _id = id;
            _ClickBack = ClickBack;
        }

        public void OnBtnClick()
        {
            // _originIconObj.SetActive(true);
            _ClickBack?.Invoke(_id);
            
            Destroy(gameObject);
        }
    }
}

