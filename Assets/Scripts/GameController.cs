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
            //X -10 ~ 10 Y -4 ~ 4 의 범위의 랜덤으로 위치 시킨다.
            }
        if (Input.GetMouseButtonDown(0))        //마우스 버튼을 누르면
        {
            RaycastHit hit;     //Ray 선언
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //카메라에서 레이를 쏴서 검출한다.
            //3D 게임에서 오브젝트를 검출 할 때 사용한다. (화면에 보이는 물체를 선택하기 위해서 사용)

            if (Physics.Raycast(ray, out hit))      //Hit 된 오브젝트를 검출한다.
            {
                if (hit.collider != null)       //null
                {
                    hit.collider.gameObject.GetComponent<Monster>().CharactorHit(50);
                }
            }
        }
    }
}
