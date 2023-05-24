using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 4f;

    void Start()
    {

    }

    void Update()
    {
        MoveDown();
        Respawn();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit" + collision.transform.name);
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
