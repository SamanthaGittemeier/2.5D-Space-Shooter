using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyShield;
    [SerializeField]
    private GameObject _enemyExplosionPrefab;
    [SerializeField]
    private GameObject _explosionsContainer;

    [SerializeField]
    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _allowedToFire = true;
        _enemySpeed = 2f;
        _randomX = Random.Range(-9.44f, 9.48f);
        _randomY = Random.Range(4.7f, 0f);
        _explosionsContainer = GameObject.Find("ExplosionsContainer");
    }

    void Update()
    {
        FireBack();
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
        }
    }

    public void FireBack()
    {
        if (Time.time > _enemyCanFire && _allowedToFire == true)
        {
            _enemyFireRate = Random.Range(3f, 7f);
            _enemyCanFire = Time.time + _enemyFireRate;
            GameObject _enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] _lasers = _enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < _lasers.Length; i++)
            {
                _lasers[i].AssignToEnemy();
            }
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

    public void DestroyPowerup()
    {
        if (_allowedToFire == true)
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