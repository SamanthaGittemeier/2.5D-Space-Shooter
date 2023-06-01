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
    private bool _stopSpawningAtomBomb;

    [SerializeField]
    private float _randomSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        _stopSpawningAtomBomb = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _pickupCollider = gameObject.GetComponent<Collider2D>();
        _pickupRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void SpawnAtomBomb()
    {
        StartCoroutine(SpawnPickup());
    }

    public IEnumerator SpawnPickup()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningAtomBomb == false)
        {
            _randomSpawnTime = Random.Range(1f, 15f);
            yield return new WaitForSeconds(_randomSpawnTime);
            gameObject.SetActive(true);
        }
        if (_stopSpawningAtomBomb == true)
        {
            StopCoroutine(SpawnPickup());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(_pickupCollider);
            Destroy(_pickupRigidbody);
            StartCoroutine(AtomBombDelay());
        }
    }

    public IEnumerator AtomBombDelay()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(false);
        Instantiate(_shockwavePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        gameObject.AddComponent<Rigidbody2D>();
        gameObject.AddComponent<Collider2D>();
    }

    public void PlayerDied()
    {
        _stopSpawningAtomBomb = true;
        Destroy(this.gameObject);
    }
}
