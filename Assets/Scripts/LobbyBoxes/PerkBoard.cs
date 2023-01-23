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

    List<SO_Perk> perkList = new List<SO_Perk>();

    public bool isSelected;
    public int index = 0;
    int indexMax;


    private void Start()
    {
        BuildList();

        if (perkList.Count > 0)
            indexMax = perkList.Count - 1;
        else
            indexMax = 0;

        UpdateBoard();
    }



    public void UpdateBoard() 
    {


        if (perkList.Count > 0)
        {

            perkName.text = perkList[index].perkName;
            perkType.text = perkList[index].perkType.ToString();

            Stats();
            UpdateSlots();

        }
        else 
        {
            perkName.text = "No Perks Available";
            perkType.text = "---";
            stat1.text = "---";
            stat2.text = "---";
            stat3.text = "---";
            UpdateSlots();
        }
    }

    public void UpdateSlots() 
    {
        if (PerkManager.instance.activePerks[0] != null)
            topSlot.text = PerkManager.instance.activePerks[0].perkName;
        else
            topSlot.text = "---";

        if (PerkManager.instance.activePerks[1] != null)
            mddlSlot.text = PerkManager.instance.activePerks[1].perkName;
        else
            mddlSlot.text = "---";

        if (PerkManager.instance.activePerks[2] != null)
            bttmSlot.text = PerkManager.instance.activePerks[2].perkName;
        else
            bttmSlot.text = "---";
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

    void Stats() 
    {
        int x = 0;
        string[] stats = { "---", "---", "---" };

        if (perkList[index].ShootDamage != 0)
        {
            stats[x] = $"Damage: {perkList[index].ShootDamage}";
            x++;
        }

        if (perkList[index].ShootDistance != 0)
        {
            stats[x] = $"Range: {perkList[index].ShootDistance}";
            x++;
        }

        if (perkList[index].ShootRate != 0)
        {
            stats[x] = $"Fire Rate: {perkList[index].ShootRate}";
            x++;
        }

        if (perkList[index].hpModifier != 0)
        {
            stats[x] = $"Health: {perkList[index].hpModifier}";
            x++;
        }

        if (perkList[index].SpeedModifier != 0)
        {
            stats[x] = $"Speed: {perkList[index].SpeedModifier}";
            x++;
        }

        stat1.text = stats[0];
        stat2.text = stats[1];
        stat3.text = stats[2];
    }

    public void IncrementIndex(int amount) 
    {
        index += amount;

        if (index > indexMax)
            index = 0;
        else if (index < 0)
            index = indexMax;

        UpdateBoard();
    }

    public void SelectOption() 
    {
        if (perkList.Count > 0)
        {

            switch (perkList[index].perkType)
            {
                case SO_Perk.PerkType.Top:
                    PerkManager.instance.activePerks[0] = perkList[index];
                    break;
                case SO_Perk.PerkType.Middle:
                    PerkManager.instance.activePerks[1] = perkList[index];
                    break;
                case SO_Perk.PerkType.Bottom:
                    PerkManager.instance.activePerks[2] = perkList[index];
                    break;
            }

            UpdateSlots();

            GameManager.instance.playerScript.DeActivatePerks();
            GameManager.instance.playerScript.ActivatePerks();
        }
    }

    public void ClearPerks() 
    {
        GameManager.instance.playerScript.DeActivatePerks();
        GameManager.instance.playerScript.ClearPerks();
        UpdateSlots();
    }

}
