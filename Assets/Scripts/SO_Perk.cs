using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class SO_Perk : ScriptableObject
{
   public string ID;
   public int ShootDistance;
   public float ShootRate;
   public int ShootDamage;
   public int hpModifier;
   public int SpeedModifier; 
   public Renderer Model;
   public Material material;
   public AudioClip[] audGunShot;
   [Range(0, 1)] public float audGunShotVol;
}
