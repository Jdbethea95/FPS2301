using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerkBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI topSlot;
    [SerializeField] TextMeshProUGUI bttmSlot;
    [SerializeField] TextMeshProUGUI mddlSlot;

    [SerializeField] TextMeshProUGUI perkName;
    [SerializeField] TextMeshProUGUI perkType;

    [SerializeField] TextMeshProUGUI stat1;
    [SerializeField] TextMeshProUGUI stat2;
    [SerializeField] TextMeshProUGUI stat3;

    List<SO_Perk> perkList;

    public bool isSelected;
    public int index = 0;
    int indexMax;


    private void Start()
    {
        BuildList();
        indexMax = perkList.Count - 1;
    }



    public void UpdateBoard() 
    {

    }

    void BuildList() 
    {
        for (int i = 0; i < ScoreManager.instance.ownedList.Count; i++)
        {
            if (PerkManager.instance.grabList.ContainsKey(ScoreManager.instance.ownedList[i])) 
            {
                perkList.Add(PerkManager.instance.grabList[ScoreManager.instance.ownedList[i]]);
            }
        }
    }

}
