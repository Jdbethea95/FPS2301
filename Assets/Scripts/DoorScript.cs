using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] int enemiesLeft;

    private void Update()
    {
        if (enemiesLeft >= GameManager.instance.enemyCount)
            gameObject.SetActive(false);
    }

}
