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
    private bool _asteroidDetroyed = false;

    [SerializeField]
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _asteroidAnimator = gameObject.GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
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
            Destroy(this.gameObject, 2.38f);
            _asteroidDetroyed = true;
            if (_asteroidDetroyed == true)
            {
                _spawnManager.StartSpawning();
            }
        }
    }
}
