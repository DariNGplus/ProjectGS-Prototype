using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IMobile, IDamageable, IShooting
{
    [field: Header("Movement")]
    [field: Range(0f, 10f)]
    [field: SerializeField] public float BaseMovementSpeed { get; set; } = 6f;
    public float CurrentMovementSpeed { get; set; }

    [field: Header("Base Properties")]
    [field: Range(2f, 10f)]
    [field: SerializeField] public float MaxHealth { get; set; } = 6f;
    public float CurrentHealth { get; set; }
    public bool IsDead { get; set; } = false;
    [field: Range(1f, 5f)]
    [field: SerializeField] public float BulletDamage { get; set; } = 1f;
    [field: Range(1f, 200f)]
    [field: SerializeField] public float BulletSpeed { get; set; } = 100f;
    [field: SerializeField] public float FireRate { get; set; } = 0.05f;

    public bool IsInvul = false;

    #region Shooting Module Properties
    public int MaxAmmo = 8;
    public int CurrentAmmo;
    public float ReloadRate = 3f;

    private bool _canShoot = true;
    private float _timeSinceLastShot = 0;
    #endregion

    #region Dash Module Properties
    [Header("Dash Properties")]
    public bool CanDash = true;
    private bool _isDashing = false;
    private float _dashDuration = 0.15f;
    private float _timeSinceLastDash = 0;

    //Dash modifiable properties
    public float DashCooldown = 2f;
    public float DashImpulse = 10f;
    #endregion

    #region Bullet Time Properties
    [Header("Bullet Time Properties")]
    public float BulletTimeFactor = 0.05f;
    public float BulletTimeLength = 2f;
    private float _timeSinceLastBulletTime = 0;

    public bool CanBulletTime = true;
    public float BulletTimeCooldown = 12f;
    #endregion

    #region Player Control Properties
    private Rigidbody2D _playerRb;
    private PlayerInput _playerInput;
    [Header("Player Controls")]
    public Vector2 _movementInput;
    public Vector2 _mouseDirection; 
    public float RotationSpeed = 2f;
    #endregion

    #region Delegate Events
    [HideInInspector] public UnityEvent<int, int> OnAmmoUpdate;

    [HideInInspector] public UnityEvent OnEngageBulletTime;
    [HideInInspector] public UnityEvent<float> OnUpdateBulletTimeCooldown;
    [HideInInspector] public UnityEvent<float> OnFinishBulletTime;

    [HideInInspector] public UnityEvent OnEngageDash;
    [HideInInspector] public UnityEvent<float> OnUpdateDashCooldown;
    [HideInInspector] public UnityEvent<float> OnFinishDash;

    [HideInInspector] public UnityEvent<float> OnHealthUpdate;
    [HideInInspector] public UnityEvent OnDeath;
    #endregion

    private GameObject _gameManager;
    private TimeManager _timeManager;
    private PlayerSpriteController _playerSpriteController;
    private AudioManager _audioManager;

    [Header("Instantiated Prefabs")]
    public GameObject PlayerBulletPrefab;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager");
        _playerSpriteController = GetComponent<PlayerSpriteController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerRb = GetComponent<Rigidbody2D>();
        _timeManager = _gameManager.GetComponent<TimeManager>();
        _audioManager = _gameManager.GetComponentInChildren<AudioManager>();
    }

    void Start()
    {
        _movementInput = new Vector2(0, 0);
        CurrentAmmo = MaxAmmo;
        CurrentHealth = MaxHealth;
        CurrentMovementSpeed = BaseMovementSpeed;
        IsDead = false;
    }

    void Update()
    {
        if (!IsDead)
        {
            ManageDashCooldown();
            ManageBulletTimeCooldown();
            _movementInput = _playerInput.actions["Move"].ReadValue<Vector2>();
            ReadMousePosition();
            ShootingBehavior();
        }

    }

    private void FixedUpdate()
    {
        if (!_isDashing && !IsDead)
        {
            _playerSpriteController.PlayerSpriteAnimator.SetBool("isWalking", _movementInput != Vector2.zero);

            Move(_movementInput);
            RotateToMouse();
        }
    }
    private void OnDestroy()
    {
        OnDeath.Invoke();
    }

    #region Rotation and Aiming Module Methods
    private void ReadMousePosition()
    {
        if (!_isDashing)
        {
            Vector3 mPos = Mouse.current.position.ReadValue();
            mPos = Camera.main.ScreenToWorldPoint(mPos);
            _mouseDirection = new(mPos.x - transform.position.x, mPos.y - transform.position.y);
        }
    }

    private void RotateToMouse()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_playerSpriteController.CurrentArm.transform.forward, -_mouseDirection);
        Quaternion rotation = Quaternion.RotateTowards(_playerSpriteController.CurrentArm.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        EvaluateArmQuadrant(_playerSpriteController.CurrentArm.transform.rotation);

        _playerSpriteController.UpdateSprites();
        _playerSpriteController.CurrentArm.transform.rotation = rotation;
    }

    private void EvaluateArmQuadrant(Quaternion rotation)
    {
        float quadrantAngle = rotation.eulerAngles.z % 360;
        bool isForward = quadrantAngle > 0;
        quadrantAngle = Mathf.Abs(quadrantAngle);

        if (quadrantAngle <= 90)
        {
            _playerSpriteController.FacingDown = true;
            _playerSpriteController.FacingRight = isForward;
        }
        else if (quadrantAngle <= 180)
        {
            _playerSpriteController.FacingDown = false;
            _playerSpriteController.FacingRight = isForward;
        }
        else if (quadrantAngle <= 270)
        {
            _playerSpriteController.FacingDown = false;
            _playerSpriteController.FacingRight = !isForward;
        }
        else
        {
            _playerSpriteController.FacingDown = true;
            _playerSpriteController.FacingRight = !isForward;
        }
    }
    #endregion

    #region Health Module Methods
    public void Die()
    {
        IsDead = true;
        IsInvul = true;
        _playerSpriteController.PlayerSpriteAnimator.SetBool("isDead", true);
    }

    public void Hurt(float amount)
    {
        if (!IsInvul)
        {
            _playerSpriteController.PlayerSpriteAnimator.Play("Hurt");
            _audioManager.SFXSource.PlayOneShot(_audioManager.PlayerHit, 1);
            ToggleInvulnerability();
            UpdateHealth(amount);
        }
    }

    private void ToggleInvulnerability() 
    {
        IsInvul = true;
    }

    public void UpdateHealth(float currentHealthModifier, float? maxHealthModifier = null)
    {
        if (maxHealthModifier != null)
        {
            MaxHealth += (float)maxHealthModifier;
            CurrentHealth = MaxHealth;
        }
        OnHealthUpdate.Invoke(currentHealthModifier);
        CurrentHealth += currentHealthModifier;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (CurrentHealth == 0)
        {
            Die();
        }
    }
    #endregion

    #region Movement Module Methods
    public void Move(Vector2 target)
    {
        _playerRb.MovePosition(_playerRb.position + (target * CurrentMovementSpeed * Time.fixedDeltaTime));
    }
    #endregion

    #region Shooting Module Methods
    private void ShootingBehavior() 
    {
        _timeSinceLastShot += Time.unscaledDeltaTime;
        Debug.Log(_timeSinceLastShot);
        if (_canShoot)
        {
            float timeUntilNextShot = 1 / FireRate;
            if (_timeSinceLastShot >= timeUntilNextShot)
            {
                Shoot();
                _timeSinceLastShot = 0;
            }
        }
        else
        {
            float timeUntilNextShot = 1 / ReloadRate;
            if (_timeSinceLastShot >= timeUntilNextShot)
            {
                FinishReload();
            }
        }
    }

    private void StartReload()
    {
        _canShoot = false;
        _gameManager.GetComponent<CursorManager>().IsReloading(true);
    }

    public void FinishReload()
    {
        CurrentAmmo = MaxAmmo;
        _canShoot = true;
        _gameManager.GetComponent<CursorManager>().IsReloading(false);
        _timeSinceLastShot = 0;
        OnAmmoUpdate.Invoke(MaxAmmo, CurrentAmmo);
    }

    public void Shoot()
    {
        Transform gunOffsetPoint = _playerSpriteController.CurrentArm.transform.Find("ArmSprite/HandSprite/WeaponGun/BulletSpawn");
        GameObject bullet = Instantiate(PlayerBulletPrefab, gunOffsetPoint.position, _playerSpriteController.CurrentArm.transform.rotation);
        bullet.GetComponent<BulletBehavior>().BulletDamage = BulletDamage;
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        _audioManager.SFXSource.PlayOneShot(_audioManager.PlayerShoot, 0.6f);

        rigidbody.velocity = BulletSpeed * gunOffsetPoint.transform.right;

        CurrentAmmo--;
        OnAmmoUpdate.Invoke(MaxAmmo, CurrentAmmo);

        if (CurrentAmmo == 0)
        {
            StartReload();
        }
    }
    #endregion

    #region Dash Module Methods
    public void Dash(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (CanDash && _movementInput != Vector2.zero)
            {
                CanDash = false;
                StartCoroutine(StartDash());
            }
        }
    }

    private IEnumerator StartDash()
    {
        OnEngageDash.Invoke();
        _isDashing = true;
        _audioManager.SFXSource.PlayOneShot(_audioManager.PlayerDash, 0.7f);
        _playerRb.excludeLayers = LayerMask.GetMask("Enemy");
        
        float bulletTimeDashFactor = _timeManager.IsBulletTimeActive ? 1 : 1;
        float axisQty = Mathf.Abs(_movementInput.x) + Mathf.Abs(_movementInput.y);

        _playerRb.velocity = new Vector2(transform.localScale.x * DashImpulse * _movementInput.x * bulletTimeDashFactor / axisQty, transform.localScale.y * DashImpulse * _movementInput.y * bulletTimeDashFactor / axisQty);

        for (int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate(_playerSpriteController.PlayerSpriteGameObject, transform.position, transform.rotation);
            go.transform.localScale = new(transform.localScale.x * go.transform.localScale.x, transform.localScale.y * go.transform.localScale.y);
            go.GetComponent<Animator>().SetBool("isAfterImage", true);
            yield return new WaitForSeconds(_dashDuration / 5);
        }

        _playerRb.velocity = Vector2.zero;
        _isDashing = false;
        _timeSinceLastDash = 0;
        _playerRb.excludeLayers = new LayerMask();
        OnFinishDash.Invoke(DashCooldown);
    }
    private void ManageDashCooldown()
    {
        _timeSinceLastDash += Time.deltaTime;

        if (_timeSinceLastDash >= DashCooldown && !_isDashing)
        {
            CanDash = true;
        }
    }

    public void UpdateDashCooldown(float newCooldown)
    {
        DashCooldown += newCooldown;
        OnUpdateDashCooldown.Invoke(DashCooldown);
    }
    #endregion

    #region Bullet Time Module Methods
    public void BulletTime(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (!_timeManager.IsBulletTimeActive && CanBulletTime)
            {
                OnEngageBulletTime.Invoke();
                CanBulletTime = false;
                _timeManager.EngageBulletTime(BulletTimeLength, BulletTimeFactor);
            }
            else if (_timeManager.IsBulletTimeActive)
            {
                CanBulletTime = false;
                _timeManager.FinishBulletTime();
            }

        }
    }
    private void ManageBulletTimeCooldown()
    {
        _timeSinceLastBulletTime += Time.deltaTime;

        if (_timeSinceLastBulletTime >= BulletTimeCooldown)
        {
            CanBulletTime = true;
        }
    }

    public void InitiateBulletTimeCountdown()
    {
        _timeSinceLastBulletTime = 0;
        OnFinishBulletTime.Invoke(BulletTimeCooldown);
    }
    public void UpdateBulletTimeCooldown(float newCooldown)
    {
        BulletTimeCooldown += newCooldown;
        OnUpdateBulletTimeCooldown.Invoke(BulletTimeCooldown);
    }
    #endregion
}
