using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _fireRate = 0.25f;
    private float _canFire = -1f;
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;

    [SerializeField]
    private bool _haveTripleShot;
    [SerializeField]
    private bool _haveSpeedBoost;
    [SerializeField]
    private bool _haveShield;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _emptyTripleShotParents;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _leftWingDamaged;
    [SerializeField]
    private GameObject _rightWingDamaged;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        _shield.SetActive(false);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _leftWingDamaged = GameObject.Find("Left Wing Damage");
        _rightWingDamaged = GameObject.Find("Right Wing Damage");
        _leftWingDamaged.gameObject.SetActive(false);
        _rightWingDamaged.gameObject.SetActive(false);
    }

    void Update()
    {
        PlayerMovement();
        PlayerBounds();
        ShootLaser();
        DestroyEmptyTripleShots();
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
        if (Input.GetKeyDown(KeyCode.Space) && _haveTripleShot == true && Time.time > _canFire)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _haveTripleShot == false && Time.time > _canFire)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }

    public void DestroyEmptyTripleShots()
    {
        if (_haveTripleShot == false)
        {
            _emptyTripleShotParents = GameObject.FindWithTag("Triple Shot");
            if (_emptyTripleShotParents != null && _emptyTripleShotParents.transform.childCount == 0)
            {
                Destroy(GameObject.FindWithTag("Triple Shot"));
            }
        }
    }

    public void FoundTripleShotPowerup()
    {
        _haveTripleShot = true;
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
        _shield.gameObject.SetActive(true);
    }

    public void Damage()
    {
        if (_haveShield == true)
        {
            _haveShield = false;
            _shield.gameObject.SetActive(false);
            return;
        }

        _lives--;
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
            Destroy(this.gameObject);
            Debug.Log(_lives + "Lives Left");
            _gameManager.GameOver();
        }
    }

    public void KilledEnemy(int _points)
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
}
