using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _fireRate = 0.25f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _ammoCount;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField]
    private int _ammoGap;

    [SerializeField]
    private bool _haveTripleShot;
    [SerializeField]
    private bool _haveSpeedBoost;
    [SerializeField]
    private bool _haveShield;
    [SerializeField]
    private bool _stopShooting;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _leftWingDamaged;
    [SerializeField]
    private GameObject _rightWingDamaged;
    [SerializeField]
    private GameObject _playerThruster;
    [SerializeField]
    private GameObject _repair;

    [SerializeField]
    private AudioSource _laserAudio;
    [SerializeField]
    private AudioSource _playerExplosionAudio;
    [SerializeField]
    private AudioSource _powerupAudio;

    [SerializeField]
    private Animator _playerExplodeAnimation;
    [SerializeField]
    private Animator _shieldAnimator;
    [SerializeField]
    private Animator _cameraAnimator;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, -4, 0);
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _playerThruster = GameObject.Find("Thruster");
        _powerupAudio = GameObject.Find("Powerup Audio").GetComponent<AudioSource>();
        _laserAudio = GameObject.Find("Laser Audio").GetComponent<AudioSource>();
        _playerExplosionAudio = gameObject.GetComponent<AudioSource>();
        _playerExplodeAnimation = gameObject.GetComponent<Animator>();
        _leftWingDamaged = GameObject.Find("Left Wing Damage");
        _rightWingDamaged = GameObject.Find("Right Wing Damage");
        _leftWingDamaged.gameObject.SetActive(false);
        _rightWingDamaged.gameObject.SetActive(false);
        _shield = GameObject.Find("Shield");
        _shieldAnimator = _shield.GetComponent<Animator>();
        _shieldAnimator.SetInteger("_shieldHealth", 2);
        _shield.SetActive(false);
        _ammoCount = 15;
        _repair = GameObject.Find("Repair");
        _repair.gameObject.SetActive(false);
        _stopShooting = false;
        _cameraAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
        _maxAmmo = 60;
    }

    void Update()
    {
        PlayerMovement();
        PlayerBounds();
        ShootLaser();
        _ammoGap = _maxAmmo - _ammoCount;
        _uiManager.UpdateMaxAmmo(_maxAmmo);
    }

    void PlayerMovement()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(hInput, vInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
    }
    void PlayerBounds()
    {
        //use -6.65 to wrap through bottom
        if (transform.position.y <= -4.75f)
        {
            //if want to continue full wrap make it 6.65f
            //use -4.75 to stop player from going past bottom of screen
            transform.position = new Vector3(transform.position.x, -4.75f, 0);
        }
        else if (transform.position.y >= 4.75)
        {
            transform.position = new Vector3(transform.position.x, 4.75f, 0);
        }

        if (transform.position.x >= 10.37f)
        {
            transform.position = new Vector3(-10.37f, transform.position.y, 0);
        }
        else if (transform.position.x <= -10.38f)
        {
            transform.position = new Vector3(10.37f, transform.position.y, 0);
        }
    }

    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _haveTripleShot == true && Time.time > _canFire && _ammoCount != 0 && _stopShooting == false)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _canFire = Time.time + _fireRate;
            _laserAudio.Play();
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _haveTripleShot == false && Time.time > _canFire && _ammoCount != 0 && _stopShooting == false)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            _canFire = Time.time + _fireRate;
            _laserAudio.Play();
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
        }
    }

    public void FoundAtomBomb()
    {
        StartCoroutine(FreezePlayer());
    }

    IEnumerator FreezePlayer()
    {
        _speed = 0;
        _stopShooting = true;
        yield return new WaitForSeconds(5f);
        _speed = 5f;
        _stopShooting = false;
    }

    public void FoundTripleShotPowerup()
    {
        _haveTripleShot = true;
        _powerupAudio.Play();
        StartCoroutine(TripleShotCooldown());
    }

    IEnumerator TripleShotCooldown()
    {
        while (_haveTripleShot == true)
        {
            yield return new WaitForSeconds(5f);
            _haveTripleShot = false;
        }
    }

    public void FoundSpeedBoost()
    {
        _haveSpeedBoost = true;
        _powerupAudio.Play();
        StartCoroutine(SpeedBoostCooldown());
    }

    IEnumerator SpeedBoostCooldown()
    {
        while (_haveSpeedBoost == true)
        {
            _speed = 8.5f;
            yield return new WaitForSeconds(5f);
            _speed = 5f;
            _haveSpeedBoost = false;
        }
    }

    public void FoundShield()
    {
        _haveShield = true;
        _powerupAudio.Play();
        _shield.gameObject.SetActive(true);
    }

    public void FoundAmmo()
    {
        if (_ammoCount < _maxAmmo)
        {
            if (_ammoCount + 15 > _maxAmmo)
            {
                _ammoCount = _ammoCount + _ammoGap;
            }
            else
            {
                _ammoCount = _ammoCount + 15;
            }
        }
        _powerupAudio.Play();
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void FoundHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            _powerupAudio.Play();
            StartCoroutine(RepairVisual());
            if (_lives == 3)
            {
                _rightWingDamaged.SetActive(false);
                _leftWingDamaged.SetActive(false);
            }
            if (_lives == 2)
            {
                _rightWingDamaged.SetActive(false);
            }
        }
    }

    IEnumerator RepairVisual()
    {
        _repair.SetActive(true);
        yield return new WaitForSeconds(.5f);
        _repair.SetActive(false);
    }

    public void Damage()
    {
        if (_haveShield == true)
        {
            ShieldHits();
            return;
        }

        _lives--;
        StartCoroutine(CameraShake());
        _uiManager.UpdateLives(_lives);

        if(_lives <= 2)
        {
            _leftWingDamaged.gameObject.SetActive(true);
        }

        if (_lives <= 1)
        {
            _rightWingDamaged.gameObject.SetActive(true);
        }

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(GameObject.FindWithTag("Triple Shot Powerup"));
            _playerExplosionAudio.Play();
            _playerExplodeAnimation.SetTrigger("PlayerDead");
            _leftWingDamaged.SetActive(false);
            _rightWingDamaged.SetActive(false);
            _playerThruster.SetActive(false);
            Destroy(this.gameObject, 1.25f);
            _gameManager.GameOver();
            _speed = 0;
        }
    }
    public void ShieldHits()
    {
        if (_shieldAnimator.GetInteger("_shieldHealth") == 2)
        {
            _shieldAnimator.SetInteger("_shieldHealth", 1);
        }
        else if (_shieldAnimator.GetInteger("_shieldHealth") == 1)
        {
            _shieldAnimator.SetInteger("_shieldHealth", 0);
        }
        else if (_shieldAnimator.GetInteger("_shieldHealth") == 0)
        {
            _shieldAnimator.SetInteger("_shieldHealth", 3);
            _shield.SetActive(false);
            _haveShield = false;
        }
    }

    IEnumerator CameraShake()
    {
        _cameraAnimator.SetBool("ShakeTheCamera", true);
        yield return new WaitForSeconds(0.1f);
        _cameraAnimator.SetBool("ShakeTheCamera", false);
    }

    public void KilledEnemy(int _points)
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }

    public void Sprint()
    {
        _speed = 6.5f;
    }

    public void ResetSpeed()
    {
        _speed = 5f;
    }
}
