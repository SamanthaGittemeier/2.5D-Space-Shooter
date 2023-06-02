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
    private Animator _pickupAnimator;

    [SerializeField]
    private Collider2D _parentCollider;

    [SerializeField]
    private Rigidbody2D _parentRigidbody;

    [SerializeField]
    private float _randomSpawnTime;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Enemy[] _currentEnemies;

    [SerializeField]
    private PowerUp[] _currentPowerups;

    [SerializeField]
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _shockwave = GameObject.Find("Shockwave");
        _shockwave.SetActive(false);
        _pickup = GameObject.Find("Pickup");
        _pickupAnimator = _pickup.GetComponent<Animator>();
        _parentCollider = gameObject.GetComponent<Collider2D>();
        _parentRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _powerupContainer = GameObject.Find("PowerupContainer");
        gameObject.transform.parent = _powerupContainer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(AtomBomb());
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

    void DisableEnemies()
    {
        _currentEnemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy target in _currentEnemies)
        {
            target.AtomBombIncoming();
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
