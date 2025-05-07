using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
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

    public Transform mergeArea;                     //���� ���� �߰�
    public Button mergeButton;                      //���� ��ư �߰�
    public int maxMergeSize = 3;                    //�ִ� ���� ��

    public GameObject[] mergeCards;                 //���� ���� �迭
    public int mergeCount;                          //���� ���� ������ �ִ� ī�� ��

    void Start()
    {
        //�迭 �ʱ�ȭ
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        //�� �ʱ�ȭ �� ����
        IntializeDeck();
        ShuffleDeck();

        if (drawButton != null)                                                      //��ư UI üũ
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);                    //���� ��� ��ư�� ������ OnDrawButtonClicked �Լ� ����
        }

        if (mergeButton != null)                                                      //��ư UI üũ
        {
            mergeButton.onClick.AddListener(OnMergeButtonClicked);                    //���� ��� ��ư�� ������ OnDrawButtonClicked �Լ� ����
            mergeButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ShuffleDeck()                  //Fisher-Yates ���� �˰���**
    {
        for (int i = 0; i < deckCount - 1; i++)
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

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];              //ī�� �� ��������
            //�̹��� �ε��� ��� (���� ���� �ٸ� �̹��� ���)
            int imageIndex = value - 1;                 //���� 1���� �����ϹǷ� �ε����� 0����
            if (imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;                         //�̹����� �����ϰų� �ε����� �߸��Ǿ��� ��� ù��° �̹��� ���
            }
            //ī�� ������Ʈ ���� (�� ��ġ)
            GameObject newCardObj = Instantiate(cardPrefabs, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(true);                 //ó������ ��Ȱ��ȭ 
            //ī�� ������Ʈ �ʱ�ȭ
            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null)
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

        for (int i = 0; i < handCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos;
            }
        }
    }

    //���� �������� ī�� ���� �Լ�
    public void ArrangeMerge()
    {
        if (mergeCount == 0)                        //���� ������ ī�尡 ������ ������ �ʿ� ���� ������ return
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;                  //ī�� �߾� ������ ���� ������ ���

        for (int i = 0; i < mergeCount; i++)                                 //�� ī�� ��ġ ����
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
            Debug.Log("ī�� ���� �ִ��Դϴ�");
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

    public void MoveCardToMerge(GameObject Card)            //ī�带 ���� �������� �̵� [ī�带 �μ��� �޴´�]
    {
        if (mergeCount >= maxMergeSize)           //���� ������ ���� á���� Ȯ��
        {
            Debug.Log("���� ������ ���� á���ϴ�!");
            return;
        }

        for (int i = 0; i < handCount; i++)             //ī�尡 ���п� �ִ��� Ȯ���ϰ� ����
        {
            if (handCards[i] == Card)
            {
                for(int j = i; j < handCount - 1; j++)                  //ī�带 �����ϰ� �迭 ����
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;
                handCount--;

                ArrangeHand();                          //���� ����
                break;                                  //for ���� �������´�.
            }
        }

        mergeCards[mergeCount] = Card;
        mergeCount++;

        Card.transform.SetParent(mergeArea);            //���� ������ �θ�� �д�. (���� ������ ������Ʈ �̵�)
        ArrangeMerge();                                 //���� ���� ����
        UpdateMergeButtonState();                       //���� ��ư ���� ������Ʈ
    }

    //���� ��ư ���� ������Ʈ
    void UpdateMergeButtonState()
    {
        if (mergeButton != null)                        //���� ��ư�� ���� ���
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);            //���� ������ ī�尡 2�� �Ǵ� 3���� �������� Ȱ��ȭ
        }
    }

    //���� ������ ī����� ���ļ� �� ī�� ����
    void MergeCards()
    {
        if (mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("������ �Ϸ��� ī�尡 2�� �Ǵ� 3�� �ʿ��մϴ�!");
            return;
        }

        int firstCardValue = mergeCards[0].GetComponent<Card>().cardValue;             //ù��° ī�忡 �ִ� ���� �����´�.
        for (int i = 0; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();
            if (card == null || card.cardValue != firstCardValue)
            {
                Debug.Log("���� ������ ī�常 ���� �� �� �ֽ��ϴ�!");
                return;
            }
        }

        int newValue = firstCardValue + 1;

        if (newValue > cardImages.Length)
        {
            Debug.Log("�ִ� ī�� ���� �����Ͽ����ϴ�. ");
            return;
        }

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                mergeCards[i].SetActive(false);
            }
        }



        //��ī�� ����
        GameObject newCard = Instantiate(cardPrefabs, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();                                //������ ���ο� ī���� ������Ʈ�� ���� �ϱ� ���� ���÷� ����
        if (newCardTemp != null)                                                         //������ ���ο� ī���� ������Ʈ�� �����ϸ� (������ ������)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        //���� ���� ����
        for (int i = 0; i < maxMergeSize; i++)                   //���� �迭�� ��ȯ�ϸ鼭 null���� �����.
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;                                         //���� ī��Ʈ�� 0���� �ʱ�ȭ ��Ų��.

        UpdateMergeButtonState();                               //���� ��ư ���� ������Ʈ

        handCards[handCount] = newCard;                         //���� ������� ī�带 ���з� �̵�
        handCount++;                                            //�ڵ� ī��Ʈ�� ������Ų��.
        newCard.transform.SetParent(handArea);                  //���� ������� ī���� ��ġ�� �ڵ� ������ �ڽ����� ���´�

        ArrangeHand();                                          //���� ����
    }

    //���� ��ư Ŭ�� �� ���� ������ ī�� �ռ�
    void OnMergeButtonClicked()
    {
        MergeCards();
    }



}
