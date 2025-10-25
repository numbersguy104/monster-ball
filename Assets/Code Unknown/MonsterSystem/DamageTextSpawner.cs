using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance;

    public RectTransform Root;
    public GameObject damageTextPrefab;
    public Camera mainCamera;

    void Awake()
    {
        Instance = this;
    }

    public void Spawn(Vector3 worldPos, int damage, bool crit)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        GameObject obj = Instantiate(damageTextPrefab, Root);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = screenPos;

        obj.GetComponent<DamageTextController>().Init(damage, crit,worldPos);
    }
}
