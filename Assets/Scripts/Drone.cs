using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField]
    private int _droneID;
    [SerializeField]
    private int _firingPatternChoice;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_firingPatternChoice == 0)
        {
            if (_droneID == 0)
            {
                gameObject.SetActive(true);
            }
            else if (_droneID != 0)
            {
                Destroy(this.gameObject);
            }
        }
        else if (_firingPatternChoice == 1)
        {
            switch (_droneID)
            {
                case 0:
                    gameObject.SetActive(true);
                    break;
                case 1:
                    gameObject.SetActive(true);
                    break;
                case 2:
                    gameObject.SetActive(true);
                    break;
                case 3:
                    gameObject.SetActive(true);
                    break;
                case 4:
                    Destroy(this.gameObject);
                    break;
                case 5:
                    Destroy(this.gameObject);
                    break;
                case 6:
                    Destroy(this.gameObject);
                    break;
                case 7:
                    Destroy(this.gameObject);
                    break;
            }
        }
        else if (_firingPatternChoice == 2)
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FiringPattern(int PatternChoice)
    {
        _firingPatternChoice = PatternChoice;
    }

    public void KillDrone()
    {
        GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _explosion.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        Destroy(_explosion, 2.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
                KillDrone();
            }
        }

        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            KillDrone();
        }

        if (collision.tag == "Shockwave")
        {
            KillDrone();
        }
    }
}
