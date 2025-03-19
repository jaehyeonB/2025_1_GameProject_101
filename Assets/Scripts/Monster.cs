using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100; //체력을 선언 한다. (정수)
    public float Timer = 1.0f; //타이머를 선언 한다. 
    public int AttackPoint = 10; //공격력을 선언 한다.

    //첫 프레임 이전에 한번 실행된다
    void Start()
    {
        Health = 100; //첫 프레임 이전에 실행될때 100체력을 추가 시켜 준다.
        
    }

    
    void Update()
    {

        CharactorHealthUp();
        CheckDeath();   //함수 호출
        
    }

    void CharactorHealthUp()
    {
        Timer -= Time.deltaTime; //시간을 매 프레임마다 감소 시킨다. (deltaTime 프레임간의 시간 간격을 의미한다.)

        if (Timer <= 0)
        {
            Timer = 1.0f; //다시 1초로 변경 시켜준다.
            Health += 20; //1초마다 체력 20을 올려준다.   (Health = Health + 20)
        }
    }
    
    public void CharactorHit(int Damage)   //커스텀 데미지를 받는 함수를 사용한다.
    {
        Health -= Damage;   //받은 공격력에 대한 체력을 감소 시킨다.
    }

    void CheckDeath()   //함수 선언
    {
        if (Health <= 0) //체력이 0 이하로 내려가면 
            Destroy(gameObject); //오브젝트를 파괴한다
    }
}
