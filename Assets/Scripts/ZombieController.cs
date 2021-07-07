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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();  //GetComponent: このファイルが設定されているオブジェクトのcomponentから用いたいものを指定して変数に格納
        agent = GetComponent<NavMeshAgent>();
    }

    // Animationを止めるための関数
    public void TurnOffTrigger()
    {
        animator.SetBool("Walk", false);   // 第一引数のパラメータ(Animatorで作成したbool型のパラメータ)をfalseにする
        animator.SetBool("Run", false);
        animator.SetBool("Death", false);
        animator.SetBool("Attack", false);
    }
    // Update is called once per frame
    void Update()
    {

        switch (state)  // stateの変数によって実行内容を変える
        {
            case STATE.IDLE:  //IDLE状態の時を定義
                TurnOffTrigger();

                if(Random.Range(0,5000) < 5)  //確率で実行される
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

                    TurnOffTrigger();  //リセット

                    agent.speed = walkingSpeed;  // ゾンビのスピードを指定
                    animator.SetBool("Walk", true);  // 歩きモーションを実行
                }

                // 確率でIDLE状態になる
                if(Random.Range(0,5000) < 5)
                {
                    state = STATE.IDLE;
                    agent.ResetPath();   // ゾンビの目的地をなしに設定する
                }
                break;
        }
    }
}
