using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float deleteTime = 5.0f;
    public GameObject boms; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //deleteTime秒後に消える
        Destroy(gameObject, deleteTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //相手がEnemyタグなら相手を削除
        //相手がEnemyタグならbomsを生成
        if(other.gameObject.CompareTag("Enemy"))
        {
            //other.gameObjectで相手（other=Enemy）のゲームオブジェクトを削除
            Destroy(other.gameObject);
            //相手にあたった位置でbomsを生成する
            Instantiate(boms,other.transform.position, Quaternion.identity);
        }
      
        //いずれにしても自分(this.gameObject)は削除
        Destroy(gameObject);
    }
}
