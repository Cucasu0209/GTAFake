using System.Collections;
using UnityEngine;
using Lean.Pool;
public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject EnemyPrefabs;
    [SerializeField] private Transform[] SpawnPositions;
    [SerializeField] private float SpawnTime = 7;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnTime);
            GameObject enemy = LeanPool.Spawn(EnemyPrefabs, SpawnPositions[Random.Range(0, SpawnPositions.Length)].position, Quaternion.identity);
            enemy.GetComponent<EnemyController>().Respawn();
            GameManager.Instance.AddNewEnemy(enemy.GetComponent<EnemyController>());
        }
    }

}
