using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // ★ Image 컴포넌트를 사용하기 위해 추가해야 합니다.

public class BossDied : MonoBehaviour
{
    [Header("UI 설정")]
    public Image gameClearedImage; // ★ CanvasGroup을 Image로 변경
    public float fadeDuration = 1.5f;

    [Header("오디오 설정")]
    public AudioSource bgmAudioSource;

    public static BossDied Instance;

    [SerializeField] private GameObject bossObject;
    [SerializeField] private Transform returnTransform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // ★ Image의 색상(알파값)을 0으로 초기화
        Color color = gameClearedImage.color;
        color.a = 0f;
        gameClearedImage.color = color;

        // ★ CanvasGroup의 interactable/blocksRaycasts 대신, Image의 raycast를 끕니다.
        gameClearedImage.raycastTarget = false; 
    }

    public IEnumerator DelayTime()
    {
        ShotManager._sM.story = true;
        // 알파값 1로 만들기
        Color color = gameClearedImage.color;
        color.a = 1f;
        gameClearedImage.color = color;

        if (bgmAudioSource != null)
        {
            bgmAudioSource.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                bgmAudioSource.Stop();
            });
        }
        yield return new WaitForSecondsRealtime(2f);
        HideGameFinishedUI();
        bossObject.transform.position = returnTransform.position;
    }

    private void HideGameFinishedUI()
    {
        // ★ Image 페이드 아웃 (알파값을 0으로)
        gameClearedImage.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            // 페이드가 끝나면 클릭 방지
            gameClearedImage.raycastTarget = false;
        });
        Died();
    }

    private void Died()
    {
        ShotManager._sM.story = true;
        FinalDialogue.instance.StartDialogue();
    }
}