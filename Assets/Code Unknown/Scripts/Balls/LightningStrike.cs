using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    private float timer = Mathf.Infinity;

    public void SetDuration(float t)
    {
        timer = t;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

}
