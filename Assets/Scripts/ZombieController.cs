using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // AIを使うため

public class ZombieController: MonoBehaviour
{

    // アニメータとAIの変数を宣言
    Animator animator;
    NavMeshAgent agent; // AI

    public float walkingSpeed; //zombieの歩くスピード用の変数

    // 列挙型であり、複数の変数に意味を持たせることができる(今回はゾンビのパターンを表現するために用いる)
    enum STATE {IDLE,WANDER,ATTACK,CHASE,DEAD};
    STATE state = STATE.IDLE;  //初期のSTATEを定義


    // プレイヤーオブジェクトとゾンビの走るスピード用の変数を宣言
    GameObject target;
    public float runSpeed;

    // ゾンビのアタック時のダメージ用の変数
    public int attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();  //GetComponent: このファイルが設定されているオブジェクトのcomponentから用いたいものを指定して変数に格納
        agent = GetComponent<NavMeshAgent>();

        if(target == null)  // targetに何も格納されていないとき
        {
            target = GameObject.FindGameObjectWithTag("Player");  // GameObjectからTagがPlayerのオブジェクト(Unity上で設定)を指定して格納
        }
    }

    // Animationを止めるための関数
    public void TurnOffTrigger()
    {
        animator.SetBool("Walk", false);   // 第一引数のパラメータ(Animatorで作成したbool型のパラメータ)をfalseにする
        animator.SetBool("Run", false);
        animator.SetBool("Death", false);
        animator.SetBool("Attack", false);
    }

    // ゾンビとプレイヤーの距離を返す関数
    float DistanceToPlayer()   // 返り値がfloat型の関数
    {
        // ゲームオーバー後にPlayerを追いかけることがないようにするため
        if (GameState.GameOver)
        {
            return Mathf.Infinity; // 無限を返す
        }
        return Vector3.Distance(target.transform.position, transform.position);  // 引数で指定した二つのオブジェクトの座標から距離を出す
    }


    // プレイヤーとの距離が近くなった時にtrueを返す関数
    bool CanSeePlayer()
    {
        if(DistanceToPlayer() < 15)
        {
            return true;
        }
        return false;  // 距離が十分近くないときはfalseを返す
    }

    // プレイヤーとの距離が十分離れたときtrueを返す関数
    bool ForGetPlayer()
    {
        if (DistanceToPlayer() > 20)
        {
            return true;
        }
        return false;
    }

    // ダメージ量をPlayerのscriptに渡す(反映させる)
    public void DamagePlayer()
    {
        if(target != null)  //Playerがtargetとなっている時
        {
            target.GetComponent<FPSController>().TakeHit(attackDamage);  // FPSControllerクラスのTakeHit関数に、こちらで定義したattackDamage変数を引数として与える
        }
    }
    // Update is called once per frame
    void Update()
    {

        switch (state)  // stateの変数によって実行内容を変える
        {
            case STATE.IDLE:  //IDLE状態の時を定義
                TurnOffTrigger();

                if (CanSeePlayer())   //プレイヤーが近くにいるとき実行
                {
                    state = STATE.CHASE;
                }
                else if (Random.Range(0, 5000) < 5)  //確率で実行される  (Playerが近くにいなければ)
                    {
                        state = STATE.WANDER;  // 状態を変更
                    }
                break;

            case STATE.WANDER:
                if (!agent.hasPath)  //zombieが目的地を持っていないとき実行
                {
                    // ゾンビの目的地を設定
                    float newX = transform.position.x + Random.Range(-5, 5);
                    float newZ = transform.position.z + Random.Range(-5, 5);

                    Vector3 NextPos = new Vector3(newX, transform.position.y, newZ); // Vector3クラスを用いて、空間内の座標を返り値として取得(クラスをインスタンス化している)

                    agent.SetDestination(NextPos);  // 目的地の座標を指定
                    agent.stoppingDistance = 0;   // 目的地にどれだけ近づいたら止まるかを指定

                    TurnOffTrigger();  //モーションリセット

                    agent.speed = walkingSpeed;  // ゾンビのスピードを指定
                    animator.SetBool("Walk", true);  // 歩きモーションを実行
                }

                // 確率でIDLE状態になる
                if(Random.Range(0,5000) < 5)
                {
                    state = STATE.IDLE;
                    agent.ResetPath();   // ゾンビの目的地をなしに設定する
                }

                if (CanSeePlayer())   //プレイヤーが近くにいるとき実行
                {
                    state = STATE.CHASE;
                }
                break;

            case STATE.CHASE:

                if (GameState.GameOver)
                {
                    TurnOffTrigger();
                    agent.ResetPath();
                    state = STATE.WANDER;

                    return;  //これ以上何も実行されないようになる
                }
                agent.SetDestination(target.transform.position);  // ゾンビの目的地をPlayerの座標に設定
                agent.stoppingDistance = 3;

                TurnOffTrigger();

                agent.speed = runSpeed;
                animator.SetBool("Run", true);  //走りモーションを実行

                // Playerに3より近づいたら実行
                if (agent.remainingDistance <= agent.stoppingDistance)  // agent.remainingDistance: agentの現在地と目的地までの残りの距離
                {
                    state = STATE.ATTACK;
                }

                if (ForGetPlayer())  // Playerがゾンビと十分離れたときに実行
                {
                    agent.ResetPath();  // 目的地をなしにする
                    state = STATE.WANDER;
                }

                break;

            case STATE.ATTACK:
                if (GameState.GameOver)
                {
                    TurnOffTrigger();
                    agent.ResetPath();
                    state = STATE.WANDER;

                    return;  //これ以上何も実行されないようになる
                }

                TurnOffTrigger();
                animator.SetBool("Attack", true);

                // ゾンビの向きをPlayerの座標方向に向ける
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

                // playerとの距離が5より遠いとき、CHASE状態にする
                if(DistanceToPlayer() > agent.stoppingDistance + 2)
                {
                    state = STATE.CHASE;
                }

                break;
        }
    }
}
