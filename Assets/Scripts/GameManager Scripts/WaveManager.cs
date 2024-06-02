using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.HID;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveContent 
    {
        [System.Serializable]
        public class EnemySpawnData 
        {
            [SerializeField] GameObject _enemy;
            [SerializeField] GameObject _drop;
            [SerializeField] Vector2 _position;

            public GameObject Spawn() 
            {
                GameObject enemy = Instantiate(_enemy, null, true);
                enemy.transform.position = _position;
                enemy.GetComponent<EnemyBase>().DropPrefab = _drop;
                return enemy;
            }
        }

        [SerializeField] private List<EnemySpawnData> _enemiesToSpawn;
        private List<GameObject> _instancedEnemies = new List<GameObject>();
        private int _deadCount = 0;
        [HideInInspector] public UnityEvent OnWaveFinished;

        public void StartWave() 
        {
            foreach (EnemySpawnData data in _enemiesToSpawn) 
            {
                GameObject enemy = data.Spawn();
                enemy.GetComponent<EnemyBase>().OnDeath.AddListener(OnWaveEnemyDeath);
                _instancedEnemies.Add(enemy);
            }
        }

        private void OnWaveEnemyDeath() 
        {
            _deadCount++;
            if (_deadCount >= _instancedEnemies.Count) 
            {
                OnWaveFinished.Invoke();
            }
        }
    }

    [SerializeField] private List<WaveContent> _waveContents = new List<WaveContent>();
    public int CurrentWave = 1;
    public float WaveDelayInSeconds = 5f;
    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = GameObject.Find("CanvasInterface").GetComponent<UIManager>();
    }

    void Start()
    {
        NextWave();   
    }

    private void NextWave() 
    {
        _uiManager.WaveText.text = "OLEADA " + CurrentWave;
        _waveContents[CurrentWave - 1].OnWaveFinished.AddListener(DoneWave);
        _waveContents[CurrentWave - 1].StartWave();
    }

    private void DoneWave() 
    {
        _waveContents[CurrentWave - 1].OnWaveFinished.RemoveListener(DoneWave);
        CurrentWave++;

        if (CurrentWave - 1 <= _waveContents.Count - 1)
        {
            StartCoroutine(DelayNextWave());
        }
        else 
        {
            FinishGame();
        }
    }

    private IEnumerator DelayNextWave() 
    {
        yield return new WaitForSeconds(WaveDelayInSeconds);

        NextWave();
    }

    private void FinishGame()
    {
        GameObject.Find("CanvasInterface").GetComponent<UIManager>().FinishGameUI();
    }
}
