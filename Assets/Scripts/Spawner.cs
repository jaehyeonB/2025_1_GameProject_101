using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs;      //���� ������
    public GameObject MissilePrefabs;

    [Header("���� Ÿ�̹� ����")]
    public float minSpawnInterval = 0.5f;       //�ּ� ���� ���� (��)
    public float maxSpawnInterval = 2.0f;       //�ִ� ���� ���� (��)

    [Header("���� ���� Ȯ�� ����")]
    [Range(0, 100)]                             //����Ƽ UI���� �� �� �ְ� �ȴ�
    public int coinSpawnChance = 50;            //������ ������ Ȯ�� ( 0 ~ 100 )

    public float timer = 0.0f;
    public float nextSpawnTime;     //���� ���� �ð�

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= nextSpawnTime)
        {
            SpawnObject();      //�Լ��� ȣ�� ���ش�
            timer = 0.0f;       //�ð��� ��¼��
            SetNextSpawnTime();
        }    
    }
    void SpawnObject()
    {
        Transform spawnTransform = transform;       //������ ������Ʈ�� ��ġ�� ȸ�� ���� �����´�

        //Ȯ���� ���� ���� �Ǵ� �̻��� ����
        int randomValue = Random.Range(0, 100);
        if(randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(MissilePrefabs, spawnTransform.position, spawnTransform.rotation);
        }

            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);     //���� �������� �ش� ��ġ�� ���� �Ѵ�.
    }
    void SetNextSpawnTime()
    {
        //�ּ�-�ִ� ������ ������ �ð� ����
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
