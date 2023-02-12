using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoxDrop")]
public class BoxDrop : ScriptableObject
{
    [Range(0,1)] public float dropChance;

    public PowerUpEffect effect;
}
