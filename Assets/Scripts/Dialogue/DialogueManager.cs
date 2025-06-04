using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Serialization;

public class DialogueManager : MonoBehaviour
{
    [Header("UI ��� - Inspector ���� ����")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("�⺻ ����")]
    public Sprite defultCharacterImage;

    [Header("Ÿ���� ȿ�� ����")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    //���� ������
    private DialogueDataSO currentDialogue;
    private int currentLineIndex = 0;               //���� ���° ��ȭ������ (0���� ����)
    private bool isDialogueActive = false;          //��ȭ ���������� Ȯ�� �ϴ� �÷���
    private bool isTyping = false;                  //���� Ÿ���� ȿ���� ���� ������ Ȯ��
    private Coroutine typingCoroutine;              //Ÿ���� ȿ�� �ڷ�ƾ ���� (������)

    private void Start()
    {
        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);
    }

    private void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }
    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < textToType.Length; i++)              //�ý�Ʈ�� �� ���ھ� �߰�
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);       //��� �ð� ����
        }
        isTyping = false;                                       //Ÿ���� �Ϸ�
    }

    void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            {
                StopCoroutine(typingCoroutine);

            }
            isTyping = false;
            
            if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
            {
                dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
            }
        }
    }

    void ShowCurrentLine()
    {
        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            if(typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    void EndDialogue()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }

    public void ShowNextLine()
    {
        currentLineIndex++;

        if(currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    public void HandleNextInput()
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if(!isTyping)
        {
            ShowNextLine();
        }
    }

    public void SkipDialogue()
    {
        EndDialogue();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialogueDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0)
            return;

        //��ȭ ���� �غ�
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        //UI ������Ʈ
        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if(characterImage != null)
        {
            if(dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;
            }
            else
            {
                characterImage.sprite = defultCharacterImage;
            }
        }
        ShowCurrentLine();
    }
}

