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
    private GameObject _tripleShotPowerupPrefab;
    [SerializeField]
    private GameObject _speedPowerupPrefab;
    [SerializeField]
    private GameObject[] _powerups;

    private float _enemyRandomX;
    private float _tripleShotPowerupRandomX;

    private bool _stopSpawningEnemeis;
    private bool _stopSpawningPowerups;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningEnemeis == false)
        {
            _enemyRandomX = Random.Range(-9.44f, 9.48f);
            GameObject NewEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(_enemyRandomX, 6.93f, 0), Quaternion.identity);
            NewEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
        if (_stopSpawningEnemeis == true)
        {
            StopCoroutine(SpawnEnemies());
        }
    }

    public IEnumerator SpawnPowerups()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningPowerups == false)
        {
            _tripleShotPowerupRandomX = Random.Range(-9.25f, 9.25f);
            int randomPowerups = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[randomPowerups], new Vector3(_tripleShotPowerupRandomX, 6.56f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
        if (_stopSpawningPowerups == true)
        {
            StopCoroutine(SpawnPowerups());
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemeis = true;
        Destroy(_enemyContainer);
        _stopSpawningPowerups = true;
    }
}
