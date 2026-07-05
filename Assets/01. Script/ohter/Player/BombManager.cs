using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance { get; private set; }

    [Header("Bomb Settings")]
    [SerializeField] private int maxBombCount = 3;
    [SerializeField] private float bombLockTime = 1f;
    [SerializeField] private float invincibleTime = 2.5f;
    [SerializeField] private float bossDamage = 150f;
    [SerializeField] private float deathBombWindow = 0.15f;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.35f;
    [SerializeField] private float shakeStrength = 0.3f;
    [SerializeField] private int shakeVibrato = 18;

    [Header("Optional Effects")]
    [SerializeField] private GameObject bombEffect;
    [SerializeField] private AudioSource bombAudio;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private BombUI bombUI;

    public int CurrentBombCount { get; private set; }
    public bool IsBombActive { get; private set; }
    public bool IsDeathBombWindow { get; private set; }

    private float bombCooldown;

    private void Awake()
    {
        Instance = this;
        CurrentBombCount = maxBombCount;
    }

    private void Start()
    {
        ResolveBombUI();

        if (bombUI != null)
        {
            bombUI.InitBombs(maxBombCount);
        }
    }

    private void Update()
    {
        if (ShotManager._sM != null && ShotManager._sM.story) return;

        if (bombCooldown > 0f)
        {
            bombCooldown -= Time.deltaTime;
        }

        if (!IsDeathBombWindow && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TriggerBomb();
        }
    }

    public bool CanStartDeathBomb()
    {
        return !IsBombActive && !IsDeathBombWindow && CurrentBombCount > 0;
    }

    public void StartDeathBombWindow(System.Action onMissedDeathBomb)
    {
        if (!CanStartDeathBomb())
        {
            onMissedDeathBomb?.Invoke();
            return;
        }

        StartCoroutine(DeathBombWindowRoutine(onMissedDeathBomb));
    }

    private void TriggerBomb()
    {
        if (IsBombActive || bombCooldown > 0f) return;

        if (CurrentBombCount <= 0)
        {
            Debug.Log("No bombs left.");
            return;
        }

        StartCoroutine(BombRoutine());
    }

    private IEnumerator BombRoutine()
    {
        CurrentBombCount--;
        if (bombUI != null)
        {
            bombUI.DestroyBomb();
        }

        IsBombActive = true;
        bombCooldown = bombLockTime;
        IsDeathBombWindow = false;

        Player player = Player.playerInstance;
        if (player != null)
        {
            player.cannotDie = true;
        }

        if (bombAudio != null)
        {
            bombAudio.Play();
        }

        if (bombEffect != null)
        {
            bombEffect.SetActive(false);
            bombEffect.SetActive(true);
        }

        ShakeCamera();
        ClearAllBullets();
        DamageBoss();

        yield return new WaitForSeconds(invincibleTime);

        if (player != null)
        {
            player.cannotDie = false;
        }

        IsBombActive = false;
    }

    private IEnumerator DeathBombWindowRoutine(System.Action onMissedDeathBomb)
    {
        IsDeathBombWindow = true;

        Player player = Player.playerInstance;
        if (player != null)
        {
            player.cannotDie = true;
        }

        float timer = deathBombWindow;
        while (timer > 0f)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                StartCoroutine(BombRoutine());
                yield break;
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        IsDeathBombWindow = false;

        if (player != null && !IsBombActive)
        {
            player.cannotDie = false;
        }

        onMissedDeathBomb?.Invoke();
    }

    private void ShakeCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        mainCamera.transform.DOComplete();
        mainCamera.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, 90f, false, true);
    }

    private void ClearAllBullets()
    {
        for (int i = BulletMove.AllBullets.Count - 1; i >= 0; i--)
        {
            if (BulletMove.AllBullets[i] != null)
            {
                BulletMove.AllBullets[i].ExplodeByBomb();
            }
        }
    }

    private void DamageBoss()
    {
        if (bossDamage <= 0f) return;

        if (bossHealth == null)
        {
            bossHealth = FindFirstObjectByType<BossHealth>();
        }

        if (bossHealth != null)
        {
            bossHealth.TakeDamage(bossDamage);
        }
    }

    private void ResolveBombUI()
    {
        if (bombUI != null) return;

        if (UIManager.Instance != null)
        {
            bombUI = UIManager.Instance.bombUI;
        }

        if (bombUI == null)
        {
            bombUI = FindFirstObjectByType<BombUI>();
        }
    }
}
