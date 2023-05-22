using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemeyPrefab_SM;
    private float _enemyRandomX;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        while (_stopSpawning == false)
        {
            _enemyRandomX = Random.Range(-9.44f, 9.48f);
            GameObject NewEnemy = Instantiate(_enemeyPrefab_SM, new Vector3(_enemyRandomX, 6.93f, 0), Quaternion.identity);
            NewEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
        if (_stopSpawning == true)
        {
            StopCoroutine(SpawnEnemies());
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        Destroy(_enemyContainer);
    }
}
