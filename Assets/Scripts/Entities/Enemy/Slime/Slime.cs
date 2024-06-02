using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyBase
{
    void Update()
    {
        if (_playerReference != null) 
        {
            if (!_playerReference.GetComponent<Player>().IsDead)
                Move(_playerReference.transform.position);
        }
            
    }
   
}
