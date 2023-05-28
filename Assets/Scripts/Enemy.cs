using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 4f;
    private float _enemyFireRate;
    private float _enemyCanFire = -1f;

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

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyAnimator = gameObject.GetComponent<Animator>();
        _enemyAudio = gameObject.GetComponent<AudioSource>();
        _enemyCollider = gameObject.GetComponent<Collider2D>();
        _enemyRB = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveDown();
        Respawn();
        FireBack();
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
    }

    public void Respawn()
    {
        if (transform.position.y <= -5.36f)
        {
            float RandomX = Random.Range(-9.47f, 9.47f);
            transform.position = new Vector3(RandomX, 6.93f, 0);
        }
    }

    public void FireBack()
    {
        if (Time.time > _enemyCanFire)
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
    }
}