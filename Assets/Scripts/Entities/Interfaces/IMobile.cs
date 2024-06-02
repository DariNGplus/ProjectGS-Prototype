using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMobile
{
    public float BaseMovementSpeed { get; set; }
    public float CurrentMovementSpeed { get; set; }
    void Move(Vector2 target);
}
