using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int MaxHealth = 5;

    private void Start()
    {
        health = MaxHealth;
        HealthUI healthUI = UIManager.Instance.healthUI;
        healthUI.InitHealth();
    }

    public void OnDamaged()
    {
        health -= 1;
        HealthUI healthUI = UIManager.Instance.healthUI;
        healthUI.DestroyHealth();
    }
}
