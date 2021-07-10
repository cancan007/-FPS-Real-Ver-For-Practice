using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;
    public float SpawnRangeX = 1;
    public float SpawnRangeZ = 1;
    private bool flag1;
    private bool flag2;
    public float posY = 0;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        flag1 = true;
        flag2 = false;
        pos = SpawnPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPosition = Player.transform.position;
        Vector3 SpawnPointPosition = SpawnPoint.transform.position;

        if (flag1 && PlayerPosition.x < SpawnPointPosition.x && PlayerPosition.x > SpawnPointPosition.x - SpawnRangeX && PlayerPosition.z < SpawnPointPosition.z + SpawnRangeZ && PlayerPosition.z > SpawnPointPosition.z + 0.2)
        //if (PlayerPosition.x < SpawnPointPosition.x && PlayerPosition.x > SpawnPointPosition.x - SpawnRangeX && PlayerPosition.z < SpawnPointPosition.z + SpawnRangeZ && PlayerPosition.z > SpawnPointPosition.z + 0.2)
        {
            //int intValue = Random.Range(0, 10);
            //if (intValue == 1)
            //{
                pos.y = posY;
                transform.position = pos;
                //Instantiate(this, SpawnPointPosition, Quaternion.identity);
                flag1 = false;
            //}
        }

        //if (flag && PlayerPosition.x < SpawnPointPosition.x && PlayerPosition.x > SpawnPointPosition.x - SpawnRangeX && PlayerPosition.z < SpawnPointPosition.z + SpawnRangeZ && PlayerPosition.z > SpawnPointPosition.z + 0.2)
        //{
        //    //Instantiate(複製するGameObject,位置,回転)の順番で記載
        //    Instantiate(this, SpawnPointPosition, Quaternion.identity);
        //    flag = false;
        //}
    }
}
