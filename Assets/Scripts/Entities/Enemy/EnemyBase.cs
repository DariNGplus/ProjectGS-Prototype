using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour, IMobile, IDamageable, ICollidable
{
    [field: Header("Movement")]
    [field: Range(0f, 10f)]
    [field: SerializeField] public float BaseMovementSpeed { get; set; }
    public float CurrentMovementSpeed { get; set; }

    [field: Header("Statistics")]
    [field: Range(3f, 10f)]
    [field: SerializeField] public float MaxHealth { get; set; } = 5f;
    public float CurrentHealth { get; set; }
    public bool IsDead { get; set; }

    [field: Range(0f, 4f)]
    [field: SerializeField] public float CollisionDamage { get; set; } = 1f;

    public GameObject DropPrefab;

    private Animator _enemyAnimator;
    private ParticleSystem _deathParticles;
    protected GameObject _playerReference;
    protected AudioManager _audioManager;

    [HideInInspector] public UnityEvent OnDeath;
    private void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
        _deathParticles = GetComponentInChildren<ParticleSystem>();
        _playerReference = GameObject.Find("Player");
        _audioManager = GameObject.Find("GameManager").GetComponentInChildren<AudioManager>();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentMovementSpeed = BaseMovementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            OnCollision(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            OnCollision(collision.gameObject);
    }

    #region Damageable Methods Implementation
    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        IsDead = true;
        _enemyAnimator.SetBool("IsDead", IsDead);
        OnDeath.Invoke();
        GameObject.Find("CanvasInterface").GetComponent<UIManager>().KillCounter.TallyNewKill();
    }

    public void Hurt(float amount)
    {
        CurrentHealth -= amount;
        _enemyAnimator.Play("Hurt");
        _audioManager.SFXSource.PlayOneShot(_audioManager.EnemyHit, 0.2f);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    #endregion

    #region Movement Methods Implementation
    public void Move(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, CurrentMovementSpeed * Time.deltaTime);
    }
    #endregion

    #region Collidable Methods Implementation
    public void OnCollision(GameObject player)
    {
        player.GetComponent<Player>().Hurt(-CollisionDamage);
    }
    #endregion

    private void TriggerDeathParticles() 
    {
        _deathParticles.Play();
        _audioManager.SFXSource.PlayOneShot(_audioManager.EnemyDie);
        if (DropPrefab != null)
        {
            GameObject go = Instantiate(DropPrefab, null, true);
            go.transform.position = transform.position;
        }
    }
}
