using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour, IDamage
{
    [SerializeField] int hp = 10;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
            Destroy(gameObject);
    }

}
