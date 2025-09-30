using UnityEngine;

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
    float spawnTime; // 记录生成时间
    private Quaternion orbitRotation = Quaternion.identity;
    private Quaternion visualRotation = Quaternion.identity;

    void Start()
    {
        spawnTime = Time.time; // 记录怪物生成时的时间

        // 如果是圆形运动，给 angle 一个随机偏移，避免所有怪物同步
        if (movementType == MovementType.Circular)
        {
            angle = Random.Range(0f, Mathf.PI * 2f);
        }
        visualRotation = transform.rotation;
    }

    void Update()
    {
        float deltaTime = Time.time - spawnTime;

        switch (movementType)
        {
            case MovementType.Horizontal:
                if (length <= 0) return;
                float x = Mathf.PingPong(deltaTime * speed + length, length * 2) - length;
                transform.position = spawnCenter + new Vector3(x, 0, 0);
                break;

            case MovementType.Vertical:
                if (length <= 0) return;
                float y = Mathf.PingPong(deltaTime * speed + length, length * 2) - length;
                transform.position = spawnCenter + new Vector3(0, y, 0);
                break;

            case MovementType.Circular:
                if (radius <= 0) return;
                // 保留 angle 偏移，避免同步
                angle += speed * Time.deltaTime;
                Vector3 local = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
                Vector3 worldOffset = orbitRotation * local;
                transform.position = spawnCenter + worldOffset;
                transform.rotation = visualRotation;
                /*float cx = Mathf.Cos(angle) * radius;
                float cy = Mathf.Sin(angle) * radius;
                transform.position = spawnCenter + new Vector3(cx, cy, 0);*/
                break;
        }
    }

    public void SetSpawnCenter(Vector3 center , Quaternion orbitRot)
    {
        spawnCenter = center;
        spawnTime = Time.time;
        orbitRotation = orbitRot;
        visualRotation = transform.rotation;
    }
    public void SetSpawnCenter(Vector3 center)
    {
        SetSpawnCenter(center, Quaternion.identity);
    }
}
