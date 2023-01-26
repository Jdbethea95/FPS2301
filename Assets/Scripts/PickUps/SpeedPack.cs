using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPack : MonoBehaviour
{
    [Tooltip("The amount player speed is increased by")]
    [Range(1, 10)] [SerializeField] int speedBoost = 5;

    [Tooltip("The amount of time speed boost is active")]
    [Range(1f, 10f)][SerializeField] int duration = 5;

    [Range(0, 1f)] [SerializeField] float soundVol;
    [SerializeField] AudioSource player;
    [SerializeField] AudioClip sound;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.currentScore.BoostScore = 1;
            GameManager.instance.playerScript.SpeedBoost(speedBoost, duration);
            player.PlayOneShot(sound, soundVol);
            StartCoroutine(SoundWait(sound));
        }
    }

    IEnumerator SoundWait(AudioClip sound)
    {
        yield return new WaitUntil(() => player.isPlaying == false);

        if (gameObject != null)
            Destroy(gameObject);

    }
}
