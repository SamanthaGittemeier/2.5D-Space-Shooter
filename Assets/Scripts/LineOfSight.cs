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
    }
}
