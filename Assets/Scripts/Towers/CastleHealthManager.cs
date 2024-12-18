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
    public TMP_Text HealthText; // Assign the HealthText Text component in the Inspector

    [Header("End Menu Settings")]
    public GameObject endMenu;     // Reference to the EndMenu GameObject
    public TMP_Text endMenuText;   // Reference to the TextMeshPro component within EndMenu

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        // Ensure EndMenu is disabled at the start
        if (endMenu != null)
        {
            endMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("EndMenu is not assigned in the CastleHealthManager.");
        }
    }

    /// <summary>
    /// Applies damage to the castle and updates the UI.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
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

    /// <summary>
    /// Updates the health bar and health text UI elements.
    /// </summary>
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

    /// <summary>
    /// Handles the castle's destruction by triggering the EndMenu.
    /// </summary>
    private void HandleCastleDestruction()
    {
        Debug.Log("The castle has been destroyed!");

        // Trigger EndMenu with "Game Over!" message
        TriggerEndMenu("Game Over!");
    }

    /// Triggers the EndMenu with the specified message.
    private void TriggerEndMenu(string message)
    {
        if (endMenu != null && endMenuText != null)
        {
            // Set the desired message
            endMenuText.text = message;

            // Enable the EndMenu GameObject
            endMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            // Pause the game
            Time.timeScale = 0f;

            this.enabled = false;
        }
        else
        {
            Debug.LogWarning("EndMenu or EndMenuText is not assigned in the CastleHealthManager.");
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
