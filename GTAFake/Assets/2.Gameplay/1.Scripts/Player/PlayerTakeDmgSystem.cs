using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDmgSystem : MonoBehaviour
{
    public static PlayerTakeDmgSystem Instance;
    private void Awake()
    {
        Instance = this;
    }


    public List<EnemyController> GetEnemyInCircleArea(Vector3 position, float radius)
    {
        List<EnemyController> reru = new List<EnemyController>();
        Vector3 enemyPos;
        Vector3 vectre = new Vector3(position.x, 0, position.z);
        foreach (var enemy in GameManager.Instance.GetEnemies())
        {
            enemyPos = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
            if (Vector3.Distance(enemyPos, vectre) < radius) reru.Add(enemy);
        }
        return reru;
    }


    public List<EnemyController> GetEnemyInBox(Vector3[] corner)
    {
        List<EnemyController> reru = new List<EnemyController>();
        Vector3 enemyPos;
        for (int i = 0; i < corner.Length; i++)
        {
            corner[i] = new Vector3(corner[i].x, 0, corner[i].z);
        }
        foreach (var enemy in GameManager.Instance.GetEnemies())
        {
            enemyPos = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
            for (int j = 0; j < corner.Length; j++)
            {
                corner[j] = new Vector3(corner[j].x, 0, corner[j].z);
            }
        }
        return reru;
    }
}
