using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{

    [SerializeField] LevelSelecter selecter;
    [SerializeField] AudioSource audPlayer;
    [SerializeField] AudioClip portalClip;


    private void Start()
    {
        audPlayer.volume = SaveManager.instance.gameData.sfxVol;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && selecter.isOpen)
        {
            audPlayer.PlayOneShot(portalClip);
            if (ScoreManager.instance.Scenes.Count > selecter.Index)
            {
                //SceneManager.LoadScene(ScoreManager.instance.Scenes[selecter.Index]);
                GameManager.instance.LoadingScene(ScoreManager.instance.Scenes[selecter.Index]);
            }
                
        }
    }
}
