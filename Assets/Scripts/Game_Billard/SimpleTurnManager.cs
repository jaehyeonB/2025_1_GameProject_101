using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    //���� ���� (��� ���� ���� �ؼ�  ��� �� �� ����
    public static bool canPlay = true;
    public static bool anyBallMoving = false;
  
    void Update()
    {
        CheckAllBalls();            //��� ���� ������ Ȯ�� �Լ� ȣ��

        if(!anyBallMoving && !canPlay)
        {
            canPlay = true;
            Debug.Log("�� ����! �ٽ� ĥ �� �ֽ��ϴ�.");
        }
    }

    void CheckAllBalls()            //��� ���� ������� Ȯ��
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoving = false;

        foreach (SimpleBallController ball in allBalls)          //�迭 ��ü Ŭ������ ��ȯ �ϸ鼭
        {
            if(ball.isMoving())         //���� �����̰� �ִ��� Ȯ�� �ϴ� �Լ��� ȣ��
            { 
                anyBallMoving = true;   //���� �����δٰ� �Լ� ����
                break;                  //�������� �������´�
            }
        }
    }

    public static void OnBallHit()
    {
        canPlay = false;                //�ٸ� ������ �������̰� ��
        anyBallMoving = true;           //���� �����̱� �����ϱ� ������ Bool �� ����
        Debug.Log("�� ����! ���� ���� �� ���� ��ٸ�����.");
    }
}
