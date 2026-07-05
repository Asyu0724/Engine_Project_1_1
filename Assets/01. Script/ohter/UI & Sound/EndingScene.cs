using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    [Header("UI 설정")]
    public CanvasGroup gameOverGroup;
    public float fadeDuration = 1.5f;

    public static EndingScene instance;
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

    public void ShowGameOverUI()
    {
        // 2. UI 페이드 인
        gameOverGroup.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            gameOverGroup.interactable = true;
            gameOverGroup.blocksRaycasts = true;
        });
    }
}
