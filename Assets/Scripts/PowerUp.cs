﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3f;

    [SerializeField]     //0 = Triple Shot 1 = Speed 2 = Shield
    private int _powerupID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScreenBounds();
        MoveDown();
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
                        break;
                    case 1:
                        player.FoundSpeedBoost();
                        Debug.Log("Speed Boost Acquired");
                        break;
                    case 2:
                        player.FoundShield();
                        Debug.Log("Shields Found");
                        break;
                    case 3:
                        player.FoundAmmo();
                        Debug.Log("Found Ammo");
                        break;
                    case 4:
                        player.FoundHealth();
                        Debug.Log("Extra Life");
                        break;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
