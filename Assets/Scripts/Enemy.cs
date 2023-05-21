using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //-5.36f bottom of screen for enemy
    //6.93f top of screen
    //-9.44f left limit x
    //9.48f right limit x

    public float EnemySpeed = 4f;

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
        transform.Translate(Vector3.down * EnemySpeed * Time.deltaTime);
    }

    public void Respawn()
    {
        if (transform.position.y <= -5.36f)
        {
            float RandomX = Random.Range(-9.44f, 9.48f);
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
