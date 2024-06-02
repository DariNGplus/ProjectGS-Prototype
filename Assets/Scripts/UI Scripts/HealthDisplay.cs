using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public GameObject HeartContainerPrefab;
    private List<GameObject> _heartContainers = new List<GameObject>();
    private Player _playerControls;

    private void Awake()
    {
        _playerControls = GameObject.Find("Player").GetComponent<Player>();
        InstantiateHeartContainers();
        _playerControls.OnHealthUpdate.AddListener(UpdateHealth);
    }

    #region Heart Container Modifying Methods
    private void InstantiateHeartContainers() 
    {
        if (_heartContainers.Count != 0) 
        {
            foreach (GameObject heartContainer in _heartContainers) 
            {
                Destroy(heartContainer);
            }
        }

        _heartContainers = new List<GameObject>();
        float heartContainerAmount = _playerControls.MaxHealth / 2;
        Vector2 localPosition = new(0,0);
        for (float i = 0; i < heartContainerAmount; i++) 
        {
            GameObject container = Instantiate(HeartContainerPrefab, transform, false);
            container.GetComponent<RectTransform>().anchoredPosition = localPosition;

            _heartContainers.Add(container);
            localPosition.x += container.GetComponent<RectTransform>().rect.width;
        }
    }

    private void UpdateHealth(float currentHealthDelta) 
    {
        StartCoroutine(UpdateHealthRoutine(currentHealthDelta));      
    }

    private IEnumerator UpdateHealthRoutine(float currentHealthDelta) 
    {
        if (_playerControls.MaxHealth / 2 != _heartContainers.Count)
        {
            InstantiateHeartContainers();
        }

        float sign = Mathf.Sign(currentHealthDelta);
        currentHealthDelta = Mathf.Abs(currentHealthDelta);

        if (sign < 0)
        {
            for (int i = _heartContainers.Count - 1; i > -1; i--)
            {
                if (_heartContainers[i].GetComponent<HeartContainer>().Value != 0 && currentHealthDelta > 0)
                {
                    HeartContainer container = _heartContainers[i].GetComponent<HeartContainer>();
                    float value = -Mathf.Clamp(currentHealthDelta, 0, 2);
                    container.FillHealth(value);
                    currentHealthDelta -= (container.TargetHealthValue - value);
                    while (container.AnimatingFill)
                        yield return new WaitForEndOfFrame();
                }
            }
        }
        else
        {
            for (int i = 0; i < _heartContainers.Count; i++)
            {
                if (_heartContainers[i].GetComponent<HeartContainer>().Value != 2 && currentHealthDelta > 0)
                {
                    HeartContainer container = _heartContainers[i].GetComponent<HeartContainer>();
                    container.FillHealth(Mathf.Clamp(currentHealthDelta, 0, 2));
                    currentHealthDelta -= 2;
                    while (container.AnimatingFill)
                        yield return new WaitForEndOfFrame();
                }
            }
        }
    }
    #endregion
}
