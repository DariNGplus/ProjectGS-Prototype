using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    private float _killAmount = 0;
    [SerializeField] private TextMeshProUGUI _killAmountCounter;
    public void TallyNewKill() 
    {
        _killAmount++;
        _killAmountCounter.text = _killAmount.ToString();
    }

}
