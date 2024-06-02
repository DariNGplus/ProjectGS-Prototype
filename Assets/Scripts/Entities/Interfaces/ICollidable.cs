using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollidable
{
    public float CollisionDamage { get; set; }
    public void OnCollision(GameObject player);
}
