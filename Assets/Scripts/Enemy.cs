﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //-5.36f bottom of screen for enemy
    //6.93f top of screen
    //-9.44f left limit x
    //9.48f right limit x

    public float EnemySpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveDown();
        Respawn();
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * EnemySpeed * Time.deltaTime);
    }

    void Respawn()
    {
        if (transform.position.y <= -5.36f)
        {
            transform.position = new Vector3(Random.Range(-9.44f, 9.48f), 6.93f, 0);
        }
    }
}
