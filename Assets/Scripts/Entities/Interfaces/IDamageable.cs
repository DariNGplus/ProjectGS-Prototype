using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void Hurt(float amount);
    void Die();
    public bool IsDead { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
}
