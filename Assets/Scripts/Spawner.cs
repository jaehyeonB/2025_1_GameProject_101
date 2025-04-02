using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs;      //동전 프리팹
    public GameObject MissilePrefabs;

    [Header("스폰 타이밍 설정")]
    public float minSpawnInterval = 0.5f;       //최소 생성 간격 (초)
    public float maxSpawnInterval = 2.0f;       //최대 생성 간격 (초)

    [Header("동전 스폰 확률 설정")]
    [Range(0, 100)]                             //유니티 UI에서 할 수 있게 된다
    public int coinSpawnChance = 50;            //동전이 생성될 확률 ( 0 ~ 100 )

    public float timer = 0.0f;
    public float nextSpawnTime;     //다음 생성 시간

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= nextSpawnTime)
        {
            SpawnObject();      //함수를 호출 해준다
            timer = 0.0f;       //시간을 어쩌구
            SetNextSpawnTime();
        }    
    }
    void SpawnObject()
    {
        Transform spawnTransform = transform;       //스포너 오브젝트의 위치와 회전 값을 가져온다

        //확률에 따라 동전 또는 미사일 생성
        int randomValue = Random.Range(0, 100);
        if(randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(MissilePrefabs, spawnTransform.position, spawnTransform.rotation);
        }

            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);     //코인 프리팹을 해당 위치에 생성 한다.
    }
    void SetNextSpawnTime()
    {
        //최소-최대 사이의 랜덤한 시간 설정
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
