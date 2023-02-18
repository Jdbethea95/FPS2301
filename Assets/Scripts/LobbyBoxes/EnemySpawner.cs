using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IDamage
{
    [SerializeField] GameObject spawnPOS;
    [SerializeField] GameObject enemy;
    GameObject currentEnemy;
    EnemyAI spawn;
    public void TakeDamage(int dmg)
    {
        if (spawn != null) 
        {
            spawn.TakeDamage(999);
            Destroy(currentEnemy);
        }
            
        if (GameManager.instance.enemyCount <= 0)
        {
            currentEnemy = Instantiate(enemy, spawnPOS.transform.position, enemy.transform.rotation);
            spawn = currentEnemy.GetComponent<EnemyAI>();
            spawn.ShootDist = 5;
            spawn.SpeedMult = 0;

        }
    }
}
