using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{

    [SerializeField] LevelSelecter selecter;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && selecter.isOpen)
        {

            if (ScoreManager.instance.Scenes.Count > selecter.Index)
            {
                SceneManager.LoadScene(ScoreManager.instance.Scenes[selecter.Index]);
            }
                
        }
    }
}
