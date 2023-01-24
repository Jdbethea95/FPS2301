using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] int enemyCount;

    [SerializeField] int timer;
    [SerializeField] List<SpawnPoint> spawnPoints;

    bool isSpawning;
    bool playerInRange;
    int enemiesSpawned;
    int index = 0;

    private void Start()
    {
        GameManager.instance.UpdateEnemiesRemaining(enemyCount);
    }


    private void Update()
    {
        if (playerInRange && !isSpawning && enemyCount > enemiesSpawned)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn() 
    {
        isSpawning = true;

        spawnPoints[index].Spawn();
        enemiesSpawned++;

        if (index < spawnPoints.Count)
            index++;
        else
            index = 0;
        
        yield return new WaitForSeconds(timer);
        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
}
