using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //프리팹 리소스
    public GameObject cardPrefabs;                  //카드 프리팹
    public Sprite[] cardImages;                     //카드 이미지 배열
    //영역 Transform
    public Transform deckArea;                      //덱 영역
    public Transform handArea;                      //손패 영역
    //UI 요소
    public Button drawButton;                       //드로우 버튼
    public TextMeshProUGUI deckCountText;           //남은 덱 카드 수 표시 텍스트
    //설정 값
    public float cardSpacing = 2.0f;                //카드 간격
    public int maxHandSize = 6;                     //최대 손패 크기

    //배열 선언
    public GameObject[] deckCards;                  //덱 카드 배열
    public int deckCount;                           //현재 덱에 있는 카드 수

    public GameObject[] handCards;                  //손패 배열
    public int handCount;                           //현재 손패에 있는 카드 수

    //미리 정의된 덱 카드 목록 (숫자만)
    public int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,            //1이 8장
        2,2,2,2,2,2,                //2가 6장
        3,3,3,3,                    //3이 4장
        4,4                         //4가 2장
    };
    void Start()
    {
        //배열 초기화
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        //덱 초기화 및 셔플
        IntializeDeck();
        ShuffleDeck();

        if(drawButton != null)                                                      //버튼 UI 체크
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);                    //있을 경우 버튼을 누르면 OnDrawButtonClicked 함수 동작
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShuffleDeck()                  //Fisher-Yates 셔플 알고리즘**
    {
        for ( int i = 0; i < deckCount -1; i++)
        {
            int j = Random.Range(i, deckCount);
            //배열 내 카드 교환
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }
    void IntializeDeck()
    {
        deckCount = prefedinedDeck.Length;

        for(int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];              //카드 값 가져오기
            //이미지 인덱스 계산 (값에 따라 다른 이미지 사용)
            int imageIndex = value - 1;                 //값이 1부터 시작하므로 인덱스는 0부터
            if(imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;                         //이미지가 부족하거나 인덱스가 잘못되었을 경우 첫번째 이미지 사용
            }
            //카드 오브젝트 생성 (덱 위치)
            GameObject newCardObj = Instantiate(cardPrefabs, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(true);                 //처음에는 비활성화 
            //카드 컴포넌트 초기화
            Card cardComp = newCardObj.GetComponent<Card>();
            if(cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;                  //배열에 저장
        }
    }
    public void ArrangeHand()
    {
        if (handCount == 0)
            return;

        float startX = -(handCount - 1) * cardSpacing / 2;

        for(int i = 0; i < handCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos;
            }
        }
    }
    void OnDrawButtonClicked()
    {
        DrawCardtoHand();
    }
    public void DrawCardtoHand()
    {
        if (handCount >= maxHandSize)
        {
            Debug.Log("손이 꽉 찼습니다!");
            return;
        }
        if (deckCount <= 0)
        {
            Debug.Log("덱에 더 이상 카드가 없습니다!");
            return;
        }
        GameObject drawnCard = deckCards[0];

        for (int i = 0; i < deckCount - 1; i++)
        {
            deckCards[i] = deckCards[i + 1];
        }
        deckCount--;

        drawnCard.SetActive(true);
        handCards[handCount] = drawnCard;
        handCount++;

        drawnCard.transform.SetParent(handArea);

        ArrangeHand();
    }
    
}
