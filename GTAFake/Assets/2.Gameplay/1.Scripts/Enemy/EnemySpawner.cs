using System.Collections;
using UnityEngine;
using Lean.Pool;
public class EnemySpawner : MonoBehaviour
{
    public int RadiousExpanding = 10;
    [SerializeField] private GameObject[] EnemyPrefabs;
    [SerializeField] private Transform[] SpawnPositions;
    [SerializeField] private PlayerController Player;
    [SerializeField] private MatrixMap Map;
    [SerializeField] private float SpawnTime = 7;
    [SerializeField] private float MaxCount = 10;
    public bool Ismove = true;
    public bool isShow;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //   if (Map.enabled == true) Map.RegisterTransformFollow(Player.transform);
        //else
        //{
        //    transform.parent = Player.transform;
        //    transform.localPosition = Vector3.zero;
        //}
        while (true)
        {

            yield return new WaitForSeconds(SpawnTime);
            if (GameManager.Instance.GetEnemies().Count >= MaxCount) continue;
            GameObject enemy;
            if (Map != null && Map.enabled)
            {
                enemy = LeanPool.Spawn(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], Map.GetRandomPositionInCircle(Player.transform, RadiousExpanding), Quaternion.identity);
            }
            else
            {
                enemy = LeanPool.Spawn(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], SpawnPositions[Random.Range(0, SpawnPositions.Length)].position, Quaternion.identity);

            }
            enemy.GetComponent<EnemyController>().Respawn();
            GameManager.Instance.AddNewEnemy(enemy.GetComponent<EnemyController>());
            enemy.GetComponent<EnemyController>().Run = Ismove;
        }
    }

    private void Update()
    {
        //if (isShow)
        //    if (Map.enabled == true) Map.UpdateFollowing(Player.transform, RadiousExpanding);
    }

}
