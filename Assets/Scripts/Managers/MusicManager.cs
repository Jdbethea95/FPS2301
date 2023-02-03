using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] int index = -1;
    [SerializeField] float musicVol;



    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        musicPlayer.volume = SaveManager.instance.gameData.menuMusicVol;
    }


    private void Update()
    {
        if (index >= 0 && !musicPlayer.isPlaying)
        {
            musicPlayer.PlayOneShot(musicClips[index]);
        }
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

}
