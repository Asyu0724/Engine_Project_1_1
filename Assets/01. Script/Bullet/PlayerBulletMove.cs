using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletMove : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BossHealth _bossHealth;

    [Header("Boss Hit Blink")]
    [SerializeField] private Color blinkColor = Color.cyan;
    [SerializeField] private float blinkInterval = 0.04f;
    [SerializeField] private int blinkCount = 2;
    [SerializeField] private AudioSource hitSound;

    private static readonly Dictionary<SpriteRenderer, Coroutine> BlinkCoroutines = new();
    private static readonly Dictionary<SpriteRenderer, Color> OriginalColors = new();

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bossHealth = GameObject.Find("Canvas").GetComponent<BossHealth>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = Vector2.up * 10f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            SpriteRenderer bossSprite = FindBossSpriteRenderer(collision);
            BlinkBossSprite(bossSprite);

            gameObject.SetActive(false);

            if (ShotManager._sM != null)
            {
                PlayerShot.instance.bulletPool.Push(gameObject);
            }

            _bossHealth.TakeDamage(3);
            hitSound.Play();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);

            if (ShotManager._sM != null)
            {
                PlayerShot.instance.bulletPool.Push(gameObject);
            }
        }
    }

    private SpriteRenderer FindBossSpriteRenderer(Collider2D collision)
    {
        SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = collision.GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = collision.GetComponentInParent<SpriteRenderer>();
        }

        return spriteRenderer;
    }

    private void BlinkBossSprite(SpriteRenderer bossSprite)
    {
        if (bossSprite == null || PlayerShot.instance == null) return;

        if (!OriginalColors.ContainsKey(bossSprite))
        {
            OriginalColors.Add(bossSprite, bossSprite.color);
        }

        if (BlinkCoroutines.TryGetValue(bossSprite, out Coroutine runningCoroutine))
        {
            PlayerShot.instance.StopCoroutine(runningCoroutine);
            bossSprite.color = OriginalColors[bossSprite];
        }

        Coroutine blinkCoroutine = PlayerShot.instance.StartCoroutine(BlinkRoutine(bossSprite));
        BlinkCoroutines[bossSprite] = blinkCoroutine;
    }

    private IEnumerator BlinkRoutine(SpriteRenderer bossSprite)
    {
        Color originalColor = OriginalColors[bossSprite];

        for (int i = 0; i < blinkCount; i++)
        {
            bossSprite.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);

            bossSprite.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        bossSprite.color = originalColor;
        BlinkCoroutines.Remove(bossSprite);
    }
}