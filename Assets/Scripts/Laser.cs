using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    [SerializeField]
    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else if (_isEnemyLaser == true)
        {
            MoveDown();
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

    public void AssignToEnemy()
    {
        _isEnemyLaser = true;
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
