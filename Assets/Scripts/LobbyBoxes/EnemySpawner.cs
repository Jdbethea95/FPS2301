using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IDamage
{
    [SerializeField] GameObject spawnPOS;
    [SerializeField] GameObject enemy;

    public void TakeDamage(int dmg)
    {
        if (GameManager.instance.enemyCount <= 0)
        {
            EnemyAI spawn;
            spawn = Instantiate(enemy, spawnPOS.transform.position, enemy.transform.rotation).GetComponent<EnemyAI>();
            spawn.ShootDist = 5;
            spawn.SpeedMult = 0;

        }
    }
}
