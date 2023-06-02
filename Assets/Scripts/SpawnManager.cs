using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemeyPrefab_SM;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    [SerializeField]
    private GameObject[] _rarePowerups;
    [SerializeField]
    private GameObject[] _powerups;

    private float _enemyRandomX;
    private float _powerupRandomX;
    private float _rarePowerupSpawnTime;

    private bool _stopSpawningEnemies;
    private bool _stopSpawningPowerups;

    void Start()
    {
        _enemyContainer = GameObject.Find("EnemyContainer");
        _powerupContainer = GameObject.Find("PowerupContainer");
    }

    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
        StartCoroutine(SpawnRarePowerup());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningEnemies == false)
        {
            _enemyRandomX = Random.Range(-9.44f, 9.48f);
            GameObject NewEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(_enemyRandomX, 6.93f, 0), Quaternion.identity);
            NewEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
        if (_stopSpawningEnemies == true)
        {
            StopCoroutine(SpawnEnemies());
        }
    }

    public IEnumerator SpawnPowerups()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningPowerups == false)
        {
            _powerupRandomX = Random.Range(-9.25f, 9.25f);
            int randomPowerups = Random.Range(0, _powerups.Length);
            GameObject NewPowerup = Instantiate(_powerups[randomPowerups], new Vector3(_powerupRandomX, 6.56f, 0), Quaternion.identity);
            NewPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
        if (_stopSpawningPowerups == true)
        {
            StopCoroutine(SpawnPowerups());
        }
    }

    public IEnumerator SpawnRarePowerup()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningPowerups == false)
        {
            _rarePowerupSpawnTime = Random.Range(5f, 15f);
            yield return new WaitForSeconds(_rarePowerupSpawnTime);
            int randomRarePowerups = Random.Range(0, _rarePowerups.Length);
            GameObject NewRarePowerup = Instantiate(_rarePowerups[randomRarePowerups], new Vector3(0, 0, 0), Quaternion.identity);
            NewRarePowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(_rarePowerupSpawnTime);
        }
        if (_stopSpawningPowerups == true)
        {
            StopCoroutine(SpawnRarePowerup());
        }
    }

    public void CallToPauseSpawning()
    {
        StopAllCoroutines();
        StartCoroutine(PauseSpawning());
    }

    IEnumerator PauseSpawning()
    {
        yield return new WaitForSeconds(5f);
        StartSpawning();
    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemies = true;
        Destroy(_enemyContainer);
        _stopSpawningPowerups = true;
        Destroy(_powerupContainer);
    }
}
