using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class SO_Perk : ScriptableObject
{
    [Header("----- Identifiers -----")]
    public string ID;
    public string perkName;

    [Header("----- Stat Modifiers -----")]
    public int ShootDistance;
    public float ShootRate;
    public int ShootDamage;
    public int hpModifier;
    public int SpeedModifier;

    [Header("----- Gun Modifiers -----")]
    public Renderer Model;
    public Material material;
    public AudioClip[] audGunShot;
    [Range(0, 1)] public float audGunShotVol;
}
