using NUnit.Framework;
using System.Collections.Generic;
using cfg;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [Tooltip("Number of monsters that will be on the field for each level")]
    [SerializeField] private List<int> monsterCounts = null;

    private int monsterCount;
    private MonsterSpawner[] spawners;

    private void Start()
    {
        //Listen to the level up event
        GameStatsManager.Instance.OnLevelUp.AddListener(LevelUp);

        spawners = GetComponentsInChildren<MonsterSpawner>();

        //Instantiate monster amounts to a default if not set in inspector
        if (monsterCounts == null || monsterCounts.Count == 0)
        {
            // Debug.LogWarning("Monster Counts field was not initialized! Initializing with a single monster cap of 3. Source: MonsterSpawnManager.cs on object " + gameObject.ToString());
            // monsterCounts = new List<int>();
            // monsterCounts.Add(3);

            TbMilestoneParam milestoneParam = LubanTablesMgr.Instance.tables.TbMilestoneParam;
            foreach (var milestone in milestoneParam.DataList)
            {
                monsterCounts.Add(milestone.MonsterSpawnSCount);
            }
        }

        LevelUp();
    }

    private void Update()
    {
        //Get which spawners have monsters spawned, and which spawners don't
        List<MonsterSpawner> activeSpawners = new List<MonsterSpawner>();
        List<MonsterSpawner> inactiveSpawners = new List<MonsterSpawner>();
        foreach (var spawner in spawners)
        {
            if (spawner.hasSpawnedMonster)
            {
                activeSpawners.Add(spawner);
            }
            else
            {
                inactiveSpawners.Add(spawner);
            }
        }

        //Spawn in any missing monsters, according to the current count
        int currentCount = activeSpawners.Count;
        while (currentCount < monsterCount)
        {
            int nextSpawnerIndex = Random.Range(0, inactiveSpawners.Count);
            MonsterSpawner nextSpawner = inactiveSpawners[nextSpawnerIndex];
            nextSpawner.SpawnRandomMonster();
            currentCount++;
        }
    }

    //For each level up, pop a new cap for the amount of monsters
    //(This is called by the GameStatsManager's OnLevelUp listener)
    private void LevelUp()
    {
        monsterCount = monsterCounts[0] + (int)GameStatsManager.Instance.artifactMonsterSpawnSCount;
        monsterCounts.RemoveAt(0);
    }
}
