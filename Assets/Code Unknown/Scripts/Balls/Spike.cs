using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public event Action<MonsterController> OnHitMonster;
    private float timer = Mathf.Infinity;

    public void SetDuration(float t)
    {
        timer = t;
    }

    public void SetVelocity(Vector3 v)
    {
        rb.linearVelocity = v;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MonsterController mc = other.GetComponent<MonsterController>();
        if (mc != null)
        {
            OnHitMonster?.Invoke(mc);
        }
    }

}
