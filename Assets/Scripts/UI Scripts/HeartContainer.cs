using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    public Image HeartContainerImage;
    public float Value = 2f;
    public float TargetHealthValue = 0f;
    private float _transitioningFillVar = 0;
    public bool AnimatingFill = false;
    private bool _isDamage = false;

    public void FillHealth(float amount) 
    {
        _transitioningFillVar = amount;
        TargetHealthValue = Value + amount;
        _isDamage = Value > TargetHealthValue;
        AnimatingFill = true;
    }

    void Update()
    {
        if (AnimatingFill) 
        {
            Value += _transitioningFillVar * Time.unscaledDeltaTime;
            
            Value = _isDamage ? Mathf.Clamp(Value, TargetHealthValue, 2) : Mathf.Clamp(Value, 0, TargetHealthValue);
            HeartContainerImage.fillAmount = Mathf.InverseLerp(0, 2, Value);

            if (Value == TargetHealthValue) 
            {
                AnimatingFill = false;
            }
        }
        
    }
}
