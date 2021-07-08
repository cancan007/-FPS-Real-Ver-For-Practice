using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool canShoot;   // static: このプロジェクトで共有できるようになる
    // static変数は、クラス名.変数名で呼び出せる

    public static bool GameOver = false;
}
