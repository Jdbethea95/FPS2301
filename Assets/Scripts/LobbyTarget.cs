using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTarget : MonoBehaviour, IDamage
{
   [Header("----- Target Stats -----")]
    [SerializeField] Renderer rend;
    [SerializeField] float flashTime;
    [SerializeField] int dashCooldown;

    [Header("----- Target Audio -----")]
    [SerializeField] AudioSource audPlayer;
    [SerializeField] AudioClip clip;
    


    Color ogColor;
    bool gained = false;

    private void Start()
    {
        ogColor = rend.material.color;
        audPlayer.volume = SaveManager.instance.gameData.sfxVol;
    }

    public void TakeDamage(int dmg)
    {
        StartCoroutine(Flash());

        if (!gained) 
        {
            StartCoroutine(DashGive());
            audPlayer.PlayOneShot(clip);
        }
            
    }


    IEnumerator Flash() 
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        rend.material.color = ogColor;
    }

    IEnumerator DashGive() 
    {
        gained = true;
        GameManager.instance.playerScript.DashPointGain();
        yield return new WaitForSeconds(dashCooldown);
        gained = false;
    }
}
