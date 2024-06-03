using System.Collections;
using UnityEngine;
using Lean.Pool;
public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] EnemyPrefabs;
    [SerializeField] private Transform[] SpawnPositions;
    [SerializeField] private PlayerController Player;
    [SerializeField] private MatrixMap Map;
    [SerializeField] private float SpawnTime = 7;
    [SerializeField] private float MaxCount = 10;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Map.RegisterTransformFollow(Player.transform);
        while (true)
        {

            yield return new WaitForSeconds(SpawnTime);
            //if (GameManager.Instance.GetEnemies().Count >= MaxCount) continue;
            //GameObject enemy = LeanPool.Spawn(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], SpawnPositions[Random.Range(0, SpawnPositions.Length)].position, Quaternion.identity);
            //enemy.GetComponent<EnemyController>().Respawn();
            //GameManager.Instance.AddNewEnemy(enemy.GetComponent<EnemyController>());
        }
    }

    private void Update()
    {
        Map.UpdateFollowing(Player.transform);
    }

}
