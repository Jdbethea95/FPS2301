using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkArrow : MonoBehaviour, IDamage
{
    [SerializeField] PerkBoard perkBoard;
    [Range(-1, 1)][SerializeField] int increment;
    public void TakeDamage(int dmg)
    {
        perkBoard.IncrementIndex(increment);
    }
}
