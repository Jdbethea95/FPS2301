using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    enum EnemyType {Base, Sniper, Smg, Chaser, Minion}

    [SerializeField] GameObject[] enemies;
    [SerializeField] EnemyType type;
    [SerializeField] float enemySightRadius;
    [SerializeField] bool isMinion;

    SphereCollider currentEnemy;
    public void Spawn() 
    {
        switch (type)
        {
            case EnemyType.Base:
                currentEnemy = Instantiate(enemies[0], transform.position, transform.rotation).GetComponent<SphereCollider>();
                break;
            case EnemyType.Sniper:
                currentEnemy = Instantiate(enemies[1], transform.position, transform.rotation).GetComponent<SphereCollider>();
                break;
            case EnemyType.Smg:
                currentEnemy = Instantiate(enemies[2], transform.position, transform.rotation).GetComponent<SphereCollider>();
                break;
            case EnemyType.Chaser:
                currentEnemy = Instantiate(enemies[3], transform.position, transform.rotation).GetComponent<SphereCollider>();
                break;
            case EnemyType.Minion:
                if(enemies.Length >= 5)
                currentEnemy = Instantiate(enemies[4], transform.position, transform.rotation).GetComponent<SphereCollider>();
                break;

        }

        currentEnemy.radius = enemySightRadius;

        if(isMinion)
        currentEnemy.GetComponent<EnemyAI>().reportDeath = false;
    }
}
