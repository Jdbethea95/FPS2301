using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotKiller : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer renderBot;
    [SerializeField] float speed = .5f;
    [SerializeField] Material matty;

    public bool killIt = false;

    float ticker = 0f;
    bool matswap = true;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (killIt)
            Dissolve();
    }

    void Dissolve() 
    {
        if (matswap)
        {
            renderBot.material = matty;
            matswap = false;
        }

        Material[] mats = renderBot.materials;

        mats[0].SetFloat("_Cutoff", Mathf.Sin(ticker * speed));
        ticker += Time.deltaTime;

       
        renderBot.materials = mats;
    }
}
