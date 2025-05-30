using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{
    public GameObject[] fruitPrefabs;                   //과일 프리팹 배열 선언

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };                 //과일 크기 선언 

    public GameObject currentFruit;                     //현재 들고 있는 과일
    public int currentFruitType;                        

    public float fruitStartHeight = 6.0f;               //과일 시작시 높이 설정
    public float gameWidth = 5.0f;                      //게임판 너비
    public bool isGameOver = false;                     //게임 상태
    public Camera mainCamera;                           //카메라 참조

    public float fruitTimer;                            //잰 시간 설정을 위한 타이머

    public float gameHeight = 4;


    void Start()
    {
        mainCamera = Camera.main;                       //메인 카메라 참조 가져 오기
        SpawnNewFruit();                                //게임 시작 시 첫 과일 생성
        fruitTimer = -3.0f;                             //
        gameHeight = fruitStartHeight + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if(fruitTimer >= 0)
        {
            fruitTimer -= Time.deltaTime;
        }

        if(fruitTimer < 0 && fruitTimer > -2)
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;
        }

        if(currentFruit != null)                        // 현재 생성된 과일이 있을 떄만 처리
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentFruit.transform.position;
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitType] / 2;
            if(newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition;
        }

        if(Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
        }

    }

    void SpawnNewFruit()
    {
        if(!isGameOver)                                 //게임오버가 아닐 때 새로운 과일 생성
        {
            currentFruitType = Random.Range(0, 3);     //0 ~ 2 사이의 랜덤 과일 타입

            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeight, 0);

            float halfFruitSize = fruitSizes[currentFruitType] / 2;

            //X의 위치가 게임의 영역을 벗어나지 않도록 제한
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitType], spawnPosition, Quaternion.identity);                             //과일 생성
            currentFruit.transform.localScale = new Vector3(fruitSizes[currentFruitType], fruitSizes[currentFruitType], 1);             //과일 크기 설정

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();                  //강체를 받아와서
            if(rb != null)
            {
                rb.gravityScale = 0f;                                                   //중력을 0으로 만들어 준다.
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.gravityScale = 1f;

            currentFruit = null;

            fruitTimer = 1.0f; 
        }
    }

    public void MergeFruit(int fruitType, Vector3 position)
    {
        if(fruitType < fruitPrefabs.Length - 1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1], position, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSizes[fruitType + 1], fruitSizes[fruitType + 1], 1.0f);

            //점수 추가 로직 등등
        }
    }

    public void CheckGameOver()
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();

        float gameOverHeight = gameHeight;

        for(int i = 0; i < allFruits.Length; i++)
        {
            if (allFruits[i] != null)
            {
                Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

                if(rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
                {
                    isGameOver = true;
                    Debug.LogError("게임오버");

                    break;
                }
            }
        }
    }
}
