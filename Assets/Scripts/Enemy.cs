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
    private int _enemyID;
    [SerializeField]
    private int _enemyShieldDecision;

    [SerializeField]
    private bool _allowedToFire;
    [SerializeField]
    private bool _enemyHasShield;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Animator _enemyAnimator;

    [SerializeField]
    private AudioSource _enemyAudio;

    [SerializeField]
    private Collider2D _enemyCollider;

    [SerializeField]
    private Rigidbody2D _enemyRB;

    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyShield;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyAnimator = gameObject.GetComponent<Animator>();
        _enemyAudio = gameObject.GetComponent<AudioSource>();
        _enemyCollider = gameObject.GetComponent<Collider2D>();
        _enemyRB = gameObject.GetComponent<Rigidbody2D>();
        _allowedToFire = true;
        _enemySpeed = 2f;
        _randomX = Random.Range(-9.44f, 9.48f);
        _randomY = Random.Range(4.7f, 0f);
        _enemyShield = GameObject.Find("Enemy Shield");
        //temp
        _enemyShield.SetActive(false);
        
        //_enemyShield.SetActive(false);
        //_enemyShieldDecision = Random.Range(0, 2);
        //if (_enemyShieldDecision == 0)
        //{
        //    _enemyHasShield = false;
        //    _enemyShield.SetActive(false);
        //}
        //else if (_enemyShieldDecision == 1)
        //{
        //    _enemyHasShield = true;
        //    _enemyShield.SetActive(true);
        //}
    }

    void Update()
    {
        FireBack();
        switch (_enemyID)
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
        }

        //if (_enemyShieldDecision == 0)
        //{
        //    _enemyHasShield = false;
        //    _enemyShield.SetActive(false);
        //}
        //else if (_enemyShieldDecision == 1)
        //{
        //    _enemyHasShield = true;
        //    _enemyShield.SetActive(true);
        //}
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

    public void EnemyID(int ID)
    {
        _enemyID = ID;
    }

    //public void EnemyShieldChoice(int Choose)
    //{
    //    _enemyShieldDecision = Choose;
    //    //if (_enemyShieldDecision == 0)
    //    //{
    //    //    _enemyShield.SetActive(false);
    //    //    _enemyHasShield = false;
    //    //}
    //    //else if (_enemyShieldDecision == 1)
    //    //{
    //    //    _enemyShield.SetActive(true);
    //    //    _enemyHasShield = true;
    //    //}
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                _enemySpeed = 0;
                _player.KilledEnemy(10);
                _enemyAnimator.SetTrigger("OnEnemyDeath");
                _enemyAudio.Play();
                _allowedToFire = false;
                Destroy(_enemyCollider);
                Destroy(_enemyRB);
                Destroy(this.gameObject, 1.25f);
            }
        }
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            _player.KilledEnemy(10);
            _enemySpeed = 0;
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemyAudio.Play();
            _allowedToFire = false;
            Destroy(_enemyCollider);
            Destroy(_enemyRB);
            Destroy(this.gameObject, 1.25f);
        }
        if (collision.tag == "Shockwave")
        {
            _player.KilledEnemy(10);
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemyAudio.Play();
            _allowedToFire = false;
            Destroy(_enemyCollider);
            Destroy(_enemyRB);
            Destroy(this.gameObject, 1.25f);
        }
    }

    //void EnemyShieldHit()
    //{
    //    _enemyShield.SetActive(false);
    //    _enemyHasShield = false;
    //}

    public void FreezeEnemy()
    {
        _enemySpeed = 0f;
        _allowedToFire = false;
    }

    //public void ShootPowerup()
    //{
    //    GameObject _enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
    //    _enemyLaser.GetComponentInChildren<Laser>().AssignToEnemy();
    //}
}