using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;
    public float SpawnRangeX = 1;
    public float SpawnRangeZ = 1;
    private bool flag;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        flag = true;
        pos = SpawnPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPosition = Player.transform.position;
        Vector3 SpawnPointPosition = SpawnPoint.transform.position;

        if (flag && PlayerPosition.x < SpawnPointPosition.x && PlayerPosition.x > SpawnPointPosition.x - SpawnRangeX && PlayerPosition.z < SpawnPointPosition.z + SpawnRangeZ && PlayerPosition.z > SpawnPointPosition.z + 0.2)
        {
            pos.y = 2.0f;
            transform.position = pos;
            flag = false;
        }

        //if (flag && PlayerPosition.x < SpawnPointPosition.x)
        //{
        //    for(int i = 0; i < 1; i++)
        //    {
        //        yield return new WaitForSeconds(0.5f);
        //        //Instantiate(複製するGameObject,位置,回転)の順番で記載
        //        Instantiate(this, SpawnPointPosition, Quaternion.identity);
        //    }
        //    flag = false;
        //}
    }
}
