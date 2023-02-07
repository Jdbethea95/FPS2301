using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] int index = -1;
    [SerializeField] float musicVol;
    [SerializeField] AudioMixer audMixer;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        musicPlayer.volume = SaveManager.instance.gameData.menuMusicVol;
        UpdateEnemySFX(SaveManager.instance.gameData.sfxVol);
        PlayThatRadio();
    }





    public void UpdateMusicVol()
    {
        if (index > 0)
            musicPlayer.volume = SaveManager.instance.gameData.musicVol;
        else if(index == 0)
            musicPlayer.volume = SaveManager.instance.gameData.menuMusicVol;
    }

    public void UpdateMusicIndex(int indx)
    {

        if (indx < musicClips.Length)
            index = indx;

        UpdateMusicVol();
    }

    public void UpdateEnemySFX(float volume) 
    {
        audMixer.SetFloat("EnemyVol", Mathf.Log10(volume) * 20);
    }

    public void PlayThatRadio(int indx = 0) 
    {
        index = indx;
        UpdateMusicVol();        
        musicPlayer.Stop();
        musicPlayer.PlayOneShot(musicClips[indx]);
    }

}
