using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action<EnemyController> OnSpawnNewEnemy;
    public Action<EnemyController> OnEnemyDied;
    public Action OnPlayerFired;
    private List<EnemyController> Enemies = new List<EnemyController>();

    private void Awake()
    {
        Instance = this;
    }
    public List<EnemyController> GetEnemies() => Enemies;
    public void AddNewEnemy(EnemyController enemy)
    {
        Enemies.Add(enemy);
        OnSpawnNewEnemy?.Invoke(enemy);
    }
    public void RemoveEnemy(EnemyController enemy)
    {
        Enemies.Remove(enemy);
        OnEnemyDied?.Invoke(enemy);
    }
}
