﻿using System.Collections;
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
    private bool _allowedToFire;

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
    private SpawnManager _spawnManager;

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
    }

    void Update()
    {
        FireBack();
        if (_enemyID == 0)
        {
            MoveDown();
        }
        else if (_enemyID == 1)
        {
            MoveRight();
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
        Debug.Log("Called to Move Down");
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.y <= -5.36f)
        {
            transform.position = new Vector3(_randomX, 6.93f, 0);
        }
    }

    public void MoveRight()
    {
        Debug.Log("Called to Move Right");
        transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        if (transform.position.x >= 11.1f)
        {
            transform.position = new Vector3(-9.44f, _randomY, 0);
        }
    }

    public void EnemyID(int ID)
    {
        _enemyID = ID;
    }

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
            Destroy(_enemyCollider);
            Destroy(_enemyRB);
            Destroy(this.gameObject, 1.25f);
        }
        if (collision.tag == "Shockwave")
        {
            _player.KilledEnemy(10);
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemyAudio.Play();
            Destroy(_enemyCollider);
            Destroy(_enemyRB);
            Destroy(this.gameObject, 1.25f);
        }
    }

    public void FreezeEnemy()
    {
        _enemySpeed = 0f;
        _allowedToFire = false;
    }
}