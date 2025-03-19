using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100; //ü���� ���� �Ѵ�. (����)
    public float Timer = 1.0f; //Ÿ�̸Ӹ� ���� �Ѵ�. 
    public int AttackPoint = 10; //���ݷ��� ���� �Ѵ�.

    //ù ������ ������ �ѹ� ����ȴ�
    void Start()
    {
        Health = 100; //ù ������ ������ ����ɶ� 100ü���� �߰� ���� �ش�.
        
    }

    
    void Update()
    {

        CharactorHealthUp();
        CheckDeath();   //�Լ� ȣ��
        
    }

    void CharactorHealthUp()
    {
        Timer -= Time.deltaTime; //�ð��� �� �����Ӹ��� ���� ��Ų��. (deltaTime �����Ӱ��� �ð� ������ �ǹ��Ѵ�.)

        if (Timer <= 0)
        {
            Timer = 1.0f; //�ٽ� 1�ʷ� ���� �����ش�.
            Health += 20; //1�ʸ��� ü�� 20�� �÷��ش�.   (Health = Health + 20)
        }
    }
    
    public void CharactorHit(int Damage)   //Ŀ���� �������� �޴� �Լ��� ����Ѵ�.
    {
        Health -= Damage;   //���� ���ݷ¿� ���� ü���� ���� ��Ų��.
    }

    void CheckDeath()   //�Լ� ����
    {
        if (Health <= 0) //ü���� 0 ���Ϸ� �������� 
            Destroy(gameObject); //������Ʈ�� �ı��Ѵ�
    }
}
