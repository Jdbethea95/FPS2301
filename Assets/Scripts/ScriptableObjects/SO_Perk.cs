using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class SO_Perk : ScriptableObject
{
    public enum PerkType { Top, Middle, Bottom }

    [Header("----- Identifiers -----")]
    public string ID;
    public string perkName;
    public PerkType perkType;


    [Header("----- Stat Modifiers -----")]
    public int ShootDistance;
    public float ShootRate;
    public int ShootDamage;
    public int hpModifier;
    public int SpeedModifier;
    public int overHeatMax;

    [Header("----- Gun Modifiers -----")]
    public Mesh Model;
    public Material material;
    public bool isColored;
    public ParticleSystem.MinMaxGradient color;
}
