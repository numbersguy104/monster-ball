using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UI
{
    public class CommonIconDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string _ballName;
        private string _ballDes;
        private Action<int> _ClickShow;
        private Action<string, string> _HoverEnter;
        private int _id;

        public void SetData(string name, string des, int id, Action<int> ClickShow, Action<string, string> HoverEnter)
        {
            _ballName = name;
            _ballDes = des;
            _ClickShow = ClickShow;
            _id = id;
            _HoverEnter = HoverEnter;
        }

        public void OnClick()
        {
            _ClickShow?.Invoke(_id);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _HoverEnter?.Invoke(_ballName, _ballDes);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _HoverEnter?.Invoke("", "");
        }
    }
}

