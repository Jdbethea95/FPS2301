using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;

    [SerializeField] List<SO_Perk> collectionList;

    public Dictionary<string, SO_Perk> grabList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

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
