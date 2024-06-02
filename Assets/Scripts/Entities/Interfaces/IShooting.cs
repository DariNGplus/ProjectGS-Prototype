using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooting
{
    public float BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float FireRate { get; set; }
    void Shoot();
}
