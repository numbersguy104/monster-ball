using System.Collections.Generic;
using UnityEngine;

public class NuclearArea : MonoBehaviour
{
    private float timer = Mathf.Infinity;
    private List<MonsterController> overlapMonsters = new List<MonsterController>();

    public void SetDuration(float t)
    {
        timer = t;
    }

    public List<MonsterController> GetMonsters()
    {
        return overlapMonsters;
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
            overlapMonsters.Add(mc);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MonsterController mc = other.GetComponent<MonsterController>();
        if (mc != null)
        {
            overlapMonsters.Remove(mc);
        }
    }

}
