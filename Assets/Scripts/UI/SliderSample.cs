using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSample : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [Header("----- Slider -----")]
    [SerializeField] Slider bar;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource audPlayer;
    [SerializeField] AudioClip sample;

    [Header("----- Toggle -----")]
    [SerializeField] BarType barType;

    enum BarType { Music, SFX, mMusic, mSFX }

    float musicVolSaver;
    float menuMusicVolSaver;

    private void Start()
    {
        musicVolSaver = SaveManager.instance.gameData.musicVol;
        menuMusicVolSaver = SaveManager.instance.gameData.menuMusicVol;
    }

    public void OnMusicChange()
    {
        SaveManager.instance.gameData.musicVol = bar.value;
        MusicManager.instance.UpdateMusicVol();
    }

    public void OnMenuMusicChange()
    {
        SaveManager.instance.gameData.menuMusicVol = bar.value;
        MusicManager.instance.UpdateMusicVol();
    }

    public void RestoreMusicVolume()
    {
        SaveManager.instance.gameData.musicVol = musicVolSaver;
        SaveManager.instance.gameData.menuMusicVol = menuMusicVolSaver;
        MusicManager.instance.UpdateMusicVol();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Released");
        if (barType == BarType.SFX || barType == BarType.mSFX)
        {
            audPlayer.PlayOneShot(sample, bar.value);
        }
    }

}
