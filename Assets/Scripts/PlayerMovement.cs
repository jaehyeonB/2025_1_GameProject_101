using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;      //이동 속도 변수 설정
    public float jumpForce = 5.0f;      //점프의 힘 값을 준다.

    public bool isGrounded = true;

    public int coinCount = 0;
    public int totalCoins = 5;

    public Rigidbody rb;

    void Start()
    {
        
    }
    void Update()
    {
        //윰직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //속도로 직접 이동
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        if(Input.GetButtonDown("Jump") && isGrounded)       //&&두 값을 만족할 때 -> (스페이스 버튼을 눌렸을 때 외 isGrounded 가 True 일 때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     //위쪽으로 설정한 힘만큼 강체에 준다.
            isGrounded = false;                                         //점프를 하는 순간 땅에서 떨어졌기 때문에 false라고 해준다.
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //코인 수집
        if (other.CompareTag("Coin"))        //코인 트리거와 충돌 하면
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
        }

        //목적지 도착 시 종료 로그 출력
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 기능
        }
    }

}
