using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;

    void Start()
    {
        
    }

    void Update()
    {
        MoveUp();
        DestroyLaser();
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        if (transform.position.y >= 8f)
        {
            Destroy(this.gameObject);
        }
    }
}
