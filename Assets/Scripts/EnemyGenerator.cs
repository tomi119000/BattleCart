using Unity.VisualScripting;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    const float Lanewidth = 6.0f; //レーン幅
    public GameObject[] dangerPrefab; //生成される危険車のプレハブ

    public float minIntervalTime = 0.1f; //インターバル最小
    public float maxIntervalTime = 1.0f;

    float timer; //経過時間観測
    float posX; //危険車の出現X座標

    GameObject cam; //カメラオブジェクト

    //初期位置設定
    public Vector3 defaultPos = new Vector3(0, 10, 120);
    Vector3 diff;
    public float followSpeed = 8; //ジェネレーターの補完スピード

    int isSky;
    void Start()
    {
        transform.position = defaultPos; //ジェネレーターの初期値
        cam = Camera.main.gameObject; //カメラのゲームオブジェクト情報
        //初期時点でのカメラとジェネレータの位置の差
        diff = transform.position - cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;

        timer -= Time.deltaTime; //カウントダウン

        if (timer <= 0) //タイマーがゼロになったら
        {
            DangerCreated(); //危険車の生成
            maxIntervalTime -= 0.1f;
            //Mathf.ClampでmaxIntervalTimeの範囲を0.1f～3.0f
            maxIntervalTime = Mathf.Clamp(maxIntervalTime, 0.1f, 3.0f);
            timer = Random.Range(minIntervalTime, maxIntervalTime + 1);

        }
    }
    //ジェネレーターがずっと追従してくるようにする
    private void FixedUpdate()
    {
        //線形補完を使って、カメラを目的の場所に動かす
        //Lerpメソッドで（今の位置、ゴール位置、割合）
        transform.position = Vector3.Lerp(transform.position, cam.transform.position, followSpeed * Time.deltaTime);
    }

    //危険車の生成メソッド
    void DangerCreated()
    {
        isSky = Random.Range(0, 2); //空中かどうかをランダム　0 or 1 
        int rand = Random.Range(-2, 3); //レーン番号をランダムに取得（-2,-1,0,1,2)
        posX = rand * Lanewidth; //レーン番号とレーン幅で座標を決める

        Vector3 v = new Vector3(posX, transform.position.y, transform.position.z);
        //もしisSkyが0なら空中座標(=y）が１
        if (isSky == 0) v.y = 1;

        //dangerPrefab(GameObject)をvの位置に、Player向きで生成する）
        Instantiate(
            dangerPrefab[isSky],
            v,
            dangerPrefab[isSky].transform.rotation //rotation y = 180（Player向き）
            );
    }
}

