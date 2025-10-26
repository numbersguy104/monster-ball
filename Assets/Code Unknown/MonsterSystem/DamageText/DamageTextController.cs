using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    public float floatSpeed = 40f;   
    public float duration = 1.2f;

    private float timer;
    private TextMeshProUGUI text;
    private Camera cam;


    private Vector3 worldPos;

    private Vector3 screenPos;

    public void Init(int damage, bool crit, Vector3 wp)
    {
        worldPos = wp;
        cam = Camera.main;

        text.text = damage.ToString();
        text.color = crit ? Color.yellow : Color.white;
        text.fontSize = crit ? 40 : 30;

        timer = duration;
    }

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
            return;
        }


        screenPos = cam.WorldToScreenPoint(worldPos);


        screenPos.y += floatSpeed * Time.deltaTime;


        transform.position = screenPos;
    }
}
