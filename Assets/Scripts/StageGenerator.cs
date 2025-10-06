using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    //生成したチップを配置するにあたってのチップの大きさ
    const int StageChipSize = 120;
    int currentChipIndex; //現在どのチップまで作ったか
    Transform player;  //プレイヤーのTransform情報
    public GameObject[] stageChips; //生成すべきObjectを配列に格納
    public int startChipIndex; //チップ番号の開始
    public int preInstantiate; //余分に作っておく数

    //現在生成したオブジェクトの管理用
    public List<GameObject> generatedStageList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Transformを獲得
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentChipIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //キャラクター（player）の位置から現在のステージチップのIndexを計算
            int charaPositionIndex = (int)(player.position.z / StageChipSize);

            //次のステージに入ったらステージの更新処理を行う
            if (charaPositionIndex + preInstantiate > currentChipIndex)
            {
                UpdateStage(charaPositionIndex + preInstantiate);
            }
        }
    }

    void UpdateStage(int toChipIndex)
    {
        if(toChipIndex <= currentChipIndex) return; 
            
        for(int i = currentChipIndex+1; i<=toChipIndex; i++)
        {
            GameObject stageObject = GeneratesStage(i);
            generatedStageList.Add(stageObject); 
        }

        while(generatedStageList.Count > preInstantiate +2) DestroyOldestStage();
        currentChipIndex = toChipIndex; 
    }

    GameObject GeneratesStage(int toChipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, toChipIndex * StageChipSize),
            Quaternion.identity
            );

        return stageObject; 
    }
        
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
