using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Tooltip("Negative amounts to heal positive damage")]
    [Range(10, 50)][SerializeField] int heal = 10;
    [Range(0, 1f)][SerializeField] float soundVol;
    [SerializeField] AudioSource player;
    [SerializeField] AudioClip sound;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.playerScript.CanHeal)
        {
            GameManager.instance.playerScript.HealPlayer(heal);
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
