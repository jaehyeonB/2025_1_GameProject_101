using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
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

    public Transform mergeArea;                     //머지 영역 추가
    public Button mergeButton;                      //머지 버튼 추가
    public int maxMergeSize = 3;                    //최대 머지 수

    public GameObject[] mergeCards;                 //머지 영역 배열
    public int mergeCount;                          //현재 머지 영역에 있는 카드 수

    void Start()
    {
        //배열 초기화
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        //덱 초기화 및 셔플
        IntializeDeck();
        ShuffleDeck();

        if (drawButton != null)                                                      //버튼 UI 체크
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);                    //있을 경우 버튼을 누르면 OnDrawButtonClicked 함수 동작
        }

        if (mergeButton != null)                                                      //버튼 UI 체크
        {
            mergeButton.onClick.AddListener(OnMergeButtonClicked);                    //있을 경우 버튼을 누르면 OnDrawButtonClicked 함수 동작
            mergeButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ShuffleDeck()                  //Fisher-Yates 셔플 알고리즘**
    {
        for (int i = 0; i < deckCount - 1; i++)
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

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];              //카드 값 가져오기
            //이미지 인덱스 계산 (값에 따라 다른 이미지 사용)
            int imageIndex = value - 1;                 //값이 1부터 시작하므로 인덱스는 0부터
            if (imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;                         //이미지가 부족하거나 인덱스가 잘못되었을 경우 첫번째 이미지 사용
            }
            //카드 오브젝트 생성 (덱 위치)
            GameObject newCardObj = Instantiate(cardPrefabs, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(true);                 //처음에는 비활성화 
            //카드 컴포넌트 초기화
            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null)
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

        for (int i = 0; i < handCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos;
            }
        }
    }

    //머지 영역에서 카드 정렬 함수
    public void ArrangeMerge()
    {
        if (mergeCount == 0)                        //머지 영역에 카드가 없으면 정렬이 필요 없기 떄문에 return
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;                  //카드 중앙 정렬을 위한 오프셋 계산

        for (int i = 0; i < mergeCount; i++)                                 //각 카드 위치 조정
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos;
            }
        }
    }
    void OnDrawButtonClicked()
    {
        DrawCardtoHand();
    }
    public void DrawCardtoHand()
    {
        if (handCount + mergeCount >= maxHandSize)
        {
            Debug.Log("카드 수가 최대입니다");
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

    public void MoveCardToMerge(GameObject Card)            //카드를 머지 영역으로 이동 [카드를 인수로 받는다]
    {
        if (mergeCount >= maxMergeSize)           //머지 영역이 가득 찼는지 확인
        {
            Debug.Log("머지 영역이 가득 찼습니다!");
            return;
        }

        for (int i = 0; i < handCount; i++)             //카드가 손패에 있는지 확인하고 제거
        {
            if (handCards[i] == Card)
            {
                for(int j = i; j < handCount - 1; j++)                  //카드를 제거하고 배열 정리
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;
                handCount--;

                ArrangeHand();                          //손패 정렬
                break;                                  //for 문을 빠져나온다.
            }
        }

        mergeCards[mergeCount] = Card;
        mergeCount++;

        Card.transform.SetParent(mergeArea);            //머지 영역을 부모로 둔다. (영역 하위로 오브젝트 이동)
        ArrangeMerge();                                 //머지 영역 정렬
        UpdateMergeButtonState();                       //머지 버튼 상태 업데이트
    }

    //머지 버튼 상태 업데이트
    void UpdateMergeButtonState()
    {
        if (mergeButton != null)                        //머지 버튼이 있을 경우
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);            //머지 영역에 카드가 2개 또는 3개만 있을때만 활성화
        }
    }

    //머지 영역의 카드들을 합쳐서 새 카드 생성
    void MergeCards()
    {
        if (mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("머지를 하려면 카드가 2개 또는 3개 필요합니다!");
            return;
        }

        int firstCardValue = mergeCards[0].GetComponent<Card>().cardValue;             //첫번째 카드에 있는 값을 가져온다.
        for (int i = 0; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();
            if (card == null || card.cardValue != firstCardValue)
            {
                Debug.Log("같은 숫자의 카드만 머지 할 수 있습니다!");
                return;
            }
        }

        int newValue = firstCardValue + 1;

        if (newValue > cardImages.Length)
        {
            Debug.Log("최대 카드 값에 도달하였습니다. ");
            return;
        }

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                mergeCards[i].SetActive(false);
            }
        }



        //새카드 생성
        GameObject newCard = Instantiate(cardPrefabs, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();                                //생성된 새로운 카드의 컴포넌트를 참조 하기 위해 로컬로 선언
        if (newCardTemp != null)                                                         //생성된 새로운 카드의 컴포넌트가 존재하면 (문제가 없으면)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        //머지 영역 비우기
        for (int i = 0; i < maxMergeSize; i++)                   //머지 배열을 순환하면서 null값을 만든다.
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;                                         //머지 카운트를 0으로 초기화 시킨다.

        UpdateMergeButtonState();                               //머지 버튼 상태 업데이트

        handCards[handCount] = newCard;                         //새로 만들어진 카드를 손패로 이동
        handCount++;                                            //핸드 카운트를 증가시킨다.
        newCard.transform.SetParent(handArea);                  //새로 만들어진 카드의 위치를 핸드 영역에 자식으로 놓는다

        ArrangeHand();                                          //손패 정렬
    }

    //머지 버튼 클릭 시 머지 영역의 카드 합성
    void OnMergeButtonClicked()
    {
        MergeCards();
    }



}
