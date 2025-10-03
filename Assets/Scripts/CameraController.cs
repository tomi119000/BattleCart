using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 diff; //ターゲットとの距離の差
    CharacterController controller;
    GameObject player; 

    public float followSpeed = 8; //カメラ補完スピード

    //カメラの初期位置
    public Vector3 defaultPos = new Vector3(0, 6, -6);
    public Vector3 defaultRotate = new Vector3(12, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //カメラを変数で決めた初期位置・角度にする
        transform.position = defaultPos;
        transform.rotation = Quaternion.Euler(defaultRotate); //RotationはQuaternion型

        //プレイヤー情報の取得
        player = GameObject.FindGameObjectWithTag("Player");

        //プレイヤーとカメラの距離感を記憶しておく
        diff = player.transform.position - transform.position; 

    }

    // Update is called once per frame
    void Update() //Update後に動くもの
    {
        //プレイヤーが見つからなければ何もしない
        if (player == null) return; 

        //線形補完を使って、カメラを目的の場所に動かす
        //Lerpメソッド：引数 = (現在地、向かう位置、進捗）
        transform.position = Vector3.Lerp(transform.position, player.transform.position - diff, followSpeed * Time.deltaTime);
    }
}
