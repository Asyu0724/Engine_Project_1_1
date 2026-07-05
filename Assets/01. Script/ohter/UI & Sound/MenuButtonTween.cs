using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // 1. DOTween 네임스페이스를 꼭 추가하세요!

public class MenuButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Text buttonText; // 텍스트 색상을 바꿀 경우

    [Header("설정")]
    public float moveDistance = 30f; // 이동할 거리
    public float duration = 0.3f;    // 애니메이션 속도
    public Color hoverColor = Color.cyan; // 마우스 올렸을 때 색상

    private Vector2 originPos;
    private Color originColor;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonText = GetComponentInChildren<Text>();

        originPos = rectTransform.anchoredPosition;
        if (buttonText != null) originColor = buttonText.color;
    }

    // 마우스를 올렸을 때 (Hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 2. DOTween의 핵심: 기존에 돌고 있던 트윈은 멈추고 새로 시작 (겹침 방지)
        rectTransform.DOKill();

        // 오른쪽으로 '스윽' 이동 (Ease 기능을 쓰면 훨씬 쫀득해짐)
        rectTransform.DOAnchorPosX(originPos.x + moveDistance, duration)
                     .SetEase(Ease.OutBack); // 살짝 튕기는 느낌 추가

        if (buttonText != null)
            buttonText.DOColor(hoverColor, duration);
    }

    // 마우스가 나갔을 때 (Exit)
    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOKill();

        // 원래 위치로 복귀
        rectTransform.DOAnchorPosX(originPos.x, duration)
                     .SetEase(Ease.OutSine);

        if (buttonText != null)
            buttonText.DOColor(originColor, duration);
    }
}