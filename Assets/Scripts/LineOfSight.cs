using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField]
    private Enemy _parentEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Powerup")
        {
            _parentEnemy.DestroyPowerup();
        }

        if (collision.tag == "Player")
        {
            _parentEnemy.StartRamming();
        }

        if(this.gameObject.name == "Backwards Line Of Sight" && collision.tag == "Player")
        {
            _parentEnemy.FireBackshot();
        }

        if (collision.tag == "Laser")
        {
            _parentEnemy.StartAvoidance();
        }
    }
}
