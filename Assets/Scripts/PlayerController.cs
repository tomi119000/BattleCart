﻿using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 6.0f;
    const float StunDuration = 0.5f;
    float recoverTime = 0.0f;

    public int life = 10;
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;
    int targetLane;

    public float gravity = 9.81f;
    public float speedZ = 10; //前進方向のスピードの上限値
    public float accelerationZ = 8; //加速度
    public float speedX = 10; //横方向のスピードの上限値
    public float speedJump = 10; //ジャンプスピード

    public GameObject body;
    public GameObject boms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.playing)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) MoveToLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) MoveToRight();
            if (Input.GetKeyDown(KeyCode.Space)) Jump();
        }
        if(IsStun())
        {
            moveDirection.x = 0;
            moveDirection.y = 0;
            //復活までの時間をカウント
            recoverTime -= Time.deltaTime;

            //点滅処理
            Blinking(); 
        }
        else
        {
            //徐々に加速しZ方向に常に前進させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(accelerationZ, 0, speedZ);

            //X方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX; 
        }

        //重力分の力をフレームに追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実効
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらY方向の速度はリセットする
        if (controller.isGrounded) moveDirection.y = 0;   

    }

    public void MoveToLeft()
    {
        if(IsStun()) return; 
        if(controller.isGrounded && targetLane > MinLane) targetLane--;
    }
    public void MoveToRight()
    {
        if (IsStun()) return;
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }
   
    public void Jump()
    {
        if (IsStun()) return;
        if (controller.isGrounded) moveDirection.y = speedJump;
    }
    
    public int Life()
    {
        return life; 
    }

    //スタン中かチェック
    bool IsStun()
    {
        //recoverTimeが作動中かLifeが0になった場合はStunフラグがON
        bool stun = recoverTime > 0.0f || life <= 0;
        //StunフラグがＯＦＦの場合はボディを確実に表示
        if (!stun) body.SetActive(true);
        //Stunフラグをリターン
        return stun; 
    }

    //接触判定
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return; 
        //ぶつかった相手がＥｎｅｍｙなら
        if(hit.gameObject.CompareTag("Enemy"))
        {
            //体力をマイナス
            life--;

            if(life <=0)
            {
                GameManager.gameState = GameState.gameover;
                //爆発エフェクトの発生
                Instantiate(boms, transform.position, Quaternion.identity);
                Destroy(gameObject, 0.5f); //少し時間差で消滅
            }
            //recoverTimeの時間を設定
            recoverTime = StunDuration;
            //接触したＥｎｅｍｙを削除
            Destroy(hit.gameObject);
        }
    }
    //点滅処理
    void Blinking()
    {
        //その時のゲーム進行で正か負かの値を算出
        float val = Mathf.Sin(Time.deltaTime*50);
        //正の周期なら表示、負の周期なら非表示
        if (val > 0) body.SetActive(true);
        else body.SetActive(false); 
    }
}
