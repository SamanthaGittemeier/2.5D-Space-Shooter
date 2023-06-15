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

    private float _powerupRandomX;
    private float _rarePowerupSpawnTime;
    private float _enemyRandomY;
    private float _enemyRandomX;

    [SerializeField]
    private int _chooseEnemyMovement;
    [SerializeField]
    private int _chooseEnemyShield;
    [SerializeField]
    private int _waveID;
    [SerializeField]
    private int _waveSize;
    [SerializeField]
    private int _spawned;

    private bool _stopSpawningEnemies;
    private bool _stopSpawningPowerups;

    void Start()
    {
        _enemyContainer = GameObject.Find("EnemyContainer");
        _powerupContainer = GameObject.Find("PowerupContainer");
        _waveID = 0;
    }

    void Update()
    {
        _enemyRandomX = Random.Range(-9.44f, 9.48f);
        _enemyRandomY = Random.Range(4.7f, 0f);
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
            if (_spawned == 0)
            {
                switch (_waveID)
                {
                    case 0:
                        _waveSize = 3;
                        break;
                    case 1:
                        _waveSize = 5;
                        break;
                    case 2:
                        _waveSize = 7;
                        break;
                    case 3:
                        _waveSize = 10;
                        break;
                    case 4:
                        _waveSize = 15;
                        break;
                }
                for (int i = 0; i < _waveSize; i++)
                {
                    _chooseEnemyMovement = Random.Range(0, 3);
                    Debug.Log(_chooseEnemyMovement);
                    //_chooseEnemyShield = Random.Range(0, 2);
                    //Debug.Log(_chooseEnemyShield);
                    GameObject newEnemy = null;
                    switch (_chooseEnemyMovement)
                    {
                        case 0:
                            newEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(_enemyRandomX, 6.93f, 0), Quaternion.identity);
                            break;
                        case 1:
                            newEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(-9.44f, _enemyRandomY, 0), Quaternion.identity);
                            break;
                        case 2:
                            newEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(9.44f, _enemyRandomY, 0), Quaternion.identity);
                            break;
                    }
                    newEnemy.GetComponent<Enemy>().EnemyID(_chooseEnemyMovement);
                    //newEnemy.GetComponent<Enemy>().EnemyShieldChoice(_chooseEnemyShield);
                    //newEnemy.transform.parent = _enemyContainer.transform;
                    newEnemy.transform.SetParent(_enemyContainer.transform);
                    _spawned++;
                    yield return new WaitForSeconds(1);
                }
            }
            if (_enemyContainer.transform.childCount == 0)
            {
                _waveID++;
                _spawned = 0;
            }
            yield return null;
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
            _rarePowerupSpawnTime = Random.Range(120, 180f);
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
