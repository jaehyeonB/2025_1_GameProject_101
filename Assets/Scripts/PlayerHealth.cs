using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;                    //최대 생명력
    public int currentLives;                    //현재 생명력

    public float invincibleTime = 1.0f;         //피격 후 무적 시간(반복 피격 방지)
    public bool isInvincible = false;           //무적 여부의 값
    void Start()
    {
        currentLives = maxLives;                //생명력 초기화
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))        //코인 트리거와 충돌 하면
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
        //현재 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
