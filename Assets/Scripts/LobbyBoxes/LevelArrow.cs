using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArrow : MonoBehaviour, IDamage
{
    [SerializeField] LevelSelecter selecter;
    [Range(-1, 1)] [SerializeField] int increment;

    public void TakeDamage(int dmg)
    {
        if (selecter.IncrementIndex(increment))
            selecter.UpdateLevelText();
    }
}
