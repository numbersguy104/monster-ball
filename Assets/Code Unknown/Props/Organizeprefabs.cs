using UnityEditor;
using UnityEngine;
using System.IO;

public class OrganizePrefabAssets
{
    [MenuItem("Assets/Organize Assets by Prefab")]
    private static void Organize()
    {
        // Get the selected prefab
        GameObject selectedPrefab = Selection.activeObject as GameObject;
        if (selectedPrefab == null || !PrefabUtility.IsPartOfPrefabAsset(selectedPrefab))
        {
            Debug.LogError("Please select a prefab asset in the Project window.");
            return;
        }

        // Get the path of the selected prefab
        string prefabPath = AssetDatabase.GetAssetPath(selectedPrefab);
        string prefabDirectory = Path.GetDirectoryName(prefabPath);

        // Create a new subfolder with the prefab's name
        string newFolderPath = Path.Combine(prefabDirectory, selectedPrefab.name + "_Assets");
        if (!AssetDatabase.IsValidFolder(newFolderPath))
        {
            AssetDatabase.CreateFolder(prefabDirectory, selectedPrefab.name + "_Assets");
        }

        // Get all dependencies of the prefab, excluding the prefab itself
        string[] dependencies = AssetDatabase.GetDependencies(prefabPath, true);

        foreach (string dependency in dependencies)
        {
            // Skip moving script files to prevent breaking project-wide dependencies
            if (dependency.EndsWith(".cs"))
            {
                continue;
            }

            // Get the file name and move the asset
            string fileName = Path.GetFileName(dependency);
            string newPath = Path.Combine(newFolderPath, fileName);

            // Only move if the asset isn't already in the target folder
            if (!dependency.StartsWith(newFolderPath))
            {
                AssetDatabase.MoveAsset(dependency, newPath);
            }
        }

        Debug.Log($"Moved assets for '{selectedPrefab.name}' to {newFolderPath}");
    }
}