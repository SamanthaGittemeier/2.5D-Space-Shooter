﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3f;
    [SerializeField]
    private float _toPlayerSpeed = 5f;

    [SerializeField]     //0 = Triple Shot 1 = Speed 2 = Shield
    private int _powerupID;

    [SerializeField]
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ScreenBounds();
        MoveDown();
        MoveToPlayer();
    }

    public void ScreenBounds()
    {
        if (transform.position.y <= -6.75f)
        {
            Debug.Log("You Missed Me");
            Destroy(this.gameObject);
        }
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
    }

    void MoveToPlayer()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _toPlayerSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.FoundTripleShotPowerup();
                        Debug.Log("Triple Shot Picked Up");
                        Destroy(this.gameObject);
                        break;
                    case 1:
                        player.FoundSpeedBoost();
                        Debug.Log("Speed Boost Acquired");
                        Destroy(this.gameObject);
                        break;
                    case 2:
                        player.FoundShield();
                        Debug.Log("Shields Found");
                        Destroy(this.gameObject);
                        break;
                    case 3:
                        player.FoundAmmo();
                        Debug.Log("Found Ammo");
                        Destroy(this.gameObject);
                        break;
                    case 4:
                        player.FoundHealth();
                        Debug.Log("Extra Life");
                        Destroy(this.gameObject);
                        break;
                    case 5:
                        player.FoundFreeze();
                        Debug.Log("Player Is Frozen");
                        Destroy(this.gameObject);
                        break;
                }
            }
        }
    }

    public void PausePowerups()
    {
        StartCoroutine(FreezePowerups());
    }

    IEnumerator FreezePowerups()
    {
        _powerupSpeed = 0;
        yield return new WaitForSeconds(5f);
        _powerupSpeed = 3f;
    }
}
