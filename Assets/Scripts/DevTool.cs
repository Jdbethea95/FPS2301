using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool : MonoBehaviour
{
    [SerializeField] GameObject panel;


    bool godMode = false;
    bool noHeat = false;
    bool maxDash = false;
    bool noClip = false;
    bool devtool = false;

    private void Update()
    {
        if (Input.GetButtonDown("DevTool"))
        {
            devtool = !devtool;
            panel.SetActive(devtool);

            if (devtool)
                GameManager.instance.PauseGame();
            else
                GameManager.instance.UnPause();

        }
    }

    public void GodMode() 
    {
        godMode = !godMode;

        if (godMode)
            GameManager.instance.playerScript.MaxHealth();
        else
            GameManager.instance.playerScript.RevertHealth();
    }

    public void NoHeat() 
    {
        noHeat = !noHeat;

        if (noHeat)
            GameManager.instance.playerScript.MaxTheHeat();
        else
            GameManager.instance.playerScript.BringTheHeat();
    }

    public void Clipless() 
    {
        noClip = !noClip;

        if (noClip)
            GameManager.instance.player.layer = 8;
        else
            GameManager.instance.player.layer = 3;

    }

    public void Dashathon() 
    {
        maxDash = !maxDash;

        if (maxDash)
            GameManager.instance.playerScript.MaxDash();
        else
            GameManager.instance.playerScript.RevertDash();
    }

    public void BoostIt() 
    {
        GameManager.instance.playerScript.SpeedBoost(5, 10);
    }

}
