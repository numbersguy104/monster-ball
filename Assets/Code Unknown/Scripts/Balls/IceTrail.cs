using System.Collections.Generic;
using UnityEngine;

public class IceTrail : MonoBehaviour
{
    private static Dictionary<MonsterController, int> touchingMonsters = new Dictionary<MonsterController, int>();
    private float timer = Mathf.Infinity;
    private List<MonsterController> overlapMonsters;

    public void SetDuration(float t)
    {
        timer = t;
    }

    public List<MonsterController> GetMonsters()
    {
        return overlapMonsters;
    }

    private void Awake()
    {
        overlapMonsters = new List<MonsterController>();
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
            if (!touchingMonsters.ContainsKey(mc))
            {
                mc.PauseMovement();
                touchingMonsters[mc] = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MonsterController mc = other.GetComponent<MonsterController>();
        if (mc != null)
        {
            overlapMonsters.Remove(mc);
            if (touchingMonsters.TryGetValue(mc, out int count))
            {
                touchingMonsters[mc] = count - 1;
                if (count - 1 <= 0)
                {
                    mc.ResumeMovement();
                    touchingMonsters.Remove(mc);
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var mc in overlapMonsters)
        {
            if (touchingMonsters.TryGetValue(mc, out int count))
            {
                touchingMonsters[mc] = count - 1;
                if (count - 1 <= 0)
                {
                    mc.ResumeMovement();
                    touchingMonsters.Remove(mc);
                }
            }
        }
        overlapMonsters.Clear();
    }
}
