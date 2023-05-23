using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3f;

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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
        }
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
}
