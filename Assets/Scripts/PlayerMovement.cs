using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;      //�̵� �ӵ� ���� ����
    public float jumpForce = 5.0f;      //������ �� ���� �ش�.

    public bool isGrounded = true;

    public int coinCount = 0;
    public int totalCoins = 5;

    public Rigidbody rb;

    void Start()
    {
        
    }
    void Update()
    {
        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //�ӵ��� ���� �̵�
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        if(Input.GetButtonDown("Jump") && isGrounded)       //&&�� ���� ������ �� -> (�����̽� ��ư�� ������ �� �� isGrounded �� True �� ��)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     //�������� ������ ����ŭ ��ü�� �ش�.
            isGrounded = false;                                         //������ �ϴ� ���� ������ �������� ������ false��� ���ش�.
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
        //���� ����
        if (other.CompareTag("Coin"))        //���� Ʈ���ſ� �浹 �ϸ�
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }

        //������ ���� �� ���� �α� ���
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ���
        }
    }

}
