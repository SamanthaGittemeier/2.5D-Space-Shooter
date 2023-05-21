using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float LaserSpeed = 8f;

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
        transform.Translate(Vector3.up * LaserSpeed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        if (transform.position.y >= 8f)
        {
            Destroy(this.gameObject);
        }
    }
}
