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
    private GameObject _explosionsContainer;
    [SerializeField]
    private GameObject _bossAttackContainer;
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
    private int _chooseEnemyTypeID;
    [SerializeField]
    private int _enemyTypeMin = 0;
    [SerializeField]
    private int _enemyTypeMax;
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
        _explosionsContainer = GameObject.Find("ExplosionsContainer");
        _bossAttackContainer = GameObject.Find("BossAttackContainer");
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
                        //6 types total
                        //0 - normal enemy
                        //1 - random enemy
                        //2 - aggressive enemy
                        //3 - smart enemy
                        //4 - avoider enemy
                        //5 - boss
                        _enemyTypeMax = 1;
                        break;
                    case 1:
                        _waveSize = 5;
                        _enemyTypeMax = 2;
                        break;
                    case 2:
                        _waveSize = 7;
                        _enemyTypeMax = 3;
                        break;
                    case 3:
                        _waveSize = 10;
                        _enemyTypeMax = 4;
                        break;
                    case 4:
                        _waveSize = 15;
                        _enemyTypeMax = 5;
                        break;
                    case 5:
                        _waveSize = 1;
                        _enemyTypeMin = 5;
                        _enemyTypeMax = 6;
                        break;
                }
                for (int i = 0; i < _waveSize; i++)
                {
                    _chooseEnemyTypeID = Random.Range(_enemyTypeMin, _enemyTypeMax);
                    Debug.Log("Enemy Type is " + _chooseEnemyTypeID);
                    GameObject newEnemy = null;
                    if (_chooseEnemyTypeID == 0 || _chooseEnemyTypeID == 2 || _chooseEnemyTypeID == 3 || _chooseEnemyTypeID == 4)
                    {
                        _chooseEnemyShield = Random.Range(0, 2);
                        Debug.Log("Enemy Shield is " + _chooseEnemyShield);
                        _chooseEnemyMovement = Random.Range(0, 3);
                        Debug.Log("Enemy Movement Selection is " + _chooseEnemyMovement);
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
                    }
                    if (_chooseEnemyTypeID == 1)
                    {
                        _chooseEnemyShield = Random.Range(0, 2);
                        Debug.Log("Enemy Shield is " + _chooseEnemyShield);
                        _chooseEnemyMovement = 3;
                        Debug.Log("Random Enemy " + _chooseEnemyMovement);
                        int _randomSpawn = Random.Range(0, 2);
                        if (_randomSpawn == 0)
                        {
                            newEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(_enemyRandomX, 6.93f, 0), Quaternion.identity);
                        }
                        else if (_randomSpawn == 1)
                        {
                            int _randomXChoice = Random.Range(0, 2);
                            if (_randomXChoice == 0)
                            {
                                newEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(-9.44f, _enemyRandomY, 0), Quaternion.identity);
                            }
                            else if (_randomXChoice == 1)
                            {
                                newEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(9.44f, _enemyRandomY, 0), Quaternion.identity);
                            }
                        }
                        newEnemy.GetComponent<Enemy>().ChooseLengths();
                    }
                    if (_chooseEnemyTypeID == 5)
                    {
                        _chooseEnemyMovement = 5;
                        Debug.Log("Boss" + _chooseEnemyMovement);
                        _chooseEnemyShield = 0;
                        newEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(0, 10, 0), Quaternion.identity);
                    }
                    newEnemy.GetComponent<Enemy>().EnemyMovementID(_chooseEnemyMovement);
                    newEnemy.GetComponent<Enemy>().EnemyShieldChoice(_chooseEnemyShield);
                    newEnemy.GetComponent<Enemy>().EnemyTypeID(_chooseEnemyTypeID);
                    newEnemy.transform.SetParent(_enemyContainer.transform);
                    _spawned++;
                    yield return new WaitForSeconds(1);
                }
            }
            if (_enemyContainer.transform.childCount == 0)
            {
                yield return new WaitForSeconds(2);
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
            _rarePowerupSpawnTime = Random.Range(60f, 90f);
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
        Destroy(_explosionsContainer);
        Destroy(_bossAttackContainer);
    }
}
