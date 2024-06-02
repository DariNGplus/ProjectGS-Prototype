using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _maxAmmoTextBox;
    [SerializeField] private TextMeshProUGUI _currentAmmoTextBox;

    public SkillTimer DashSkillTimer;
    public SkillTimer BulletTimeSkillTimer;
    public KillCounter KillCounter;

    public GameObject ResetUI;
    public GameObject VictoryUI;
    public GameObject HUD;
    public TextMeshProUGUI WaveText; 

    [Header("Instantiated Objects")]
    public GameObject prefabCollectible;

    public void UpdateCurrentAmmo(int maxAmmo, int currentAmmo)
    {
        _maxAmmoTextBox.text = maxAmmo.ToString();
        _currentAmmoTextBox.text = currentAmmo.ToString();
    }

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().OnAmmoUpdate.AddListener(UpdateCurrentAmmo);

        player.GetComponent<Player>().OnEngageBulletTime.AddListener(BulletTimeSkillTimer.SetFillToZero);
        player.GetComponent<Player>().OnUpdateBulletTimeCooldown.AddListener(BulletTimeSkillTimer.SetDuration);
        player.GetComponent<Player>().OnFinishBulletTime.AddListener(BulletTimeSkillTimer.StartTimer);

        player.GetComponent<Player>().OnEngageDash.AddListener(DashSkillTimer.SetFillToZero);
        player.GetComponent<Player>().OnUpdateDashCooldown.AddListener(DashSkillTimer.SetDuration);
        player.GetComponent<Player>().OnFinishDash.AddListener(DashSkillTimer.StartTimer);

        player.GetComponent<Player>().OnDeath.AddListener(ToggleUIOnPlayerDeath);
    }

    private void ToggleUIOnPlayerDeath()
    {
        ResetUI.SetActive(true);
        HUD.SetActive(false);
    }

    public void ObjectCollected(PowerupData data) 
    {
        GameObject go = Instantiate(prefabCollectible, HUD.transform);
        go.GetComponent<PowerupSlidingTab>().SetData(data);
    }

    public void FinishGameUI() 
    {
        VictoryUI.SetActive(true);
        HUD.SetActive(false);
    }
}
