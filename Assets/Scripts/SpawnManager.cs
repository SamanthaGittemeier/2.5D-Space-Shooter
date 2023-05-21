using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject EnemeyPrefab_SM;
    public float EnemyRandomX;
    public GameObject Player_SM;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void EnemySpawnDelay()
    //{
    //    yield return new WaitForSeconds(5);
    //}

    IEnumerator SpawnEnemies()
    {
        while (Player_SM.GetComponent<Player>().Lives >= 1)
        {
            EnemyRandomX = Random.Range(-9.44f, 9.48f);
            Instantiate(EnemeyPrefab_SM, new Vector3(EnemyRandomX, 6.93f, 0), Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
        }
        if (Player_SM.GetComponent<Player>().Lives <= 0)
        {
            StopCoroutine(SpawnEnemies());
        }
    }
}
