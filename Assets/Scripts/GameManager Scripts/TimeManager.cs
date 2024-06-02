using JetBrains.Annotations;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    private float _bulletTimeLength = 2f;
    [SerializeField] private float _speedingFactor = 1.2f;

    private float _timeSinceBulletTimeStart = 0;
    public bool IsBulletTimeActive = false;

    private void Update()
    {
        if (IsBulletTimeActive)
        {
            _timeSinceBulletTimeStart += Time.unscaledDeltaTime;

            if (_timeSinceBulletTimeStart >= _bulletTimeLength)
            {
                FinishBulletTime();
            }
        }
        else if (Time.timeScale != 1) 
        {
            Time.timeScale += (1 / _speedingFactor) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        }
    }

    public void FinishBulletTime()
    {
        IsBulletTimeActive = false;
        _timeSinceBulletTimeStart = 0;
        GetComponentInChildren<AudioManager>().ToggleSlowmoPitch(false);
        GameObject.Find("Player").GetComponent<Player>().InitiateBulletTimeCountdown();
    }

    public void EngageBulletTime(float length, float timescaleFactor) 
    {
        GetComponentInChildren<AudioManager>().ToggleSlowmoPitch(true);
        _bulletTimeLength = length;
        Time.timeScale = timescaleFactor;
        IsBulletTimeActive = true;
    }

}
