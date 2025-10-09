using UnityEngine;


namespace UI
{
    public class UIShop : MonoBehaviour
    {



        public void OnBtnSkip()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}

