using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject EnemeyPrefab_SM;
    public float EnemyRandomX;
    public GameObject Player_SM;
    public GameObject EnemyContainer;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        while (Player_SM.GetComponent<Player>().Lives >= 1)
        {
            EnemyRandomX = Random.Range(-9.44f, 9.48f);
            GameObject NewEnemy = Instantiate(EnemeyPrefab_SM, new Vector3(EnemyRandomX, 6.93f, 0), Quaternion.identity);
            NewEnemy.transform.parent = EnemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
        if (Player_SM.GetComponent<Player>().Lives <= 0)
        {
            StopCoroutine(SpawnEnemies());
        }
    }
}
