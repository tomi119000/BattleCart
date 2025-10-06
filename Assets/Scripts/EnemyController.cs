using UnityEngine;

public class EnemyController : MonoBehaviour
{
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;
    public float gravity = 9.81f; //Gravity
    public float speedZ = -10; //前進方向のスピード上限値
    public float accelerationZ = -8; //加速度

    public float deletePosY = -10f; //削除される基準のy座標
    public bool useGravity; //重力摘要するか否か

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
        // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= deletePosY)
        {
            Destroy(gameObject);
            return; 
        }
        //徐々に加速しZ方向に常に前進させる
        float acceleratedZ = moveDirection.z * (accelerationZ *Time.deltaTime);
        moveDirection.z = Mathf.Clamp(acceleratedZ, speedZ, 0); //acceleratedZの速さ, min:speedZ, max:0

        if(useGravity)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0;
        }

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        if (controller.isGrounded) moveDirection.y = 0;
    }
}
