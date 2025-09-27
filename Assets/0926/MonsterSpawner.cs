using System.Collections.Generic;
using UnityEngine;
//生成器组件
public enum SpawnerCollisionBehavior { WithCollision, WithoutCollision, Mixed }

[ExecuteAlways]
public class MonsterSpawner : MonoBehaviour
{
    [Header("References")]
    public MonsterDatabase database;

    [Header("Spawner Behavior")]
    public SpawnerCollisionBehavior collisionBehavior = SpawnerCollisionBehavior.Mixed;

    [Header("Movement (spawner default, injected to spawned monster)")]
    public MovementType movementType = MovementType.None;
    public float movementSpeed = 1f;
    [Tooltip("If horizontal/vertical: this is half-length (units to each side). If circular: this is radius.")]
    public float movementLengthOrRadius = 0f;

    [Header("Runtime")]
    public bool hasSpawnedMonster = false;
    public GameObject spawnedInstance;

    [Header("Spawn Options")]
    public bool spawnOnStart = true;

    // Spawn now (manual call)
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

        // instantiate
        GameObject inst = Instantiate(choice.prefab, transform.position, transform.rotation);
        var mc = inst.GetComponent<MonsterController>();
        if (mc != null)
        {
            mc.InitializeFromParam(choice);
            // set movement parameters
            MovementType mt = ConvertMovementType(movementType);
            mc.SetMovement(mt, movementSpeed, movementLengthOrRadius);
        }
        else
        {
            Debug.LogWarning("Spawned prefab missing MonsterController component.");
        }

        // keep track
        spawnedInstance = inst;
        hasSpawnedMonster = true;

        // return ID
        return choice.monsterID;
    }

    public void DespawnCurrent()
    {
        if (spawnedInstance != null)
        {
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

    MovementType ConvertMovementType(MovementType m)
    {
        return m;
    }

    // Example: auto spawn on start (for testing)
    void Start()
    {
        if (!Application.isPlaying) return;
        if (spawnOnStart && !hasSpawnedMonster)
        {
            SpawnRandomMonster();
        }
    }
}
