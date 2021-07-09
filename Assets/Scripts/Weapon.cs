using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    // 銃操作時のサウンドの変数を宣言
    public AudioSource weapon;  // スピーカー用の変数を宣言
    public AudioClip reloadingSE, fireSE, triggerSE;  // それぞれの音源の変数を宣言

    public static Weapon instance; // instanceをどこでも共有できるようにしている(staticで)

    public Transform shotDirection;  // 弾を発射する位置のオブジェクト用の変数
    // Start is called before the first frame update

    // この関数によって、FPSController.csでWeapon.csの関数や変数を呼び出せるようにしている
    private void Awake()  // Awake(): Start()よりも早く実行される
    {  // ほかのファイルから呼び出すときは、Weapon.instance.変数名 という形で呼ぶ
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 弾の軌道を表示
        Debug.DrawRay(shotDirection.position, shotDirection.transform.forward * 10, Color.green);
    }

    public void CanShoot()   
    {
        GameState.canShoot = true;
    }

    // 撃つときのサウンド用の関数
    public void FireSE()
    {
        weapon.clip = fireSE;
        weapon.Play();
    }
    // リロード時のサウンド用の関数
    public void ReloadingSE()
    {
        weapon.clip = reloadingSE;
        weapon.Play();
    }
    // 弾がないときのトリガー用のサウンド関数
    public void TriggerSE()
    {
        if (!weapon.isPlaying)  //音が鳴っていないとき
        {
            weapon.clip = triggerSE;
            weapon.Play();
        }
    }

    // 当たり判定の弾を飛ばす関数
    public void Shooting()
    {
        RaycastHit hitInfo;  //あたったオブジェクトの情報を格納する変数

        // オブジェクトにあたったとき
        if (Physics.Raycast(shotDirection.transform.position, shotDirection.transform.forward, out hitInfo, 300))   // Physics.Raycastp(): 弾を飛ばす関数,  引数:(弾の発射座標, 飛ばす方向, あたったオブジェクトの情報を格納する変数, 弾の届く距離)  out: 空の変数でも渡せるようになる
        {
            // あたったオブジェクトがZombieControllerコンポーネントを持っていたら実行
            if(hitInfo.collider.gameObject.GetComponent<ZombieController>() != null)
            {
                ZombieController hitZombie = hitInfo.collider.gameObject.GetComponent<ZombieController>();  //弾に当たったゾンビのZombieControllerをhitZombieに格納

                hitZombie.ZombieDeath();  //あたったゾンビのZombieController.csからZombieDeath()関数を実行
            }
        }
    }
}
