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
    private GameObject _tripleShotPowerupContainer;

    private float _enemyRandomX;
    private float _tripleShotRandomX;

    private bool _stopSpawningEnemeis;
    private bool _stopSpawningTripleShotPowerup;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnTripleShotPowerup());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
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

    public IEnumerator SpawnTripleShotPowerup()
    {
        while (_stopSpawningTripleShotPowerup == false)
        {
            _tripleShotRandomX = Random.Range(-9.25f, 9.25f);
            Instantiate(_tripleShotPowerupPrefab, new Vector3(_tripleShotRandomX, 6.56f, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
        if (_stopSpawningTripleShotPowerup == true)
        {
            StopCoroutine(SpawnTripleShotPowerup());
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemeis = true;
        Destroy(_enemyContainer);
        _stopSpawningTripleShotPowerup = true;
    }
}
