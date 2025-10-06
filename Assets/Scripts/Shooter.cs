using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    Transform player;
    GameObject gate;
    public float shootSpeed =100f;
    public float upSpeed = 8f; //投げた時の上向きの力

    bool possibleShoot; //ショット可能フラグ

    public int shotPower = 10;
    public int recoverySeconds = 3;

    Camera cam; //カメラ情報の取得

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //時間差でシュート可能にする
        Invoke("ShootEnabled", 0.5f); 

        //プレイヤーのTransform情報を取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Playerに付いているgateオブジェクト情報の取得
        gate = player.Find("Gate").gameObject;

        //カメラ情報の取得（MainCameraタグがついているカメラ情報は簡単に参照可能）
        cam = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;
        if(Input.GetMouseButtonDown(0)) //もし左クリックされたら
        {
            if (possibleShoot) Shot(); //FlagがOnなら発射するメソッド
        }
    }

    void ShootEnabled()
    {
        possibleShoot = true; 
    }

    void Shot()
    {
        //playerがいない、またはshotPowerが0以下の場合は何もしない（return）
        if (player == null || shotPower <= 0) return;

        //bulletのプレハブを生成
        GameObject obj = Instantiate(bulletPrefab, gate.transform.position, Quaternion.identity);

        Rigidbody rbody = obj.GetComponent<Rigidbody>();

        Vector3 v = new Vector3(
            cam.transform.forward.x * shootSpeed,
            cam.transform.forward.y * upSpeed,
            cam.transform.forward.z * shootSpeed
            );

        rbody.AddForce(v, ForceMode.Impulse);

        ConsumePower();
    }

    //shotPowerを消費
    void ConsumePower()
    {
        shotPower--;
        StartCoroutine(RecoverPower()); //回復コルーチン
    }

    //回復コルーチン
    IEnumerator RecoverPower()
    {
        //recoverySeconds待つ
        yield return new WaitForSeconds(recoverySeconds);
        shotPower++; //1つ回復
    }

}
