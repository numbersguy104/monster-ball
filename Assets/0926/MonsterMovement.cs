using UnityEngine;
//处理三种移动模式（Horizontal / Vertical / Circular）
public enum MovementType { None, Horizontal, Vertical, Circular }

public class MonsterMovement : MonoBehaviour
{
    public MovementType movementType = MovementType.None;
    public float speed = 1f;

    // horizontal/vertical config
    public float length = 0f; // patrol half-length from spawn centre

    // circular config
    public float radius = 0f;

    Vector3 spawnCenter;
    float angle = 0f;
    int dir = 1;
    void Start()
    {
        spawnCenter = transform.position;
    }

    void Update()
    {
        if (movementType == MovementType.Horizontal)
        {
            if (length <= 0) return;
            float x = Mathf.PingPong(Time.time * speed, length * 2) - length;
            transform.position = spawnCenter + new Vector3(x, 0, 0);
        }
        else if (movementType == MovementType.Vertical)
        {
            if (length <= 0) return;
            float y = Mathf.PingPong(Time.time * speed, length * 2) - length;
            transform.position = spawnCenter + new Vector3(0, y, 0);
        }
        else if (movementType == MovementType.Circular)
        {
            if (radius <= 0) return;
            angle += speed * Time.deltaTime; // speed treated as angular speed (radians/sec)
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            transform.position = spawnCenter + new Vector3(x, y, 0);
        }
    }

    // helper to set center when spawned to ensure patrol centre is spawn point
    public void SetSpawnCenter(Vector3 center)
    {
        spawnCenter = center;
    }
}
