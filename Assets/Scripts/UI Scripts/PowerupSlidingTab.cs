using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupSlidingTab : MonoBehaviour
{
    public Image CollectibleSprite;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;

    public void SetData(PowerupData data) 
    {
        CollectibleSprite.sprite = data.Sprite;
        Title.text = data.Name;
        Description.text = data.Description;
    }

    public void LifeCycleComplete() 
    {
        Destroy(gameObject);
    }
}
