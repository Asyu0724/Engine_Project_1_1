using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerDied : MonoBehaviour
{
    [Header("UI 설정")]
    public CanvasGroup gameOverGroup;
    public float fadeDuration = 1.5f;

    [Header("오디오 설정")]
    public AudioSource bgmAudioSource; // ★ 현재 재생 중인 배경음악 AudioSource 연결
    public AudioSource diedAudioSource; // ★ 재생할 AudioSource 연결

    public static PlayerDied instance;
    void Start()
    {
        gameOverGroup.alpha = 0f;
        gameOverGroup.interactable = false;
        gameOverGroup.blocksRaycasts = false;
    }
    private void Awake()
    {
        instance = this;
    }

    public IEnumerator DelayTime()
    {
        Time.timeScale = 0f;

        if (bgmAudioSource != null)
        {
            // 볼륨을 0으로 만드는 데 fadeDuration만큼 걸림. (SetUpdate(true)로 타임스케일 무시!)
            bgmAudioSource.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                // 볼륨이 0이 되면 완전히 재생을 멈춰서 최적화 (선택 사항)
                bgmAudioSource.Stop();
            });
        }
        yield return new WaitForSecondsRealtime(2f);
        ShowGameOverUI();
    }

    public void ShowGameOverUI()
    {
        diedAudioSource.Play();
        // 2. UI 페이드 인
        gameOverGroup.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            gameOverGroup.interactable = true;
            gameOverGroup.blocksRaycasts = true;
        });
    }
}