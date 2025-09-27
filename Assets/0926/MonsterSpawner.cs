using System.Collections.Generic;
using UnityEngine;

public enum SpawnerCollisionBehavior { WithCollision, WithoutCollision, Mixed }

[ExecuteAlways]
public class MonsterSpawner : MonoBehaviour
{
    [Header("References")]
    public MonsterDatabase database;

    [Header("Spawner Collision Behavior")]
    public SpawnerCollisionBehavior collisionBehavior = SpawnerCollisionBehavior.Mixed;

    [Header("Spawner Movement Settings")]
    public MovementType spawnerMovementType = MovementType.None;
    public float spawnerSpeed = 1f;
    [Tooltip("If horizontal/vertical: this is half-length (units to each side). If circular: this is radius.")]
    public float spawnerLengthOrRadius = 0f;

    private Vector3 spawnerOrigin;
    private float angle = 0f;

    [Header("Movement Injected To Spawned Monsters")]
    public MovementType monsterMovementType = MovementType.None;
    public float monsterSpeed = 1f;
    [Tooltip("If horizontal/vertical: half-length. If circular: radius.")]
    public float monsterLengthOrRadius = 0f;

    [Header("Runtime")]
    public bool hasSpawnedMonster = false;
    public GameObject spawnedInstance;

    [Header("Spawn Options")]
    public bool spawnOnStart = true;

    void Start()
    {
        if (!Application.isPlaying) return;

        spawnerOrigin = transform.position;

        if (spawnOnStart && !hasSpawnedMonster)
            SpawnRandomMonster();
    }

    void Update()
    {
        if (!Application.isPlaying) return;

        UpdateSpawnerMovement();
    }

    void UpdateSpawnerMovement()
    {
        if (spawnerMovementType == MovementType.None) return;

        if (spawnerOrigin == Vector3.zero) spawnerOrigin = transform.position;

        if (spawnerMovementType == MovementType.Horizontal)
        {
            float x = Mathf.PingPong(Time.time * spawnerSpeed, spawnerLengthOrRadius * 2) - spawnerLengthOrRadius;
            transform.position = spawnerOrigin + new Vector3(x, 0, 0);
        }
        else if (spawnerMovementType == MovementType.Vertical)
        {
            float y = Mathf.PingPong(Time.time * spawnerSpeed, spawnerLengthOrRadius * 2) - spawnerLengthOrRadius;
            transform.position = spawnerOrigin + new Vector3(0, y, 0);
        }
        else if (spawnerMovementType == MovementType.Circular)
        {
            angle += spawnerSpeed * Time.deltaTime;
            float x = Mathf.Cos(angle) * spawnerLengthOrRadius;
            float y = Mathf.Sin(angle) * spawnerLengthOrRadius;
            transform.position = spawnerOrigin + new Vector3(x, y, 0);
        }
    }

    public string SpawnRandomMonster()
    {
        if (database == null)
        {
            Debug.LogWarning("MonsterSpawner: database not assigned.");
            return null;
        }

        List<MonsterParam> choices = GetEligibleMonsterList();
        if (choices == null || choices.Count == 0)
        {
            Debug.LogWarning("MonsterSpawner: no eligible monsters found for selection.");
            return null;
        }

        MonsterParam choice = choices[Random.Range(0, choices.Count)];
        if (choice == null || choice.prefab == null)
        {
            Debug.LogWarning("MonsterSpawner: chosen monster or prefab null.");
            return null;
        }

        // instantiate at spawner's current position
        GameObject inst = Instantiate(choice.prefab, transform.position, transform.rotation);
        var mc = inst.GetComponent<MonsterController>();
        if (mc != null)
        {
            mc.InitializeFromParam(choice);

            // 设置怪物的运动模式，传入生成时 spawner 的位置
            mc.SetMovement(monsterMovementType, monsterSpeed, monsterLengthOrRadius, transform.position);
        }
        else
        {
            Debug.LogWarning("Spawned prefab missing MonsterController component.");
        }

        spawnedInstance = inst;
        hasSpawnedMonster = true;

        return choice.monsterID;
    }

    public void DespawnCurrent()
    {
        if (spawnedInstance != null)
        {
            if (Application.isPlaying)
                Destroy(spawnedInstance);
            else
                DestroyImmediate(spawnedInstance);

            spawnedInstance = null;
            hasSpawnedMonster = false;
        }
    }

    List<MonsterParam> GetEligibleMonsterList()
    {
        if (database == null) return null;
        if (collisionBehavior == SpawnerCollisionBehavior.Mixed)
            return database.GetAll();
        else if (collisionBehavior == SpawnerCollisionBehavior.WithCollision)
            return database.GetByCollision(MonsterCollisionType.WithCollision);
        else
            return database.GetByCollision(MonsterCollisionType.WithoutCollision);
    }
}
