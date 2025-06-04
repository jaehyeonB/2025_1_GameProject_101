using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Serialization;

public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 - Inspector 에서 연결")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본 설정")]
    public Sprite defultCharacterImage;

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    //내부 변수들
    private DialogueDataSO currentDialogue;
    private int currentLineIndex = 0;               //현재 몇번째 대화중인지 (0부터 시작)
    private bool isDialogueActive = false;          //대화 진행중인지 확인 하는 플래그
    private bool isTyping = false;                  //현재 타이핑 효과가 진행 중인지 확인
    private Coroutine typingCoroutine;              //타이핑 효과 코루틴 참조 (중지용)

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

        for (int i = 0; i < textToType.Length; i++)              //택스트를 한 글자씩 추가
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);       //대기 시간 설정
        }
        isTyping = false;                                       //타이핑 완료
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

        //대화 시작 준비
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        //UI 업데이트
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

