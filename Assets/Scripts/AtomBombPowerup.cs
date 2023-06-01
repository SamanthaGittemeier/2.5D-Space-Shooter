using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBombPowerup : MonoBehaviour
{
    [SerializeField]
    private GameObject _shockwavePrefab;

    [SerializeField]
    private Collider2D _pickupCollider;

    [SerializeField]
    private Rigidbody2D _pickupRigidbody;

    [SerializeField]
    private float _randomSpawnTime;

    [SerializeField]
    private Animator _atomBombAnimator;

    [SerializeField]
    private Player _atomBombPlayer;

    [SerializeField]
    private Enemy _atomBombEnemy;

    [SerializeField]
    private SpawnManager _atomBombSpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _pickupCollider = gameObject.GetComponent<Collider2D>();
        _pickupRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _atomBombAnimator = gameObject.GetComponent<Animator>();
        _atomBombPlayer = GameObject.Find("Player").GetComponent<Player>();
        _atomBombEnemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        _atomBombSpawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(_pickupCollider);
            Destroy(_pickupRigidbody);
            _atomBombAnimator.SetTrigger("SettingOff");
            _atomBombPlayer.FoundAtomBomb();
            _atomBombEnemy.AtomBombAwakens();
            _atomBombSpawnManager.CallToPauseSpawning();
            Destroy(this.gameObject, 3f);
        }
    }
}
