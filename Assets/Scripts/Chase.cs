using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;
    public float SpawnRangeX = 1;
    public float SpawnRangeZ = 1;
    private bool flag;

    private Vector3 prevPlayerPos;
    private Vector3 posVector;
    public float distance = 3.0f;
    public float chickenSpeed = 1.0f;
    public float attackDamage = 2.0f;
    private bool damageFlag = true;
    private float seconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
        prevPlayerPos = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPosition = Player.transform.position;
        Vector3 SpawnPointPosition = SpawnPoint.transform.position;

        if (flag || (PlayerPosition.x < SpawnPointPosition.x && PlayerPosition.x > SpawnPointPosition.x - SpawnRangeX && PlayerPosition.z < SpawnPointPosition.z + SpawnRangeZ && PlayerPosition.z > SpawnPointPosition.z + 0.2))
        {
            //followObject
            Vector3 currentPlayerPos = Player.transform.position;
            Vector3 backVector = (prevPlayerPos - currentPlayerPos).normalized;
            posVector = (backVector == Vector3.zero) ? posVector : backVector;
            Vector3 targetPos = currentPlayerPos + distance * posVector;
            targetPos.y = targetPos.y + 0.5f;
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                targetPos,
                chickenSpeed * Time.deltaTime
            );
            this.transform.LookAt(Player.transform.position);
            prevPlayerPos = Player.transform.position;

            flag = true;
        }

        if(damageFlag && (distance > CalcDistance()))
        {
            DamagePlayer();
            damageFlag = false;
            seconds += Time.deltaTime;
        }
        else if(!damageFlag && seconds > 5)
        {
            damageFlag = true;
        }
    }

    float CalcDistance()
    {
        return Vector3.Distance(Player.transform.position, transform.position);
    }

    // ダメージ量をPlayerのscriptに渡す(反映させる)
    public void DamagePlayer()
    {
        if (Player != null)  //Playerがtargetとなっている時
        {
            Player.GetComponent<FPSController>().TakeHit(attackDamage);  // FPSControllerクラスのTakeHit関数に、こちらで定義したattackDamage変数を引数として与える
        }
    }
}
