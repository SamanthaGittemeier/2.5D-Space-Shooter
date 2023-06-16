using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;
    [SerializeField]
    private float _closestDistance = Mathf.Infinity;
    [SerializeField]
    private float _distanceToTarget;
    [SerializeField]
    private float _angleToEnemy;

    [SerializeField]
    private bool _isEnemyLaser = false;
    [SerializeField]
    private bool _isHomingLaser = false;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Transform _targetEnemy;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (_isEnemyLaser == true)
        {
            MoveDown();
        }
        if (_isHomingLaser == true)
        {
            FindClosestEnemy();
            MoveToClosestEnemy();
        }
        else if (_isEnemyLaser == false && _isHomingLaser == false)
        {
            MoveUp();
        }
    }

    public void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y >= 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        if (transform.position.y <= -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void FindClosestEnemy()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            _distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);
            if (_distanceToTarget < _closestDistance)
            {
                _closestDistance = _distanceToTarget;
                _targetEnemy = enemy.transform;
            }
        }
    }

    public void MoveToClosestEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetEnemy.transform.position, _laserSpeed * Time.deltaTime);
        Vector3 targetDirection = _targetEnemy.position - transform.position;
        _angleToEnemy = Vector3.Angle(targetDirection, transform.up);
        transform.Rotate(0, 0, _angleToEnemy);
    }

    public void AssignToEnemy()
    {
        _isEnemyLaser = true;
    }

    public void AssignAsHoming()
    {
        _isHomingLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isEnemyLaser == true && collision.tag == "Player")
        {
            Debug.Log("Hit Player");
            _player.Damage();
            Destroy(this.gameObject);
        }
        
        if (collision.tag == "Shockwave")
        {
            Debug.Log("Atom Bomb!");
            Destroy(this.gameObject);
        }
    }
}
