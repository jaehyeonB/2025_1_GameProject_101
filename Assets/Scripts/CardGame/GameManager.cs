using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //������ ���ҽ�
    public GameObject cardPrefabs;                  //ī�� ������
    public Sprite[] cardImages;                     //ī�� �̹��� �迭
    //���� Transform
    public Transform deckArea;                      //�� ����
    public Transform handArea;                      //���� ����
    //UI ���
    public Button drawButton;                       //��ο� ��ư
    public TextMeshProUGUI deckCountText;           //���� �� ī�� �� ǥ�� �ؽ�Ʈ
    //���� ��
    public float cardSpacing = 2.0f;                //ī�� ����
    public int maxHandSize = 6;                     //�ִ� ���� ũ��

    //�迭 ����
    public GameObject[] deckCards;                  //�� ī�� �迭
    public int deckCount;                           //���� ���� �ִ� ī�� ��

    public GameObject[] handCards;                  //���� �迭
    public int handCount;                           //���� ���п� �ִ� ī�� ��

    //�̸� ���ǵ� �� ī�� ��� (���ڸ�)
    public int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,            //1�� 8��
        2,2,2,2,2,2,                //2�� 6��
        3,3,3,3,                    //3�� 4��
        4,4                         //4�� 2��
    };
    void Start()
    {
        //�迭 �ʱ�ȭ
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        //�� �ʱ�ȭ �� ����
        IntializeDeck();
        ShuffleDeck();

        if(drawButton != null)                                                      //��ư UI üũ
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);                    //���� ��� ��ư�� ������ OnDrawButtonClicked �Լ� ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShuffleDeck()                  //Fisher-Yates ���� �˰���**
    {
        for ( int i = 0; i < deckCount -1; i++)
        {
            int j = Random.Range(i, deckCount);
            //�迭 �� ī�� ��ȯ
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
            int value = prefedinedDeck[i];              //ī�� �� ��������
            //�̹��� �ε��� ��� (���� ���� �ٸ� �̹��� ���)
            int imageIndex = value - 1;                 //���� 1���� �����ϹǷ� �ε����� 0����
            if(imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;                         //�̹����� �����ϰų� �ε����� �߸��Ǿ��� ��� ù��° �̹��� ���
            }
            //ī�� ������Ʈ ���� (�� ��ġ)
            GameObject newCardObj = Instantiate(cardPrefabs, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(true);                 //ó������ ��Ȱ��ȭ 
            //ī�� ������Ʈ �ʱ�ȭ
            Card cardComp = newCardObj.GetComponent<Card>();
            if(cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;                  //�迭�� ����
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
            Debug.Log("���� �� á���ϴ�!");
            return;
        }
        if (deckCount <= 0)
        {
            Debug.Log("���� �� �̻� ī�尡 �����ϴ�!");
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
