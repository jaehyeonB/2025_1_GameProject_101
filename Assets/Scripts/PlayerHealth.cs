using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;                    //�ִ� �����
    public int currentLives;                    //���� �����

    public float invincibleTime = 1.0f;         //�ǰ� �� ���� �ð�(�ݺ� �ǰ� ����)
    public bool isInvincible = false;           //���� ������ ��
    void Start()
    {
        currentLives = maxLives;                //����� �ʱ�ȭ
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))        //���� Ʈ���ſ� �浹 �ϸ�
        {
            currentLives--;
            Destroy(other.gameObject);
            
            if(currentLives<=0)
            {
                GameOver();
            }
        }
    } 
    void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestartGame", 3.0f);

    }
    void RestartGame()
    {
        //���� �� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
