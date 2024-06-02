using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollectibleScript : MonoBehaviour
{
    [SerializeField] private PowerupData _itemData;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _itemData.Sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            _itemData.Collect(collision.gameObject); 
            GameObject.Find("CanvasInterface").GetComponent<UIManager>().ObjectCollected(_itemData);
            Destroy(gameObject);
        }
    }
}
