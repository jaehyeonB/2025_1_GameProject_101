using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    //전역 변수 (모든 공이 공유 해서  사용 할 수 있음
    public static bool canPlay = true;
    public static bool anyBallMoving = false;
  
    void Update()
    {
        CheckAllBalls();            //모든 공의 움직임 확인 함수 호출

        if(!anyBallMoving && !canPlay)
        {
            canPlay = true;
            Debug.Log("턴 종료! 다시 칠 수 있습니다.");
        }
    }

    void CheckAllBalls()            //모든 공이 멈췄는지 확인
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoving = false;

        foreach (SimpleBallController ball in allBalls)          //배열 전체 클래스를 순환 하면서
        {
            if(ball.isMoving())         //공이 움직이고 있는지 확인 하는 함수를 호출
            { 
                anyBallMoving = true;   //공이 움직인다고 함수 변경
                break;                  //루프문을 빠져나온다
            }
        }
    }

    public static void OnBallHit()
    {
        canPlay = false;                //다른 공들을 못움직이게 함
        anyBallMoving = true;           //공이 움직이기 시작하기 때문에 Bool 값 변경
        Debug.Log("턴 시작! 공이 멈출 때 까지 기다리세요.");
    }
}
