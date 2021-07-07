using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    // 銃操作時のサウンドの変数を宣言
    public AudioSource weapon;  // スピーカー用の変数を宣言
    public AudioClip reloadingSE, fireSE, triggerSE;  // それぞれの音源の変数を宣言

    public static Weapon instance; // instanceをどこでも共有できるようにしている(staticで)
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
}
