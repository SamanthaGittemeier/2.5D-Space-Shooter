using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBombPowerup : MonoBehaviour
{
    [SerializeField]
    private GameObject _pickup;
    [SerializeField]
    private GameObject _shockwave;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private GameObject _smokeCloudPrefab;

    [SerializeField]
    private Animator _pickupAnimator;

    [SerializeField]
    private Collider2D _parentCollider;

    [SerializeField]
    private Rigidbody2D _parentRigidbody;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Enemy[] _currentEnemies;

    [SerializeField]
    private PowerUp[] _currentPowerups;

    [SerializeField]
    private SpawnManager _spawnManager;

    void Start()
    {
        _shockwave = GameObject.Find("Shockwave");
        _shockwave.SetActive(false);
        _pickup = GameObject.Find("Pickup");
        _pickupAnimator = _pickup.GetComponent<Animator>();
        _parentCollider = gameObject.GetComponent<Collider2D>();
        _parentRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _powerupContainer = GameObject.Find("PowerupContainer");
        gameObject.transform.parent = _powerupContainer.transform;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(AtomBomb());
        }
        if (collision.tag == "Enemy Laser")
        {
            SmokeCloud();
            Destroy(this.gameObject);
        }
    }

    IEnumerator AtomBomb()
    {
        _pickupAnimator.SetTrigger("SettingOff");
        Destroy(_parentCollider);
        Destroy(_parentRigidbody);
        yield return new WaitForSeconds(2.5f);
        _pickup.SetActive(false);
        _shockwave.SetActive(true);
        _spawnManager.CallToPauseSpawning();
        _player.FoundAtomBomb();
        DisableEnemies();
        StopPowerups();
        yield return new WaitForSeconds(5f);
        Destroy(_pickup);
        Destroy(_shockwave);
        StopCoroutine(AtomBomb());
    }

    void SmokeCloud()
    {
        Instantiate(_smokeCloudPrefab, transform.position + new Vector3(0, -.75f, 0), Quaternion.identity);
    }

    void DisableEnemies()
    {
        _currentEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy target in _currentEnemies)
        {
            target.FreezeEnemy();
        }
    }

    void StopPowerups()
    {
        _currentPowerups = GameObject.FindObjectsOfType<PowerUp>();
        foreach (PowerUp powerup in _currentPowerups)
        {
            powerup.PausePowerups();
        }
    }
}
