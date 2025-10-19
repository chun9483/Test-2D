using UnityEngine;
using Terresquall;

public class PlayerMovement : MonoBehaviour{
    public float speed = 2;
    public Rigidbody2D rb; //鋼體，可以用來做角色移動

    bool isMovingUp;
    bool isMovingLeft;
    bool isMovingDown;
    bool isMovingRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        
    }

    // FixedUpdate is called 50 times/s,frame time=0.02(幀數)(預設的)
    void FixedUpdate(){
        //讀輸入，方向控制
        isMovingUp = Input.GetKey(KeyCode.W) |
                    (VirtualJoystick.GetAxis("Vertical")>0.3);
        isMovingLeft = Input.GetKey(KeyCode.A) |
                    (VirtualJoystick.GetAxis("Horizontal")<-0.3);
        isMovingDown = Input.GetKey(KeyCode.S) |
                    (VirtualJoystick.GetAxis("Vertical")<-0.3);
        isMovingRight = Input.GetKey(KeyCode.D) |
                    (VirtualJoystick.GetAxis("Horizontal")>0.3);

        //移動
        if (isMovingUp){
            Debug.Log("Move up");
            transform.Translate(0, speed, 0);
        }
        if (isMovingLeft){
            Debug.Log("Move left");
            transform.Translate(-speed, 0, 0);
        }
        if (isMovingDown){
            Debug.Log("Move down");
            transform.Translate(0, -speed, 0);
        }
        if (isMovingRight){
            Debug.Log("Move right");
            transform.Translate(speed, 0, 0);
        }
    }
}
