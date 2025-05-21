using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    public int fruitType;                   //���� Ÿ�� (0 : ��� , 1: ��纣�� , 2 : ���ڳ� ) int �� index ����

    public bool hasMerged = false;          //������ �������°�

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged)                      //�̹� ������ ������ ����
            return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();          //�ٸ� ���ϰ� �浹�ߴ��� Ȯ��

        if(otherFruit != null && !otherFruit.hasMerged && otherFruit.fruitType == fruitType)            //�浹�� ���� �����̰� Ÿ���� ���ٸ� (�������� �ʾ��� ���)
        {
            hasMerged = true;                   //���ƴٰ� ǥ��
            otherFruit.hasMerged = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f;          //�� ������ �߰��� ���

            //���� �Ŵ������� Merge �����Ȱ��� ȣ��
            FruitGame gameManager = FindObjectOfType<FruitGame>();
            if(gameManager != null)
            {
                gameManager.MergeFruit(fruitType, mergePosition);
            }


            //������ ����
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }
}
