using System;
using System.Collections;
using UnityEngine;
using DG.Tweening; // DOTween 사용을 위한 필수 네임스페이스

public class BossDieEffect : MonoBehaviour
{
    [Header("연출 시간 설정")]
    public float flashDuration = 0.2f; // 번쩍! 하고 발광하는데 걸리는 시간
    public float fadeDuration = 1.5f;  // 서서히 투명해지며 사라지는 시간

    [Header("발광 색상 (HDR)")]
    // [ColorUsage] 속성을 넣으면 인스펙터에서 HDR(발광) 색상을 쉽게 선택할 수 있습니다.
    [ColorUsage(true, true)] 
    public Color glowColor = new Color(3f, 3f, 3f, 1f); // 기본값: 강한 흰색 발광

    [SerializeField] private AudioSource vicBGM;
    
    [field:SerializeField] public ScoreUI scoreUI {get; private set;}

    private SpriteRenderer spriteRenderer;
    public static BossDieEffect instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 2D 환경이므로 SpriteRenderer 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 보스가 죽을 때 이 함수를 호출해주세요!
    public void PlayDeathEffect()
    {
        // 1. DOTween 시퀀스(연속 동작) 생성
        Sequence deathSequence = DOTween.Sequence();

        // 2. 순식간에 강하게 발광 (현재 색상 -> 지정한 HDR 색상)
        deathSequence.Append(spriteRenderer.DOColor(glowColor, flashDuration));

        // 3. 발광 상태에서 서서히 알파값을 0으로 빼면서 페이드아웃
        // 발광하던 색상(glowColor)을 유지한 채 알파(a)값만 0인 상태로 부드럽게 넘어갑니다.
        Color clearColor = new Color(glowColor.r, glowColor.g, glowColor.b, 0f);
        deathSequence.Append(spriteRenderer.DOColor(clearColor, fadeDuration));

        // 4. 애니메이션이 모두 끝나면(발광 후 투명해짐) 오브젝트 삭제
        deathSequence.OnComplete(() =>
        {
            Destroy(gameObject);
            scoreUI.UpdateHighScore(scoreUI.currentScore);
            EndingScene.instance.ShowGameOverUI();  
            vicBGM.Play();
        });
    }
}