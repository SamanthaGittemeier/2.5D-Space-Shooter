using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 2f;
    private float _enemyFireRate;
    private float _enemyCanFire = -1f;
    [SerializeField]
    private float _randomX;
    [SerializeField]
    private float _randomY;
    [SerializeField]
    private float _randomTimeLength;

    [SerializeField]
    private int _enemyMovementID;
    [SerializeField]
    private int _enemyTypeID;
    [SerializeField]
    private int _enemyShieldDecision;
    [SerializeField]
    private int _randomMoveChoice;

    [SerializeField]
    private bool _allowedToFire;
    [SerializeField]
    private bool _enemyHasShield;
    [SerializeField]
    private bool _isRandomEnemy = false;
    [SerializeField]
    private bool _isAggressiveEnemy = false;
    [SerializeField]
    private bool _isSmartEnemy = false;
    [SerializeField]
    private bool _isAvoiderEnemy = false;
    [SerializeField]
    private bool _isBoss = false;

    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyShield;
    [SerializeField]
    private GameObject _enemyExplosionPrefab;
    [SerializeField]
    private GameObject _explosionsContainer;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Animator _colorAnimator;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _allowedToFire = true;
        _enemySpeed = 2f;
        _randomX = Random.Range(-9.44f, 9.48f);
        _randomY = Random.Range(4.7f, 0f);
        _explosionsContainer = GameObject.Find("ExplosionsContainer");
        switch (_enemyTypeID)
        {
            case 0:
                _colorAnimator.SetInteger("TypeID", 0);
                break;
            case 1:
                _isRandomEnemy = true;
                _colorAnimator.SetInteger("TypeID", 1);
                FireBeams();
                break;
            case 2:
                _isAggressiveEnemy = true;
                _colorAnimator.SetInteger("TypeID", 2);
                break;
            case 3:
                _isSmartEnemy = true;
                _colorAnimator.SetInteger("TypeID", 3);
                break;
            case 4:
                _isAvoiderEnemy = true;
                _colorAnimator.SetInteger("TypeID", 4);
                break;
            case 5:
                _isBoss = true;
                //call boss behavior
                break;
        }
    }

    void Update()
    {
        Fire();
        switch (_enemyMovementID)
        {
            case 0:
                MoveDown();
                break;
            case 1:
                MoveRight();
                break;
            case 2:
                MoveLeft();
                break;
            case 3:
                MoveRandom();
                break;
            case 4:
                MoveTowardsPlayer();
                break;
        }
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.y <= -5.36f)
        {
            transform.position = new Vector3(_randomX, 6.93f, 0);
        }
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        if (transform.position.x >= 11.1f)
        {
            transform.position = new Vector3(-9.44f, _randomY, 0);
        }
    }

    public void MoveLeft()
    {
        transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        if (transform.position.x <= -11.1f)
        {
            transform.position = new Vector3(9.44f, _randomY, 0);
        }
    }

    public void MoveRandom()
    {
        if (_randomMoveChoice == 0)
        {
            MoveDown();
        }
        if (_randomMoveChoice == 1)
        {
            MoveRight();
        }
        if (_randomMoveChoice == 2)
        {
            MoveLeft();
        }
    }

    public void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _enemySpeed * Time.deltaTime);
    }

    public void MoveToCenter()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.x == 0 && transform.position.y == 0)
        {
            _enemySpeed = 0;
        }
    }

    public void Fire()
    {
        if (Time.time > _enemyCanFire && _allowedToFire == true && _isRandomEnemy == false)
        {
            _enemyFireRate = Random.Range(3f, 7f);
            _enemyCanFire = Time.time + _enemyFireRate;
            if (_enemyTypeID == 1 || _enemyTypeID == 2 || _enemyTypeID == 4)
            {
                GameObject _enemySingleLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
                Laser _singleLaser = _enemySingleLaser.GetComponent<Laser>();
                _singleLaser.AssignAsSingleLaser();
                _singleLaser.AssignToEnemy();
            }
            else if (_enemyTypeID == 0 || _enemyTypeID == 3)
            {
                GameObject _enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
                Laser _lasers = _enemyLaser.GetComponent<Laser>();
                _lasers.AssignToEnemy();
            }
        }
    }

    public void FireBeams()
    {
        if (Time.time > _enemyCanFire && _allowedToFire == true && _isRandomEnemy == true)
        {
            _enemyFireRate = Random.Range(4f, 9f);
            _enemyCanFire = Time.time + _enemyFireRate;
            StartCoroutine(FiringBeams());
            GameObject _enemyLaserBeams = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
            Laser[] _laserBeams = _enemyLaserBeams.GetComponentsInChildren<Laser>();
            for (int i = 0; i < _laserBeams.Length; i++)
            {
                _laserBeams[i].AssignToEnemy();
                _laserBeams[i].AssignAsLaserBeams();
            }
        }
    }

    public void FireBackshot()
    {
        if (_allowedToFire == true && _isSmartEnemy == true)
        {
            GameObject _enemyBackshot = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -.5f, 0), Quaternion.identity);
            Laser[] _laserBackshot = _enemyBackshot.GetComponentsInChildren<Laser>();
            for (int i = 0; i < _laserBackshot.Length; i++)
            {
                _laserBackshot[i].AssignAsBackshot();
            }
        }
    }

    IEnumerator FiringBeams()
    {
        _enemySpeed = 0;
        yield return new WaitForSeconds(1f);
        _enemySpeed = 2f;
    }

    public void ChooseLengths()
    {
        StartCoroutine(ChooseRandomLengths());
    }

    public IEnumerator ChooseRandomLengths()
    {
        _randomTimeLength = Random.Range(1, 4);
        _randomMoveChoice = Random.Range(0, 3);
        yield return new WaitForSeconds(_randomTimeLength);
        StartCoroutine(ChooseRandomLengths());
    }

    public void StartRamming()
    {
        if (_isAggressiveEnemy == true)
        {
            StartCoroutine(RamPlayer());
        }
    }

    IEnumerator RamPlayer()
    {
        _enemySpeed = 5;
        _enemyMovementID = 4;
        yield return new WaitForSeconds(3f);
        _enemySpeed = 2;
        _enemyMovementID = Random.Range(0, 3);
    }

    public void StartAvoidance()
    {
        if (_isAvoiderEnemy == true)
        {
            if (_enemyMovementID == 0)
            {
                _enemyMovementID = Random.Range(1, 3);
            }
            StartCoroutine(AvoidShot());
        }
    }

    IEnumerator AvoidShot()
    {
        _enemySpeed = 3;
        yield return new WaitForSeconds(1.5f);
        _enemySpeed = 2;
        _enemyMovementID = Random.Range(0, 3);
    }

    public void DestroyPowerup()
    {
        if (_allowedToFire == true && _enemyTypeID == 0 - 4)
        {
            GameObject _enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Debug.Log("Found Powerup");
            Laser[] _lasers = _enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < _lasers.Length; i++)
            {
                _lasers[i].AssignToEnemy();
            }
        }
    }

    public void EnemyMovementID(int ID)
    {
        _enemyMovementID = ID;
    }

    public void EnemyTypeID(int typeID)
    {
        _enemyTypeID = typeID;
    }

    public void EnemyShieldChoice(int Choose)
    {
        _enemyShieldDecision = Choose;
        if (_enemyShieldDecision == 0)
        {
            _enemyShield.SetActive(false);
            _enemyHasShield = false;
        }
        else if (_enemyShieldDecision == 1)
        {
            _enemyShield.SetActive(true);
            _enemyHasShield = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                if (_enemyHasShield == false)
                {
                    DeathRoutine();
                }
                else 
                {
                    _enemyHasShield = false;
                    _enemyShield.SetActive(false);
                }
            }
        }
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            if (_enemyHasShield == false)
            {
                DeathRoutine();
            }
            else
            {
                _enemyHasShield = false;
                _enemyShield.SetActive(false);
            }
        }
        if (collision.tag == "Shockwave")
        {
            if (_enemyHasShield == false)
            {
                DeathRoutine();
            }
            else
            {
                _enemyHasShield = false;
                _enemyShield.SetActive(false);
            }
        }
    }

    public void DeathRoutine()
    {
        _player.KilledEnemy(10);
        _enemySpeed = 0;
        GameObject explosion = Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.parent = _explosionsContainer.transform;
        Destroy(explosion, 2.5f);
        _allowedToFire = false;
        Destroy(this.gameObject);
    }

    public void FreezeEnemy()
    {
        _enemySpeed = 0f;
        _allowedToFire = false;
    }
}