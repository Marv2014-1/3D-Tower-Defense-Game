using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthManager : MonoBehaviour
{
    [Header("Castle Health Settings")]
    public int maxHealth = 20;
    private int currentHealth;

    [Header("UI Elements")]
    public Image HealthBarFill; // Assign the HealthBarFill Image in the Inspector
    public TMP_Text HealthText;     // Assign the HealthText Text component in the Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    /// Applies damage to the castle and updates the UI.
    public void DamageCastle(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            HandleCastleDestruction();
        }
    }

    /// Updates the health bar and health text UI elements.
    private void UpdateUI()
    {
        if (HealthBarFill != null)
        {
            HealthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }

        if (HealthText != null)
        {
            HealthText.text = $"Castle: {currentHealth}/{maxHealth}";
        }
    }

    // Game Over
    private void HandleCastleDestruction()
    {
        Debug.Log("The castle has been destroyed!");
    }

    /// Optional: Method to get current health.
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
