using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace UI
{
    public class CommonBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TextMeshProUGUI text;
        public Image background;
        public float duration;
        public TMP_FontAsset FontAsset0;
        public TMP_FontAsset FontAsset1;

        private Tween _currentTween;
    
        private void Start()
        {
        
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentTween?.Kill();
            _currentTween = background.DOFillAmount(1f, duration).SetEase(Ease.OutCubic);
            text.color = new Color(0, 0, 0, 1);
            text.font = FontAsset1;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentTween?.Kill();
            _currentTween = background.DOFillAmount(0f, duration).SetEase(Ease.OutCubic);
            text.color = new Color(1, 1, 1, 1);
            text.font = FontAsset0;
        }
    }
}
