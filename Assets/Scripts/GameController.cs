using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float GameTimer = 3.0f;
    public GameObject MonsterGO;
    void Update()
    {
        GameTimer -= Time.deltaTime;

        if (GameTimer <= 0)
            {
                GameTimer = 3.0f;

            GameObject Temp = Instantiate(MonsterGO);
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f);
            //X -10 ~ 10 Y -4 ~ 4 �� ������ �������� ��ġ ��Ų��.
            }
        if (Input.GetMouseButtonDown(0))        //���콺 ��ư�� ������
        {
            RaycastHit hit;     //Ray ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //ī�޶󿡼� ���̸� ���� �����Ѵ�.
            //3D ���ӿ��� ������Ʈ�� ���� �� �� ����Ѵ�. (ȭ�鿡 ���̴� ��ü�� �����ϱ� ���ؼ� ���)

            if (Physics.Raycast(ray, out hit))      //Hit �� ������Ʈ�� �����Ѵ�.
            {
                if (hit.collider != null)       //null
                {
                    hit.collider.gameObject.GetComponent<Monster>().CharactorHit(50);
                }
            }
        }
    }
}
