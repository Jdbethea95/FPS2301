using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("----- Explode Stats -----")]
    [SerializeField] int knockBackAmount;
    [SerializeField] bool pull;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeMagnitude;

    [Header("----- Damage Stats -----")]
    [SerializeField] bool doesDamage;
    [SerializeField] int explosionDmg;

    [Header("----- Grow Stats -----")]
    [SerializeField] bool isGrowing;
    [SerializeField] float growthRate;
    [SerializeField] int maxGrowth;
    [SerializeField] MeshRenderer renderSphere;
    [SerializeField] int shimmerSpeed;
    float timer = 3.8f;
    [Tooltip("The amount reduced from maxGrowth to Lower Linger Time")]
    [Range(.55f, .11f)][SerializeField] float lingerTime;

    //_FresnelPower

    private void Update()
    {

        if (isGrowing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * maxGrowth, growthRate * Time.deltaTime);

            Material[] mats = renderSphere.materials;

            
            mats[0].SetFloat("_FresnelPower", timer);
            timer -= shimmerSpeed * Time.deltaTime;
            if (timer <= 0)
                timer = 0;

            renderSphere.materials = mats;

            if (transform.localScale.y >= maxGrowth - lingerTime)
                Destroy(gameObject); 
        }
    }


    void Damage() 
    {
        if (doesDamage)
        {
            GameManager.instance.playerScript.TakeDamage(explosionDmg, transform.position);

            if(GameManager.instance.playerScript.CurrentHealth > 0)
            GameManager.instance.playerScript.cam.ActivateShake(shakeDuration, shakeMagnitude);

            doesDamage = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pull)
        {
            GameManager.instance.playerScript.pushBack = (GameManager.instance.player.transform.position -
                                                          transform.position).normalized * knockBackAmount;
            Damage();
        }
        else 
        {
            GameManager.instance.playerScript.pushBack = (transform.position - 
                                                         GameManager.instance.player.transform.position).normalized * knockBackAmount;
            Damage();
        }
    }


}
