using System;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class CommonIconPrepare : MonoBehaviour
    {
        public Image Icon;
        
        // private string _icon;
        private GameObject _originIconObj;
        private int _id;
        private Action<int> _ClickBack;
        
        

        public void SetData(GameObject originIcon, int id, Action<int> ClickBack)
        {
            _originIconObj = originIcon;
            _id = id;
            _ClickBack = ClickBack;

            var arti = LubanTablesMgr.Instance.tables.TbArtifactParam[id].AritfactIcon;
            
            UICommonUtils.LoadArtifectIcon(Icon, arti);
        }

        public void OnBtnClick()
        {
            // _originIconObj.SetActive(true);
            _ClickBack?.Invoke(_id);
            
            Destroy(gameObject);
        }
    }
}

