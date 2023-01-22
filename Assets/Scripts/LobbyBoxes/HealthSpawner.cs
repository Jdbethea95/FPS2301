using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour, IDamage
{

    [SerializeField] GameObject spawnPOS;
    [SerializeField] GameObject health;

    GameObject hp;

    float healthTimer = 0f;
    [SerializeField] int healthCoolDown;

    public void TakeDamage(int dmg)
    {
        if (HealthTimer())
        {
            if (hp != null)
                Destroy(hp);

            hp = Instantiate(health, spawnPOS.transform.position, health.transform.rotation);
        }
         
    }

    bool HealthTimer()
    {

        if (healthTimer <= 0)
        {
            healthTimer = Time.time;
            return true;
        }
        else if (Time.time > healthTimer + healthCoolDown)
        {
            healthTimer = 0f;
            return true;
        }

        return false;
    }
}
