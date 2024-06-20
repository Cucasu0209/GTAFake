using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//public class FindMissingScripts: MonoBehaviour
public static class FindMissingScripts
{
    //[Button(ButtonHeight = 50)]
    [MenuItem("Ultilities/Find Missing script in project")]
    private static void FindMissingScriptOnWholeProject()
    {
        string[] prefabPaths = AssetDatabase.GetAllAssetPaths().Where(path
            => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();
        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            foreach (Component component in prefab.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    Debug.LogError("Prefab found with missing script: " + path, prefab);
                    break;
                }
            }
        }
    }

    [MenuItem("Ultilities/Find Missing script in Scene")]
    private static void FindMissingScriptOnScene()
    {
        string[] prefabPaths = AssetDatabase.GetAllAssetPaths().Where(path
            => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

        foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
        {
            foreach (Component component in gameObject.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    Debug.LogError("GameObject found with missing script: " + gameObject.name, gameObject);
                    break;
                }
            }
        }
    }
}
