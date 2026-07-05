using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private bool Finished;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBarImage;

    [Header("Settings")]
    [SerializeField] private float maxHealth = 4000f; // 인스펙터에서 수정 가능하도록 변경
    [SerializeField] private float healthPerBar = 800f;
    [SerializeField] private float lerpSpeed = 5f;

    // 1. 실제 체력 (프로퍼티로 캡슐화)
    // 이제 외부에서 bossHealth.Health -= 100; 을 해도 안전합니다.
    private float _currentHealth;
    public float Health
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    // 2. 시각적으로 깎이는 체력 (Lerp 연출용)
    private float _visualHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
        _visualHealth = maxHealth;
        UpdateVisuals(true);
        Finished = false;
    }

    private void Update()
    {
        // 시각적 체력이 실제 체력과 다를 때만 부드럽게 따라가며 UI 갱신
        if (Mathf.Abs(_visualHealth - _currentHealth) > 0.1f)
        {
            _visualHealth = Mathf.Lerp(_visualHealth, _currentHealth, Time.deltaTime * lerpSpeed);
            UpdateVisuals(false);
        }
        else if (_visualHealth != _currentHealth)
        {
            // 목표치에 거의 도달하면 오차 방지를 위해 정확한 값으로 맞춤
            _visualHealth = _currentHealth;
            UpdateVisuals(false);
        }
    }

    private void FixedUpdate()
    {
        if (Finished) return;
        if (Health <= 0)
        {
            if (Patterns.instance.cannotDie) return;
            GameFinished();
            Finished = true;
        }
    }
    private void GameFinished()
    { 
        BossDied.Instance.StartCoroutine(BossDied.Instance.DelayTime());
        Debug.Log("Boss Defeated!");
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    // UI 갱신은 오직 "_visualHealth" (서서히 깎이는 체력)를 기준으로만 작동합니다.
    private void UpdateVisuals(bool isInstant)
    {
        // 즉시 갱신해야 할 때는 실제 체력을, 평소엔 서서히 깎이는 체력을 사용
        float healthToDisplay = isInstant ? _currentHealth : _visualHealth;

        // 1. 남은 페이즈 계산
        int remainingPhases = Mathf.Max(0, Mathf.CeilToInt(healthToDisplay / healthPerBar) - 1);

        // 2. 현재 줄의 Fill Amount 계산
        float currentBarHP = healthToDisplay % healthPerBar;

        // 부동소수점 오차로 인해 0.001처럼 남았을 때 빈 체력바가 되는 것 방지
        if (healthToDisplay > 0 && currentBarHP <= 0.01f)
        {
            currentBarHP = healthPerBar;
        }

        // 3. UI 적용
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentBarHP / healthPerBar;
        }

        if (healthText != null)
            healthText.text = $"Boss HP : {(int)healthToDisplay}";

        if (phaseText != null)
            phaseText.text = $"x {remainingPhases}";
    }
}