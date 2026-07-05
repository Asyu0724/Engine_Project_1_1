using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EffectEnabled : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Graphic graphic;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform effectTransform;

    [Header("Appear")]
    [SerializeField] private float startScale = 0.7f;
    [SerializeField] private float appearScale = 1f;
    [SerializeField] private float appearDuration = 1.2f;
    [SerializeField] private float appearAlpha = 1f;
    [SerializeField] private Ease appearEase = Ease.OutSine;

    [Header("Cast")]
    [SerializeField] private float disappearScale = 1.55f;
    [SerializeField] private float disappearDuration = 0.8f;
    [SerializeField] private Ease disappearEase = Ease.InSine;

    [Header("State")]
    [SerializeField] private bool hideOnAwake = true;

    private Sequence sequence;
    private Vector3 originScale;
    private bool initialized;

    private void Awake()
    {
        Initialize();

        if (hideOnAwake)
        {
            gameObject.SetActive(false);
        }
    }

    private void Initialize()
    {
        if (initialized)
        {
            return;
        }

        if (effectTransform == null)
        {
            effectTransform = transform;
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (graphic == null)
        {
            graphic = GetComponentInChildren<Graphic>(true);
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }

        originScale = effectTransform.localScale;
        initialized = true;
    }

    private void OnDisable()
    {
        KillSequence();
    }

    public Tween PlayReady()
    {
        Initialize();
        KillSequence();

        gameObject.SetActive(true);
        SetAlpha(0f);
        effectTransform.localScale = originScale * startScale;

        sequence = DOTween.Sequence()
            .SetTarget(this)
            .Append(effectTransform.DOScale(originScale * appearScale, appearDuration).SetEase(appearEase));

        JoinFade(sequence, appearAlpha, appearDuration);

        return sequence;
    }

    public Tween PlayCast()
    {
        Initialize();
        KillSequence();

        gameObject.SetActive(true);
        SetAlpha(appearAlpha);
        effectTransform.localScale = originScale * appearScale;

        sequence = DOTween.Sequence()
            .SetTarget(this)
            .Append(effectTransform.DOScale(originScale * disappearScale, disappearDuration).SetEase(disappearEase));

        JoinFade(sequence, 0f, disappearDuration);
        sequence.AppendCallback(() => gameObject.SetActive(false));

        return sequence;
    }

    public void Hide()
    {
        KillSequence();
        gameObject.SetActive(false);
    }

    private void KillSequence()
    {
        if (sequence != null && sequence.IsActive())
        {
            sequence.Kill();
            sequence = null;
        }
    }

    private void SetAlpha(float alpha)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
            return;
        }

        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
            return;
        }

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    private void JoinFade(Sequence targetSequence, float alpha, float duration)
    {
        if (canvasGroup != null)
        {
            targetSequence.Join(canvasGroup.DOFade(alpha, duration));
            return;
        }

        if (graphic != null)
        {
            targetSequence.Join(graphic.DOFade(alpha, duration));
            return;
        }

        if (spriteRenderer != null)
        {
            targetSequence.Join(spriteRenderer.DOFade(alpha, duration));
        }
    }
}