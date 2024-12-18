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

    void Awake()
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
            Debug.LogError("EndMenu is not assigned in the Inspector!");
        }
    }

    void Start()
    {
        if (endMenu != null)
        {
            Debug.Log("EndMenu successfully set.");
        }
        else
        {
            Debug.LogError("EndMenu is null in Start!");
        }
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

    /// Handles the castle's destruction by triggering the EndMenu.
    private void HandleCastleDestruction()
    {
        Debug.Log("The castle has been destroyed!");

        Time.timeScale = 0f;

        // Set the desired message
        endMenuText.text = "Game Over!";

        // Enable the EndMenu GameObject
        endMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;

        this.enabled = false;
    }

    /// Triggers the EndMenu with the specified message.
    // private void TriggerEndMenu(string message)
    // {
    //         // Set the desired message
    //         endMenuText.text = message;

    //         // Enable the EndMenu GameObject
    //         endMenu.SetActive(true);

    //         Cursor.lockState = CursorLockMode.None;

    //         // Pause the game
    //         Time.timeScale = 0f;

    //         this.enabled = false;
    // }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
