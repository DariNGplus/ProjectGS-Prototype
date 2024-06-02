using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillTimer : MonoBehaviour
{
    [SerializeField] private Image _timerFill;
    private float _remainingDuration;
    private float _duration;
    private bool _isRunning;

    public void StartTimer(float cooldownDuration) {
        _isRunning = true;
        _duration = cooldownDuration;
        _remainingDuration = 0;
    }

    public void SetDuration(float cooldownDuration)
    {
        _duration = cooldownDuration;
    }

    public void SetFillToZero() 
    {
        _timerFill.fillAmount = 0;
    }

    void Update()
    {
        if (_isRunning) 
        {
            _remainingDuration += Time.deltaTime;
            _timerFill.fillAmount = Mathf.InverseLerp(0, _duration, _remainingDuration);

            if (_remainingDuration >= _duration) 
            {
                _isRunning = false;
            }
        }
    }
}
