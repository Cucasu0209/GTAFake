using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyPrefabs;
    public Transform[] SpawnPositions;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            GameObject enemy = LeanPool.Spawn(EnemyPrefabs, SpawnPositions[Random.Range(0, SpawnPositions.Length)].position, Quaternion.identity);
            enemy.GetComponent<EnemyController>().Respawn();
        }
    }

}
