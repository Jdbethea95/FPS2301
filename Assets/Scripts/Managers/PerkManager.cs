using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;

    [SerializeField] List<SO_Perk> collectionList;
    //public List<string> ownedList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

}
