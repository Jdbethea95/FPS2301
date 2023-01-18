using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptObj : MonoBehaviour
{
   public string ID;
   public int ShootDistance;
   public float ShootRate;
   public int ShootDamage;
   public int hpModifier;
   public int SpeedModifier; 
   public Renderer Model;
   public Material material;
   public AudioClip Sound;
   [Range(0, 1f)] public float SoundVol;
}
