using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    public string[] prefabName;
    public int row;
    public int colum;
    public string sizeSaved;
    public void log()
    {
        Debug.Log(prefabName.Length);
    }
}