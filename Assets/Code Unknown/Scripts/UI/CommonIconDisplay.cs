using System;
using UnityEngine;


namespace UI
{
    public class CommonIconDisplay : MonoBehaviour
    {
        private string _ballName;
        private string _ballDes;
        private Action<string, string, int, GameObject> _ClickShow;
        private int _id;

        public void SetData(string name, string des, int id, Action<string, string, int, GameObject> ClickShow)
        {
            _ballName = name;
            _ballDes = des;
            _ClickShow = ClickShow;
            _id = id;
        }

        public void OnClick()
        {
            _ClickShow?.Invoke(_ballName, _ballDes, _id, gameObject);
        }
    }
}

