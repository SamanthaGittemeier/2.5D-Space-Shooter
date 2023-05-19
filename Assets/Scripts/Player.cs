﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //take player current position = 0, 0, 0
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerBounds();
    }

    void PlayerMovement()
    {
        //Player Input
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * Speed * Time.deltaTime);
        }
    }

    void PlayerBounds()
    {
        //Player Bounds
        if (transform.position.y <= -4.93f)
        {
            transform.position = new Vector3(transform.position.x, 6.98f, 0);
        }

        else if (transform.position.y >= 6.99f)
        {
            transform.position = new Vector3(transform.position.x, -4.92f, 0);
        }

        if (transform.position.x >= 10.37f)
        {
            transform.position = new Vector3(-10.37f, transform.position.y, 0);
        }

        else if (transform.position.x <= -10.38f)
        {
            transform.position = new Vector3(10.36f, transform.position.y, 0);
        }
    }
}
