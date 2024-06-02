using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float BulletDamage = 1f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            collision.gameObject.GetComponent<EnemyBase>().Hurt(BulletDamage);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_spriteRenderer.isVisible) 
        {
            Destroy(gameObject);
        }
    }
}
