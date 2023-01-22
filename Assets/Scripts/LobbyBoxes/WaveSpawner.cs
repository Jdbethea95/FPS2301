using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour, IDamage
{
    [Header("----- Spawner Stats -----")]
    [SerializeField] GameObject enemy;
    [SerializeField] Transform spawnPos;
    [SerializeField] int enemyCount;
    [SerializeField] int timer;
    [SerializeField] float ySpawn;

    [Header("----- EnemySpawn Stats -----")]
    [SerializeField] int radius;

    bool isSpawning;
    public bool playerInRange;
    public bool activated = false;
    int enemySpawned;

    List<GameObject> spawns;

    private void Update()
    {
        if (playerInRange && !isSpawning && activated)
            StartCoroutine(Spawn());

    }

    IEnumerator Spawn() 
    {
        isSpawning = true;
        GameObject spawn = Instantiate(enemy, new Vector3(spawnPos.position.x + Random.Range(-5, 5), ySpawn, 
                                       spawnPos.position.z + Random.Range(-5, 5)), enemy.transform.rotation);
        spawn.GetComponent<SphereCollider>().radius = radius;        
        enemyCount++;
        yield return new WaitForSeconds(timer);
        isSpawning = false;
    }

    public void TakeDamage(int dmg)
    {
        activated = !activated;
    }

    

}
