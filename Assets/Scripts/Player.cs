using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 0.25f;
    private float _canFire = -1f;
    [SerializeField]
    private float _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        PlayerMovement();
        PlayerBounds();
        ShootLaser();
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
        if (transform.position.y <= -6.65f)
        {
            transform.position = new Vector3(transform.position.x, 6.65f, 0);
        }
        else if (transform.position.y >= 6.66f)
        {
            transform.position = new Vector3(transform.position.x, -6.65f, 0);
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Debug.Log ("Space was pressed");
        }
    }
    public void Damage()
    {
        _lives--;
        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            Debug.Log(_lives + "Lives Left");
        }
    }
}
