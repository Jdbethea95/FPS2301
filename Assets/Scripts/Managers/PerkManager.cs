using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;

    [SerializeField] List<SO_Perk> collectionList;
    public SO_Perk[] activePerks = new SO_Perk[3];
    public Dictionary<string, SO_Perk> grabList = new Dictionary<string, SO_Perk>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        BuildDictionary();
    }

    void BuildDictionary() 
    {
        for (int i = 0; i < collectionList.Count; i++)
        {
            grabList.Add(collectionList[i].ID, collectionList[i]);            
        }
    }

}
