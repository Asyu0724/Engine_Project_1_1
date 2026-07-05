using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // DOTween 사용 선언!

// 어떤 연출을 할지 선택하는 스위치
public enum EmotionAnim { None, Shake, Jump, Pop }

// 누가 말하는지 구분하기 위한 enum
public enum SpeakerPosition { Left, Right }

// 인스펙터에서 대화 데이터를 쉽게 넣기 위한 클래스
[System.Serializable]
public class DialogueLine
{
    public SpeakerPosition speakerPosition; // Left면 주인공, Right면 보스
    public string speakerName;              // 말하는 캐릭터 이름

    [TextArea(3, 5)]
    public string dialogueText;             // 대화 내용

    public Sprite leftSprite;               // 왼쪽 일러스트 (바꿀 때만 넣고 아니면 비워둠)
    public Sprite rightSprite;              // 오른쪽 일러스트 (바꿀 때만 넣고 아니면 비워둠)

    [Header("DOTween 연출")]
    public EmotionAnim emotionAnimation;    // 캐릭터 애니메이션 (흔들림, 점프 등)
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image leftCharacter;
    public Image rightCharacter;

    [Header("연출 설정")]
    public AudioSource nextTextAudio; // 다음 글자 나올 때 효과음
    public float activeScale = 1.1f; // 말할 때 커지는 크기
    public Color dimColor = new Color(0.4f, 0.4f, 0.4f, 1f); // 안 말할 때 어두워지는 색상
    public float typingSpeed = 0.05f; // 글자 출력 속도

    [Header("대화 데이터")]
    public DialogueLine[] dialogues;

    private int currentLineIndex = 0;
    private bool isTyping = false;

    // DOTween 버그 방지용 초기 위치 저장 변수
    private Vector2 leftDefaultPos;
    private Vector2 rightDefaultPos;
    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 시작할 때 캐릭터들의 원래 위치(좌표)를 기억해둠
        leftDefaultPos = leftCharacter.rectTransform.anchoredPosition;
        rightDefaultPos = rightCharacter.rectTransform.anchoredPosition;

        // 씬이 시작되자마자 대화 시작
        StartDialogue();
        ShotManager._sM.story = true; // 대화 시작 시 탄막 패턴 비활성화
    }

    void Update()
    {
        if (InGameESC.instance.isPaused) return;
        
        // 대화창이 켜져있고, 'Z' 키를 눌렀을 때
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Z))
        {
            if (isTyping)
            {
                // 타이핑 중이면 즉시 전체 텍스트 출력 (스킵)
                StopAllCoroutines();
                dialogueText.text = dialogues[currentLineIndex].dialogueText;
                isTyping = false;
            }
            else
            {
                // 다음 대화로 넘어가기
                NextDialogue();
            }
        }
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.X))
        {
            StopAllCoroutines();
            EndDialogue();
        }
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        DialogueLine line = dialogues[currentLineIndex];

        nameText.text = line.speakerName;

        // 일러스트 띄우기 (할당된 이미지가 있다면 교체)
        if (line.leftSprite != null) leftCharacter.sprite = line.leftSprite;
        if (line.rightSprite != null) rightCharacter.sprite = line.rightSprite;

        // 화자를 찾아서 연출 함수로 넘김
        Image activeImage = (line.speakerPosition == SpeakerPosition.Left) ? leftCharacter : rightCharacter;
        Image inactiveImage = (line.speakerPosition == SpeakerPosition.Left) ? rightCharacter : leftCharacter;

        // 크기/명암 및 위치 초기화 연출
        HighlightCharacter(activeImage, inactiveImage);

        // DOTween 감정 연출 실행
        PlayEmotionAnimation(activeImage, line.emotionAnimation);

        // 화자에 따른 정렬
        if (line.speakerPosition == SpeakerPosition.Left)
        {
            dialogueText.alignment = TextAlignmentOptions.Left;
            nameText.alignment = TextAlignmentOptions.Left;
        }
        else
        {
            dialogueText.alignment = TextAlignmentOptions.Right;
            nameText.alignment = TextAlignmentOptions.Right;
        }

        // 타이핑 코루틴 시작
        StartCoroutine(TypeSentence(line.dialogueText));
    }

    private void HighlightCharacter(Image activeImg, Image inactiveImg)
    {
        // Z키 연타 시 버그를 막기 위해 진행 중인 찌그러짐/흔들림 애니메이션 강제 종료
        activeImg.rectTransform.DOKill();
        inactiveImg.rectTransform.DOKill();

        // 원래 위치로 뼈대 맞추기 (이탈 방지)
        activeImg.rectTransform.anchoredPosition = (activeImg == leftCharacter) ? leftDefaultPos : rightDefaultPos;
        inactiveImg.rectTransform.anchoredPosition = (inactiveImg == leftCharacter) ? leftDefaultPos : rightDefaultPos;

        // 말하는 쪽: 크기 키우고, 밝게 하고, UI 레이어 맨 앞으로 오게 설정
        activeImg.transform.localScale = new Vector3(activeScale, activeScale, 1f);
        activeImg.color = Color.white;
        activeImg.transform.SetAsLastSibling();

        // 안 말하는 쪽: 원래 크기로 되돌리고, 살짝 어둡게
        inactiveImg.transform.localScale = Vector3.one;
        inactiveImg.color = dimColor;
    }

    private void PlayEmotionAnimation(Image targetImg, EmotionAnim animType)
    {
        switch (animType)
        {
            case EmotionAnim.None:
                break;
            case EmotionAnim.Shake:
                targetImg.rectTransform.DOShakeAnchorPos(0.5f, new Vector2(10f, 0f), 20, 90f, false, true);
                break;
            case EmotionAnim.Jump:
                targetImg.rectTransform.DOPunchAnchorPos(new Vector2(0, 30f), 0.4f, 1, 1f);
                break;
            case EmotionAnim.Pop:
                targetImg.rectTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 0.3f, 2, 1f);
                break;
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void NextDialogue()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogues.Length)
        {
            ShowDialogue();
            nextTextAudio.Play(); // 다음 글자 나올 때 효과음 재생
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        ShotManager._sM.story = false; // 대화 끝나면 탄막 패턴 활성화
        TurnOnBGM.instance.TurnOn(); // 대화 끝나면 BGM 켜기
        // 여기에 보스 탄막 슈팅 시작 코드 연결
    }
}