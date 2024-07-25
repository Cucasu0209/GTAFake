using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;
    public Action<EnemyController> OnSpawnNewEnemy;
    public Action<EnemyController> OnEnemyDied;
    public Action OnPlayerFired;
    [SerializeField] private List<EnemyController> Enemies = new List<EnemyController>();
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (EnemyController enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
        {
            Enemies.Add(enemy);
        }
        playerController = FindAnyObjectByType<PlayerController>();
        Physics.IgnoreLayerCollision(9, 10, true);
        Physics.IgnoreLayerCollision(10, 10, true);
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
