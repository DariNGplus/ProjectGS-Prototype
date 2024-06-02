using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "PowerupData")]
public class PowerupData : ScriptableObject
{
    public string Name;
    public string Description;

    public Sprite Sprite;
    public List<PowerupEffect> PowerupEffects = new List<PowerupEffect>();

    public void Collect(GameObject player)
    {
        foreach (PowerupEffect powerupEffect in PowerupEffects)
        {
            powerupEffect.Apply(player);
        }
    }
}
