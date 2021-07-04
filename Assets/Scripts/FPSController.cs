using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 参考URL: https://youtu.be/hFyKH5jh6UQ

public class FPSController : MonoBehaviour
{
    // 移動用の変数を作成
    float x, z;

    // スピード調整用の変数を作成
    float speed = 0.1f;

    public GameObject cam;  // カメラ
    Quaternion cameraRot, characterRot;  // カメラとプレイヤーの角度(Quaternion は角度を用いる時に使う)
    float Xsensitivity = 3f, Ysensitivity = 3f;  //カメラとプレイヤーの操作感度

    bool cursorLock = true;     //true:非表示,  false:表示

    float minX = -90f, maxX = 90f;   //角度に制限を儲けるための数値

    public Animator animator;   // プレイヤーの動きとボタンを関連付けるための変数を宣言

    // Start is called before the first frame update
    void Start()   // void: 戻り値を返さない関数を定義するときに使う
    {
        cameraRot = cam.transform.localRotation;  // 初めに作成しておきたいので、Start内に
        characterRot = transform.localRotation;

        GameState.canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensitivity;　// Input 入力キーに対する操作
        float yRot = Input.GetAxis("Mouse Y") * Xsensitivity; // xを軸に取った縦方向の回転

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);  //(x,y,z) それぞれの角度を設定 (-にしたのは、今回逆だから)
                     // yに設定しないのは、カメラ方向に基づいて移動するときに、水平方向のみ移動可能にするため
                 // 水平方向の回転は親オブジェクトであるplayerに対応させて、カメラはⅹを軸にした縦方向の回転縛りにしたい    

        characterRot *= Quaternion.Euler(0, xRot, 0);  // 軸を元に回転するので、x座標にyRot. y座標にxRot
                                                       // Playerはyを軸にした水平方向の回転縛りにしたい

        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot; // カメラ角度の更新を反映
        transform.localRotation = characterRot;  // プレイヤー角度更新を反映

        UpdateCursorLock();

       // プレイヤーの動作とボタンを関連付けている
        if (Input.GetMouseButton(0) && GameState.canShoot)   // 左マウスボタンで撃つ(GameStateファイルから変数を呼んでいる)
        {
            animator.SetTrigger("Fire");
            GameState.canShoot = false;   // 連続でfireのモーションが実行されるのを防ぐ
        }
        if (Input.GetKeyDown(KeyCode.R))  // Rボタンでリロード
        {
            animator.SetTrigger("Reload");
        }

        // Walkを設定
        if(Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)  //動いている時に、歩くアニメーションを行う(この行ではBool型であるWalkがfalseのとき)
        {               // ||: or演算子
            if (!animator.GetBool("Walk"))
            {
                animator.SetBool("Walk", true);
            }
        }
        else if (animator.GetBool("Walk"))  // 一つ上の条件が当てはまらないときにこれが実効
        {
            animator.SetBool("Walk", false);  // 動いていないのにtrueはおかしいので、アニメーションをfalseにする
        }

        // Runを設定
        if(z>0 && Input.GetKey(KeyCode.LeftShift))  // zは;で前、-で後ろなので、後ろ向きで走れなくしたいから絶対値はつけない
        {    // &&: AND演算子      左のシフトボタンを押している間(GetKey:押している間, GetKeyDown:一回押したら)実行
            if (!animator.GetBool("Run"))
            {
                animator.SetBool("Run", true);
                speed = 0.25f;
            }
        }
        else if(animator.GetBool("Run"))
        {
            animator.SetBool("Run", false);
            speed = 0.1f;
        }
    }

    private void FixedUpdate()  //0.02秒ごとに行う
    {
        x = 0;
        z = 0;

        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;

        //transform.position += new Vector3(x, 0, z);  // プレイヤーの位置を更新

        transform.position += cam.transform.forward * z + cam.transform.right * x;  // カメラの向いている方向に基づいて進むように設定
                  // forward: z軸,    right: x軸
    }

    public void UpdateCursorLock()   //カーソルを表示するかしないか
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //Escapeボタンを押したとき
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))   //マウスボタンの左を押したとき
        {
            cursorLock = true;
        }

        if (cursorLock)  //trueのとき
        {
            Cursor.lockState = CursorLockMode.Locked;    // 非表示にする
        }

        else if(!cursorLock)  //falseのとき
        {
            Cursor.lockState = CursorLockMode.None;   // 表示にする
        }
    }

    public Quaternion ClampRotation(Quaternion q)   //この関数ではQuaternionを戻り値として返す
    {
        //q = x,y,z,w (x,y,zはベクトル(量と向き)  :wはスカラー(座標とは無関係の量:回転する))

        // 単位ベクトルにする(角度を求めるため)
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;   //角度をラジアンで表示

        angleX = Mathf.Clamp(angleX, minX, maxX);  // Clamp:　値の下限と上限を設定

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}
