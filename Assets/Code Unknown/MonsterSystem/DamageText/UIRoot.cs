using UnityEngine;

public class UIRoot : MonoBehaviour
{
    public static RectTransform Root;

    private void Awake()
    {
        Root = GetComponent<RectTransform>();
    }
}
