using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1.0f;
    
    
    public override void OnStartServer()
    {
        InvokeRepeating("SpawnEnemy", this.spawnInterval, this.spawnInterval); // Repeatedly runs the SpawnEnemy() function, waiting for the duration of spawnInterval between each execution.
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(4, -4), this.transform.position.y); // Randomly determines a spawn location within a certain X distance of the spawner gameobject.
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity) as GameObject; // Instantiates enemy prefab in determined spawn location
        NetworkServer.Spawn(enemy);
    }
}
