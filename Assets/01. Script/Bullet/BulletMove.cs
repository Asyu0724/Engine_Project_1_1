using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public enum BulletType { C, CP, CG, SP }

    public BulletType type;
    [SerializeField] private float speed = 5.0f;

    public static List<BulletMove> AllBullets = new();

    private void OnEnable()
    {
        AllBullets.Add(this);
    }

    private void OnDisable()
    {
        AllBullets.Remove(this);
        CancelInvoke(nameof(Die));
    }

    private void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime), Space.Self);
    }

    public void Die()
    {
        if (!gameObject.activeSelf) return;

        gameObject.SetActive(false);

        if (ShotManager._sM == null) return;

        switch (type)
        {
            case BulletType.C:
                ShotManager._sM.cbulletPool.Push(gameObject);
                break;
            case BulletType.CP:
                ShotManager._sM.cpbulletPool.Push(gameObject);
                break;
            case BulletType.CG:
                ShotManager._sM.cgbulletPool.Push(gameObject);
                break;
            case BulletType.SP:
                ShotManager._sM.spbulletPool.Push(gameObject);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerHit(collision.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall hit.");
            Die();
        }
    }

    private void HandlePlayerHit(GameObject playerObject)
    {
        Player player = Player.playerInstance;
        if (player == null || player.cannotDie) return;
        if (ShotManager._sM != null && ShotManager._sM.story) return;

        if (BombManager.Instance != null && BombManager.Instance.CanStartDeathBomb())
        {
            Die();
            BombManager.Instance.StartDeathBombWindow(() => ApplyPlayerDamage(playerObject));
            return;
        }

        ApplyPlayerDamage(playerObject);
        Die();
    }

    private void ApplyPlayerDamage(GameObject playerObject)
    {
        Player player = Player.playerInstance;
        HealthUI healthUI = UIManager.Instance.healthUI;

        if (player == null || healthUI == null) return;

        if (healthUI.currentHealth > 1)
        {
            player.StartCoroutine(player.CannotDie());
            healthUI.currentHealth -= 1;
            healthUI.DestroyHealth();
            return;
        }

        player.StartCoroutine(player.CannotDie());
        playerObject.SetActive(false);
        healthUI.DestroyHealth();
        PlayerDied.instance.StartCoroutine(PlayerDied.instance.DelayTime());
        Hitted.instance.OnHitEffect();
    }

    public void ExplodeByBomb()
    {
        Die();
        UIManager.Instance.scoreUI.UpdateScore(100);
    }
}
