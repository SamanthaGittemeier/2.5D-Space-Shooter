using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _asteroidSpeed = 0.25f;

    [SerializeField]
    private Animator _asteroidAnimator;

    [SerializeField]
    private AudioSource _asteroidAudio;

    [SerializeField]
    private bool _asteroidDetroyed = false;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private Collider2D _asteroidCollider;

    [SerializeField]
    private Rigidbody2D _asteroidRB;

    // Start is called before the first frame update
    void Start()
    {
        _asteroidAnimator = gameObject.GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _asteroidAudio = gameObject.GetComponent<AudioSource>();
        _asteroidCollider = gameObject.GetComponent<Collider2D>();
        _asteroidRB = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3 (0, 0, 360) * _asteroidSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            _asteroidSpeed = 0;
            _asteroidAnimator.SetTrigger("OnAsteroidHit");
            Destroy(collision.gameObject);
            _asteroidAudio.Play();
            Destroy(_asteroidCollider);
            Destroy(_asteroidRB);
            Destroy(this.gameObject, 2.38f);
            _asteroidDetroyed = true;
            if (_asteroidDetroyed == true)
            {
                _spawnManager.StartSpawning();
            }
        }
    }
}
