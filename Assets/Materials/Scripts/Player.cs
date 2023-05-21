﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 5f;
    public GameObject LaserPrefab;
    private float FireRate = 0.25f;
    private float CanFire = -1f;
    public float Lives = 3;
    public GameObject SpawnManager;

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        SpawnManager = GameObject.FindGameObjectWithTag("Spawn Manager");
    }

    void Update()
    {
        PlayerMovement();
        PlayerBounds();
        ShootLaser();
    }

    public void Damage()
    {
        Lives--;
        if (Lives <= 0)
        {
            Destroy(SpawnManager.GetComponent<SpawnManager>().EnemyContainer);
            Destroy(this.gameObject);
            Debug.Log(Lives + "Lives Left");
        }
    }

    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > CanFire)
        {
            CanFire = Time.time + FireRate;
            Instantiate(LaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            Debug.Log ("Space was pressed");
        }
    }

    void PlayerMovement()
    {
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