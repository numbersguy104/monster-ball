using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonsterSpawner))]
public class MonsterSpawnerEditor : Editor
{
    SerializedProperty databaseProp;
    SerializedProperty collisionBehaviorProp;
    SerializedProperty movementTypeProp;
    SerializedProperty movementSpeedProp;
    SerializedProperty movementLengthOrRadiusProp;
    SerializedProperty spawnOnStartProp;
    SerializedProperty hasSpawnedMonsterProp;
    SerializedProperty spawnedInstanceProp;

    void OnEnable()
    {
        databaseProp = serializedObject.FindProperty("database");
        collisionBehaviorProp = serializedObject.FindProperty("collisionBehavior");
        movementTypeProp = serializedObject.FindProperty("movementType");
        movementSpeedProp = serializedObject.FindProperty("movementSpeed");
        movementLengthOrRadiusProp = serializedObject.FindProperty("movementLengthOrRadius");
        spawnOnStartProp = serializedObject.FindProperty("spawnOnStart");
        hasSpawnedMonsterProp = serializedObject.FindProperty("hasSpawnedMonster");
        spawnedInstanceProp = serializedObject.FindProperty("spawnedInstance");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(databaseProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spawner Behavior", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(collisionBehaviorProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Movement Settings (applied to spawned monster)", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(movementTypeProp);

        MovementType movementType = (MovementType)movementTypeProp.enumValueIndex;

        // always show speed entry if any movement selected (per requirement)
        if (movementType != MovementType.None)
        {
            EditorGUILayout.PropertyField(movementSpeedProp, new GUIContent("Movement Speed"));
        }

        // show length or radius depending on selection
        if (movementType == MovementType.Horizontal || movementType == MovementType.Vertical)
        {
            EditorGUILayout.PropertyField(movementLengthOrRadiusProp, new GUIContent("Movement Length (half-range)"));
            EditorGUILayout.HelpBox("Movement length is half-range. Example: length=5 => patrol 5 units to each side.", MessageType.Info);
        }
        else if (movementType == MovementType.Circular)
        {
            EditorGUILayout.PropertyField(movementLengthOrRadiusProp, new GUIContent("Radius"));
            EditorGUILayout.HelpBox("Radius indicates size of circular path.", MessageType.Info);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime / Test Controls", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(spawnOnStartProp);

        serializedObject.ApplyModifiedProperties();

        // Spawn / Despawn buttons (for Edit mode testing)
        MonsterSpawner spawner = (MonsterSpawner)target;

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Now"))
        {
            if (!Application.isPlaying)
            {
                // create prefab instance in editor
                string returnedID = spawner.SpawnRandomMonster();
                if (!string.IsNullOrEmpty(returnedID))
                    Debug.Log($"Spawned monster ID: {returnedID}");
            }
            else
            {
                spawner.SpawnRandomMonster();
            }
        }

        if (GUILayout.Button("Despawn"))
        {
            spawner.DespawnCurrent();
        }
        EditorGUILayout.EndHorizontal();

        // Show spawned prefab reference
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spawned Instance", EditorStyles.boldLabel);
        if (spawner.spawnedInstance != null)
        {
            EditorGUILayout.ObjectField(spawner.spawnedInstance, typeof(GameObject), true);
        }
        else
        {
            EditorGUILayout.LabelField("No monster currently spawned.");
        }
    }
}
