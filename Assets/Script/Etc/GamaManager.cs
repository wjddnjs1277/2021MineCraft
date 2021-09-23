using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : Singleton<GamaManager>
{
    [SerializeField] Item[] item;
    [SerializeField] float respawnTime;
    [SerializeField] EnemyController[] enemyArray;
    [SerializeField] GameObject player;


    private void Start()
    {
        PlayerState.Instance.Inven.Push(item[0]);
        PlayerState.Instance.Inven.Push(item[1]);
        for(int i = 0; i < 50; i++)
        {
            PlayerState.Instance.Inven.Push(item[2]);
            PlayerState.Instance.Inven.Push(item[3]);
        }

        for (int i = 0; i < 3; i++)
        {
            enemyArray[i].Setup(i);
            enemyArray[i].OnDeadEnemy += OnDeadEnemy;
        }

    }

    void OnDeadEnemy(int index)
    {
        StartCoroutine(EnemyRespawn(index));
    }

    IEnumerator EnemyRespawn(int index)
    {
        yield return new WaitForSeconds(respawnTime);

        float x = Random.Range(player.transform.position.x + 7, 15);
        float z = Random.Range(player.transform.position.z + 7, 15);

        Vector3 spawnPoint = new Vector3(x, 30f, z);

        enemyArray[index].Respawn();
        enemyArray[index].transform.position = spawnPoint;
    }
}
