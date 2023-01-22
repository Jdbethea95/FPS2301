using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkTarget : MonoBehaviour, IDamage
{
    [SerializeField] PerkBoard perkBoard;

    public void TakeDamage(int dmg)
    {
        perkBoard.SelectOption();
    }
}
